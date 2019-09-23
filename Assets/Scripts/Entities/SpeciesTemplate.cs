using System.Collections.Generic;

namespace Gamepackage
{
    public class SpeciesTemplate
    {
        public string Identifier;
        public string Name;
        public string DefaultWeaponIdentifier;
        public string DefaultAIClassName;
        public Dictionary<Attributes, int> TemplateAttributes;
    }
}
