using System.Collections.Generic;

namespace Gamepackage
{
    public class EntityTemplate
    {
        public string Identifier;
        public ProbabilityTable NameList;
        public bool IsCombatant;
        public string DefaultWeaponIdentifier;
        public bool BlocksPathing;
        public string ViewTemplateIdentifier;
        public string AIClassName;
        public bool IsAlwaysVisible;
        public string Trigger;
        public FloatingState isFloating;
        public ShadowCastState CastsShadow;

        public Dictionary<Attributes, int> TemplateAttributes;
        public List<string> EquipmentTables;
        public List<string> InventoryTables;
    }
}
