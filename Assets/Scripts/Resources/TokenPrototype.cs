using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public bool BlocksPathing { get; set; }
        public List<string> Tags = new List<string>(0);
        public AIBehaviourType BehaviorIdentifier;
        public ViewType ViewType;
        public UniqueIdentifier TriggerPrototypeUniqueIdentifier;
        public UniqueIdentifier ViewUniqueIdentifier;
    }
}
