using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class ASyncAction
    {
        public bool HasStarted = false;
        public bool Completed = false;
        public bool IsRunning = false;

        [JsonIgnore]
        public Entity Entity { get; set; }

        [JsonIgnore]
        public abstract bool IsEndable
        {
            get;
        }

        public abstract bool IsStartable
        {
            get;
        }

        public virtual void FailToStart()
        {

        }

        public virtual void Enter()
        {
            HasStarted = true;
            IsRunning = true;
            Completed = false;
        }

        public virtual void Exit()
        {
            Completed = true;
            IsRunning = false;
        }

        public virtual void Reset()
        {
            HasStarted = false;
            IsRunning = false;
            Completed = false;
        }

        public virtual void Process()
        {

        }

        public virtual void Do()
        {
            if (!HasStarted)
            {
                if(!IsStartable)
                {
                    FailToStart();
                    return;
                }
                Enter();
            }
            Process();
            if (IsEndable)
            {
                Exit();
            }
        }

        public void Rewire(Entity entity)
        {
            Entity = entity;
        }
    }
}
