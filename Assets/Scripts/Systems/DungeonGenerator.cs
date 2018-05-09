using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using TinyIoC;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class DungeonGenerator : IDungeonGenerator
    {
        public IGameStateSystem GameStateSystem { get; set; }
        public IPrototypeFactory PrototypeFactory { get; set; }
        public IResourceManager ResourceManager { get; set; }
        public ILogSystem LogSystem { get; set; }
        public TinyIoCContainer Container { get; set; }

        public DungeonGenerator()
        {
        }

        public void GenerateDungeon()
        {
            var levelPrototypes = ResourceManager.GetPrototypes<LevelPrototype>();
            var roomPrototypes = ResourceManager.GetPrototypes<RoomPrototype>();
            var roomPrototypesByLevel = new Dictionary<int, List<RoomPrototype>>();
            var spawnTables = ResourceManager.GetPrototypes<SpawnTable>();
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

            GameStateSystem.Game.Dungeon.Levels = new Level[numberOfLevelsInArray];
            var levels = GameStateSystem.Game.Dungeon.Levels;
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

                var roomConnections = new List<KeyValuePair<Room, Room>>();
                foreach (var roomPrototypeToSpawn in roomPrototypesToSpawn)
                {
                    Room previousRoom;
                    Rectangle rect = FindNextRoomRect(level, roomPrototypeToSpawn, out previousRoom);
                    var newRoom = roomPrototypeToSpawn.RoomGenerator.Generate(level, rect);
                    newRoom.Tags.AddRange(roomPrototypeToSpawn.Tags);
                    level.Rooms.Add(newRoom);
                    if (previousRoom != null)
                    {
                        roomConnections.Add(new KeyValuePair<Room, Room>(previousRoom, newRoom));
                    }
                }

                // Do this after creating rooms so that the walls dont cause pathing issues
                foreach(var pair in roomConnections)
                {
                    ConnectTwoRooms(level, pair.Key, pair.Value);
                }

                var numberOfSpawnTablesToSpawn = 5;
                var spawnTablesOnLevel = spawnTablesByLevel[level.LevelIndex];
                var spawnTablesToSpawn = new List<SpawnTable>();

                spawnTablesToSpawn.AddRange(spawnTablesOnLevel.FindAll((x) => x.Mandatory));
                numberOfSpawnTablesToSpawn = numberOfSpawnTablesToSpawn - spawnTablesToSpawn.Count;
                spawnTablesToSpawn.ForEach((spawnTableBeingSpawned) =>
                {
                    // If it is unique, dont allow us to choose it for spawning.
                    if (spawnTableBeingSpawned.Unique)
                    {
                        spawnTablesOnLevel.Remove(spawnTableBeingSpawned);
                    }
                });

                // If we have stuff we can spawn, and we need some more stuff to spawn
                if (numberOfSpawnTablesToSpawn > 0 && spawnTablesOnLevel.Count > 0)
                {
                    spawnTablesToSpawn.AddRange(MathUtil.ChooseNRandomElements(numberOfSpawnTablesToSpawn, spawnTablesOnLevel));
                }

                foreach (var spawnTableToSpawn in spawnTablesToSpawn)
                {
                    List<TokenPrototype> thingsToSpawn = spawnTableToSpawn.ProbabilityTable.Next();

                    Point floodFillStartPoint;
                    if(spawnTableToSpawn.ConstraintSpawnToRoomWithTag != null)
                    {
                        var roomForSpawn = level.Rooms.Find((room) => room.Tags.Contains(spawnTableToSpawn.ConstraintSpawnToRoomWithTag));
                        var floorTilesInRoom = FloorTilesInRect(level, roomForSpawn.BoundingBox);
                        floodFillStartPoint = MathUtil.ChooseRandomElement<Point>(floorTilesInRoom);
                    }
                    else
                    {
                        var floorTilesInRoom = FloorTilesInRect(level, level.Domain);
                        floodFillStartPoint = MathUtil.ChooseRandomElement<Point>(floorTilesInRoom);
                    }

                    var pointsInDomain = MathUtil.PointsInRect(level.Domain);
                    for (int i = 2; i < 8; i = i + 2)
                    {
                        List<Point> spawnPoints = new List<Point>();
                        MathUtil.FloodFill(floodFillStartPoint, i, ref spawnPoints, MathUtil.FloodFillType.Surrounding, (piq)=> { return level.TilesetGrid[piq.X, piq.Y].TileType == TileType.Floor; });
                        
                        foreach(var alreadyExistingToken in level.Tokens)
                        {
                            spawnPoints.RemoveAll((poi) => alreadyExistingToken.Position == poi);
                        }

                        if (spawnPoints.Count >= thingsToSpawn.Count)
                        {
                            foreach(var thingToSpawn in thingsToSpawn)
                            {
                                var spawnPoint = MathUtil.ChooseRandomElement<Point>(spawnPoints);
                                spawnPoints.Remove(spawnPoint);
                                var thingSpawned= PrototypeFactory.BuildToken(thingToSpawn);
                                thingSpawned.Position = spawnPoint;
                                level.Tokens.Add(thingSpawned);
                            }
                            break;
                        }
                    }
                }
            }
            GameStateSystem.Game.Resolve(Container);
        }

        private void ConnectTwoRooms(Level level, Room previousRoom, Room newRoom)
        {
            var previousTiles = FloorTilesInRect(level, previousRoom.BoundingBox);
            var currentTiles = FloorTilesInRect(level, newRoom.BoundingBox);
            if(previousTiles.Count == 0 || currentTiles.Count == 0)
            {
                throw new SystemException("Rooms contain no walkable tiles?");
            }
            
            var previousTile = MathUtil.ChooseRandomElement<Point>(previousTiles);
            var currentTile = MathUtil.ChooseRandomElement<Point>(currentTiles);

            ConnectTwoPoints(level, previousRoom, newRoom, previousTile, currentTile);
        }

        private void ConnectTwoPoints(Level level, Room previousRoom, Room nextRoom, Point previousTile, Point nextTile)
        {
            var pathA = ConnectPointsByX(level, previousTile.X, nextTile.X, previousTile.Y);
            pathA.AddRange(ConnectPointsByY(level, previousTile.Y, nextTile.Y, nextTile.X));

            var pathB = ConnectPointsByY(level, previousTile.Y, nextTile.Y, previousTile.X);
            pathB.AddRange(ConnectPointsByX(level, previousTile.X, nextTile.X, nextTile.Y));
            var pathBIntersectsOtherRooms = level.Rooms.FindAll((roomInSearch) => PathIntersectsRoom(roomInSearch, pathB) && roomInSearch != nextRoom && roomInSearch != previousRoom).Count > 0;

            var path = pathBIntersectsOtherRooms && UnityEngine.Random.Range(0, 1) == 0 ? pathA : pathB;
            foreach (var pathPoint in path)
            {
                Carve(level, pathPoint);
            }
        }

        private void Carve(Level level, Point centerPoint)
        {
            level.TilesetGrid[centerPoint.X, centerPoint.Y].TileType = TileType.Floor;
            foreach (var point in MathUtil.SurroundingPoints(centerPoint))
            {
                if (level.TilesetGrid[point.X, point.Y].TileType == TileType.Empty)
                {
                    level.TilesetGrid[point.X, point.Y].TileType = TileType.Wall;
                }
            }
        }

        private bool PathIntersectsRoom(Room room, List<Point> path)
        {
            foreach(var point in path)
            {
                if(room.BoundingBox.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        private List<Point> ConnectPointsByX(Level level, int x1, int x2, int currentY)
        {

            var path = new List<Point>();
            for (var x = Math.Min(x1,x2); x < Math.Max(x1,x2) + 1; x++)
            {
                path.Add(new Point(x, currentY));
            }
            return path;
        }

        private List<Point> ConnectPointsByY(Level level, int y1, int y2, int currentX)
        {
            var path = new List<Point>();
            for (var y = Math.Min(y1, y2); y < Math.Max(y1, y2) + 1; y++)
            {
                path.Add(new Point(currentX, y));
            }
            return path;
        }

        private static List<Point> FloorTilesInRect(Level level, Rectangle rect)
        {
            var walkableTiles = new List<Point>();
            for (var x = rect.Position.X; x < rect.Position.X + rect.Width; x++)
            {
                for (var y = rect.Position.Y; y < rect.Position.Y + rect.Height; y++)
                {
                    if (level.TilesetGrid[x, y].TileType == TileType.Floor)
                    {
                        walkableTiles.Add(new Point(x, y));
                    }
                }
            }
            return walkableTiles;
        }

        private static Rectangle FindNextRoomRect(Level level, RoomPrototype roomPrototypeToSpawn, out Room previousRoom)
        {
            var width = UnityEngine.Random.Range(roomPrototypeToSpawn.RoomGenerator.MinimumWidth, roomPrototypeToSpawn.RoomGenerator.MaximumWidth);
            var height = UnityEngine.Random.Range(roomPrototypeToSpawn.RoomGenerator.MinimumHeight, roomPrototypeToSpawn.RoomGenerator.MaximumHeight);

            if (level.Rooms.Count == 0)
            {
                previousRoom = null;
                return new Rectangle()
                {
                    Position = new Point((level.Domain.Width / 4) + UnityEngine.Random.Range(0, 3), (level.Domain.Height / 4) + UnityEngine.Random.Range(0, 3)),
                    Height = height,
                    Width = width
                };
            }

            var guage = new Rectangle()
            {
                Position = new Point(0, 0),
                Width = width,
                Height = height,
            };

            Dictionary<Room, List<Rectangle>> rectsForRooms = new Dictionary<Room, List<Rectangle>>();
            List<Room> validRooms = new List<Room>();
            foreach (var room in level.Rooms)
            {
                var rects = FindValidSurroundingRectangles(room.BoundingBox, guage, level.Domain);
                rects = rects.FindAll((possibleRect) =>
                {
                    return level.Rooms.FindAll((roomInLevel) => roomInLevel.BoundingBox.Adjacent(possibleRect) || roomInLevel.BoundingBox.Intersects(possibleRect)).Count == 0;
                });
                if(rects.Count > 0)
                {
                    Assert.IsFalse(rectsForRooms.ContainsKey(room));
                    validRooms.Add(room);
                    rectsForRooms.Add(room, new List<Rectangle>());
                    rectsForRooms[room].AddRange(rects);
                }
            }
            previousRoom = MathUtil.ChooseRandomElement<Room>(validRooms);
            return MathUtil.ChooseRandomElement<Rectangle>(rectsForRooms[previousRoom]);
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
