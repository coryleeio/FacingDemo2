using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }

        public bool BlocksPathing { get; set; }
        public List<Traits> Traits = new List<Traits>(0);

        public CombatantComponent CombatantComponent;
        public TriggerComponent TriggerComponent;
        public MovementComponent MovementComponent;
        public ViewComponent ViewComponent;
    }
}
