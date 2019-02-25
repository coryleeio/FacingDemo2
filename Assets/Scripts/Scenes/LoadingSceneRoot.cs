using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public class LoadingSceneRoot : MonoBehaviour
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
            StartCoroutine(LoadPrototypes());
        }

        IEnumerator LoadPrototypes()
        {
            if (Context.Game == null)
            {
                SaveUtil.NewGame();
                yield return new WaitForEndOfFrame();
                DungeonGenerator.GenerateDungeon();
                yield return new WaitForEndOfFrame();
            }
            Context.ResourceManager.LoadAllPrototypes();
            yield return new WaitForEndOfFrame();
            SceneManager.LoadScene((int) Scenes.Game);
            yield return new WaitForEndOfFrame();
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
