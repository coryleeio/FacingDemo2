using Mono.Data.Sqlite;
using System.Collections;
using System.Data;
using UnityEngine;

namespace Gamepackage
{
    public class LoadingResourcesState : IStateMachineState<Root>
    {
        private LoadingScene _loadingScene;
        private ILogSystem _logSystem;
        private IPrototypeSystem _prototypeSystem;
        private IModSystem _modSystem;
        private IPrototypeFactory _prototypeFactory;
        private IGameStateSystem _gameStateSystem;

        public LoadingResourcesState(
            LoadingScene loadingScene, 
            ILogSystem logSystem, 
            IPrototypeSystem prototypeSystem, 
            IModSystem modSystem, 
            IPrototypeFactory prototypeFactory,
            IGameStateSystem gameStateSystem
        )
        {
            _loadingScene = loadingScene;
            _logSystem = logSystem;
            _prototypeSystem = prototypeSystem;
            _modSystem = modSystem;
            _prototypeFactory = prototypeFactory;
            _gameStateSystem = gameStateSystem;
        }

        public void Enter(Root owner)
        {
            _gameStateSystem.NewGame();
            _loadingScene.Load();
            owner.StartCoroutine(LoadPrototypes(owner));
        }

        IEnumerator LoadPrototypes(Root owner)
        {
            string conn = "Data Source=:memory:";
            using (var dbConnection = new SqliteConnection(conn) as IDbConnection)
            {
                dbConnection.Open();
                _modSystem.PopulateModList();
                yield return new WaitForEndOfFrame();

                _modSystem.LoadAssemblies();
                yield return new WaitForEndOfFrame();

                _prototypeFactory.LoadTypes();
                yield return new WaitForEndOfFrame();

                _modSystem.LoadAssetBundles();
                yield return new WaitForEndOfFrame();

                _modSystem.LoadSqlFiles(dbConnection);
                yield return new WaitForEndOfFrame();

                _prototypeSystem.LoadAllPrototypes(dbConnection);
                yield return new WaitForEndOfFrame();

                dbConnection.Close();
            }
            owner.StateMachine.ChangeState(owner.StateMachine.GamePlayState);
        }

        public void Exit(Root owner)
        {
            _loadingScene.Unload();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
            _logSystem.Log("LoadingResourcesState");
        }
    }
}
