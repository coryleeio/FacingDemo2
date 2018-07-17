using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Ability : Action
    {
        public abstract bool CanPerform(AbilityTriggerContext abilityTriggerContext);
        public abstract AbilityTriggerContext Perform(AbilityTriggerContext abilityTriggerContext);

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
