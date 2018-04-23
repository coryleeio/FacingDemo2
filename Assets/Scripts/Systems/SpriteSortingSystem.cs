using UnityEngine;

namespace Gamepackage
{
    public class SpriteSortingSystem : ISpriteSortingSystem
    {
        public ILogSystem _logSystem;
        public SpriteRenderer[,] Tiles;
        public IGameStateSystem _gameStateSystem;

        public SpriteSortingSystem(ILogSystem logSystem, IGameStateSystem gameStateSystem)
        {
            _logSystem = logSystem;
            _gameStateSystem = gameStateSystem;
        }

        public void Init()
        {
            var level = _gameStateSystem.Game.CurrentLevel;
            Tiles = new SpriteRenderer[level.Domain.Width, level.Domain.Height];
        }

        public void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position)
        {
            Tiles[position.X, position.Y] = tileSpriteRenderer;
        }

        public void Sort()
        {
            var level = _gameStateSystem.Game.CurrentLevel;
            var sortOrder = 0;
            for(var x = 0; x < level.Domain.Width; x++)
            {
                for(var y= 0; y < level.Domain.Height; y++)
                {
                    var tileSpriteRenderer = Tiles[x, y];
                    if(tileSpriteRenderer != null)
                    {
                        tileSpriteRenderer.sortingOrder = sortOrder;
                        sortOrder = sortOrder + 1;
                    }
                }
            }
        }
    }
}
