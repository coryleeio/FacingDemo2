using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
        private ILogSystem _logger;
        private IPrototypeFactory _prototypeFactory;
        private IGameStateSystem _gameStateSystem;
        private GameSceneCameraDriver CameraDriver;
        public GameScene(ILogSystem logger, IPrototypeFactory prototypeFactory, IGameStateSystem gameStateSystem)
        {
            _logger = logger;
            _prototypeFactory = prototypeFactory;
            _gameStateSystem = gameStateSystem;
        }

        protected override void BuildScene()
        {
            GameObject obj = new GameObject
            {
                tag = "MainCamera",
                name = "Camera"
            };
            var camera = obj.AddComponent<Camera>();
            var gameSceneCameraDriver = obj.AddComponent<GameSceneCameraDriver>();
            obj.transform.position = new Vector3(0, 0, -30);
            camera.orthographicSize = 1.5f;
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.15f, 0.15f);

            _prototypeFactory.BuildGrid(_gameStateSystem.Game.CurrentLevel);
            Token Player = null;
            foreach (var token in _gameStateSystem.Game.CurrentLevel.Tokens)
            {
                if(token.Tags.Contains(Tags.Player))
                {
                    Player = token;
                }
                token.TokenView.BuildView();
            }
            gameSceneCameraDriver.target = Player.TokenView.GameObject;
            CameraDriver = gameSceneCameraDriver;
        }

        public GameSceneCameraDriver GetCamera()
        {
            return CameraDriver;
        }
    }
}