using Newtonsoft.Json;
using System.IO;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class GameStateManager
    {
        public Game Game { get; private set; }
        public Logger LogSystem { get; set; }
        public ResourceManager ResourceManager { get; set; }
        public TokenSystem TokenSystem { get; set; }

        public GameStateManager()
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
                IsPlayerTurn = true,
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
            foreach(var level in Game.Dungeon.Levels)
            {
                if(level != null)
                {
                    level.TokenGrid = new ListGrid<Token>(level.TilesetGrid.Width, level.TilesetGrid.Height);
                    foreach (var token in level.Tokens)
                    {
                        TokenSystem.Register(token, level);
                    }
                }
            }
        }
    }
}