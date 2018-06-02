using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component : IHasApplicationContext
    {
        protected ApplicationContext Context;

        [JsonIgnore]
        public Entity Entity;

        public virtual void InjectContext(ApplicationContext context)
        {
            Context = context;
        }
    }
}
