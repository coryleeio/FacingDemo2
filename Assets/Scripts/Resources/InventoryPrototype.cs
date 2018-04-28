using System.Collections.Generic;

namespace Gamepackage
{
    public class InventoryPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public string ClassName { get; set; }
        public List<InventoryTable> InventoryTables = new List<InventoryTable>(0);
    }
}
