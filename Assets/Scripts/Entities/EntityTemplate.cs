using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityTemplate
    {
        public string Identifier;
        public ProbabilityTable NameList;
        public string ViewTemplateIdentifier;
        public List<string> EquipmentTables;
        public List<string> InventoryTables;
        public string EntityTypeIdentifier;
        public int Level;
        public string TriggerTemplateIdentifier;
        public bool BlocksPathing;
    }
}
