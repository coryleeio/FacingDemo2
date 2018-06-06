using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component
    {
        [JsonIgnore]
        public Entity Entity;

        public virtual void InjectContext(Entity entity)
        {
            Entity = entity;
        }
    }
}
