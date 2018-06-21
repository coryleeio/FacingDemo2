using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Gamepackage
{
    public class GameStateManager
    {
        public Game Game { get; private set; }

        public GameStateManager() {}

        public void NewGame()
        {
            Debug.Log("New Game");
            Game = new Game
            {
                FurthestLevelReached = 1,
                CurrentLevelIndex = 1,
                MonstersKilled = 0,
                CurrentTurn = 0,
                IsPlayerTurn = true,
            };
            Game.Dungeon = new Dungeon();
        }

        public void Clear()
        {
            ServiceLocator.EntitySystem.Clear();
            Debug.Log("Clearing game state");
            Game = null;
        }

        public void SaveGame()
        {
            Debug.Log("Saving game");
            // parameters are required to deserialize abstract types
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(UnityEngine.Application.persistentDataPath + "/dev.sav", JsonConvert.SerializeObject(Game, Formatting.Indented, parameters));
        }

        public void LoadGame()
        {
            Clear();
            Debug.Log("Loading game");
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(UnityEngine.Application.persistentDataPath + "/dev.sav"), parameters);
            Game.Rewire();
        }
    }
}