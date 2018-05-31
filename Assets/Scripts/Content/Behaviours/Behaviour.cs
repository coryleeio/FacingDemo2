using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Behaviour : IHasApplicationContext
    {
        public abstract List<EntityAction> GetNextActions();

        public virtual bool IsPlayer
        {
            get
            {
                return false;
            }
        }

        [JsonIgnore]
        protected ApplicationContext Context;
        public void InjectContext(ApplicationContext context)
        {
            Context = context;
        }
    }
}
