namespace Gamepackage
{
    public class GamePlayState : IStateMachineState<Root>
    {
        private GameScene _gamePlayScene;
        private ILogSystem _logSystem;
        public GamePlayState(GameScene gamePlayScene, ILogSystem logSystem)
        {
            _gamePlayScene = gamePlayScene;
            _logSystem = logSystem;
        }

        public void Enter(Root owner)
        {
            _gamePlayScene.Load();
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
            _logSystem.Log("GamePlayState");
        }
    }
}
