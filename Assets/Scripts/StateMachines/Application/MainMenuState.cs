using UnityEngine;

namespace Gamepackage
{ 
    public class MainMenuState : IStateMachineState
    {
        public ApplicationContext ApplicationContext { get; set; }
        
        public void Enter()
        {
            ApplicationContext.GameStateManager.Clear();
            ApplicationContext.TokenSystem.Clear();
            ApplicationContext.MainMenuScene.Load();
        }

        public void Process()
        {

        }

        public void Exit()
        {
            ApplicationContext.MainMenuScene.Unload();
        }
    }
}
