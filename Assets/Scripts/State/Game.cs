using System.Collections.Generic;

namespace Gamepackage
{
    public class Game
    {
        public int FurthestLevelReached { get; set; }
        public int CurrentLevel { get; set; }
        public int MonstersKilled { get; set; }
        public int Time { get; set; }

        public Game()
        {

        }

        public Dungeon Dungeon;
    }
}