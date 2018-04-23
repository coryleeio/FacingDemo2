using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
        private ILogSystem _logger;
        private IPrototypeFactory _prototypeFactory;
        private IGameStateSystem _gameStateSystem;
        public GameScene(ILogSystem logger, IPrototypeFactory prototypeFactory, IGameStateSystem gameStateSystem)
        {
            _logger = logger;
            _prototypeFactory = prototypeFactory;
            _gameStateSystem = gameStateSystem;
        }

        protected override void BuildScene()
        {
            GameObject obj = new GameObject();
            obj.tag = "MainCamera";
            obj.name = "Camera";
            var camera = obj.AddComponent<Camera>();
            obj.transform.position = new Vector3(0, 0, -30);
            camera.orthographicSize = 1.5f;
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.15f, 0.15f);

            _prototypeFactory.BuildGrid(_gameStateSystem.Game.CurrentLevel);
            foreach (var token in _gameStateSystem.Game.CurrentLevel.Tokens)
            {
                token.View.BuildView();
            }
        }
    }
}