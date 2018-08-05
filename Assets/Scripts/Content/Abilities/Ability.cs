using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Ability : Action
    {
        public abstract bool CanApply(AbilityContext abilityTriggerContext);
        public abstract AbilityContext Apply(AbilityContext abilityTriggerContext);

        public abstract string DisplayName
        {
            get;
        }
        public abstract string Description
        {
            get;
        }

        [JsonIgnore]
        public abstract TriggerType TriggeredBy
        {
            get;
        }
    }
}
