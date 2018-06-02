using Newtonsoft.Json;

namespace Gamepackage
{
    public class MeleeAttack : EntityAction
    {
        private float TimeStart;
        private float Duration = 0.5f;

        public int TargetId;

        [JsonIgnore]
        private Entity _target;
        [JsonIgnore]
        public Entity Target
        {
            get
            {
                if (_target == null)
                {
                    _target = Context.EntitySystem.GetEntityById(TargetId);
                }
                return _target;
            }
        }

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
            Context.CombatSystem.DealDamage(Entity, Target, 1);
        }

        public override bool IsEndable
        {
            get
            {
                return TimeStart + Duration >= UnityEngine.Time.deltaTime;
            }
        }

        public override bool IsAMovementAction
        {
            get
            {
                return false;
            }
        }

        public override bool IsStartable
        {
            get
            {
                if(Entity == null && Target == null)
                {
                    return false;
                }
                return Context.CombatSystem.CanMelee(Entity, Target);
            }
        }
    }
}
