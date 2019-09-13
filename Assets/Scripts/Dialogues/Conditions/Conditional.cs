using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    public class Conditional
    {
        public string conditionalImplClassName;
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();

        [JsonIgnore]
        private ConditionalImpl _conditionalImpl;
        [JsonIgnore]
        public ConditionalImpl ConditionalImpl
        {
            get
            {
                if (_conditionalImpl == null)
                {
                    _conditionalImpl = Context.ResourceManager.CreateInstanceFromAbstractOrInterfaceTypeAndName(typeof(ConditionalImpl), conditionalImplClassName) as ConditionalImpl;
                }
                return _conditionalImpl;
            }
        }
    }
}
