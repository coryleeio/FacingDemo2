using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class DungeonGenerator : IDungeonGenerator
    {
        IGameStateSystem _gameStateSystem;
        IPrototypeFactory _prototypeFactory;
        IResourceManager _resourceManager;

        public DungeonGenerator(IGameStateSystem gameStateSystem, IPrototypeFactory prototypeFactory, IResourceManager resourceManager)
        {
            _gameStateSystem = gameStateSystem;
            _prototypeFactory = prototypeFactory;
            _resourceManager = resourceManager;
        }

        public void GenerateDungeon()
        {
            var levelPrototypes = _resourceManager.GetPrototypes<LevelPrototype>();
            var roomPrototypes = _resourceManager.GetPrototypes<RoomPrototype>();
            var roomPrototypesByLevel = new Dictionary<int, List<RoomPrototype>>();
            var spawnTables = _resourceManager.GetPrototypes<SpawnTable>();
            var spawnTablesByLevel = new Dictionary<int, List<SpawnTable>>();
            int numberOfLevelsInArray = levelPrototypes.Count + 1;

            for (var i = 1; i < numberOfLevelsInArray; i++)
            {
                foreach (var roomPrototype in roomPrototypes)
                {
                    if (roomPrototype.AvailableOnLevels.Contains(i))
                    {
                        if (!roomPrototypesByLevel.ContainsKey(i))
                        {
                            roomPrototypesByLevel[i] = new List<RoomPrototype>();
                        }
                        roomPrototypesByLevel[i].Add(roomPrototype);
                    }
                }
                foreach (var spawnTable in spawnTables)
                {
                    if (spawnTable.AvailableOnLevels.Contains(i))
                    {
                        if (!spawnTablesByLevel.ContainsKey(i))
                        {
                            spawnTablesByLevel[i] = new List<SpawnTable>();
                        }
                        spawnTablesByLevel[i].Add(spawnTable);
                    }
                }
            }

            _gameStateSystem.Game.Dungeon.Levels = new Level[numberOfLevelsInArray];
            var levels = _gameStateSystem.Game.Dungeon.Levels;
            foreach (var levelPrototype in levelPrototypes)
            {
                var level = new Level();
                level.LevelIndex = levelPrototype.LevelIndex;
                int size = 40;
                level.Domain = new Rectangle
                {
                    Position = new Point(0, 0),
                    Width = size,
                    Height = size
                };
                level.Tokens = new List<Token>();
                levels[levelPrototype.LevelIndex] = level;
                level.TilesetGrid = new TileInfo[size, size];

                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < size; y++)
                    {
                        level.TilesetGrid[x, y] = new TileInfo()
                        {
                            TilesetIdentifier = null,
                            TileType = TileType.Empty,
                        };
                        level.TilesetGrid[x, y].TilesetIdentifier = levelPrototype.DefaultTileset.UniqueIdentifier;
                    }
                }

                var numberOfRoomsToSpawn = 5;
                var roomPrototypesOnLevel = roomPrototypesByLevel[level.LevelIndex];
                var roomPrototypesToSpawn = new List<RoomPrototype>();

                roomPrototypesToSpawn.AddRange(roomPrototypesOnLevel.FindAll((x) => x.Mandatory));
                numberOfRoomsToSpawn = numberOfRoomsToSpawn - roomPrototypesToSpawn.Count;
                roomPrototypesToSpawn.ForEach((roomPrototypeBeingSpawned) =>
                {
                    // If it is unique, dont allow us to choose it for spawning.
                    if (roomPrototypeBeingSpawned.Unique)
                    {
                        roomPrototypesOnLevel.Remove(roomPrototypeBeingSpawned);
                    }
                });

                // If we have stuff we can spawn, and we need some more stuff to spawn
                if (numberOfRoomsToSpawn > 0 && roomPrototypesOnLevel.Count > 0)
                {
                    roomPrototypesToSpawn.AddRange(MathUtil.ChooseNRandomElements(numberOfRoomsToSpawn, roomPrototypesOnLevel));
                }

                foreach (var roomPrototypeToSpawn in roomPrototypesToSpawn)
                {
                    var width = UnityEngine.Random.Range(roomPrototypeToSpawn.RoomGenerator.MinimumWidth, roomPrototypeToSpawn.RoomGenerator.MaximumWidth);
                    var height = UnityEngine.Random.Range(roomPrototypeToSpawn.RoomGenerator.MinimumHeight, roomPrototypeToSpawn.RoomGenerator.MaximumHeight);
                    var guage = new Rectangle()
                    {
                        Position = new Point(0, 0),
                        Width = width,
                        Height = height,
                    };

                    Rectangle roomToSurround = null;
                    if (level.Rooms.Count > 0)
                    {
                        roomToSurround = MathUtil.ChooseRandomElement<Room>(level.Rooms).BoundingBox;
                    }
                    else
                    {
                        roomToSurround = new Rectangle()
                        {
                            Position = new Point((level.Domain.Width / 4) + UnityEngine.Random.Range(0, 3), (level.Domain.Height / 4) + UnityEngine.Random.Range(0, 3)),
                            Height = UnityEngine.Random.Range(2, 3),
                            Width = UnityEngine.Random.Range(2, 3)
                        };
                    }
                    var rects = FindValidSurroundingRectangles(roomToSurround, guage, level.Domain);
                    rects = rects.FindAll((possibleRect) =>
                    {
                        return level.Rooms.FindAll((room) => room.BoundingBox.Adjacent(possibleRect) || room.BoundingBox.Intersects(possibleRect)).Count == 0;
                    });

                    level.Rooms.Add(roomPrototypeToSpawn.RoomGenerator.Generate(level, MathUtil.ChooseRandomElement<Rectangle>(rects)));
                }
            }



            /*
            var placementLevel = levels[0];
            for (var x = 0; x < 5; x++)
            {
                for (var y = 0; y < 5; y++)
                {
                    if (x == 0 || y == 0 || x == 4 || y == 4)
                    {
                        placementLevel.TilesetGrid[x, y].TileType = TileType.Wall;
                    }
                    else
                    {
                        placementLevel.TilesetGrid[x, y].TileType = TileType.Floor;
                    }
                }
            }

            var token = _prototypeFactory.BuildToken("Poncy");
            token.Position = new Point(0, 0);
            placementLevel.Tokens.Add(token);
            */
        }

        public static List<Rectangle> FindValidSurroundingRectangles(Rectangle source, Rectangle guage, Rectangle boundingBox)
        {
            var offsets = MathUtil.SurroundingOffsets;
            var retval = new List<Rectangle>();

            foreach (var offset in offsets)
            {
                var relevantWidth = offset.X > 0 ? source.Width : guage.Width;
                var relevantHeight = offset.Y > 0 ? source.Height : guage.Height;
                var newPositionX = source.Position.X + (offset.X * (relevantWidth + UnityEngine.Random.Range(1, 8)));
                var newPositionY = source.Position.Y + (offset.Y * (relevantHeight + UnityEngine.Random.Range(1, 8)));

                var newRect = new Rectangle()
                {
                    Position = new Point(newPositionX, newPositionY),
                    Width = guage.Width,
                    Height = guage.Height,
                };

                if (boundingBox.Contains(newRect))
                {
                    retval.Add(newRect);
                }
            }
            return retval;
        }
    }
}
