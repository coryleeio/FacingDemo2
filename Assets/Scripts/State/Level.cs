using Newtonsoft.Json;
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
        public List<Token>[,] TokenGrid;

        [JsonIgnore]
        private Token _player;

        [JsonIgnore]
        public Token Player
        {
            get
            {
                if (_player == null)
                {
                    _player = Tokens.Find((t) => t.IsPlayer());
                }
                return _player;
            }
        }

        public Level()
        {
            
        }

        public TileInfo GetTileInfo(Point p)
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

        public void Resolve(TinyIoCContainer container)
        {
            TokenGrid = new List<Token>[TilesetGrid.GetLength(0), TilesetGrid.GetLength(1)];
            for(var x = 0; x < TokenGrid.GetLength(0); x++)
            {
                for(var y= 0; y < TilesetGrid.GetLength(1); y++)
                {
                    TokenGrid[x, y] = new List<Token>(0);
                }
            }
            foreach (var token in Tokens)
            {
                token.Resolve(container);
                token.Level = this;
            }
        }
    }
}