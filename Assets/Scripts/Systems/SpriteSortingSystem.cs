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
        private ListGrid<Token> NotMovingTokens;
        private ListGrid<Token> MovingTokens;
        public SpriteSortingSystem()
        {

        }

        public void Init()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            Tiles = new SpriteRenderer[level.BoundingBox.Width, level.BoundingBox.Height];
            MovingTokens = new ListGrid<Token>(level.BoundingBox.Width, level.BoundingBox.Height);
            NotMovingTokens = new ListGrid<Token>(level.BoundingBox.Width, level.BoundingBox.Height);
        }

        public void RegisterTile(SpriteRenderer tileSpriteRenderer, Point position)
        {
            Tiles[position.X, position.Y] = tileSpriteRenderer;
        }

        public void Process()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            foreach (var token in level.Tokens)
            {
                if (token.TargetPosition != token.Position &&
                                token.TargetPosition != null)
                {
                    MovingTokens[token.Position].Add(token);
                }
                else
                {
                    NotMovingTokens[token.Position].Add(token);
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
                    if (level.TokenGrid != null)
                    {
                        foreach (var token in NotMovingTokens[x,y])
                        {
                            sortOrder = sortToken(sortOrder, token);
                        }

                        foreach (var token in MovingTokens[x,y])
                        {
                            sortOrder = sortToken(sortOrder, token);
                        }
                    }
                    NotMovingTokens[x,y].Clear();
                    MovingTokens[x,y].Clear();
                }
            }
        }

        private static int sortToken(int sortOrder, Token token)
        {
            if (token.ViewType == ViewType.StaticSprite)
            {
                var spriteRenderer = token.View.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = sortOrder;
                return sortOrder + 1;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
