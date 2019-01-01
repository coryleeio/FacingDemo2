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

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.regen.apply.message".Localize(), owner.Name));
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.regen.remove.message".Localize(), owner.Name));
        }

        public override void Tick(Entity entity)
        {
            ActionOutcome outcome = new ActionOutcome
            {
                AttackParameters = new AttackParameters
                {
                    DamageType = DamageTypes.HEALING,
                    AttackMessage = "effect.regen.tick.message".Localize(),
                    DyeNumber = 1,
                    DyeSize = HealAmount,
                    Bonus = 0
                }
            };
            outcome.Target = entity;
            CombatUtil.ApplyEntityStateChange(outcome);
            base.Tick(entity);
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }
    }
}
