using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class MeleeAttack : TargetableAction
    {
        private float TimeStart;
        private float Duration = 0.5f;

        public int TargetId;

        public override int TimeCost
        {
            get
            {
                return 250;
            }
        }

        public override void Enter()
        {
            base.Enter();
            TimeStart = UnityEngine.Time.deltaTime;
        }

        public override void Exit()
        {
            base.Exit();
            if(Source.Body.Attacks.Count == 0)
            {
                throw new NotImplementedException("You don't have any attacks, but are trying to attack anyway?");
            }

            AttackParameters attackParameters = null;
            List<Ability> onHitAbilities = new List<Ability>();
            if(CombatUtil.HasWeapon(Source))
            {
                var weapon = Source.Inventory.GetItemBySlot(ItemSlot.MainHand);
                attackParameters = MathUtil.ChooseRandomElement<AttackParameters>(weapon.AttackParameters);
                onHitAbilities.AddRange(weapon.Abilities.FindAll((possibleAbilities) => { return possibleAbilities.TriggeredBy == TriggerType.OnHit; }));
            }
            else
            {
                attackParameters = MathUtil.ChooseRandomElement<AttackParameters>(Source.Body.Attacks);
                onHitAbilities.AddRange(Source.Body.Abilities.FindAll((possibleAbilities) => { return possibleAbilities.TriggeredBy == TriggerType.OnHit; }));
            }

            foreach(var target in Targets)
            {
                var abilityContext = new AbilityContext();
                abilityContext.Source = Source;
                abilityContext.Targets.Add(target);
                abilityContext.AttackParameters = attackParameters;
                abilityContext.OnHitAbilities.AddRange(onHitAbilities);
                CombatUtil.Apply(abilityContext);
            }
        }

        public override bool IsEndable
        {
            get
            {
                return TimeStart + Duration >= UnityEngine.Time.deltaTime;
            }
        }
    }
}
