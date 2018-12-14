using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component
    {
        [JsonIgnore]
        public Entity entity;

        public virtual void Rewire(Entity entity)
        {
            this.entity = entity;
        }
    }
}
