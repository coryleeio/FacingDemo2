
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class CombatSystem
    {
        public CombatSystem() {}

        public bool CanMelee(Entity a, Entity b)
        {
            if(a == null || b == null)
            {
                return false;
            }
            return a.Position.IsAdjacentTo(b.Position) && a.Position.IsOrthogonalTo(b.Position);
        }

        public void DealDamage(Entity source, Entity target, AttackParameters attackParameters)
        {
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

            target.Body.CurrentHealth = target.Body.CurrentHealth - damage;
            var sourceName = source.Name;
            var targetName = target.Name;
            ServiceLocator.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("{0}", damage), target.IsPlayer ? Color.red : Color.magenta, 35, MathUtil.MapToWorld(target.Position));

            ServiceLocator.UIController.TextLog.AddText(string.Format(attackParameters.AttackMessage, sourceName, targetName, damage, DamageTypeToDisplayString(attackParameters.DamageType)));

            if (target.Body.CurrentHealth <= 0)
            {
                ServiceLocator.UIController.FloatingCombatTextManager.ShowCombatText(string.Format("Dead!", damage), Color.black, 35, MathUtil.MapToWorld(target.Position));
                ServiceLocator.UIController.TextLog.AddText(string.Format("{0} has been slain!", targetName));
                target.Body.IsDead = true;
                var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
                if (!target.IsPlayer)
                {
                    ServiceLocator.EntitySystem.Deregister(target, level);
                }

                if (target.BlocksPathing)
                {
                    ServiceLocator.GameStateManager.Game.CurrentLevel.Grid[target.Position].Walkable = true;
                }

                if (target.View.ViewGameObject != null)
                {
                    target.View.ViewGameObject.AddComponent<DeathAnimation>();
                }
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
    }
}
