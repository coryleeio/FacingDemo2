using System.Collections.Generic;

namespace Gamepackage
{
    public class Player : BehaviourImpl
    {
        protected override void SetActionsForThisTurn()
        {
            throw new NotImplementedException("Player should not try to calculate next move, it should be filled in by player controller.");
        }

        public override bool IsPlayer
        {
            get
            {
                return true;
            }
        }
    }
}
