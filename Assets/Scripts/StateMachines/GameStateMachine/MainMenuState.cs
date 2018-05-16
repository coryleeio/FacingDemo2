namespace Gamepackage
{
    public class MainMenuState : IStateMachineState
    {
        public MainMenuScene MainMenuScene { get; set; }
        public Logger LogSystem { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public TokenSystem TokenSystem { get; set; }

        public MainMenuState()
        {
        }

        public void Enter()
        {
            GameStateManager.Clear();
            TokenSystem.Clear();
            MainMenuScene.Load();
        }

        public void Process()
        {
        }

        public void Exit()
        {
            MainMenuScene.Unload();
        }
    }
}