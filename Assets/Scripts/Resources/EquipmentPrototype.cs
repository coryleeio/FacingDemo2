using System.Collections.Generic;

namespace Gamepackage
{
    public class EquipmentPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public string ClassName { get; set; }
        public List<EquipmentTable> EquipmentTables = new List<EquipmentTable>(0);
    }
}
