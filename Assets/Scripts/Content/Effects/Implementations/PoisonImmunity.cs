namespace Gamepackage
{
    public class PoisonImmunity : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.poison.immunity.name";
            }
        }

        public override string Description
        {
            get
            {
                return "effect.poison.immunity.description";
            }
        }

        public override void OnApply(EntityStateChange combatContext)
        {
            base.OnApply(combatContext);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.immunity.apply.message".Localize(), combatContext.Target.Name));
            EntityStateChange outcome = new EntityStateChange();
            outcome.Source = combatContext.Source;
            outcome.Target = combatContext.Target;
            var effectsToAttemptRemoval = CombatUtil.GetEntityEffectsByType(outcome.Target, (effectInQuestion) => { return effectInQuestion is Poison; });
            outcome.RemovedEffects.AddRange(effectsToAttemptRemoval);
            CombatUtil.ApplyEntityStateChange(outcome);
        }

        public override void OnRemove(Entity entity)
        {
            base.OnRemove(entity);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.immunity.remove.message".Localize(), entity.Name));
        }

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }

        public override bool CanAffectIncomingAttack(CalculatedAttack calculatedAttack, EntityStateChange outcome)
        {
            return true;
        }

        public override EntityStateChange CalculateAffectIncomingAttackEffects(CalculatedAttack calculatedAttack, EntityStateChange outcome)
        {
            var effectsBlocked = outcome.AppliedEffects.FindAll((effectInQuestion) => { return effectInQuestion.Identifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON || effectInQuestion.Identifier == UniqueIdentifier.EFFECT_APPLIED_STRONG_POISON; });

            foreach(var effect in effectsBlocked)
            {
                outcome.AppliedEffects.Remove(effect);
            }
            if (effectsBlocked.Count > 0)
            {
                outcome.LateMessages.AddLast(string.Format("effect.poison.immunity.block.message".Localize(), outcome.Target.Name));
            }
            return outcome;
        }
    }
}
