using Newtonsoft.Json;

namespace Gamepackage
{
    public class MeleeAttack : Action
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
            var attackParameters = MathUtil.ChooseRandomElement<AttackParameters>(Source.Body.Attacks);
            foreach(var target in Targets)
            {
                ServiceLocator.CombatSystem.DealDamage(Source, target, attackParameters);
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
