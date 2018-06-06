using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component
    {
        [JsonIgnore]
        public Entity Entity;

        public virtual void Rereference(Entity entity)
        {
            Entity = entity;
        }
    }
}
