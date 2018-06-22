using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Action : IStateMachineState
    {
        [JsonIgnore]
        public Entity Source;

        [JsonIgnore]
        public List<Entity> Targets = new List<Entity>(0);

        public abstract int TimeCost
        {
            get;
        }

        public bool Started = false;
        public bool Done = false;

        [JsonIgnore]
        public abstract bool IsEndable
        {
            get;
        }

        public virtual void Enter()
        {
            Started = true;
            Done = false;
        }

        public virtual void Exit()
        {
            Done = true;
        }

        public virtual void Process()
        {

        }

        public virtual void Reset()
        {
            Started = false;
            Done = false;
            Targets.Clear();
        }

        public virtual void Do()
        {
            if (!Started)
            {
                Enter();
            }
            Process();
            if (IsEndable)
            {
                Exit();
            }
        }
    }
}
