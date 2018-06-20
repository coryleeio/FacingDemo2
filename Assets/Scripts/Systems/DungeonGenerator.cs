using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class DungeonGenerator
    {

        public DungeonGenerator() {}

        public void GenerateDungeon()
        {
            var numberOfLevels = 2;
            var spawnTables = ServiceLocator.ResourceManager.GetPrototypes<EncounterPrototype>();

            int numberOfLevelsInArray = numberOfLevels + 1;

            ServiceLocator.GameStateManager.Game.Dungeon.Levels = new Level[numberOfLevelsInArray];
            var levels = ServiceLocator.GameStateManager.Game.Dungeon.Levels;
            for (int levelNumber = 1; levelNumber <= numberOfLevels; levelNumber++)
            {
                var level = new Level();
                level.LevelIndex = levelNumber;
                int size = 40;
                level.BoundingBox = new Rectangle
                {
                    Position = new Point(0, 0),
                    Width = size,
                    Height = size
                };
                level.Entitys = new List<Entity>();
                levels[levelNumber] = level;
                level.TilesetGrid = new Grid<Tile>(size, size);
                level.TilesetGrid.Each((x, y, v) =>
                {
                    level.TilesetGrid[x, y] = new Tile()
                    {
                        TilesetIdentifier = UniqueIdentifier.TILESET_STONE,
                        TileType = TileType.Empty,
                    };
                });
                level.EntityGrid = new ListGrid<Entity>(size, size);

                var numberOfRoomsToSpawn = 3;

                var roomConnections = new List<KeyValuePair<Room, Room>>();
                for(var roomNumber = 0; roomNumber < numberOfRoomsToSpawn; roomNumber ++)
                {
                    Room previousRoom;
                    Rectangle rect = FindNextRoomRect(level, out previousRoom);
                    var newRoom = Generate(level, rect);
                    level.Rooms.Add(newRoom);
                    if (previousRoom != null)
                    {
                        roomConnections.Add(new KeyValuePair<Room, Room>(previousRoom, newRoom));
                    }
                }

                // Do this after creating rooms so that the walls dont cause pathing issues
                foreach (var pair in roomConnections)
                {
                    ConnectTwoRooms(level, pair.Key, pair.Value);
                }

                var numberOfSpawnTablesToSpawn = 7;

                for(var spawnNumber = 0; spawnNumber < numberOfSpawnTablesToSpawn; spawnNumber ++)
                {
                    var spawnTableToSpawn = ServiceLocator.ResourceManager.GetPrototype<EncounterPrototype>(UniqueIdentifier.ENCOUNTER_BEE_SWARM);
                    List<UniqueIdentifier> thingsToSpawn = spawnTableToSpawn.ProbabilityTable.Next();

                    Point floodFillStartPoint;
                    var floorTilesInRoom = FloorTilesInRect(level, level.BoundingBox);
                    floodFillStartPoint = MathUtil.ChooseRandomElement<Point>(floorTilesInRoom);

                    var pointsInDomain = MathUtil.PointsInRect(level.BoundingBox);
                    for (int i = 2; i < 8; i = i + 2)
                    {
                        List<Point> spawnPoints = new List<Point>();
                        MathUtil.FloodFill(floodFillStartPoint, i, ref spawnPoints, MathUtil.FloodFillType.Surrounding, (piq) => { return level.TilesetGrid[piq.X, piq.Y].TileType == TileType.Floor; });

                        foreach (var alreadyExistingEntity in level.Entitys)
                        {
                            spawnPoints.RemoveAll((poi) => alreadyExistingEntity.Position == poi);
                        }

                        if (spawnPoints.Count >= thingsToSpawn.Count)
                        {
                            foreach (var thingToSpawn in thingsToSpawn)
                            {
                                var spawnPoint = MathUtil.ChooseRandomElement<Point>(spawnPoints);
                                spawnPoints.Remove(spawnPoint);
                                var thingSpawned = ServiceLocator.PrototypeFactory.BuildEntity(thingToSpawn);
                                thingSpawned.Position = spawnPoint;
                                level.Entitys.Add(thingSpawned);
                            }
                            break;
                        }
                    }
                }
                if (level.LevelIndex == 1)
                {
                    SpawnOnLevel(UniqueIdentifier.TOKEN_PONCY, level);
                }
            }

            ConnectLevelsByStairway(levels[1], levels[2]);

            var game = ServiceLocator.GameStateManager.Game;
            BuildPathfindingGrid(game);
        }

        private void ConnectLevelsByStairway(Level level1, Level level2)
        {
            Assert.AreNotEqual(level1.LevelIndex, level2.LevelIndex);
            var lowerLevel = level1.LevelIndex < level2.LevelIndex ? level1 : level2;
            var higherLevel = lowerLevel == level1 ? level2 : level1;

            var downStair = SpawnOnLevel(UniqueIdentifier.TOKEN_STAIRS_DOWN, ServiceLocator.GameStateManager.Game.Dungeon.Levels[lowerLevel.LevelIndex]);
            var upStair = SpawnOnLevel(UniqueIdentifier.TOKEN_STAIRS_UP, ServiceLocator.GameStateManager.Game.Dungeon.Levels[higherLevel.LevelIndex]);

            var downStairTraverseAction = downStair.Trigger.TriggerAction as TraverseStaircase;
            downStairTraverseAction.Parameters.Add(TraverseStaircase.Params.TARGET_LEVEL_ID.ToString(), higherLevel.LevelIndex.ToString());
            downStairTraverseAction.Parameters.Add(TraverseStaircase.Params.TARGET_POSX.ToString(), upStair.Position.X.ToString());
            downStairTraverseAction.Parameters.Add(TraverseStaircase.Params.TARGET_POSY.ToString(), upStair.Position.Y.ToString());

            var upStairTraverseAction = upStair.Trigger.TriggerAction as TraverseStaircase;
            upStairTraverseAction.Parameters.Add(TraverseStaircase.Params.TARGET_LEVEL_ID.ToString(), lowerLevel.LevelIndex.ToString());
            upStairTraverseAction.Parameters.Add(TraverseStaircase.Params.TARGET_POSX.ToString(), downStair.Position.X.ToString());
            upStairTraverseAction.Parameters.Add(TraverseStaircase.Params.TARGET_POSY.ToString(), downStair.Position.Y.ToString());
        }

        private Entity SpawnOnLevel(UniqueIdentifier identifier, Level level)
        {
            var possiblePlayerSpawnPoints = FloorTilesInRect(level, level.BoundingBox);
            foreach (var alreadyExistingEntity in level.Entitys)
            {
                possiblePlayerSpawnPoints.RemoveAll((poi) => alreadyExistingEntity.Position == poi);
            }
            var spawnPoint = MathUtil.ChooseRandomElement<Point>(possiblePlayerSpawnPoints);
            var thing = ServiceLocator.PrototypeFactory.BuildEntity(identifier);
            thing.Position = spawnPoint;
            level.Entitys.Add(thing);
            return thing;
        }

        private void BuildPathfindingGrid(Game game)
        {
            var levels = game.Dungeon.Levels;
            foreach (var level in levels)
            {
                if (level != null)
                {
                    level.TilesetGrid.Each((x, y, v) =>
                    {
                        var occupied = level.EntityGrid[x, y].FindAll((entity) => { return entity.EntityPrototype.BlocksPathing; }).Count > 0;
                        v.Walkable = level.TilesetGrid[x, y].TileType == TileType.Floor && !occupied;
                        v.Weight = 1;
                    });
                }
            }
        }

        private void ConnectTwoRooms(Level level, Room previousRoom, Room newRoom)
        {
            var previousTiles = FloorTilesInRect(level, previousRoom.BoundingBox);
            var currentTiles = FloorTilesInRect(level, newRoom.BoundingBox);
            if (previousTiles.Count == 0 || currentTiles.Count == 0)
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
            foreach (var point in path)
            {
                if (room.BoundingBox.Contains(point))
                {
                    return true;
                }
            }
            return false;
        }

        private List<Point> ConnectPointsByX(Level level, int x1, int x2, int currentY)
        {

            var path = new List<Point>();
            for (var x = Math.Min(x1, x2); x < Math.Max(x1, x2) + 1; x++)
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

        private static Room Generate(Level level, Rectangle rectangle)
        {
            for (var x = rectangle.Position.X; x < rectangle.Position.X + rectangle.Width; x++)
            {
                for (var y = rectangle.Position.Y; y < rectangle.Position.Y + rectangle.Height; y++)
                {
                    if (x == rectangle.Position.X || y == rectangle.Position.Y || x == rectangle.Position.X + rectangle.Width - 1 || y == rectangle.Position.Y + rectangle.Height - 1)
                    {
                        level.TilesetGrid[x, y].TileType = TileType.Wall;
                    }
                    else
                    {
                        level.TilesetGrid[x, y].TileType = TileType.Floor;
                    }
                }
            }
            return new Room()
            {
                BoundingBox = new Rectangle()
                {
                    Height = rectangle.Height,
                    Width = rectangle.Width,
                    Position = new Point(rectangle.Position.X, rectangle.Position.Y)
                }
            };
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

        private static Rectangle FindNextRoomRect(Level level, out Room previousRoom)
        {
            var width = UnityEngine.Random.Range(5, 9);
            var height = UnityEngine.Random.Range(5, 9);

            if (level.Rooms.Count == 0)
            {
                previousRoom = null;
                return new Rectangle()
                {
                    Position = new Point((level.BoundingBox.Width / 4) + UnityEngine.Random.Range(0, 3), (level.BoundingBox.Height / 4) + UnityEngine.Random.Range(0, 3)),
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
                var rects = FindValidSurroundingRectangles(room.BoundingBox, guage, level.BoundingBox);
                rects = rects.FindAll((possibleRect) =>
                {
                    return level.Rooms.FindAll((roomInLevel) => roomInLevel.BoundingBox.Adjacent(possibleRect) || roomInLevel.BoundingBox.Intersects(possibleRect)).Count == 0;
                });
                if (rects.Count > 0)
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
