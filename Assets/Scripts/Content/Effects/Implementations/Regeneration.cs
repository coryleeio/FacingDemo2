using System.Collections.Generic;

namespace Gamepackage
{
    public class Regeneration : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.regen.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.regen.description".Localize();
            }
        }

        public int HealAmount;

        public override void OnApply(EntityStateChange outcome)
        {
            base.OnApply(outcome);
            Context.UIController.TextLog.AddText(string.Format("effect.regen.apply.message".Localize(), outcome.Target.Name));
        }

        public override void OnRemove(Entity entity)
        {
            base.OnRemove(entity);
            Context.UIController.TextLog.AddText(string.Format("effect.regen.remove.message".Localize(), entity.Name));
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            var calculated = CombatUtil.CalculateSimpleDamage(
                entity,
                "effect.regen.tick.message",
                HealAmount,
                DamageTypes.HEALING
            );
            CombatUtil.ApplyAttackInstantly(calculated);
        }

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }
    }
}
