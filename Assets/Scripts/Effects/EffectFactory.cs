using System.Collections.Generic;

namespace Gamepackage
{
    public static class EffectFactory
    {
        public static Effect Build(string templateIdentifier)
        {
            var template = Context.ResourceManager.Load<EffectTemplate>(templateIdentifier);
            var effect = new Effect
            {
                TemplateIdentifier = template.Identifier,
                effectImplClassName = template.EffectImplClassName,
                TurnsRemaining = template.Duration,
                Data = new Dictionary<string, string>(),
                Attributes = new Dictionary<Attributes, int>(),
            };

            foreach (var pair in template.TemplateData)
            {
                effect.Data.Add(pair.Key, pair.Value);
            }

            foreach (var pair in template.TemplateAttributes)
            {
                effect.Attributes.Add(pair.Key, pair.Value);
            }
            return effect;
        }

        public static List<Effect> BuildAll(List<string> templateIdentifiers)
        {
            var retval = new List<Effect>();
            foreach (var templateIdentifier in templateIdentifiers)
            {
                retval.Add(EffectFactory.Build(templateIdentifier));
            }
            return retval;
        }
    }
}
