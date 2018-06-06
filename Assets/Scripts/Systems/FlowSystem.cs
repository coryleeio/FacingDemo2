
namespace Gamepackage
{
    public class FlowSystem
    {
        public StateMachine StateMachine { get; private set; }

        public FlowSystem()
        {
            StateMachine = new StateMachine();
        }

        public void Init()
        {
            StateMachine.ChangeState(FlowStateMachine.DoTurn);
        }

        public void Process()
        {
            StateMachine.Process();
        }
    }
}
