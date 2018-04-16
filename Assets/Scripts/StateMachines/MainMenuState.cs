namespace Gamepackage
{
    public class MainMenuState : IStateMachineState<Root>
    {
        private MainMenuScene _mainMenuScene;
        private ILogSystem _logSystem;
        private IGameStateSystem _gameStateSystem;

        public MainMenuState(MainMenuScene mainMenuScene, ILogSystem logSystem, IGameStateSystem gameStateSystem)
        {
            _mainMenuScene = mainMenuScene;
            _logSystem = logSystem;
            _gameStateSystem = gameStateSystem;
        }

        public void Enter(Root owner)
        {
            _gameStateSystem.NewGame();
            _mainMenuScene.Load();
        }

        public void Exit(Root owner)
        {
            _mainMenuScene.Unload();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
        }
    }
}