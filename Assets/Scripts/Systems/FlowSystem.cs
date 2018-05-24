using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class FlowSystem
    {
        public ApplicationContext Context { get; set; }
        public StateMachine StateMachine { get; private set; }

        public FlowSystem()
        {
            StateMachine = new StateMachine();
        }

        public void Init()
        {
            StateMachine.ChangeState(Context.DoTurn);
        }

        public void Process()
        {
            StateMachine.Process();
        }
    }
}
