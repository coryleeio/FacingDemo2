using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamepackage
{
    public class TokenPrototype : IPrototype
    {
        public string UniqueIdentifier { get; set; }
        public string BehaviourClassName { get; set; }
        public string EquipmentClassName { get; set; }
        public string InventoryClassName { get; set; }
        public string MotorClassName { get; set; }
        public string PersonaClassName { get; set; }
        public string TriggerBehaviourClassName { get; set; }
        public string ViewClassName { get; set; }
    }
}
