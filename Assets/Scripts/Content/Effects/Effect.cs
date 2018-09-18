using Newtonsoft.Json;

namespace Gamepackage
{
    public abstract class Effect
    {
        public abstract string DisplayName
        {
            get;
        }
        public abstract string Description
        {
            get;
        }

        public Ticker Ticker;
        public IStackingStrategy StackingStrategy;

        [JsonIgnore]
        public bool CanTick
        {
            get
            {
                return Ticker != null;
            }
        }

        public virtual void Tick(Entity owner)
        {
            if(CanTick)
            {
                Ticker.Tick(owner);
            }
        }

        // Call when applying for the first time, may actually do state changes, or apply 
        // non permanent visual effects on initial application
        public virtual void OnApply(Entity owner)
        {
            ApplyPersistentVisualEffects(owner);
        }

        // When an effect is removed, perform any state changes, and remove the persistent visual effects
        // Do not remove temporary effects as they may not exist at this point
        public virtual void OnRemove(Entity owner)
        {
            RemovePersistantVisualEffects(owner);
        }

        // Apply persistent visual effects, be sure to call this when loading the game for all effects
        public virtual void ApplyPersistentVisualEffects(Entity owner)
        {

        }

        public virtual void RemovePersistantVisualEffects(Entity owner)
        {

        }

        public abstract string RemovalText
        {
            get;
        }

        public virtual bool CanAffectIncomingAttack(EntityStateChange abilityTriggerContext)
        {
            return false;
        }
        public virtual EntityStateChange AffectIncomingAttack(EntityStateChange ctx)
        {
            return ctx;
        }

        public virtual bool CanTriggerOnStep()
        {
            return false;
        }
        public virtual EntityStateChange TriggerOnStep(EntityStateChange ctx)
        {
            return ctx;
        }
    }
}
