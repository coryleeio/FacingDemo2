
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

        public static void Apply(AbilityContext result)
        {
            Assert.IsNotNull(result.Source);
            Assert.IsNotNull(result.Targets);
            Assert.IsNotNull(result.OnHitAbilities);
            Assert.IsNotNull(result.AttackParameters);

            foreach (var target in result.Targets)
            {
                if (result.AttackParameters.DamageType == DamageTypes.NOT_SET)
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

                var damage = 0;
                for (var numDyeRolled = 0; numDyeRolled < result.AttackParameters.DyeNumber; numDyeRolled++)
                {
                    damage += UnityEngine.Random.Range(1, result.AttackParameters.DyeSize + 1);
                }
                damage += result.AttackParameters.Bonus;

                result.Damage = damage;

                var damageIsLethal = target.Body.CurrentHealth - damage <= 0;

                if (damageIsLethal)
                {
                    HandleDamageIsLethal(result);
                }

                var sourceName = result.Source.Name;
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
                Context.UIController.TextLog.AddText(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, damage, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
                target.Body.CurrentHealth = target.Body.CurrentHealth - damage;

                Context.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("{0}", damage), target.IsPlayer ? Color.red : Color.magenta, 35, MathUtil.MapToWorld(target.Position));

                HandleOnHit(result);

                if (target.Body.CurrentHealth <= 0)
                {
                    Context.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("Dead!", damage), Color.black, 35, MathUtil.MapToWorld(target.Position));
                    Context.UIController.TextLog.AddText(string.Format("{0} has been slain!", targetName));
                    target.Name = string.Format("( Corpse ) {0}", target.Name);
                    target.Body.IsDead = true;
                    target.Body.DeadForTurns = 0;
                    Context.EntitySystem.MarkAsDead(target);
                    target.Behaviour = null;
                    var level = Context.GameStateManager.Game.CurrentLevel;

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
                }

                if (target.IsPlayer && Context.UIController.InventoryWindow.isActiveAndEnabled)
                {
                    Context.UIController.Refresh();
                }
            }
        }

        private static void HandleOnHit(AbilityContext ctx)
        {
            if(!ctx.WasShortCircuited)
            {
                foreach(var ability in ctx.OnHitAbilities)
                {
                    Assert.IsTrue(ability.TriggeredBy == TriggerType.OnHit);
                    if(ability.CanPerform(ctx))
                    {
                        ability.Perform(ctx);
                    }
                }
            }
        }

        private static void HandleDamageIsLethal(AbilityContext result)
        {
            foreach(var target in result.Targets)
            {
                var itemsCopy = new List<Item>();
                itemsCopy.AddRange(target.Inventory.Items);
                HandleLethalDamageCallbacksForListOfItems(result, itemsCopy);

                var equipmentCopy = new List<Item>();
                equipmentCopy.AddRange(target.Inventory.EquippedItemBySlot.Values);
                HandleLethalDamageCallbacksForListOfItems(result, equipmentCopy);
            }
        }

        private static void HandleLethalDamageCallbacksForListOfItems(AbilityContext result, List<Item> itemsCopy)
        {
            foreach (var item in itemsCopy)
            {
                if(item != null)
                {
                    foreach (var ability in item.Abilities)
                    {
                        if (ability.TriggeredBy == TriggerType.OnDamageWouldKill)
                        {
                            if (ability.CanPerform(result))
                            {
                                ability.Perform(result);
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
                var ability = potentialTrigger.Trigger.Ability;
                var ctx = new AbilityContext();
                ctx.Source = potentialTrigger;
                ctx.Targets.Add(Source);
                if (potentialTrigger.Trigger.Ability.CanPerform(ctx))
                {
                    ability.Perform(ctx);
                    return true;
                }
            }
            return false;
        }
    }
}
