using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public abstract class Scene
    {
        private static int SceneUniqueCounter = 0;
        private UnityEngine.SceneManagement.Scene SceneCreated;
        public void Load()
        {
            LoadInternal();
            BuildScene();
        }

        public void Unload()
        {
            if(SceneCreated.buildIndex != -1)
            {
                SceneManager.UnloadSceneAsync(SceneCreated.name);
            }
        }

        protected void LoadInternal()
        {
            SceneCreated = SceneManager.CreateScene(this.GetType().ToString() + SceneUniqueCounter.ToString());
            SceneManager.MoveGameObjectToScene(UnityEngine.GameObject.Find("Root"), SceneCreated);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.SetActiveScene(SceneCreated);

            var root = SceneManager.GetSceneByName("root");
            if (root.isLoaded)
            {
                SceneManager.UnloadSceneAsync(root);
            }
            SceneUniqueCounter++;
        }

        protected abstract void BuildScene();
    }
}
