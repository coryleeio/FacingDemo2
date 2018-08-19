using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Effect
    {
        public abstract bool CanApply(AttackContext abilityTriggerContext);
        public abstract AttackContext Apply(AttackContext abilityTriggerContext);

        public abstract string DisplayName
        {
            get;
        }
        public abstract string Description
        {
            get;
        }

        [JsonIgnore]
        public abstract EffectTriggerType EffectApplicationTrigger
        {
            get;
        }
    }
}
