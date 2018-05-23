using System.Collections;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState
    {
        public ApplicationContext ApplicationContext { get; set; }

        public void Enter()
        {
            ApplicationContext.LoadingScene.Load();
            ApplicationContext.Application.StartCoroutine(LoadPrototypes());
        }

        IEnumerator LoadPrototypes()
        {
            ApplicationContext.ResourceManager.LoadAllPrototypes();
            if (ApplicationContext.GameStateManager.Game == null)
            {
                ApplicationContext.GameStateManager.NewGame();
                ApplicationContext.DungeonGenerator.GenerateDungeon();
            }
            ApplicationContext.Application.StateMachine.ChangeState(ApplicationContext.GamePlayState);
            yield return new WaitForEndOfFrame();
        }

        public void Process()
        {

        }

        public void Exit()
        {
            ApplicationContext.LoadingScene.Unload();
        }
    }
}
