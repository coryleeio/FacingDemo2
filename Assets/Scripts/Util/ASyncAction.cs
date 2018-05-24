using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class ASyncAction : IStateMachineState
    {
        public bool HasStarted = false;
        public bool Completed = false;
        public bool IsRunning = false;

        [JsonIgnore]
        public abstract bool ShouldEnd
        {
            get;
        }

        public virtual void Enter()
        {
            HasStarted = true;
            IsRunning = true;
        }

        public virtual void Exit()
        {
            Completed = true;
            IsRunning = false;
        }

        public virtual void Process()
        {

        }

        [JsonIgnore]
        protected ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
        }


        public virtual void Do()
        {
            if (!HasStarted)
            {
                Enter();
            }
            Process();
            if (ShouldEnd)
            {
                Exit();
            }
        }
    }
}
