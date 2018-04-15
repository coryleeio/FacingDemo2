namespace Gamepackage
{
    public class MainMenuState : IStateMachineState<Root>
    {
        private MainMenuScene _mainMenuScene;
        private ILogSystem _logSystem;
        private ITokenSystem _tokenSystem;

        public MainMenuState(MainMenuScene mainMenuScene, ILogSystem logSystem, ITokenSystem tokenSystem)
        {
            _mainMenuScene = mainMenuScene;
            _logSystem = logSystem;
            _tokenSystem = tokenSystem;
        }

        public void Enter(Root owner)
        {
            _mainMenuScene.Load();
            _tokenSystem.Clear();
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
            _logSystem.Log("MainMenuState");
        }
    }
}