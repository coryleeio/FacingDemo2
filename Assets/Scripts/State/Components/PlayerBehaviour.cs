namespace Gamepackage
{
    public class PlayerBehaviour : Behaviour
    {
        public override bool IsPlayer
        {
            get
            {
                return true;
            }
        }

        public override void FigureOutNextAction()
        {
            // player controller should fill out next action
            throw new System.NotImplementedException();
        }
    }
}
