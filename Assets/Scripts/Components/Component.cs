using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Component
    {
        [JsonIgnore]
        public Token Owner;
    }
}
