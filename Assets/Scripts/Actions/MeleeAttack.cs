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

            AttackParameters attack = null;
            if(CombatUtil.HasWeapon(Source))
            {
                var weapon = Source.Inventory.GetItemBySlot(ItemSlot.Weapon);
                attack = MathUtil.ChooseRandomElement<AttackParameters>(weapon.AttackParameters);
            }
            else
            {
                attack = MathUtil.ChooseRandomElement<AttackParameters>(Source.Body.Attacks);
            }

            foreach(var target in Targets)
            {
                CombatUtil.DealDamage(Source, target, attack);
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
