using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class FlowSystem
    {
        public ApplicationContext ApplicationContext { get; set; }
        public StateMachine StateMachine { get; private set; }

        public FlowSystem()
        {
            StateMachine = new StateMachine();
        }

        public void Init()
        {
            StateMachine.ChangeState(ApplicationContext.DoTurn);
        }

        public void Process()
        {
            StateMachine.Process();
        }
    }
}
