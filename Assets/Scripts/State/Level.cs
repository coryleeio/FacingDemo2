using System.Collections.Generic;

namespace Gamepackage
{
    public class Level
    {
        public Rectangle Domain;
        public MapVisibilityState[,] VisibilityGrid;
        public TileInfo[,] TilesetGrid;

        public List<Token> Tokens;

        public Level()
        {
            
        }

        public TileInfo GetTileInfo(Point p)
        {
            return TilesetGrid[p.X, p.Y];
        }
    }
}