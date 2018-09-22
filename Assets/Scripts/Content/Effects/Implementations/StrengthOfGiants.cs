namespace Gamepackage
{
    public class StrengthOfGiants : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "Strength of Giants";
            }
        }

        public override string Description
        {
            get
            {
                return "Your strength feels boundless";
            }
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("{0} is filled with boundless strength!", owner.Name));
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("{0} feels much weaker...", owner.Name));
        }
    }
}
