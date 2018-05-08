namespace Gamepackage
{
    public class MainMenuState : IStateMachineState<Root>
    {
        public MainMenuScene MainMenuScene { get; set; }
        public ILogSystem LogSystem { get; set; }
        public IGameStateSystem GameStateSystem { get; set; }
        public ITokenSystem TokenSystem { get; set; }

        public MainMenuState()
        {
        }

        public void Enter(Root owner)
        {
            GameStateSystem.Clear();
            TokenSystem.Clear();
            MainMenuScene.Load();
        }

        public void Exit(Root owner)
        {
            MainMenuScene.Unload();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
        }
    }
}