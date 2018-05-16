using System;

namespace Gamepackage
{
    public abstract class TokenAction : IStateMachineState
    {
        private Action Complete;
        private TokenAction() {}

        public TokenAction(Action CompletedHandler)
        {

        }

        public void Enter() {}
        public void Process() { Complete(); }
        public void Exit() {}

        public virtual int TimeCost()
        {
            return 1;
        }
    }
}
