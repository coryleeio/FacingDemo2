namespace Gamepackage
{
    public abstract class TokenAction : IStateMachineState
    {
        public bool HasStarted = false;

        public abstract int TimeCost
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

        public abstract bool IsComplete
        {
             get;
        }
    }
}
