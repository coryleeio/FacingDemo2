using System.Collections.Generic;

namespace Gamepackage
{
    public class PoisonImmunity : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "Cures poison";
            }
        }

        public override string Description
        {
            get
            {
                return "Removes all poisons from your body.";
            }
        }

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("Your body is cleansed of all toxins", owner.Name));
            EntityStateChange ctx = new EntityStateChange();
            ctx.Source = owner;
            ctx.Targets.Add(owner);
            var effectsToAttemptRemoval = CombatUtil.GetEntityEffectsByType(owner, (effectInQuestion) => { return effectInQuestion is Poison; });
            ctx.RemovedEffects.AddRange(effectsToAttemptRemoval);
            CombatUtil.ApplyEntityStateChange(ctx);
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("{0} is no longer immune to poison...", owner.Name));
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }

        public override bool CanAffectIncomingAttack(EntityStateChange ctx)
        {
            return true;
        }

        public override EntityStateChange AffectIncomingAttackEffects(EntityStateChange ctx)
        {
            foreach(var target in ctx.Targets)
            {
                var effectsBlocked = ctx.AppliedEffects.FindAll((effectInQuestion) => { return effectInQuestion.Identifier == UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON || effectInQuestion.Identifier == UniqueIdentifier.EFFECT_APPLIED_STRONG_POISON; });

                foreach(var effect in effectsBlocked)
                {
                    ctx.AppliedEffects.Remove(effect);
                }
                if (effectsBlocked.Count > 0)
                {
                    ctx.LateMessages.AddLast(string.Format("The poison had no effect on {0}.", target.Name));
                }
            }
            return ctx;
        }
    }
}
