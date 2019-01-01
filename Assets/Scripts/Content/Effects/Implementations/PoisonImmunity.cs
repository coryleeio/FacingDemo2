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

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.immunity.apply.message".Localize(), owner.Name));
            EntityStateChange ctx = new EntityStateChange();
            ctx.Source = owner;
            ctx.Target = owner;
            var effectsToAttemptRemoval = CombatUtil.GetEntityEffectsByType(owner, (effectInQuestion) => { return effectInQuestion is Poison; });
            ctx.RemovedEffects.AddRange(effectsToAttemptRemoval);
            CombatUtil.ApplyEntityStateChange(ctx);
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.immunity.remove.message".Localize(), owner.Name));
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }

        public override bool CanAffectIncomingAttack(EntityStateChange ctx)
        {
            return true;
        }

        public override EntityStateChange CalculateAffectIncomingAttackEffects(EntityStateChange ctx)
        {
            var effectsBlocked = ctx.AppliedEffects.FindAll((effectInQuestion) => { return effectInQuestion.Identifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON || effectInQuestion.Identifier == UniqueIdentifier.EFFECT_APPLIED_STRONG_POISON; });

            foreach(var effect in effectsBlocked)
            {
                ctx.AppliedEffects.Remove(effect);
            }
            if (effectsBlocked.Count > 0)
            {
                ctx.LateMessages.AddLast(string.Format("effect.poison.immunity.block.message".Localize(), ctx.Target.Name));
            }
            return ctx;
        }
    }
}
