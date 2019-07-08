using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class EffectImpl
    {
        public virtual void Tick(Effect effect, Entity owner)
        {
            // Dont call base bc it will throw not implemented
            if (!effect.Template.HasUnlimitedDuration)
            {
                effect.TurnsRemaining--;
            }
        }

        public void HandleStacking(Effect state, EntityStateChange outcome)
        {
            if (state.Template.StackingStrategy == StackingStrategy.AddDuration)
            {
                var matchingEffects = outcome.Target.TemporaryEffects.FindAll((x) => { return x.TemplateIdentifier == state.TemplateIdentifier; });
                if (matchingEffects.Count > 1)
                {
                    throw new NotImplementedException("If these effects add to the duration of a matching effect, how the hell did you get two of them?");
                }

                if (matchingEffects.Count == 0)
                {
                    outcome.Target.TemporaryEffects.Add(state);
                    OnApplyOther(state, outcome);
                    return;
                }
                if (matchingEffects.Count == 1)
                {
                    var existingEffect = matchingEffects[0];
                    existingEffect.TurnsRemaining = (existingEffect.TurnsRemaining + state.TurnsRemaining);
                }
            }
            else if (state.Template.StackingStrategy == StackingStrategy.AddDuplicate)
            {
                outcome.Target.TemporaryEffects.Add(state);
                OnApplyOther(state, outcome);
            }
            else if(state.Template.StackingStrategy == StackingStrategy.IgnoreDuplicates)
            {
                var matchingEffects = outcome.Target.TemporaryEffects.FindAll((x) => { return x.TemplateIdentifier == state.TemplateIdentifier; });
                if(matchingEffects.Count == 0)
                {
                    outcome.Target.TemporaryEffects.Add(state);
                    OnApplyOther(state, outcome);
                }
            }
        }

        // When an effect is removed, perform any state changes, and remove the persistent visual effects
        // Do not remove temporary effects as they may not exist at this point
        // This signature is different because you can have effects removed due to duration
        // not just as the result of an action
        public virtual void OnRemove(Effect state, Entity target)
        {
            RemovePersistantVisualEffects(target);
            if (target.IsCombatant)
            {
                var aliveBeforeRemoval = target.CurrentHealth > 0;
                if (state.Template.TemplateAttributes.ContainsKey(Gamepackage.Attributes.MAX_HEALTH))
                {
                    target.CurrentHealth -= state.Template.TemplateAttributes[Gamepackage.Attributes.MAX_HEALTH];
                }
                if (aliveBeforeRemoval && target.CurrentHealth < 0)
                {
                    target.CurrentHealth = 1;
                }
            }
            ShowRemoveMessages(state, target);
        }

        // Apply persistent visual effects, be sure to call this when loading the game for all effects
        public virtual void ApplyPersistentVisualEffects(Entity owner)
        {

        }

        public virtual void RemovePersistantVisualEffects(Entity owner)
        {

        }

        public virtual EntityStateChange CalculateAffectIncomingAttackEffects(Effect state, CalculatedCombatAction calculatedAttack, EntityStateChange outcome)
        {
            return outcome;
        }

        public virtual EntityStateChange CalculateAffectOutgoingAttack(Effect state, CalculatedCombatAction calculatedAttack, EntityStateChange outcome)
        {
            return outcome;
        }

        public virtual void ShowApplySelfMessages(Effect state, Entity entity)
        {
            CombatUtil.ShowFloatingMessage(entity.Position, new FloatingTextMessage()
            {
                Message = string.Format("+{0}", state.Name.Localize()),
                Color = DisplayUtil.EffectDisplayColor(),
                target = entity,
                AllowLeftRightDrift = true,
            });
            Context.UIController.TextLog.AddText(string.Format((state.ApplyMessage).Localize(), entity.Name));
        }

        public virtual void ShowApplyOtherMessages(Effect state, EntityStateChange outcome)
        {
            outcome.FloatingTextMessage.AddLast(new FloatingTextMessage()
            {
                Message = string.Format("+{0}", state.Name.Localize()),
                Color = DisplayUtil.EffectDisplayColor(),
                target = outcome.Target,
                AllowLeftRightDrift = true,
            });
            outcome.LogMessages.AddLast(string.Format(string.Format((state.ApplyMessage).Localize(), outcome.Target.Name)));
        }

        public virtual void ShowRemoveMessages(Effect state, Entity entity)
        {
            CombatUtil.ShowFloatingMessage(entity.Position, new FloatingTextMessage()
            {
                Message = string.Format("-{0}", state.Name.Localize()),
                Color = DisplayUtil.EffectDisplayColor(),
                target = entity,
                AllowLeftRightDrift = true,
            });
            Context.UIController.TextLog.AddText(string.Format((state.RemoveMessage).Localize(), entity.Name));
        }

        // Call when applying for the first time, may actually do state changes, or apply 
        // non permanent visual effects on initial application
        public virtual void OnApplyOther(Effect state, EntityStateChange outcome)
        {
            ShowApplyOtherMessages(state, outcome);
            OnApplyInternal(state, outcome.Target);
        }

        // For applying persistant visual effects from items, since this does not have a actionOutcome
        // this should not be overwritten, only called.  It is just a work around for not having an
        // action outcome for equiping an item.
        public void OnApplySelf(Effect state, Entity entity)
        {
            ShowApplySelfMessages(state, entity);
            OnApplyInternal(state, entity);
        }

        private void OnApplyInternal(Effect state, Entity entity)
        {
            ApplyPersistentVisualEffects(entity);
            HandleRemovalOfBlockedEffects(state, entity);
        }

        private void HandleRemovalOfBlockedEffects(Effect state, Entity entity)
        {
            var stateChanges = new List<EntityStateChange>();
            foreach (var entityTag in entity.Tags)
            {
                foreach (var effectOnEntity in entity.TemporaryEffects)
                {
                    foreach (var tagThatBlocksThisEffect in effectOnEntity.Template.TagsThatBlockThisEffect)
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
    }
}
