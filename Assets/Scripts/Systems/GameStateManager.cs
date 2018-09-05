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
                FurthestLevelReached = 0,
                CurrentLevelIndex = 0,
                MonstersKilled = 0,
                CurrentTurn = 0,
                IsPlayerTurn = true,
            };
            Game.Dungeon = new Dungeon();
        }

        public void Clear()
        {
            Context.EntitySystem.Clear();
            Game = null;
        }

        public void SaveGame()
        {
            Debug.Log("Saving game");
            // parameters are required to deserialize abstract types
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(GetDefaultSaveLocation(), JsonConvert.SerializeObject(Game, Formatting.Indented, parameters));
        }

        public bool HasGameToLoad()
        {
            return File.Exists(GetDefaultSaveLocation());
        }

        public void LoadGame()
        {
            Clear();
            Debug.Log("Loading game");
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(GetDefaultSaveLocation()), parameters);
        }

        private static string GetDefaultSaveLocation()
        {
            return UnityEngine.Application.persistentDataPath + "/dev.sav";
        }
    }
}