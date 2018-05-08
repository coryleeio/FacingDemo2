using Newtonsoft.Json;
using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class Game : IResolvableReferences
    {
        public int FurthestLevelReached { get; set; }
        public int CurrentLevelIndex { get; set; }
        public int MonstersKilled { get; set; }
        public int Time { get; set; }
        public IdManager IdManager { get; set; }

        public Game()
        {

        }

        public Dungeon Dungeon;

        [JsonIgnore]
        public Level CurrentLevel
        {
            get
            {
                return Dungeon.Levels[CurrentLevelIndex];
            }
        }

        public void Resolve(TinyIoCContainer container)
        {
            Dungeon.Resolve(container);
        }
    }
}