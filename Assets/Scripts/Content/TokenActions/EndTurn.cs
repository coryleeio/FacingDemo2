namespace Gamepackage
{
    public class EndTurn : TokenAction
    {
        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        public override bool IsEndable
        {
            get
            {
                return true;
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
                return true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Token.IsDoneThisTurn = true;
            if (Token.IsPlayer)
            {
                // If the token who ended their turn is the player we need to push the time accrued by
                // our actions to them, and change to the NPC turn.
                foreach (var tokenToPushTime in Game.CurrentLevel.Tokens)
                {
                    if (!tokenToPushTime.IsPlayer)
                    {
                        tokenToPushTime.TimeAccrued = Token.TimeAccrued;
                    }
                }
                Game.IsPlayerTurn = !Game.IsPlayerTurn;
            }

        }
    }
}
