using Newtonsoft.Json;
using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class Level
    {
        public int LevelIndex;
        public Rectangle BoundingBox;
        public List<Token> Tokens;
        public List<Room> Rooms = new List<Room>(0);
        public Grid<MapVisibilityState> VisibilityGrid;
        public Grid<Tile> TilesetGrid;

        [JsonIgnore]
        public ListGrid<Token> TokenGrid;

        [JsonIgnore]
        private Token _player;

        [JsonIgnore]
        public Token Player
        {
            get
            {
                if (_player == null)
                {
                    _player = Tokens.Find((t) => t.IsPlayer);
                }
                return _player;
            }
        }

        public Level()
        {
            
        }

        public Tile GetTileInfo(Point p)
        {
            return TilesetGrid[p.X, p.Y];
        }


        public void UnindexToken(Token token, Point oldPosition)
        {
            if(TokenGrid[oldPosition.X, oldPosition.Y].Contains(token))
            {
                TokenGrid[oldPosition.X, oldPosition.Y].Remove(token);
            }
        }

        public void IndexToken(Token token, Point newPosition)
        {
            if (!TokenGrid[newPosition.X, newPosition.Y].Contains(token))
            {
                TokenGrid[newPosition.X, newPosition.Y].Add(token);
            }
        }
    }
}