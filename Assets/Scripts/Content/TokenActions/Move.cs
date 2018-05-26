using Newtonsoft.Json;

namespace Gamepackage
{
    public class Move : TokenAction
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
            Context.MovementSystem.MoveTo(Token, TargetLocation);
        }

        public override void Exit()
        {
            base.Exit();
            Token.HasMovedSinceLastTriggerCheck = true;
        }

        public override bool IsEndable
        {
            get
            {
                return Token.Position == TargetLocation;
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
                return true;
            }
        }
    }
}
