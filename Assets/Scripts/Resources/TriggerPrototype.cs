using System.Collections.Generic;

namespace Gamepackage
{
    public class TriggerPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public TriggerAction TriggerAction { get; set; }
        public Dictionary<string, string> Parameters = new Dictionary<string, string>();
    }
}
