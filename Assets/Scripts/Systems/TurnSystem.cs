namespace Gamepackage
{
    public class TurnSystem
    {
        public GameStateManager GameStateManager { get; set; }
        public StateMachine StateMachine { get; set; }
        public DoNextAction DoNextAction { get; set; }

        public void Init()
        {
            StateMachine.ChangeState(DoNextAction);
        }

        public void Process()
        {
            StateMachine.Process();
        }
    }
}
