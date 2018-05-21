namespace Gamepackage
{
    public class Move : TokenAction
    {
        public Token Token;
        public Point TargetLocation;
        public MovementSystem MovementSystem;

        public Move(MovementSystem movementSystem)
        {
            MovementSystem = movementSystem;
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
            MovementSystem.MoveTo(Token, TargetLocation);
        }
        public override void Exit()
        {
            base.Exit();
            Token.CanTriggerNow = true;
        }

        public override bool IsComplete
        {
            get
            {
                return Token.Position == TargetLocation;
            }
        }
    }
}
