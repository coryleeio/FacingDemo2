using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class EntityAction : ASyncAction
    {
        public abstract int TimeCost
        {
            get;
        }

        public int EntityId;
        [JsonIgnore]
        public Entity Entity
        {
            get
            {
                return Context.EntitySystem.GetEntityById(EntityId);
            }
        }

        [JsonIgnore]
        public Game Game
        {
            get
            {
                return Context.GameStateManager.Game;
            }
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            Entity.TimeAccrued += TimeCost;
            if(Entity.ActionQueue.Contains(this))
            {
                Entity.ActionQueue.Dequeue();
            }
        }

        public abstract bool IsAMovementAction
        {
            get;
        }

        public override void FailToStart()
        {
            base.FailToStart();
            Entity.ActionQueue.Clear();
        }
    }
}
