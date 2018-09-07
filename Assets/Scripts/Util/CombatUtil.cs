
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

        public static void ApplyAttackResult(AttackContext result)
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

                var damageIsLethal = target.Body.CurrentHealth - damage <= 0;

                if (damageIsLethal)
                {
                    HandleDamageIsLethal(result);
                }

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
                        var corpseGameObject = Context.PrototypeFactory.BuildView(target);
                        target.View.ViewGameObject = corpseGameObject;
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
            if (entity.Trigger != null && entity.Trigger.Effect != null)
            {
                effectsAggregate.Add(entity.Trigger.Effect);
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                foreach (var effect in item.Effects)
                {
                    effectsAggregate.Add(effect);
                }
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    foreach (var effect in item.Effects)
                    {
                        effectsAggregate.Add(effect);
                    }
                }
            }

            if (entity.Body != null)
            {
                foreach (var effect in entity.Body.Effects)
                {
                    effectsAggregate.Add(effect);
                }
            }
            if(filter == null)
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
            // Note that we never remove attack specific effects
            if (entity.Trigger != null && entity.Trigger.Effect != null && effectsThatShouldExpire.Contains(entity.Trigger.Effect))
            {
                entity.Trigger.Effect = null;
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                item.Effects.RemoveAll((eff) => { return effectsThatShouldExpire.Contains(eff); });
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    item.Effects.RemoveAll((eff) => { return effectsThatShouldExpire.Contains(eff); });
                }
            }

            if (entity.Body != null)
            {
                entity.Body.Effects.RemoveAll((eff) => { return effectsThatShouldExpire.Contains(eff); });
            }

            foreach (var effect in effectsThatShouldExpire)
            {
                if (effect.RemovalText != null && effect.RemovalText != "")
                {
                    Context.UIController.TextLog.AddText(effect.RemovalText);
                }
                effect.OnRemove(entity);
            }
        }

        private static void HandleAppliedEffects(AttackContext ctx)
        {
            if (!ctx.WasShortCircuited)
            {
                foreach (var ability in ctx.AppliedEffects)
                {
                    if (ability.CanTrigger(ctx))
                    {
                        ability.Trigger(ctx);
                    }
                }
            }
        }

        private static void HandleDamageIsLethal(AttackContext result)
        {
            foreach (var target in result.Targets)
            {
                var itemsCopy = new List<Item>();
                itemsCopy.AddRange(target.Inventory.Items);
                HandleLethalDamageCallbacksForListOfItems(result, itemsCopy);

                var equipmentCopy = new List<Item>();
                equipmentCopy.AddRange(target.Inventory.EquippedItemBySlot.Values);
                HandleLethalDamageCallbacksForListOfItems(result, equipmentCopy);
            }
        }

        private static void HandleLethalDamageCallbacksForListOfItems(AttackContext result, List<Item> itemsCopy)
        {
            foreach (var item in itemsCopy)
            {
                if (item != null)
                {
                    foreach (var effect in item.Effects)
                    {
                        if (effect.EffectApplicationTrigger == EffectTriggerType.OnDamageWouldKill)
                        {
                            if (effect.CanTrigger(result))
                            {
                                effect.Trigger(result);
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

        public static bool PerformTriggerStepAbilityIfSteppedOn(Entity Source, Entity potentialTrigger, List<Point> points)
        {
            if (points.Contains(Source.Position))
            {
                var ability = potentialTrigger.Trigger.Effect;
                var attack = new AttackContext();
                attack.Source = potentialTrigger;
                attack.Targets.Add(Source);
                if (potentialTrigger.Trigger.Effect.CanTrigger(attack))
                {
                    ability.Trigger(attack);
                    return true;
                }
            }
            return false;
        }
    }
}
