namespace Gamepackage
{
    public abstract class StateMachine<TOwner> : IHandleMessage
    {
        public StateMachine(TOwner owner)
        {
            this.Owner = owner;
        }

        public TOwner Owner;
        public IStateMachineState<TOwner> CurrentState;
        public IStateMachineState<TOwner> PreviousState;
        public IStateMachineState<TOwner> GlobalState;

        public void Process()
        {
            if(GlobalState != null)
            {
                GlobalState.Process(Owner);
            }
            if (CurrentState != null)
            {
                CurrentState.Process(Owner);
            }
        }

        public void ChangeState(IStateMachineState<TOwner> state)
        {
            if(CurrentState != null)
            {
                CurrentState.Exit(Owner);
                PreviousState = CurrentState;
            }
            CurrentState = state;
            CurrentState.Enter(Owner);
        }

        public void RevertToPreviousState()
        {
            ChangeState(PreviousState);
        }

        public void HandleMessage(Message messageToHandle)
        {
            if(GlobalState != null)
            {
                GlobalState.HandleMessage(messageToHandle);
            }
            if (CurrentState != null)
            {
                CurrentState.HandleMessage(messageToHandle);
            }
        }
    }
}