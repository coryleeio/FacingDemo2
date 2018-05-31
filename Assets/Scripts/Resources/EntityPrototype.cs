using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public bool BlocksPathing { get; set; }
        public CombatantComponent CombatantComponent;
        public TriggerComponent TriggerComponent;
        public MovementComponent MovementComponent;
        public ViewComponent ViewComponent;
        public TurnComponent TurnComponent;
    }
}
