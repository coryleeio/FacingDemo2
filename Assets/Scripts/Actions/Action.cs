using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public abstract class Action
    {
        [JsonIgnore]
        public abstract Entity Source
        {
            get; set;
        }

        public bool Started = false;
        public bool Done = false;

        [JsonIgnore]
        public abstract bool IsEndable
        {
            get;
        }

        public virtual bool IsValid()
        {
            return true;
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
