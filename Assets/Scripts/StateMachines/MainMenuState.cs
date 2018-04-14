namespace Gamepackage
{
    public class MainMenuState : IStateMachineState<Root>
    {
        private MainMenuScene _mainMenuScene;
        private ILogSystem _logSystem;
        public MainMenuState(MainMenuScene mainMenuScene, ILogSystem logSystem)
        {
            _mainMenuScene = mainMenuScene;
            _logSystem = logSystem;
        }

        public void Enter(Root owner)
        {
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
            _logSystem.Log("MainMenuState");
        }
    }
}