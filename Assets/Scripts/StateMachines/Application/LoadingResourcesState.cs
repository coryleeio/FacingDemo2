using System.Collections;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState
    {
        public void Enter()
        {
            ServiceLocator.LoadingScene.Load();
            ServiceLocator.Application.StartCoroutine(LoadPrototypes());
        }

        IEnumerator LoadPrototypes()
        {
            ServiceLocator.ResourceManager.LoadAllPrototypes();
            if (ServiceLocator.GameStateManager.Game == null)
            {
                ServiceLocator.GameStateManager.NewGame();
                ServiceLocator.DungeonGenerator.GenerateDungeon();
            }
            ServiceLocator.Application.StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            yield return new WaitForEndOfFrame();
        }

        public void Process()
        {

        }

        public void Exit()
        {
            ServiceLocator.LoadingScene.Unload();
        }
    }
}
