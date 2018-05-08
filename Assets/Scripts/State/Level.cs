using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class Level : IResolvableReferences
    {
        public int LevelIndex;
        public Rectangle Domain;
        public MapVisibilityState[,] VisibilityGrid;
        public TileInfo[,] TilesetGrid;
        public List<Token> Tokens;
        public List<Room> Rooms = new List<Room>(0);

        public Level()
        {
            
        }

        public TileInfo GetTileInfo(Point p)
        {
            return TilesetGrid[p.X, p.Y];
        }

        public void Resolve(TinyIoCContainer container)
        {
            foreach(var token in Tokens)
            {
                token.Resolve(container);
            }
        }
    }
}