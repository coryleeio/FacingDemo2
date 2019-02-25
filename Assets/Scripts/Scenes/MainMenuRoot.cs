using UnityEngine;

namespace Gamepackage
{
    public class MainMenuRoot : MonoBehaviour
    {
        public void Start()
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

            var MainMenuPrefab = Resources.Load<GameObject>("UI/MainMenu");
            var MainMenu = GameObject.Instantiate(MainMenuPrefab);
            MainMenu.name = "MainMenu";
            var mainMenuInstance = MainMenu.GetComponent<MainMenu>();
        }

        public void Update()
        {
            
        }

        public void OnDisable()
        {
            Context.PathFinder.Cleanup();
        }
    }
}
