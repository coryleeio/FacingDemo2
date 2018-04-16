namespace Gamepackage
{
    public interface IStateMachineState<OwnerType> : IHandleMessage
    {
        void Enter(OwnerType owner);

        void Process(OwnerType owner);

        void Exit(OwnerType owner);
    }
}