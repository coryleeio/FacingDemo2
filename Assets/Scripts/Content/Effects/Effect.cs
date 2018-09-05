using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Effect
    {
        public Duration Ticker;

        public abstract bool CanTrigger(AttackContext abilityTriggerContext);
        public abstract AttackContext Trigger(AttackContext abilityTriggerContext);

        public virtual void Tick(Entity owner)
        {
            if(IsTickingEffect)
            {
                Ticker.Tick(owner);
            }
        }

        public bool IsTickingEffect
        {
            get
            {
                return Ticker != null;
            }
        }

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

        public virtual void OnRemove()
        {

        }

        public abstract string RemovalText
        {
            get;
        }
    }
}
