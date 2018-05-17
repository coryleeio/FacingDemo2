using Mono.Data.Sqlite;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState
    {
        public LoadingScene LoadingScene { get; set; }
        public Logger Logger { get; set; }
        public ResourceManager ResourceManager { get; set; }
        public PrototypeFactory PrototypeFactory { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public DungeonGenerator DungeonGenerator { get; set; }
        public Root Root { get; set; }

        public LoadingResourcesState(
        )
        {
        }

        public void Enter()
        {   
            LoadingScene.Load();
            Root.StartCoroutine(LoadPrototypes(Root));
        }

        IEnumerator LoadPrototypes(Root owner)
        {
            ResourceManager.LoadAllPrototypes();
            if (GameStateManager.Game == null)
            {
                GameStateManager.NewGame();
                DungeonGenerator.GenerateDungeon();
            }
            owner.StateMachine.ChangeState(owner.StateMachine.GamePlayState);
            yield return new WaitForEndOfFrame();
        }

        public void Process()
        {
        }

        public void Exit()
        {
            LoadingScene.Unload();
        }
    }
}
