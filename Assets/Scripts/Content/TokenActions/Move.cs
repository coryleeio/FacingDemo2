namespace Gamepackage
{
    public class Move : TokenAction
    {
        public Token Token;
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
            Token.NeedToCheckIfMovementTriggeredTriggers = true;
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
