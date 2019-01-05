namespace Gamepackage
{
    public class Poison : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.poison.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.poison.description".Localize();
            }
        }

        public int PoisonAmount;

        public override void OnApply(ActionOutcome outcome)
        {
            base.OnApply(outcome);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.apply.message".Localize(), outcome.Target.Name));
        }

        public override void OnRemove(Entity entity)
        {
            base.OnRemove(entity);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.remove.message".Localize(), entity.Name));
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            ActionOutcome outcome = new ActionOutcome
            {
                AttackParameters = new AttackParameters
                {
                    DamageType = DamageTypes.POISON,
                    AttackMessage = "effect.poison.tick.message".Localize(),
                    DyeNumber = 1,
                    DyeSize = PoisonAmount,
                    Bonus = 0
                }
            };
            outcome.Target = entity;
            CombatUtil.ApplyEntityStateChange(outcome);
        }

        public override void HandleStacking(ActionOutcome outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }
    }
}
