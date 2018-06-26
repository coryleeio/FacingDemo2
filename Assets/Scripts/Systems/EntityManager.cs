using KDSharp.KDTree;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityManager
    {
        public EntityManager() {}

        private Dictionary<int, Entity> EntityMap = new Dictionary<int, Entity>();
        public KDTree<Entity> Tree = new KDTree<Entity>(2, 5);

        public void Register(Entity entity, Level level)
        {
            if(entity.Id == 0)
            {
                entity.Id = ServiceLocator.GameStateManager.Game.NextId;
            }
            if(!EntityMap.ContainsKey(entity.Id))
            {
                EntityMap.Add(entity.Id, entity);
            }
            if(!level.Entitys.Contains(entity))
            {
                level.Entitys.Add(entity);
            }
            entity.Rewire();
            level.IndexEntity(entity, entity.Position);
            Tree.AddPoint(new double[] { entity.Position.X, entity.Position.Y }, entity);
        }

        public void Clear()
        {
            EntityMap.Clear();
            Tree.Clear();
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
            Tree.Remove(entity);
        }

        public void Init()
        {
            Clear();
            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            foreach (var entity in level.Entitys)
            {
                ServiceLocator.EntitySystem.Register(entity, level);
            }
        }
    }
}