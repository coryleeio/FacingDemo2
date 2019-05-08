namespace Gamepackage
{
    public class StrengthOfGiants : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.strength.of.giants.name";
            }
        }

        public override string Description
        {
            get
            {
                return "effect.strength.of.giants.description";
            }
        }

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }

        public override void OnApply(EntityStateChange outcome)
        {
            base.OnApply(outcome);
            Context.UIController.TextLog.AddText(string.Format("effect.strength.of.giants.apply".Localize(), outcome.Target.Name));
        }

        public override void OnRemove(Entity entity)
        {
            base.OnRemove(entity);
            Context.UIController.TextLog.AddText(string.Format("effect.strength.of.giants.remove".Localize(), entity.Name));
        }
    }
}
