using UnityEngine;

namespace Gamepackage
{ 
    public class MainMenuState : IStateMachineState
    {
        public void Enter()
        {
            ServiceLocator.GameStateManager.Clear();
            ServiceLocator.EntitySystem.Clear();
            ServiceLocator.MainMenuScene.Load();
        }

        public void Process()
        {

        }

        public void Exit()
        {
            ServiceLocator.MainMenuScene.Unload();
        }
    }
}
