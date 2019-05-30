using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public abstract class Effect
    {
        public string LocalizationName
        {
            get; set;
        }

        public virtual string DisplayName
        {
            get
            {
                return "effect." + LocalizationName + ".name";
            }
        }
        public virtual string Description
        {
            get
            {
                return "effect." + LocalizationName + ".description";
            }
        }

        public UniqueIdentifier Identifier;
        public Dictionary<Attributes, int> Attributes;

        public virtual void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuplicate(outcome, this);
        }

        public Ticker Ticker;

        // Is the application of this effect, taken alone, a hostile action, or a positive one.
        // One causes aggro, and is displayed as bad, the others are not.
        // though if applied as part of an attack the entire attack may still be interpreted as hostile.
        public AttackHostility Hostility = AttackHostility.NOT_SET;

        // A good practice is to use a static to avoid making a new list all the time.
        [JsonIgnore]
        public static readonly List<Tags> EmptyTagList = new List<Tags>(0);

        // If an entity has a tag in this list, this effect cannot be applied.
        // a good practice is to use a static to avoid making a new list all the time.
        [JsonIgnore]
        public virtual List<Tags> TagsAppliedToEntity
        {
            get
            {
                return EmptyTagList;
            }
        }

        // If an entity has a tag in this list, this effect cannot be applied.
        [JsonIgnore]
        public virtual List<Tags> TagsThatBlockThisEffect
        {
            get
            {
                return EmptyTagList;
            }
        }

        [JsonIgnore]
        public bool CanTick
        {
            get
            {
                return Ticker != null;
            }
        }

        [JsonIgnore]
        public bool IsHostile
        {
            get
            {
                Assert.IsTrue(Hostility != AttackHostility.NOT_SET);
                if(Hostility == AttackHostility.HOSTILE)
                {
                    return true;
                }
                if(Hostility == AttackHostility.NOT_HOSTILE)
                {
                    return false;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public virtual void Tick(Entity owner)
        {
            if (CanTick)
            {
                Ticker.Tick(owner);
            }
        }

        public virtual void ShowAddMessages(Entity entity)
        {
            CombatUtil.ShowFloatingMessage(entity.Position, new FloatingTextMessage()
            {
                Message = string.Format("+{0}", this.DisplayName.Localize()),
                Color = DisplayUtil.DamageDisplayColor(entity.IsPlayer, IsHostile),
                target = entity,
                AllowLeftRightDrift = true,
            });
            Context.UIController.TextLog.AddText(string.Format(("effect." + LocalizationName + ".apply.message").Localize(), entity.Name));
        }

        public virtual void ShowRemoveMessages(Entity entity)
        {
            CombatUtil.ShowFloatingMessage(entity.Position, new FloatingTextMessage()
            {
                Message = string.Format("-{0}", this.DisplayName.Localize()),
                Color = DisplayUtil.DamageDisplayColor(entity.IsPlayer, !IsHostile),
                target = entity,
                AllowLeftRightDrift = true,
            });
            Context.UIController.TextLog.AddText(string.Format(("effect." + LocalizationName + ".remove.message").Localize(), entity.Name));
        }

        // For applying persistant visual effects from items, since this does not have a actionOutcome
        // this should not be overwritten, only called.  It is just a work around for not having an
        // action outcome for equiping an item.
        public void OnApply(Entity entity)
        {
            OnApplyInternal(entity);
        }

        // Call when applying for the first time, may actually do state changes, or apply 
        // non permanent visual effects on initial application
        public virtual void OnApply(EntityStateChange outcome)
        {
            OnApplyInternal(outcome.Target);
        }

        private void OnApplyInternal(Entity entity)
        {
            ApplyPersistentVisualEffects(entity);
            ShowAddMessages(entity);
            HandleRemovalOfBlockedEffects(entity);
        }

        private void HandleRemovalOfBlockedEffects(Entity entity)
        {
            var stateChanges = new List<EntityStateChange>();
            foreach(var entityTag in entity.Tags)
            {
                foreach (var effectOnEntity in entity.GetEffects())
                {
                    foreach (var tagThatBlocksThisEffect in effectOnEntity.TagsThatBlockThisEffect)
                    {
                        if (entityTag == tagThatBlocksThisEffect)
                        {
                            EntityStateChange outcome = new EntityStateChange();
                            outcome.Source = entity;
                            outcome.Target = entity;
                            outcome.RemovedEffects.Add(effectOnEntity);
                            stateChanges.Add(outcome);
                        }
                    }
                }

            }
            foreach (var stateChange in stateChanges)
            {
                CombatUtil.ApplyEntityStateChange(stateChange);
            }
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
            ShowRemoveMessages(target);
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
