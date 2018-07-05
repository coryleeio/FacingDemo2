using Newtonsoft.Json;

namespace Gamepackage
{
    public class EndTurn : TargetableAction
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

        public override void Enter()
        {
            base.Enter();
            Source.Behaviour.IsDoneThisTurn = true;
        }
    }
}
