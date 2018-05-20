namespace Gamepackage
{
    public class DoTriggers : IStateMachineState
    {
        public Token NextTrigger { get; set; }
        public GameStateManager GameStateManager { get; set; }

        public void Enter()
        {
            NextTrigger = null;
        }

        public void Process()
        {
            if(NextTrigger == null)
            {

            }
        }

        public void Exit()
        {

        }
    }
}
