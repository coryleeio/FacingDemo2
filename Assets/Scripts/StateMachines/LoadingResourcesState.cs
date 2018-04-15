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

        public LoadingResourcesState(LoadingScene loadingScene, ILogSystem logSystem, IPrototypeSystem prototypeSystem, IModSystem modSystem)
        {
            _loadingScene = loadingScene;
            _logSystem = logSystem;
            _prototypeSystem = prototypeSystem;
            _modSystem = modSystem;
        }

        public void Enter(Root owner)
        {
            _loadingScene.Load();
            owner.StartCoroutine(LoadPrototypes(owner));
        }

        IEnumerator LoadPrototypes(Root owner)
        {
            string conn = "Data Source=:memory:";
            using (var dbConnection = new SqliteConnection(conn) as IDbConnection)
            {
                dbConnection.Open();
                _modSystem.LoadAll(dbConnection);
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
