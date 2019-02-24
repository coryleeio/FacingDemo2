using UnityEngine;

namespace Gamepackage
{ 
    public class MainMenuState : IStateMachineState
    {
        public void Enter()
        {
            Context.MainMenuScene.Load();
            var MainMenuPrefab = Resources.Load<GameObject>("UI/MainMenu");
            var MainMenu = GameObject.Instantiate(MainMenuPrefab);
            MainMenu.name = "MainMenu";
            var mainMenuInstance = MainMenu.GetComponent<MainMenu>();
            Context.MainMenu = mainMenuInstance;
        }

        public void Process()
        {

        }

        public void Exit()
        {
            Context.MainMenuScene.Unload();
            Context.MainMenu = null;
        }
    }
}
