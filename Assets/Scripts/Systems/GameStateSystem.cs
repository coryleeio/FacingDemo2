using Newtonsoft.Json;
using System.IO;
using UnityEngine;

namespace Gamepackage
{
    public class GameStateSystem : IGameStateSystem
    {
        public Game Game { get; private set; }
        private ITokenSystem _tokenSystem { get; set; }
        private ILogSystem _logSystem { get; set; }

        public GameStateSystem(ITokenSystem tokenSystem, ILogSystem logSystem)
        {
            _tokenSystem = tokenSystem;
            _logSystem = logSystem;
        }

        public void NewGame()
        {
            _logSystem.Log("New Game");
            Game = new Game
            {
                FurthestLevelReached = 1,
                CurrentLevelIndex = 0,
                MonstersKilled = 0,
                Time = 0
            };
            Game.Dungeon = new Dungeon();
        }


        public void Clear()
        {
            _logSystem.Log("Clearing game state");
            _tokenSystem.Clear();
            Game = null;
        }

        public void SaveGame()
        {
            _logSystem.Log("Saving game");
            File.WriteAllText(Application.persistentDataPath + "/dev.sav", JsonConvert.SerializeObject(Game, Formatting.Indented));
        }

        public void LoadGame()
        {
            _logSystem.Log("Loading game");
            Game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(Application.persistentDataPath + "/dev.sav")); ;
        }
    }
}