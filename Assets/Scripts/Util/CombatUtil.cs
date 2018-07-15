
using System;
using System.Collections.Generic;
using UnityEngine;

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

        public class AttackResult
        {
            public AttackParameters Parameters;
            public int damage;
            public bool WasShortCircuited = false;
            public string ShortCircuitedMessage = null;
            public string ShortCircuitedFloatingText = null;
            public Color ShortCircuitedFloatingTextColor = Color.green;
        }

        public static void DealDamage(Entity source, Entity target, AttackParameters attackParameters)
        {
            var result = new AttackResult()
            {
                Parameters = attackParameters
            };

            if (attackParameters.DamageType == DamageTypes.NOT_SET)
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
            for (var numDyeRolled = 0; numDyeRolled < attackParameters.DyeNumber; numDyeRolled++)
            {
                damage += UnityEngine.Random.Range(1, attackParameters.DyeSize + 1);
            }
            damage += attackParameters.Bonus;

            result.damage = damage;

            var rawDamageIsLethal = target.Body.CurrentHealth - damage <= 0;

            if(rawDamageIsLethal)
            {
                HandleRawDamageIsLethal(target, result);
            }
            var sourceName = source.Name;
            var targetName = target.Name;

            
            if (result.WasShortCircuited)
            {
                Context.UIController.TextLog.AddText(string.Format(attackParameters.AttackMessage, sourceName, targetName, damage, DamageTypeToDisplayString(attackParameters.DamageType)));
                if (result.ShortCircuitedMessage != null)
                {
                    Context.UIController.TextLog.AddText(result.ShortCircuitedMessage);
                }
                
                if(result.ShortCircuitedFloatingText != null)
                {
                    Context.UIController.FloatingCombatTextManager.ShowCombatText(result.ShortCircuitedFloatingText, result.ShortCircuitedFloatingTextColor, 35, MathUtil.MapToWorld(target.Position));
                }
                return; // do short circuit
            }
            Context.UIController.TextLog.AddText(string.Format(attackParameters.AttackMessage, sourceName, targetName, damage, DamageTypeToDisplayString(attackParameters.DamageType)));
            target.Body.CurrentHealth = target.Body.CurrentHealth - damage;

            Context.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("{0}", damage), target.IsPlayer ? Color.red : Color.magenta, 35, MathUtil.MapToWorld(target.Position));


            if (target.Body.CurrentHealth <= 0)
            {
                Context.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("Dead!", damage), Color.black, 35, MathUtil.MapToWorld(target.Position));
                Context.UIController.TextLog.AddText(string.Format("{0} has been slain!", targetName));
                target.Body.IsDead = true;
                var level = Context.GameStateManager.Game.CurrentLevel;
                if (!target.IsPlayer)
                {
                    Context.EntitySystem.Deregister(target, level);
                }

                if (target.BlocksPathing)
                {
                    Context.GameStateManager.Game.CurrentLevel.Grid[target.Position].Walkable = true;
                }

                if (target.View.ViewGameObject != null)
                {
                    target.View.ViewGameObject.AddComponent<DeathAnimation>();
                }
            }
        }

        private static void HandleRawDamageIsLethal(Entity target, AttackResult result)
        {
            var ctx = new DamageWouldKillContext
            {
                Target = target,
                attackResult = result
            };

            var itemsCopy = new List<Item>();
            itemsCopy.AddRange(target.Inventory.Items);
            HandleLethalDamageCallbacksForListOfItems(ctx, itemsCopy);

            var equipmentCopy = new List<Item>();
            equipmentCopy.AddRange(target.Inventory.EquippedItemBySlot.Values);
            HandleLethalDamageCallbacksForListOfItems(ctx, equipmentCopy);
        }

        private static void HandleLethalDamageCallbacksForListOfItems(DamageWouldKillContext ctx, List<Item> itemsCopy)
        {
            foreach (var item in itemsCopy)
            {
                if(item != null)
                {
                    foreach (var ability in item.Abilities)
                    {
                        if (ability.TriggeredBy == TriggerType.OnDamageWouldKill)
                        {
                            if (ability.CanPerform(ctx))
                            {
                                ability.Perform(ctx);
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

        private static string DamageTypeToDisplayString(DamageTypes damageType)
        {
            if (damageType == DamageTypes.FIRE)
            {
                return "fire";
            }
            else if (damageType == DamageTypes.COLD)
            {
                return "cold";
            }
            else if (damageType == DamageTypes.LIGHTNING)
            {
                return "lightning";
            }
            else if (damageType == DamageTypes.SLASHING)
            {
                return "slashing";
            }
            else if (damageType == DamageTypes.BLUDGEONING)
            {
                return "bludgeoning";
            }
            else if (damageType == DamageTypes.PIERCING)
            {
                return "piercing";
            }
            else if (damageType == DamageTypes.ARCANE)
            {
                return "arcane";
            }
            else if (damageType == DamageTypes.NEGATIVE)
            {
                return "negative";
            }
            else if (damageType == DamageTypes.HOLY)
            {
                return "holy";
            }
            else
            {
                throw new NotImplementedException("Need to add a display string for this damage type: " + damageType);
            }
        }

        public static bool PerformTriggerStepAbilityIfSteppedOn(Entity Source, Entity potentialTrigger, List<Point> points)
        {
            if (points.Contains(Source.Position))
            {
                var ability = potentialTrigger.Trigger.Ability;
                TriggerStepContext cxt = new TriggerStepContext();
                cxt.Source = potentialTrigger;
                cxt.Targets.Add(Source);
                if (potentialTrigger.Trigger.Ability.CanPerform(cxt))
                {
                    ability.Perform(cxt);
                    return true;
                }
            }
            return false;
        }
    }
}
