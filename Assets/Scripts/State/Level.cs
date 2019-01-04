using Newtonsoft.Json;
using System.Collections.Generic;
namespace Gamepackage
{
    public class Level
    {
        public int LevelIndex;
        public Rectangle BoundingBox;
        public List<Entity> Entitys;
        public List<Room> Rooms = new List<Room>(0);
        public Grid<Tile> Grid;

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
            return Grid[p.X, p.Y];
        }

        public void UnindexAll()
        {
            foreach(var entity in Entitys)
            {
                UnindexEntity(entity, entity.Position);
            }
        }

        public void UnindexEntity(Entity entity, Point oldPosition)
        {
            if(Grid[oldPosition.X, oldPosition.Y].EntitiesInPosition.Contains(entity))
            {
                Grid[oldPosition.X, oldPosition.Y].EntitiesInPosition.Remove(entity);
            }
        }

        public void IndexEntity(Entity entity, Point newPosition)
        {
            if (!Grid[newPosition.X, newPosition.Y].EntitiesInPosition.Contains(entity))
            {
                Grid[newPosition.X, newPosition.Y].EntitiesInPosition.Add(entity);
            }
        }
    }
}