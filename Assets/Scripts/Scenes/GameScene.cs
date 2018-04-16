using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
        private ILogSystem _logger;
        private IPrototypeFactory _prototypeFactory;
        public GameScene(ILogSystem logger, IPrototypeFactory prototypeFactory)
        {
            _logger = logger;
            _prototypeFactory = prototypeFactory;
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
        }
    }
}