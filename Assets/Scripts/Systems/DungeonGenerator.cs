using System.Collections.Generic;

namespace Gamepackage
{
    public class DungeonGenerator : IDungeonGenerator
    {
        IGameStateSystem _gameStateSystem;

        public DungeonGenerator(IGameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        public void GenerateDungeon()
        {
            _gameStateSystem.Game.Dungeon.Levels = new List<Level>(10);
            var levels = _gameStateSystem.Game.Dungeon.Levels;
            for (var i = 0; i < 10; i++)
            {
                var level = new Level();
                int size = 40;
                level.Domain = new Rectangle
                {
                    Position = new Point(0, 0),
                    Width = size,
                    Height = size
                };
                levels.Add(level);
            }
        }
    }
}
