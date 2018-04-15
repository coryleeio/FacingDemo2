using UnityEngine;

namespace Gamepackage
{
    public class LoadingScene : Scene
    {
        private ILogSystem _logger;
        public LoadingScene(ILogSystem logger)
        {
            _logger = logger;
        }

        protected override void BuildScene()
        {
            GameObject obj = new GameObject();
            obj.name = "Camera";
            obj.AddComponent<Camera>();

            var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            plane.transform.position = new Vector3(0, 0, 10);
            plane.transform.localScale = new Vector3(50, 50, 50);
            plane.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }
    }
}