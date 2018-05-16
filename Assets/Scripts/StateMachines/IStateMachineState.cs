namespace Gamepackage
{
    public interface IStateMachineState
    {
        void Enter();
        void Process();
        void Exit();
    }
}