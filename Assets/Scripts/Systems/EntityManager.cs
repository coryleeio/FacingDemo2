using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityManager
    {
        public ApplicationContext Context { get; set; }

        public EntityManager() {}

        private Dictionary<int, Entity> EntityMap = new Dictionary<int, Entity>();

        public void Register(Entity entity, Level level)
        {
            if(entity.Id == 0)
            {
                entity.Id = Context.GameStateManager.Game.NextId;
            }
            if(!EntityMap.ContainsKey(entity.Id))
            {
                EntityMap.Add(entity.Id, entity);
            }
            if(!level.Entitys.Contains(entity))
            {
                level.Entitys.Add(entity);
            }
            level.IndexEntity(entity, entity.Position);
        }

        public void Clear()
        {
            EntityMap.Clear();
        }

        public Entity GetEntityById(int id)
        {
            if(!EntityMap.ContainsKey(id))
            {
                return null;
            }
            return EntityMap[id];
        }

        public void Deregister(Entity entity, Level level)
        {
            if(EntityMap.ContainsKey(entity.Id))
            {
                EntityMap.Remove(entity.Id);
            }
            if (level.Entitys.Contains(entity))
            {
                level.Entitys.Remove(entity);
            }
            level.UnindexEntity(entity, entity.Position);
        }
    }
}