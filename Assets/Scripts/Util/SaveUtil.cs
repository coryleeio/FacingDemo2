using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Gamepackage
{
    public static class SaveUtil
    {
        public static void NewGame()
        {
            var gameSceneRoot = GameObject.FindObjectOfType<GameSceneRoot>();
            if (gameSceneRoot != null)
            {
                gameSceneRoot.Stopped = true;
            }
            Clear();
            Context.Game = new Game
            {
                FurthestLevelReached = 0,
                CurrentLevelIndex = 0,
                MonstersKilled = 0,
                CurrentTurn = 0,
                IsPlayerTurn = true,
            };
            Context.Game.Dungeon = new Dungeon();
        }

        public static void Clear()
        {
            var gameSceneRoot = GameObject.FindObjectOfType<GameSceneRoot>();
            if (gameSceneRoot != null)
            {
                gameSceneRoot.Stopped = true;
            }
            Context.EntitySystem.Clear();
            Context.Game = null;
        }

        public static void SaveGame(Game game, string saveLocation)
        {
            // parameters are required to deserialize abstract types
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            File.WriteAllText(saveLocation, JsonConvert.SerializeObject(Context.Game, Formatting.Indented, parameters));
        }

        public static bool HasGameToLoad(string saveLocation)
        {
            return File.Exists(saveLocation);
        }

        public static void LoadGame()
        {
            NewGame();
            var parameters = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
            Context.Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(GetDefaultSaveLocation()), parameters);
        }

        public static string GetDefaultSaveLocation()
        {
            return UnityEngine.Application.persistentDataPath + "/dev.sav";
        }
    }
}
