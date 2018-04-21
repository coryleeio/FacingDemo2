using System.Collections.Generic;

namespace Gamepackage
{
    public class DungeonGenerator : IDungeonGenerator
    {
        IGameStateSystem _gameStateSystem;
        IPrototypeFactory _prototypeFactory;

        public DungeonGenerator(IGameStateSystem gameStateSystem, IPrototypeFactory prototypeFactory)
        {
            _gameStateSystem = gameStateSystem;
            _prototypeFactory = prototypeFactory;
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
                level.Tokens = new List<Token>();
                levels.Add(level);
            }
            _prototypeFactory.BuildToken("Poncy", 0, new Point(0, 0));
        }
    }
}
