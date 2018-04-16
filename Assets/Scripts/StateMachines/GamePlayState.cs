namespace Gamepackage
{
    public class GamePlayState : IStateMachineState<Root>
    {
        private GameScene _gamePlayScene;
        private ILogSystem _logSystem;
        private IVisibilitySystem _visibilitySystem;
        public GamePlayState(GameScene gamePlayScene, ILogSystem logSystem, IVisibilitySystem visibilitySystem)
        {
            _gamePlayScene = gamePlayScene;
            _logSystem = logSystem;
            _visibilitySystem = visibilitySystem;
        }

        public void Enter(Root owner)
        {
            _gamePlayScene.Load();
            _visibilitySystem.Init();
        }

        public void Exit(Root owner)
        {
            _gamePlayScene.Unload();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
        }
    }
}
