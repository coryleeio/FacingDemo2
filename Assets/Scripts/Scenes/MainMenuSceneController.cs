using UnityEngine;

namespace Gamepackage
{
    public class MainMenuSceneController : MonoBehaviour
    {

        public void Start()
        {
            Context.ModManager.LoadModsAndResources();
            SceneUtil.FindOrCreateEventSystem();
            SceneUtil.FindOrCreatUIController();
            SceneUtil.FindOrCreateMainMenuCamera();
            Context.UIController.Init();
            Context.UIController.MainMenu.Show();
        }
    }
}
