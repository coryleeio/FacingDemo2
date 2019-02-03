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

        public virtual void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuplicate(outcome, this);
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

        // For applying persistant visual effects from items, since this does not have a actionOutcome
        // this should not be overwritten, only called.  It is just a work around for not having an
        // action outcome for equiping an item.
        public void OnApply(Entity entity)
        {
            ApplyPersistentVisualEffects(entity);
        }

        // Call when applying for the first time, may actually do state changes, or apply 
        // non permanent visual effects on initial application
        public virtual void OnApply(EntityStateChange outcome)
        {
            ApplyPersistentVisualEffects(outcome.Target);
        }

        // When an effect is removed, perform any state changes, and remove the persistent visual effects
        // Do not remove temporary effects as they may not exist at this point
        // This signature is different because you can have effects removed due to duration
        // not just as the result of an action
        public virtual void OnRemove(Entity target)
        {
            RemovePersistantVisualEffects(target);
            if (target.Body != null)
            {
                var aliveBeforeRemoval = target.Body.CurrentHealth > 0;
                if (Attributes.ContainsKey(Gamepackage.Attributes.MAX_HEALTH))
                {
                    target.Body.CurrentHealth -= Attributes[Gamepackage.Attributes.MAX_HEALTH];
                }
                if(aliveBeforeRemoval && target.Body.CurrentHealth < 0)
                {
                    target.Body.CurrentHealth = 1;
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

        public virtual bool CanAffectIncomingAttack(CalculatedAttack calculatedAttack, EntityStateChange abilityTriggerContext)
        {
            return false;
        }
        public virtual EntityStateChange CalculateAffectIncomingAttackEffects(CalculatedAttack calculatedAttack, EntityStateChange outcome)
        {
            return outcome;
        }

        public virtual bool CanAffectOutgoingAttack(CalculatedAttack calculatedAttack, EntityStateChange abilityTriggerContext)
        {
            return false;
        }
        public virtual EntityStateChange CalculateAffectOutgoingAttack(CalculatedAttack calculatedAttack, EntityStateChange outcome)
        {
            return outcome;
        }

        public virtual bool CanTriggerOnStep()
        {
            return false;
        }
        public virtual EntityStateChange TriggerOnStep(EntityStateChange outcome)
        {
            return outcome;
        }

        public virtual bool CanTriggerOnPress()
        {
            return false;
        }

        public virtual EntityStateChange TriggerOnPress(EntityStateChange outcome)
        {
            return outcome;
        }
    }
}
