using UnityEngine;

namespace Gamepackage
{
    public class MainMenuScene : Scene
    {
        private ILogSystem _logger;
        public MainMenuScene(ILogSystem logger)
        {
            _logger = logger;
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
            camera.backgroundColor = Color.black;
        }
    }
}