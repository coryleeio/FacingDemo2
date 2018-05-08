using Mono.Data.Sqlite;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState<Root>
    {
        public LoadingScene LoadingScene { get; set; }
        public ILogSystem LogSystem { get; set; }
        public IResourceManager PrototypeSystem { get; set; }
        public IModSystem ModSystem { get; set; }
        public IPrototypeFactory PrototypeFactory { get; set; }
        public IGameStateSystem GameStateSystem { get; set; }
        public IDungeonGenerator DungeonGenerator { get; set; }

        public LoadingResourcesState(
        )
        {
        }

        public void Enter(Root owner)
        {   
            LoadingScene.Load();
            owner.StartCoroutine(LoadPrototypes(owner));
        }

        IEnumerator LoadPrototypes(Root owner)
        {
            string conn = "Data Source=:memory:";
            using (var dbConnection = new SqliteConnection(conn) as IDbConnection)
            {
                dbConnection.Open();
                ModSystem.PopulateModList();
                yield return new WaitForEndOfFrame();

                ModSystem.LoadAssemblies();
                yield return new WaitForEndOfFrame();

                PrototypeFactory.LoadTypes();
                yield return new WaitForEndOfFrame();

                ModSystem.LoadAssetBundles();
                yield return new WaitForEndOfFrame();

                ModSystem.LoadSqlFiles(dbConnection);
                yield return new WaitForEndOfFrame();

                PrototypeSystem.LoadAllPrototypes(dbConnection);
                yield return new WaitForEndOfFrame();

                if (GameStateSystem.Game == null)
                {
                    GameStateSystem.NewGame();
                    DungeonGenerator.GenerateDungeon();
                }

                dbConnection.Close();
            }
            owner.StateMachine.ChangeState(owner.StateMachine.GamePlayState);
        }

        public void Exit(Root owner)
        {
            LoadingScene.Unload();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
        }
    }
}
