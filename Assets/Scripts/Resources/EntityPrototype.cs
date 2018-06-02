using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public bool BlocksPathing { get; set; }
        public Body Body;
        public Trigger Trigger;
        public Motor Motor;
        public View ViewComponent;
        public Behaviour TurnComponent;
    }
}
