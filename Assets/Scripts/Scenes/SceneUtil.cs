using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public static class SceneUtil
    {
        public static EventSystem FindOrCreateEventSystem()
        {
            return FindOrCreatePrefabByName<EventSystem>("Prefabs/UI/EventSystem");
        }

        public static Camera FindOrCreateMainMenuCamera()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/UI/MainMenuCamera");
            return GameObject.Instantiate(prefab).GetComponentInChildren<Camera>();
        }

        public static Camera FindOrCreateLoadingSceneCamera()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/UI/LoadingSceneCamera");
            return GameObject.Instantiate(prefab).GetComponentInChildren<Camera>();
        }

        public static Camera FindOrCreateGameSceneCamera()
        {
            var prefab = Resources.Load<GameObject>("Prefabs/UI/GameSceneCamera");
            return GameObject.Instantiate(prefab).GetComponentInChildren<Camera>();
        }

        public static UIController FindOrCreatUIController()
        {
            var uiController = FindOrCreatePrefabByName<UIController>("Prefabs/UI/UI");
            Context.UIController = uiController;
            return uiController;
        }

        private static TDef FindOrCreatePrefabByName<TDef>(string name) where TDef : MonoBehaviour
        {
            TDef gameObject = GameObject.FindObjectOfType<TDef>();
            if(gameObject == null)
            {
                var prefab = Resources.Load<TDef>(name);
                gameObject = GameObject.Instantiate(prefab).GetComponentInChildren<TDef>(true);
            }
            return gameObject;
        }
    }
}
