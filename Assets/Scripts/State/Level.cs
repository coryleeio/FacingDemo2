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
        public Grid<Tile> GridWithoutPlayerUnits;

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

        public void UpdatePathfindingForEntity(Entity entity)
        {
            Grid[entity.Position].Walkable = !entity.BlocksPathing;
            if(!entity.IsCombatant || entity.ActingTeam != Team.Player)
            {
                GridWithoutPlayerUnits[entity.Position].Walkable = !entity.BlocksPathing;
            }
        }

        public void ReleasePathfindingAtPosition(Entity entity, Point oldPosition)
        {
            if(entity.BlocksPathing)
            {
                Grid[oldPosition].Walkable = true;
                GridWithoutPlayerUnits[oldPosition].Walkable = true;
            }
        }
    }
}