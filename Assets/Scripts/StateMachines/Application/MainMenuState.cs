using UnityEngine;

namespace Gamepackage
{ 
    public class MainMenuState : IStateMachineState
    {
        public void Enter()
        {
            Context.GameStateManager.Clear();
            Context.EntitySystem.Clear();
            Context.MainMenuScene.Load();
        }

        public void Process()
        {

        }

        public void Exit()
        {
            Context.MainMenuScene.Unload();
        }
    }
}
