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
            List<Effect> onHitEffects = new List<Effect>();
            if(CombatUtil.HasWeapon(Source))
            {
                var weapon = Source.Inventory.GetItemBySlot(ItemSlot.MainHand);
                attackParameters = MathUtil.ChooseRandomElement<AttackParameters>(weapon.AttackParameters);
                onHitEffects.AddRange(weapon.Effects.FindAll((possibleAbilities) => { return possibleAbilities.EffectApplicationTrigger == EffectTriggerType.OnHit; }));
                onHitEffects.AddRange(attackParameters.AttackSpecificEffects);
            }
            else
            {
                attackParameters = MathUtil.ChooseRandomElement<AttackParameters>(Source.Body.Attacks);
                onHitEffects.AddRange(Source.Body.Effects.FindAll((possibleAbilities) => { return possibleAbilities.EffectApplicationTrigger == EffectTriggerType.OnHit; }));
                onHitEffects.AddRange(attackParameters.AttackSpecificEffects);
            }

            foreach(var target in Targets)
            {
                var attack = new AttackContext();
                attack.Source = Source;
                attack.Targets.Add(target);
                attack.AttackParameters = attackParameters;
                attack.AppliedEffects.AddRange(onHitEffects);
                CombatUtil.ApplyAttackResult(attack);
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
