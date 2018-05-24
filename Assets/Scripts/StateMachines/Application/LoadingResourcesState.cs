using System.Collections;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState
    {
        public ApplicationContext Context { get; set; }

        public void Enter()
        {
            Context.LoadingScene.Load();
            Context.Application.StartCoroutine(LoadPrototypes());
        }

        IEnumerator LoadPrototypes()
        {
            Context.ResourceManager.LoadAllPrototypes();
            if (Context.GameStateManager.Game == null)
            {
                Context.GameStateManager.NewGame();
                Context.DungeonGenerator.GenerateDungeon();
            }
            Context.Application.StateMachine.ChangeState(Context.GamePlayState);
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
