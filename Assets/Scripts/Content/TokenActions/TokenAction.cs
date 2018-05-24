namespace Gamepackage
{
    public abstract class TokenAction : IHasApplicationContext
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

        protected ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
        }
    }
}
