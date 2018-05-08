using Newtonsoft.Json;
using System.IO;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class GameStateSystem : IGameStateSystem
    {
        public Game Game { get; private set; }
        private ILogSystem _logSystem { get; set; }
        private IResourceManager _resourceManager { get; set; }

        public ITokenSystem _tokenSystem { get; set; }
        public TinyIoCContainer _container { get; set; }

        public GameStateSystem(ILogSystem logSystem, IResourceManager resourceManager)
        {
            _logSystem = logSystem;
            _resourceManager = resourceManager;
        }

        public void NewGame()
        {
            _logSystem.Log("New Game");
            Game = new Game
            {
                FurthestLevelReached = 1,
                CurrentLevelIndex = 1,
                MonstersKilled = 0,
                Time = 0,
                IdManager = new IdManager()
            };
            Game.Dungeon = new Dungeon();
        }


        public void Clear()
        {
            _logSystem.Log("Clearing game state");
            Game = null;
        }

        public void SaveGame()
        {
            _logSystem.Log("Saving game");
            // parameters are required to deserialize abstract types
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(Application.persistentDataPath + "/dev.sav", JsonConvert.SerializeObject(Game, Formatting.Indented, parameters));
        }

        public void LoadGame()
        {
            _logSystem.Log("Loading game");
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(Application.persistentDataPath + "/dev.sav"), parameters);
            Game.Resolve(_container);
        }
    }
}