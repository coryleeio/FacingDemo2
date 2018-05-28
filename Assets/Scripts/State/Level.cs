using Newtonsoft.Json;
using System.Collections.Generic;
using TinyIoC;

namespace Gamepackage
{
    public class Level : IHasApplicationContext
    {
        public int LevelIndex;
        public Rectangle BoundingBox;
        public List<Entity> Entitys;
        public List<Room> Rooms = new List<Room>(0);
        public Grid<MapVisibilityState> VisibilityGrid;
        public Grid<Tile> TilesetGrid;

        [JsonIgnore]
        public ListGrid<Entity> EntityGrid;

        [JsonIgnore]
        private Entity _player;

        [JsonIgnore]
        public Entity Player
        {
            get
            {
                if (_player == null)
                {
                    _player = Entitys.Find((t) => t.IsPlayer);
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


        public void UnindexEntity(Entity entity, Point oldPosition)
        {
            if(EntityGrid[oldPosition.X, oldPosition.Y].Contains(entity))
            {
                EntityGrid[oldPosition.X, oldPosition.Y].Remove(entity);
            }
        }

        public void IndexEntity(Entity entity, Point newPosition)
        {
            if (!EntityGrid[newPosition.X, newPosition.Y].Contains(entity))
            {
                EntityGrid[newPosition.X, newPosition.Y].Add(entity);
            }
        }

        private ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
            foreach(var entity in Entitys)
            {
                entity.InjectContext(context);
            }
        }
    }
}