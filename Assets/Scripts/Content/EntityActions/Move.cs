using Newtonsoft.Json;

namespace Gamepackage
{
    public class Move : EntityAction
    {
        public Point TargetLocation;

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
            ServiceLocator.MovementSystem.MoveTo(Entity, TargetLocation);
        }

        public override void Exit()
        {
            base.Exit();
            Entity.Motor.HasMovedSinceLastTriggerCheck = true;
        }

        public override bool IsEndable
        {
            get
            {
                return Entity.Position == TargetLocation;
            }
        }

        public override bool IsAMovementAction
        {
            get
            {
                return true;
            }
        }

        public override bool IsStartable
        {
            get
            {
                return ServiceLocator.GameStateManager.Game.CurrentLevel.Grid[TargetLocation].Walkable && TargetLocation.IsAdjacentTo(Entity.Position);
            }
        }
    }
}
