using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public bool BlocksPathing { get; set; }
        public List<Traits> Traits = new List<Traits>(0);
        public AIBehaviourType BehaviorIdentifier;
        public ViewType ViewType;
        public UniqueIdentifier ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED;
        public UniqueIdentifier TriggerUniqueIdentifier = UniqueIdentifier.TRIGGER_NONE;
        public int StartingMaxHealth;
    }
}
