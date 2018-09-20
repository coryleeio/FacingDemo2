
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class CombatUtil
    {
        public static bool CanMelee(Entity a, Entity b)
        {
            if (a == null || b == null)
            {
                return false;
            }
            return a.Position.IsAdjacentTo(b.Position) && a.Position.IsOrthogonalTo(b.Position);
        }

        public static void ApplyEntityStateChange(EntityStateChange result)
        {
            Assert.IsNotNull(result.Targets);
            Assert.IsNotNull(result.AppliedEffects);

            foreach (var target in result.Targets)
            {
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
                    continue;
                }

                var damage = 0;
                if (result.AttackParameters != null)
                {
                    for (var numDyeRolled = 0; numDyeRolled < result.AttackParameters.DyeNumber; numDyeRolled++)
                    {
                        damage += UnityEngine.Random.Range(1, result.AttackParameters.DyeSize + 1);
                    }
                    damage += result.AttackParameters.Bonus;
                    result.Damage = damage;
                }

                HandleModifyIncomingAttack(result);

                var sourceName = "";
                if (result.Source != null)
                {
                    sourceName = result.Source.Name;
                }
                var targetName = target.Name;

                if (result.WasShortCircuited)
                {
                    Context.UIController.TextLog.AddText(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, damage, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
                    if (result.ShortCircuitedMessage != null)
                    {
                        Context.UIController.TextLog.AddText(result.ShortCircuitedMessage);
                    }

                    if (result.ShortCircuitedFloatingText != null)
                    {
                        Context.UIController.FloatingCombatTextManager.ShowCombatText(result.ShortCircuitedFloatingText, result.ShortCircuitedFloatingTextColor, 35, MathUtil.MapToWorld(target.Position));
                    }
                    return; // do short circuit
                }
                if (result.AttackParameters != null)
                {
                    Context.UIController.TextLog.AddText(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, damage, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
                    target.Body.CurrentHealth = target.Body.CurrentHealth - damage;
                    Context.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("{0}", damage), target.IsPlayer ? Color.red : Color.magenta, 35, MathUtil.MapToWorld(target.Position));
                }

                HandleAppliedEffects(result);

                if (target.Body.CurrentHealth <= 0)
                {
                    Context.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("Dead!", damage), Color.black, 35, MathUtil.MapToWorld(target.Position));
                    Context.UIController.TextLog.AddText(string.Format("{0} has been slain!", targetName));
                    target.Name = string.Format("( Corpse ) {0}", target.Name);
                    target.Body.IsDead = true;
                    target.Body.DeadForTurns = 0;
                    Context.EntitySystem.MarkAsDead(target);
                    target.Behaviour = null;
                    var game = Context.GameStateManager.Game;
                    var level = game.CurrentLevel;

                    foreach (var pair in target.Inventory.EquippedItemBySlot)
                    {
                        var item = pair.Value;
                        foreach (var effect in item.Effects.AllValues)
                        {
                            effect.RemovePersistantVisualEffects(target);
                        }
                    }

                    if (target.BlocksPathing)
                    {
                        Context.GameStateManager.Game.CurrentLevel.Grid[target.Position].Walkable = true;
                        target.BlocksPathing = false;
                    }

                    if (target.View.ViewGameObject != null)
                    {
                        target.View.ViewGameObject.AddComponent<DeathAnimation>();
                    }

                    if (target.Inventory.HasAnyItems)
                    {
                        target.View.ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_CORPSE;
                        Context.PrototypeFactory.BuildView(target);
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
        }

        public static List<Effect> GetEntityEffectsByType(Entity entity, Predicate<Effect> filter = null)
        {
            var effectsAggregate = new List<Effect>();
            if (entity.Trigger != null && entity.Trigger.Effects.Count > 0)
            {
                effectsAggregate.AddRange(entity.Trigger.Effects.AllValues);
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                effectsAggregate.AddRange(item.Effects.AllValues);
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    effectsAggregate.AddRange(item.Effects.AllValues);
                }
            }

            if (entity.Body != null)
            {
                effectsAggregate.AddRange(entity.Body.Effects.AllValues);
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

        public static void RemoveEntityEffects(Entity entity, Effect effectThatShouldExpire)
        {
            RemoveEntityEffects(entity, new List<Effect>() { effectThatShouldExpire });
        }

        public static void RemoveEntityEffects(Entity entity, List<Effect> effectsThatShouldExpire)
        {
            // Note that we never remove attack specific effects
            if (entity.Trigger != null && entity.Trigger.Effects.Count > 0)
            {
                entity.Trigger.Effects.RemoveAll(entity, (eff) => { return effectsThatShouldExpire.Contains(eff); });
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                item.Effects.RemoveAll(entity, (eff) => { return effectsThatShouldExpire.Contains(eff); });
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    item.Effects.RemoveAll(entity, (eff) => { return effectsThatShouldExpire.Contains(eff); });
                }
            }

            if (entity.Body != null)
            {
                entity.Body.Effects.RemoveAll(entity, (eff) => { return effectsThatShouldExpire.Contains(eff); });
            }
        }

        private static void HandleAppliedEffects(EntityStateChange ctx)
        {
            if (!ctx.WasShortCircuited)
            {
                foreach (var effect in ctx.AppliedEffects)
                {
                    if(effect is AppliedEffect)
                    {
                        var appliedEffect = (AppliedEffect) effect;
                        if(appliedEffect.CanApply(ctx))
                        {
                            appliedEffect.Apply(ctx);
                        }
                        else
                        {
                            throw new NotImplementedException("Your items applied effect did not receive a target");
                        }
                    }
                    else
                    {
                        foreach (var target in ctx.Targets)
                        {
                            effect.HandleStacking(target);
                        }
                    }
                }
            }
        }

        private static void HandleModifyIncomingAttack(EntityStateChange result)
        {
            foreach (var target in result.Targets)
            {
                var itemsCopy = new List<Item>();
                itemsCopy.AddRange(target.Inventory.Items);
                itemsCopy.AddRange(target.Inventory.EquippedItemBySlot.Values);
                foreach (var item in itemsCopy)
                {
                    if (item != null)
                    {
                        foreach (var effect in item.Effects.AllValues)
                        {
                            if (effect.CanAffectIncomingAttack(result))
                            {
                                effect.AffectIncomingAttack(result);
                            }
                        }
                    }
                }
            }
        }

        internal static bool HasWeapon(Entity source)
        {
            if (source.Inventory == null)
            {
                return false;
            }
            else
            {
                var item = source.Inventory.GetItemBySlot(ItemSlot.MainHand);
                return item != null && item.AttackParameters.Count > 0;
            }
        }

        public static void PerformTriggerStepAbilityIfSteppedOn(Entity Source, Entity potentialTrigger, List<Point> points)
        {
            if (points.Contains(Source.Position))
            {
                var abilities = potentialTrigger.Trigger.Effects;
                foreach(var effect in abilities.AllValues)
                {
                    var attack = new EntityStateChange();
                    attack.Source = potentialTrigger;
                    attack.Targets.Add(Source);
                    effect.TriggerOnStep(attack);
                }
            }
        }

        public static void ConsumeItemCharges(Entity owner, Item item, int NumberConsumed = 1)
        {
            if(!item.HasUnlimitedCharges && item.HasCharges)
            {
                item.ExactNumberOfChargesRemaining = item.ExactNumberOfChargesRemaining - NumberConsumed;
                if(item.ExactNumberOfChargesRemaining < 0)
                {
                    item.ExactNumberOfChargesRemaining = 0;
                }
                if(item.DestroyWhenAllChargesAreConsumed)
                {
                    owner.Inventory.RemoveItemStack(item);
                    Context.UIController.Refresh();
                }
            }
        }
    }
}
