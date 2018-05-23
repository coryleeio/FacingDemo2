namespace Gamepackage
{
    public abstract class TokenAction
    {
        public abstract int TimeCost
        {
            get;
        }

        public bool HasStarted = false;

        public abstract bool IsComplete
        {
            get;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Process()
        {

        }
    }
}
