using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityManager
    {
        public EntityManager() { }

        private Dictionary<int, Entity> EntityMap = new Dictionary<int, Entity>();

        public void Register(Entity entity, Level level)
        {
            if (entity.Id == 0)
            {
                entity.Id = Context.Game.NextId;
            }
            if (!EntityMap.ContainsKey(entity.Id))
            {
                EntityMap.Add(entity.Id, entity);

                // Ensures that if we are loading and have added a mod with new skills 
                // that they get properly populated.
                SkillUtil.PopulateSkills(entity);
            }
            if (!level.Entitys.Contains(entity))
            {
                level.Entitys.Add(entity);
            }
            level.IndexEntity(entity, entity.Position);
            level.UpdatePathfindingForEntity(entity);
        }

        public void Clear()
        {
            EntityMap.Clear();
        }

        public Entity GetEntityById(int id)
        {
            if (!EntityMap.ContainsKey(id))
            {
                return null;
            }
            return EntityMap[id];
        }

        public void Deregister(Entity entity, Level level)
        {
            if (EntityMap.ContainsKey(entity.Id))
            {
                EntityMap.Remove(entity.Id);
            }
            if (level.Entitys.Contains(entity))
            {
                level.Entitys.Remove(entity);
            }
            level.UnindexEntity(entity, entity.Position);
            level.ReleasePathfindingAtPosition(entity, entity.Position);
        }

        public void Init()
        {
            Clear();
            var level = Context.Game.CurrentLevel;
            foreach (var entity in level.Entitys)
            {
                Context.EntitySystem.Register(entity, level);
            }
        }
    }
}