using System.Collections.Generic;

namespace Gamepackage
{
    public class TriggerStepContext : AbilityTriggerContext
    {
        public Entity Source;
        public List<Entity> Targets = new List<Entity>(0);
    }
}
