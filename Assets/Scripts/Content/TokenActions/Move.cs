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
        }

        public override bool ShouldEnd
        {
            get
            {
                return Token.Position == TargetLocation;
            }
        }
    }
}
