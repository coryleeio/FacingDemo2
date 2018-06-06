using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class TriggerAction : ASyncAction
    {
        public abstract List<Point> Offsets
        {
            get;
        }
        public List<int> TargetIds = new List<int>(0);

        [JsonIgnore]
        public List<Entity> Targets
        {
            get
            {
                var list = new List<Entity>(TargetIds.Count);
                foreach(var id in TargetIds)
                {
                    var entity = Context.EntitySystem.GetEntityById(id);
                    list.Add(entity);
                }
                return list;
            }
        }

        public override void Reset()
        {
            base.Reset();
            TargetIds.Clear();
        }

        public void InjectContext(Entity entity)
        {
            Entity = entity;
        }
    }
}
