namespace Gamepackage
{
    public class StateMachine
    {
        public StateMachine() {}
        public IStateMachineState CurrentState;
        public IStateMachineState PreviousState;
        public IStateMachineState GlobalState;

        public void Process()
        {
            if(GlobalState != null)
            {
                GlobalState.Process();
            }
            if (CurrentState != null)
            {
                CurrentState.Process();
            }
        }

        public void ChangeState(IStateMachineState state)
        {
            if(CurrentState != null)
            {
                CurrentState.Exit();
                PreviousState = CurrentState;
            }
            CurrentState = state;
            CurrentState.Enter();
        }

        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }
    }
}