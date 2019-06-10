using System.Collections.Generic;

namespace Gamepackage
{
    public class EffectTemplate
    {
        public string Identifier;
        public string LocalizationPrefix;
        public string EffectImplClassName;
        public bool HasUnlimitedDuration;
        public int Duration;
        public StackingStrategy StackingStrategy;

        public List<string> TagsThatBlockThisEffect;
        public List<string> TagsAppliedToEntity;
        public Dictionary<string, string> TemplateData;
        public Dictionary<Attributes, int> TemplateAttributes;

    }
}
