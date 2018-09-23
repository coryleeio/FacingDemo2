namespace Gamepackage
{
    public class StrengthOfGiants : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.strength.of.giants.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.strength.of.giants.description".Localize();
            }
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.strength.of.giants.apply".Localize(), owner.Name));
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.strength.of.giants.remove".Localize(), owner.Name));
        }
    }
}
