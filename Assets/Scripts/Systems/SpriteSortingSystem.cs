using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class SpriteSortingSystem
    {
        private SpriteRenderer[,] Tiles;

        public SpriteSortingSystem() { }

        public void Init()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            Tiles = new SpriteRenderer[level.BoundingBox.Width, level.BoundingBox.Height];
        }

        public void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position)
        {
            Tiles[position.X, position.Y] = tileSpriteRenderer;
        }

        public void Process()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            var sortOrdersPerTile = 20;

            for (var x = 0; x < level.BoundingBox.Width; x++)
            {
                for (var y = 0; y < level.BoundingBox.Height; y++)
                {
                    if (level.Grid[x, y].TileType == TileType.Floor)
                    {
                        var tileSpriteRenderer = Tiles[x, y];
                        tileSpriteRenderer.sortingOrder = -1; // floor tiles always go in the back
                    }
                }
            }

            for (var x = 0; x < level.BoundingBox.Width; x++)
            {
                for (var y = 0; y < level.BoundingBox.Height; y++)
                {
                    var sortOrder = (y * (level.BoundingBox.Width * sortOrdersPerTile) + (x * sortOrdersPerTile));
                    var tileSpriteRenderer = Tiles[x, y];

                    if (tileSpriteRenderer != null && level.Grid[x, y].TileType != TileType.Floor)
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
                    if (level.Grid != null)
                    {
                        foreach(var entity in level.Grid[x,y].EntitiesInPosition)
                        {
                            sortOrder = SortEntity(sortOrder, entity);
                        }
                    }
                }
            }
        }

        private static int SortEntity(int sortOrder, Entity entity)
        {
            if (entity.View.ViewType == ViewType.StaticSprite)
            {
                if (entity.View != null && entity.View.ViewGameObject != null)
                {
                    var spriteRenderer = entity.View.ViewGameObject.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.sortingOrder = sortOrder;
                        return sortOrder + 1;
                    }
                }
                return sortOrder;
            }
            else if (entity.View.ViewType == ViewType.MultipleStaticSprites)
            {
                if (entity.View != null && entity.View.ViewGameObject != null)
                {
                    var spriteRenderers = entity.View.ViewGameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var renderer in spriteRenderers)
                    {
                        renderer.sortingOrder = sortOrder;
                        sortOrder++;
                    }
                    return sortOrder;
                }
                return sortOrder;
            }
            else if (entity.View.ViewType == ViewType.Spine)
            {
                if (entity.View != null && entity.View.ViewGameObject != null)
                {
                    var meshRenderer = entity.View.ViewGameObject.GetComponentInChildren<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.sortingOrder = sortOrder;
                        sortOrder++;
                        return sortOrder;
                    }
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
