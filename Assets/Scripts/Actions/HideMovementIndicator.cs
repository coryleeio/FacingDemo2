namespace Gamepackage
{
    public class HideMovementIndicator : Action
    {
        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        public override void Enter()
        {
            base.Enter();
            Context.PlayerController.ShowMovementIndicator(false);
        }

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }
    }
}
