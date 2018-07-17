using UnityEngine;

namespace Gamepackage
{
    public class MainMenuScene : Scene
    {
        public Logger Logger { get; set; }

        public MainMenuScene()
        {

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

            var EventSystemPrefab = Resources.Load<GameObject>("UI/EventSystem");
            var eventSYstem = GameObject.Instantiate(EventSystemPrefab);
            eventSYstem.name = "EventSystem";
        }
    }
}