using Newtonsoft.Json;
using System.IO;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class GameStateSystem : IGameStateSystem
    {
        public Game Game { get; private set; }
        public ILogSystem LogSystem { get; set; }
        public IResourceManager ResourceManager { get; set; }

        public ITokenSystem TokenSystem { get; set; }
        public TinyIoCContainer Container { get; set; }

        public GameStateSystem()
        {

        }

        public void NewGame()
        {
            LogSystem.Log("New Game");
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
            LogSystem.Log("Clearing game state");
            Game = null;
        }

        public void SaveGame()
        {
            LogSystem.Log("Saving game");
            // parameters are required to deserialize abstract types
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(Application.persistentDataPath + "/dev.sav", JsonConvert.SerializeObject(Game, Formatting.Indented, parameters));
        }

        public void LoadGame()
        {
            LogSystem.Log("Loading game");
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(Application.persistentDataPath + "/dev.sav"), parameters);
            Game.Resolve(Container);
        }
    }
}