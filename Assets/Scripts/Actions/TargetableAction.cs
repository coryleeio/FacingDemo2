using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class TargetableAction : Action
    {
        [JsonIgnore]
        public Entity Source;

        [JsonIgnore]
        public List<Entity> Targets = new List<Entity>(0);
    }
}
