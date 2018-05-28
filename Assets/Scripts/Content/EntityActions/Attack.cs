using Newtonsoft.Json;

namespace Gamepackage
{
    public class Attack : EntityAction
    {
        private float TimeStart;
        private float Duration = 0.5f;

        public int TargetEntityId;
        [JsonIgnore]
        public Entity TargetEntity
        {
            get
            {
                return Context.EntitySystem.GetEntityById(TargetEntityId);
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
            Context.CombatSystem.DealDamage(Entity, TargetEntity, 1);
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
                return Entity.Position.IsOrthogonalTo(TargetEntity.Position);
            }
        }
    }
}
