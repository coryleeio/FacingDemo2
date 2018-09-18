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

            foreach(var target in Targets)
            {
                var attack = new EntityStateChange();
                attack.Source = Source;
                attack.Targets.Add(target);
                List<Effect> onHitEffects = new List<Effect>();

                if (CombatUtil.HasWeapon(Source))
                {
                    var weapon = Source.Inventory.GetItemBySlot(ItemSlot.MainHand);
                    attack.AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(weapon.AttackParameters);
                    onHitEffects.AddRange(weapon.Effects.Values);
                    onHitEffects.AddRange(attack.AttackParameters.AttackSpecificEffects.Values);
                }
                else
                {
                    attack.AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(Source.Body.Attacks);
                    onHitEffects.AddRange(Source.Body.Effects.Values);
                    onHitEffects.AddRange(attack.AttackParameters.AttackSpecificEffects.Values);
                }
                attack.AppliedEffects.AddRange(onHitEffects);
                CombatUtil.ApplyEntityStateChange(attack);
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
