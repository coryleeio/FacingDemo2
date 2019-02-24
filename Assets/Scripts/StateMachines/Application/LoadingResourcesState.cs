using System.Collections;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState
    {
        public void Enter()
        {
            Context.LoadingScene.Load();
            Context.Application.StartCoroutine(LoadPrototypes());
        }

        IEnumerator LoadPrototypes()
        {
            if (Context.Game == null)
            {
                SaveUtil.NewGame();
                DungeonGenerator.GenerateDungeon();
            }
            Context.ResourceManager.LoadAllPrototypes();
            Context.Application.StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            yield return new WaitForEndOfFrame();
        }

        public void Process()
        {

        }

        public void Exit()
        {
            Context.LoadingScene.Unload();
        }
    }
}
