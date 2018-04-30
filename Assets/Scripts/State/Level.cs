using System.Collections.Generic;

namespace Gamepackage
{
    public class Level : IResolvableReferences
    {
        public int LevelIndex;
        public Rectangle Domain;
        public MapVisibilityState[,] VisibilityGrid;
        public TileInfo[,] TilesetGrid;
        public List<Token> Tokens;
        public PrototypeReference<LevelPrototype> LevelPrototypeReference;
        public List<Room> Rooms = new List<Room>(0);

        public Level()
        {
            
        }

        public TileInfo GetTileInfo(Point p)
        {
            return TilesetGrid[p.X, p.Y];
        }

        public void Resolve(IResourceManager resourceManager)
        {
            LevelPrototypeReference.Resolve(resourceManager);
        }
    }
}