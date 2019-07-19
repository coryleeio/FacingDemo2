using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityTemplate
    {
        public string Identifier;
        public ProbabilityTable NameList;
        public string ViewTemplateIdentifierOverride;
        public List<string> EquipmentTables;
        public List<string> InventoryTables;
        public string RaceIdentifier;
        public int Level;
    }
}
