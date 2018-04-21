namespace Gamepackage
{
    public class MainMenuState : IStateMachineState<Root>
    {
        private MainMenuScene _mainMenuScene;
        private ILogSystem _logSystem;
        private IGameStateSystem _gameStateSystem;
        private ITokenSystem _tokenSystem;

        public MainMenuState(MainMenuScene mainMenuScene, ILogSystem logSystem, IGameStateSystem gameStateSystem, ITokenSystem tokenSystem)
        {
            _mainMenuScene = mainMenuScene;
            _logSystem = logSystem;
            _gameStateSystem = gameStateSystem;
            _tokenSystem = tokenSystem;
        }

        public void Enter(Root owner)
        {
            _gameStateSystem.Clear();
            _tokenSystem.Clear();
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