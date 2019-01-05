namespace Gamepackage
{
    public class PoisonImmunity : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.poison.immunity.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.poison.immunity.description".Localize();
            }
        }

        public override void OnApply(ActionOutcome combatContext)
        {
            base.OnApply(combatContext);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.immunity.apply.message".Localize(), combatContext.Target.Name));
            ActionOutcome outcome = new ActionOutcome();
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

        public override void HandleStacking(ActionOutcome outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }

        public override bool CanAffectIncomingAttack(ActionOutcome outcome)
        {
            return true;
        }

        public override ActionOutcome CalculateAffectIncomingAttackEffects(ActionOutcome outcome)
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
