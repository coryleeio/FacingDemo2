
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class CombatUtil
    {
        public static Predicate<Effect> AppliedEffects = (effectInQuestion) => { return effectInQuestion is AppliedEffect; };
        public static Predicate<Entity> HittableEntities = (t) => t.IsCombatant && t.Body != null && !t.Body.IsDead;

        public static Predicate<Point> NonoccupiedTiles = (t) => { return Context.GameStateManager.Game.CurrentLevel.Grid[t].Walkable && Context.GameStateManager.Game.CurrentLevel.Grid[t].EntitiesInPosition.FindAll(CombatUtil.HittableEntities).Count == 0; };
        public static Predicate<Entity> LootableEntities = (ent) => { return ent.PrototypeIdentifier == UniqueIdentifier.ENTITY_GROUND_DROP || (ent.Body != null && ent.Body.IsDead && ent.Inventory.HasAnyItems); };
        public static Predicate<Point> FloorTiles = (pointOnLevel) => { return Context.GameStateManager.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.GameStateManager.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType == TileType.Floor; };
        public static Predicate<Point> VisibleTiles = (pointOnLevel) => { return Context.GameStateManager.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.GameStateManager.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType != TileType.Empty; };

        public static List<Entity> HittableEntitiesInPositionsOnLevel(Point point, Level level)
        {
            return HittableEntitiesInPositionsOnLevel(new List<Point>() { point }, level);
        }

        public static List<Entity> HittableEntitiesInPositionsOnLevel(List<Point> points, Level level)
        {
            List<Entity> entities = new List<Entity>();
            foreach (var point in points)
            {
                entities.AddRange(level.Grid[point].EntitiesInPosition.FindAll(CombatUtil.HittableEntities));
            }
            return entities;
        }

        public static void ShowMessages(ActionOutcome result)
        {
            foreach (var msg in result.LogMessages)
            {
                Context.UIController.TextLog.AddText(msg);
            }
            foreach (var msg in result.LateMessages)
            {
                Context.UIController.TextLog.AddText(msg);
            }
            foreach (var msg in result.FloatingTextMessage)
            {
                Context.UIController.FloatingCombatTextManager.ShowCombatText(msg.Message, msg.Color, msg.FontSize, MathUtil.MapToWorld(result.Target.Position));
            }
            // Because in some circumstances this can be caused twice, clear the buffer.
            result.LogMessages.Clear();
            result.LateMessages.Clear();
            result.FloatingTextMessage.Clear();
        }

        public static void ApplyEntityStateChange(ActionOutcome result)
        {
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.AppliedEffects);
            result.Resolve(); // will not recalculate
            var target = result.Target;

            Assert.IsNotNull(target);
            if (result.Source != null)
            {
                var newFacingDirection = MathUtil.RelativeDirection(target.Position, result.Source.Position);
                if (target.View != null && target.View.SkeletonAnimation != null)
                {
                    var skeletonAnimation = target.View.SkeletonAnimation;
                    skeletonAnimation.AnimationState.ClearTracks();
                    skeletonAnimation.Skeleton.SetToSetupPose();
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.GetHit, newFacingDirection), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, newFacingDirection), true, 0.0f);
                }
            }
            if (result.AttackParameters != null && result.AttackParameters.DamageType == DamageTypes.NOT_SET)
            {
                throw new NotImplementedException("You forgot to set the damage type on this attack");
            }

            if (!target.IsCombatant)
            {
                throw new NotImplementedException("Cannot deal damage to non combatants");
            }

            if (target.Body.CurrentHealth <= 0)
            {
                // if you keep hitting him he doesn't get dead-er..
                return;
            }

            var sourceName = "";
            if (result.Source != null)
            {
                sourceName = result.Source.Name;
            }
            var targetName = target.Name;

            if (result.WasShortCircuited)
            {
                result.LogMessages.AddLast(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, result.HealthChange, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
            }
            else
            {
                if (result.AttackParameters != null && result.HealthChange != 0)
                {
                    result.LogMessages.AddLast(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, result.HealthChange < 0 ? result.HealthChange * -1 : result.HealthChange, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
                    target.Body.CurrentHealth = target.Body.CurrentHealth - result.HealthChange;

                    var healthChangeDisplay = result.HealthChange > 0 ? "-" : "+";
                    healthChangeDisplay += Math.Abs(result.HealthChange).ToString();
                    Color healthChangeColor = Color.black;

                    if (target.IsPlayer)
                    {
                        if (result.HealthChange > 0)
                        {
                            // Damage to player
                            healthChangeColor = Color.red;
                        }
                        else
                        {
                            // Healing to player
                            healthChangeColor = Color.green;
                        }
                    }
                    else
                    {
                        if (result.HealthChange > 0)
                        {
                            // Damage to NPC
                            healthChangeColor = Color.magenta;
                        }
                        else
                        {
                            // Healing to NPC
                            healthChangeColor = Color.blue;
                        }
                    }

                    result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                    {
                        Message = string.Format("{0}", healthChangeDisplay),
                        Color = healthChangeColor,
                        target = target,
                    });

                    // Go ahead and show messages so that onApply messages appear in the correct order
                    // onapply lacks a combat context so its messages appear immediately
                    // We do this here so that the messages appear before the effect messages
                    ShowMessages(result);
                }

                HandleAppliedEffects(result);
                HandleRemovedEffects(result);

                if(target.IsNPC)
                {
                    if((result.AppliedEffects.Count > 0 || result.HealthChange > 0) && result.Target.Behaviour != null && result.Source != null)
                    {
                        var level = Context.GameStateManager.Game.CurrentLevel;
                        result.Target.Behaviour.ShoutAboutHostileTarget(level, result.Source, result.Target.CalculateValueOfAttribute(Attributes.SHOUT_RADIUS));
                        if(result.Target.Behaviour.LastKnownTargetPosition == null)
                        {
                            result.Target.Behaviour.LastKnownTargetPosition = new Point(result.Source.Position);
                        }
                    }
                }
                CombatUtil.CapAttributes(target);

                if (target.Body.CurrentHealth <= 0)
                {
                    if (target.View != null && target.View.SkeletonAnimation != null)
                    {
                        var skeletonAnimation = target.View.SkeletonAnimation;
                        skeletonAnimation.AnimationState.ClearTracks();
                        skeletonAnimation.Skeleton.SetToSetupPose();
                        skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.FallDown, target.Direction), false);
                    }
                    result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                    {
                        Message = string.Format("Dead!", result.HealthChange),
                        Color = Color.black,
                        target = target,
                    });
                    result.LogMessages.AddLast(string.Format("{0} has been slain!", targetName));
                    target.Name = string.Format("( Corpse ) {0}", target.Name);
                    target.Body.IsDead = true;
                    // Is not deregistered because the corpse should still be available
                    target.Behaviour = null;
                    var game = Context.GameStateManager.Game;
                    var level = game.CurrentLevel;

                    foreach (var pair in target.Inventory.EquippedItemBySlot)
                    {
                        var item = pair.Value;
                        foreach (var effect in item.Effects)
                        {
                            effect.RemovePersistantVisualEffects(target);
                        }
                    }

                    if (target.BlocksPathing)
                    {
                        // You cant deregister to ReleasePathfindingAtPosition bc we want to keep the corpse
                        level.ReleasePathfindingAtPosition(target, target.Position);
                        target.BlocksPathing = false;
                    }

                    if (target.View.ViewGameObject != null)
                    {
                        target.View.ViewGameObject.AddComponent<DeathAnimation>();

                    }

                    if (target.Inventory.HasAnyItems)
                    {
                        target.View.ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_CORPSE;
                        Context.ViewFactory.BuildView(target, false);
                    }
                    if (!target.IsPlayer)
                    {
                        game.MonstersKilled++;
                    }
                }

                if (target.IsPlayer && Context.UIController.InventoryWindow.isActiveAndEnabled)
                {
                    Context.UIController.Refresh();
                }
            }
            ShowMessages(result);
        }

        public static List<Effect> GetEntityEffectsByType(Entity entity, Predicate<Effect> filter = null)
        {
            var effectsAggregate = new List<Effect>();
            if (entity.Trigger != null && entity.Trigger.Effects.Count > 0)
            {
                effectsAggregate.AddRange(entity.Trigger.Effects);
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                effectsAggregate.AddRange(item.Effects);
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    effectsAggregate.AddRange(item.Effects);

                }
            }

            if (entity.Body != null)
            {
                effectsAggregate.AddRange(entity.Body.Effects);
            }
            if (filter == null)
            {
                return effectsAggregate;
            }
            else
            {
                return effectsAggregate.FindAll(filter);
            }
        }

        public static void RemoveEntityEffects(Entity entity, List<Effect> effectsThatShouldExpire)
        {
            Predicate<Effect> IsAnAffectThatShouldExpire = (eff) => { return effectsThatShouldExpire.Contains(eff); };

            // Note that we never remove attack specific effects
            if (entity.Trigger != null && entity.Trigger.Effects.Count > 0)
            {
                var expiring = entity.Trigger.Effects.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.OnRemove(entity);
                }
                entity.Trigger.Effects.RemoveAll(IsAnAffectThatShouldExpire);
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                var expiring = item.Effects.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.OnRemove(entity);
                }
                item.Effects.RemoveAll(IsAnAffectThatShouldExpire);
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    var expiring = item.Effects.FindAll(IsAnAffectThatShouldExpire);
                    foreach (var expire in expiring)
                    {
                        expire.OnRemove(entity);
                    }
                    item.Effects.RemoveAll(IsAnAffectThatShouldExpire);
                }
            }

            if (entity.Body != null)
            {
                var expiring = entity.Body.Effects.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.OnRemove(entity);
                }
                entity.Body.Effects.RemoveAll(IsAnAffectThatShouldExpire);
            }
        }

        private static void HandleRemovedEffects(ActionOutcome outcome)
        {
            if (!outcome.WasShortCircuited)
            {
                foreach (var effect in outcome.RemovedEffects)
                {
                    RemoveEntityEffects(outcome.Target, outcome.RemovedEffects);
                }
            }
        }

        public static void CapAttributes(Entity entity)
        {
            if (entity.Body != null)
            {
                if (entity.Body.CurrentHealth > entity.CalculateValueOfAttribute(Gamepackage.Attributes.MAX_HEALTH))
                {
                    entity.Body.CurrentHealth = entity.CalculateValueOfAttribute(Gamepackage.Attributes.MAX_HEALTH);
                }
            }
        }

        private static void HandleAppliedEffects(ActionOutcome outcome)
        {
            if (!outcome.WasShortCircuited)
            {
                foreach (var effect in outcome.AppliedEffects)
                {
                    if (effect is AppliedEffect)
                    {
                        var appliedEffect = (AppliedEffect)effect;
                        if (appliedEffect.CanApply(outcome))
                        {
                            appliedEffect.Apply(outcome);
                        }
                    }
                    else
                    {
                        effect.HandleStacking(outcome);
                    }
                }
            }
        }

        public static void PerformTriggerStepAbilityIfSteppedOn(Entity Source, Entity potentialTrigger, List<Point> points)
        {
            if (points.Contains(Source.Position))
            {
                var abilities = potentialTrigger.Trigger.Effects;
                foreach (var effect in abilities)
                {
                    var attack = new ActionOutcome();
                    attack.Source = potentialTrigger;
                    attack.Target = Source;
                    effect.TriggerOnStep(attack);
                }
            }
        }

        public static void ConsumeItemCharges(Entity owner, Item item, int NumberConsumed = 1)
        {
            if (!item.HasUnlimitedCharges && item.HasCharges)
            {
                item.ExactNumberOfChargesRemaining = item.ExactNumberOfChargesRemaining - NumberConsumed;
                if (item.ExactNumberOfChargesRemaining < 0)
                {
                    item.ExactNumberOfChargesRemaining = 0;
                }
                if (item.DestroyWhenAllChargesAreConsumed)
                {
                    owner.Inventory.RemoveItemStack(item);
                    Context.UIController.Refresh();
                    Context.ViewFactory.BuildView(owner);
                }
            }
        }
    }
}
