using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Trigger : IHasApplicationContext
    {
        public bool HasStarted = false;

        public abstract List<Point> Offsets
        {
            get;
        }

        public abstract bool IsComplete
        {
            get;
        }

        public virtual void Enter()
        {

        }

        public virtual void Process()
        {

        }

        public virtual void Exit()
        {

        }

        protected ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
        }
    }
}
