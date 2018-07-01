using KDSharp.KDTree;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityManager
    {
        public EntityManager() { }

        private Dictionary<int, Entity> EntityMap = new Dictionary<int, Entity>();
        public KDTree<Entity> PlayerTeamTree = new KDTree<Entity>(2, 5);
        public KDTree<Entity> EnemyTeamTree = new KDTree<Entity>(2, 5);

        public void Register(Entity entity, Level level)
        {
            if (entity.Id == 0)
            {
                entity.Id = ServiceLocator.GameStateManager.Game.NextId;
            }
            if (!EntityMap.ContainsKey(entity.Id))
            {
                EntityMap.Add(entity.Id, entity);
            }
            if (!level.Entitys.Contains(entity))
            {
                level.Entitys.Add(entity);
            }
            entity.Rewire();
            level.IndexEntity(entity, entity.Position);
            if (entity.Behaviour != null)
            {
                if (entity.Behaviour.Team == Team.PLAYER)
                {
                    PlayerTeamTree.AddPoint(new double[] { entity.Position.X, entity.Position.Y }, entity);
                }
                else if (entity.Behaviour.Team == Team.ENEMY)
                {
                    EnemyTeamTree.AddPoint(new double[] { entity.Position.X, entity.Position.Y }, entity);
                }
            }
        }

        public void Clear()
        {
            EntityMap.Clear();
            PlayerTeamTree.Clear();
            EnemyTeamTree.Clear();
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
            if (entity.Behaviour != null)
            {
                if (entity.Behaviour.Team == Team.PLAYER)
                {
                    PlayerTeamTree.Remove(entity);
                }
                else if (entity.Behaviour.Team == Team.ENEMY)
                {
                    EnemyTeamTree.Remove(entity);
                }
            }
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