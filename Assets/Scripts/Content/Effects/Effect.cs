using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Effect
    {
        public Ticker Ticker;
        public IStackingStrategy StackingStrategy;

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

        public virtual void OnAdd(Entity owner)
        {
            AddVisualEffects(owner);
        }

        protected virtual void AddVisualEffects(Entity owner)
        {

        }

        public virtual void OnRemove(Entity owner)
        {
            RemoveVisualEffects(owner);
        }

        protected virtual void RemoveVisualEffects(Entity owner)
        {

        }

        public abstract string RemovalText
        {
            get;
        }
    }
}
