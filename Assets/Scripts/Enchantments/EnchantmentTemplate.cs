using System.Collections.Generic;

namespace Gamepackage
{
    public class EnchantmentTemplate
    {
        public string Identifier;
        public string NameModifier;
        public int MinCharges;
        public int MaxCharges;
        public Dictionary<CombatActionType, CombatActionDescriptor> CombatActionDescriptor;
        public List<string> WornEffects;
    }
}
