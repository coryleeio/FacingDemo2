using UnityEngine;

namespace Gamepackage
{
    public class SpriteSortingSystem : ISpriteSortingSystem
    {
        public ILogSystem LogSystem { get; set; }
        public IGameStateSystem GameStateSystem { get; set; }
        public IOverlaySystem OverlaySystem { get; set; }
        private SpriteRenderer[,] Tiles;

        public SpriteSortingSystem()
        {

        }

        public void Init()
        {
            var level = GameStateSystem.Game.CurrentLevel;
            Tiles = new SpriteRenderer[level.Domain.Width, level.Domain.Height];
        }

        public void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position)
        {
            Tiles[position.X, position.Y] = tileSpriteRenderer;
        }

        public void Sort()
        {
            var level = GameStateSystem.Game.CurrentLevel;
            var sortOrder = 0;
            for(var x = 0; x < level.Domain.Width; x++)
            {
                for(var y= 0; y < level.Domain.Height; y++)
                {
                    var tileSpriteRenderer = Tiles[x, y];
                    if(tileSpriteRenderer != null)
                    {
                        tileSpriteRenderer.sortingOrder = sortOrder;
                        sortOrder++;
                    }
                    if(OverlaySystem != null)
                    {
                        var tiles = OverlaySystem.GetTilesInPosition(x, y);
                        foreach(var tile in tiles)
                        {
                            tile.sortingOrder = sortOrder;
                            sortOrder++;
                        }
                    }
                    if(level.TokenGrid != null)
                    {
                        foreach(var token in level.TokenGrid[x,y])
                        {
                            token.TokenView.SortOrder = sortOrder;
                            sortOrder++;
                        }
                    }
                }
            }
        }
    }
}
