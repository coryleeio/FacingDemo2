using UnityEngine;

namespace Gamepackage
{ 
    public class MainMenuState : IStateMachineState
    {
        public ApplicationContext Context { get; set; }
        
        public void Enter()
        {
            Context.GameStateManager.Clear();
            Context.TokenSystem.Clear();
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
