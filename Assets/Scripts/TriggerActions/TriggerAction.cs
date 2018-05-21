using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class TriggerAction
    {
        public virtual void Enter(Dictionary<string, string> parameters)
        {

        }

        public virtual void Exit(Dictionary<string, string> parameters)
        {

        }

        public virtual void Process(Dictionary<string, string> parameters)
        {

        }
    }
}
