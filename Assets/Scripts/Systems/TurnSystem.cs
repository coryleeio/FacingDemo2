namespace Gamepackage
{
    public class TurnSystem
    {
        public GameStateManager GameStateManager { get; set; }
        public TurnStateMachine TurnStateMachine { get; set; }

        public TurnSystem() {
            
        }

        public void Init()
        {
            TurnStateMachine.ChangeState(TurnStateMachine.DoNextAction);
        }

        public void Process()
        {
            TurnStateMachine.Process();
        }
    }
}
