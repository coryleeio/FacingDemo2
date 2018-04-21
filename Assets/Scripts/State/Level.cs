using System.Collections.Generic;

namespace Gamepackage
{
    public class Level
    {
        public Rectangle Domain;
        public MapVisibilityState[,] VisibilityGrid;
        public List<Token> Tokens;

        public Level()
        {
            
        }
    }
}