using System.Collections;
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
        public Application Application { get; set; }
        public GamePlayState GamePlayState { get; set; }

        public void Enter()
        {
            LoadingScene.Load();
            Application.StartCoroutine(LoadPrototypes());
        }

        IEnumerator LoadPrototypes()
        {
            ResourceManager.LoadAllPrototypes();
            if (GameStateManager.Game == null)
            {
                GameStateManager.NewGame();
                DungeonGenerator.GenerateDungeon();
            }
            Application.StateMachine.ChangeState(GamePlayState);
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
