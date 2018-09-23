using Newtonsoft.Json;
using System.Collections.Generic;

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

        public UniqueIdentifier Identifier;
        public Dictionary<Attributes, int> Attributes;

        public virtual void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuplicate(entity, this);
        }

        public Ticker Ticker;

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
            if (CanTick)
            {
                Ticker.Tick(owner);
            }
        }

        // Call when applying for the first time, may actually do state changes, or apply 
        // non permanent visual effects on initial application
        public virtual void OnApply(Entity owner)
        {
            ApplyPersistentVisualEffects(owner);
            if (owner.Body != null)
            {
                if (owner.Body.CurrentHealth > owner.CalculateValueOfAttribute(Gamepackage.Attributes.MAX_HEALTH))
                {
                    owner.Body.CurrentHealth = owner.CalculateValueOfAttribute(Gamepackage.Attributes.MAX_HEALTH);
                }
            }
        }

        // When an effect is removed, perform any state changes, and remove the persistent visual effects
        // Do not remove temporary effects as they may not exist at this point
        public virtual void OnRemove(Entity owner)
        {
            RemovePersistantVisualEffects(owner);
            if (owner.Body != null)
            {
                var aliveBeforeRemoval = owner.Body.CurrentHealth > 0;
                if (Attributes.ContainsKey(Gamepackage.Attributes.MAX_HEALTH))
                {
                    owner.Body.CurrentHealth -= Attributes[Gamepackage.Attributes.MAX_HEALTH];
                }
                if(aliveBeforeRemoval && owner.Body.CurrentHealth < 0)
                {
                    owner.Body.CurrentHealth = 1;
                }
            }
        }

        // Apply persistent visual effects, be sure to call this when loading the game for all effects
        public virtual void ApplyPersistentVisualEffects(Entity owner)
        {

        }

        public virtual void RemovePersistantVisualEffects(Entity owner)
        {

        }

        public virtual bool CanAffectIncomingAttack(EntityStateChange abilityTriggerContext)
        {
            return false;
        }
        public virtual EntityStateChange AffectIncomingAttackEffects(EntityStateChange ctx)
        {
            return ctx;
        }

        public virtual bool CanAffecOutgoingAttack(EntityStateChange abilityTriggerContext)
        {
            return false;
        }
        public virtual EntityStateChange AffectOutgoingAttack(EntityStateChange ctx)
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

        public virtual bool CanTriggerOnPress()
        {
            return false;
        }

        public virtual EntityStateChange TriggerOnPress(EntityStateChange ctx)
        {
            return ctx;
        }
    }
}
