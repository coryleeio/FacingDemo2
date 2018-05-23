using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class FlowSystem
    {
        public GameStateManager GameStateManager { get; set; }
        public StateMachine StateMachine { get; set; }
        private Token NextActor;
        private TokenAction StartedAction;
        public TinyIoCContainer Container { get; set; }
        public DoTurn DoTurn { get; set; }

        public void Init()
        {
            StateMachine.ChangeState(DoTurn);
        }

        public void Process()
        {
            StateMachine.Process();
        }
    }
}
