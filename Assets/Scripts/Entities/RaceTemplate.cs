using System.Collections.Generic;

namespace Gamepackage
{
    public class RaceTemplate
    {
        public string Identifier;
        public string Name;
        public bool IsCombatant;
        public string DefaultWeaponIdentifier;
        public bool BlocksPathing;
        public string DefaultViewTemplateIdentifier;
        public string DefaultAIClassName;
        public bool IsAlwaysVisible;
        public string Trigger;
        public FloatingState isFloating;
        public ShadowCastState CastsShadow;
        public Dictionary<Attributes, int> TemplateAttributes;
    }
}
