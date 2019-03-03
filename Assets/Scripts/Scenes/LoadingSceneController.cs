using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public class LoadingSceneController : MonoBehaviour
    {
        public void Start()
        {
            SceneUtil.FindOrCreateEventSystem();
            SceneUtil.FindOrCreatUIController();
            SceneUtil.FindOrCreateLoadingSceneCamera();
            Context.UIController.Init();
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
