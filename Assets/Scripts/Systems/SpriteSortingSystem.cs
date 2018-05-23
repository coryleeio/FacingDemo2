using UnityEngine;

namespace Gamepackage
{
    public class SpriteSortingSystem
    {
        public ApplicationContext ApplicationContext { get; set; }
        private SpriteRenderer[,] Tiles;

        public SpriteSortingSystem()
        {

        }

        public void Init()
        {
            var level = ApplicationContext.GameStateManager.Game.CurrentLevel;
            Tiles = new SpriteRenderer[level.BoundingBox.Width, level.BoundingBox.Height];
        }

        public void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position)
        {
            Tiles[position.X, position.Y] = tileSpriteRenderer;
        }

        public void Process()
        {
            var level = ApplicationContext.GameStateManager.Game.CurrentLevel;
            var sortOrder = 0;
            for (var x = 0; x < level.BoundingBox.Width; x++)
            {
                for (var y = 0; y < level.BoundingBox.Height; y++)
                {
                    var tileSpriteRenderer = Tiles[x, y];
                    if (tileSpriteRenderer != null)
                    {
                        tileSpriteRenderer.sortingOrder = sortOrder;
                        sortOrder++;
                    }
                    if (ApplicationContext.OverlaySystem != null)
                    {
                        var tiles = ApplicationContext.OverlaySystem.GetTilesInPosition(x, y);
                        foreach (var tile in tiles)
                        {
                            tile.sortingOrder = sortOrder;
                            sortOrder++;
                        }
                    }
                    if (level.TokenGrid != null)
                    {
                        foreach (var token in level.TokenGrid[x, y])
                        {
                            if(token.ViewType == ViewType.StaticSprite)
                            {
                                var spriteRenderer = token.View.GetComponent<SpriteRenderer>();
                                spriteRenderer.sortingOrder = sortOrder;
                                sortOrder++;
                            }
                            else
                            {
                                throw new NotImplementedException();
                            }
                        }
                    }
                }
            }
        }
    }
}
