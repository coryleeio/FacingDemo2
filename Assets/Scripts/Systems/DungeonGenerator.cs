using System.Collections.Generic;
using UnityEngine;

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
                level.TilesetGrid = new TileInfo[size, size];
                for(var x = 0; x < size; x++)
                {
                    for(var y = 0; y < size; y++)
                    {
                        level.TilesetGrid[x, y] = new TileInfo()
                        {
                            TilesetIdentifier = null,
                            TileType = TileType.Empty,
                        };
                    }
                }
            }

            var placementLevel = levels[0];
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    if(x == 0 || y == 0 || x == 4 || y == 4)
                    {
                        placementLevel.TilesetGrid[x, y].TileType = TileType.Wall;
                    }
                    else
                    {
                        placementLevel.TilesetGrid[x, y].TileType = TileType.Floor;
                    }
                    
                    placementLevel.TilesetGrid[x, y].TilesetIdentifier = "Stone";
                }
            }
            var token = _prototypeFactory.BuildToken("Poncy");
            token.Position = new Point(0, 0);
            placementLevel.Tokens.Add(token);
        }
    }
}
