using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class EntityAction : ASyncAction
    {
        // Should this action be executed immediately on the current turn(Knockbacks etc)
        public bool IsImmediate = false;

        public abstract int TimeCost
        {
            get;
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
            Entity.Behaviour.TimeAccrued += TimeCost;
            if(Entity.Behaviour.ActionList.Contains(this))
            {
                Entity.Behaviour.ActionList.Remove(this);
            }
        }

        public abstract bool IsAMovementAction
        {
            get;
        }

        public override void FailToStart()
        {
            base.FailToStart();
            Entity.Behaviour.ActionList.Clear();
        }
    }
}
