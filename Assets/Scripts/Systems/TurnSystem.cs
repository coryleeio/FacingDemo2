namespace Gamepackage
{
    public class TurnSystem : ITurnSystem
    {
        public TurnStateMachine TurnStateMachine { get; set; }
        public AdvancingTime AdvancingTime { get; set; }

        public void Init()
        {
            TurnStateMachine.ChangeState(AdvancingTime);
        }

        public void Process()
        {
            TurnStateMachine.Process();
        }
    }
}
