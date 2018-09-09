using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Effect
    {
        public Ticker Ticker;
        public IStackingStrategy StackingStrategy;

        public abstract bool CanTrigger(EntityStateChange abilityTriggerContext);
        public abstract EntityStateChange Trigger(EntityStateChange abilityTriggerContext);

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

        // Call when applying for the first time, may actually do state changes, or apply 
        // non permanent visual effects on initial application
        public virtual void OnApply(Entity owner)
        {
            ApplyPersistentVisualEffects(owner);
        }

        // Apply persistent visual effects, be sure to call this when loading the game for all effects
        public virtual void ApplyPersistentVisualEffects(Entity owner)
        {

        }

        // When an effect is removed, perform any state changes, and remove the persistent visual effects
        // Do not remove temporary effects as they may not exist at this point
        public virtual void OnRemove(Entity owner)
        {
            RemovePersistantVisualEffects(owner);
        }

        public virtual void RemovePersistantVisualEffects(Entity owner)
        {

        }

        public abstract string RemovalText
        {
            get;
        }
    }
}
