using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class SpriteSortingSystem
    {
        public ApplicationContext Context { get; set; }
        private SpriteRenderer[,] Tiles;
        private static List<Direction> HigherSortingPositions = new List<Direction>() { Direction.East, Direction.SouthEast, Direction.South, Direction.SouthWest };
        private static List<Direction> LowerSortingPositions = new List<Direction>() { Direction.West, Direction.NorthWest, Direction.North, Direction.NorthEast };
        private ListGrid<Entity> NotMovingEntitys;
        private ListGrid<Entity> MovingEntitys;

        public SpriteSortingSystem() { }

        public void Init()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            Tiles = new SpriteRenderer[level.BoundingBox.Width, level.BoundingBox.Height];
            MovingEntitys = new ListGrid<Entity>(level.BoundingBox.Width, level.BoundingBox.Height);
            NotMovingEntitys = new ListGrid<Entity>(level.BoundingBox.Width, level.BoundingBox.Height);
        }

        public void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position)
        {
            Tiles[position.X, position.Y] = tileSpriteRenderer;
        }

        public void Process()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            foreach (var entity in level.Entitys)
            {
                if (entity.Motor != null && entity.Motor.MoveTargetPosition != entity.Position &&
                                entity.Motor.MoveTargetPosition != null)
                {
                    MovingEntitys[entity.Motor.MoveTargetPosition].Add(entity);
                }
                else
                {
                    NotMovingEntitys[entity.Position].Add(entity);
                }
            }

            var sortOrdersPerTile = 20;

            for (var x = 0; x < level.BoundingBox.Width; x++)
            {
                for (var y = 0; y < level.BoundingBox.Height; y++)
                {
                    var sortOrder = (y * (level.BoundingBox.Width * sortOrdersPerTile) + (x * sortOrdersPerTile));
                    var tileSpriteRenderer = Tiles[x, y];
                    if (tileSpriteRenderer != null)
                    {
                        tileSpriteRenderer.sortingOrder = sortOrder;
                        sortOrder++;
                    }
                    if (Context.OverlaySystem != null)
                    {
                        var tiles = Context.OverlaySystem.GetTilesInPosition(x, y);
                        foreach (var tile in tiles)
                        {
                            tile.sortingOrder = sortOrder;
                            sortOrder++;
                        }
                    }
                    sortOrder++;
                    if (level.EntityGrid != null)
                    {
                        foreach (var entity in NotMovingEntitys[x, y])
                        {
                            sortOrder = sortEntity(sortOrder, entity);
                        }

                        foreach (var entity in MovingEntitys[x, y])
                        {
                            sortOrder = sortEntity(sortOrder, entity);
                        }
                    }
                    NotMovingEntitys[x, y].Clear();
                    MovingEntitys[x, y].Clear();
                }
            }
        }

        private static int sortEntity(int sortOrder, Entity entity)
        {
            if (entity.View.ViewType == ViewType.StaticSprite)
            {
                if (entity.View != null&& entity.View.ViewGameObject != null)
                {
                    var spriteRenderer = entity.View.ViewGameObject.GetComponent<SpriteRenderer>();
                    spriteRenderer.sortingOrder = sortOrder;
                    return sortOrder + 1;
                }
                return sortOrder;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
