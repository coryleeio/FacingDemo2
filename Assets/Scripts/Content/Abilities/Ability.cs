using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Ability : Action
    {
        public abstract bool CanPerform
        {
            get;
        }

        [JsonIgnore]
        public abstract TriggerType TriggeredBy
        {
            get;
        }

        public enum TriggerType
        {
            OnTriggerStep,
        }
    }
}
