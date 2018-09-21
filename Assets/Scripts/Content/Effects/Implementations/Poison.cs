namespace Gamepackage
{
    public class Poison : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "Poison";
            }
        }

        public override string Description
        {
            get
            {
                return "You are poisoned...";
            }
        }

        public int PoisonAmount;

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("{0} is now poisoned...", owner.Name));
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("{0} is no longer poisoned!", owner.Name));
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            EntityStateChange ctx = new EntityStateChange
            {
                AttackParameters = new AttackParameters
                {
                    DamageType = DamageTypes.POISON,
                    AttackMessage = "Poison hurts {1} for {2} point of damage!",
                    DyeNumber = 1,
                    DyeSize = PoisonAmount,
                    Bonus = 0
                }
            };
            ctx.Targets.Add(entity);
            CombatUtil.ApplyEntityStateChange(ctx);
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }
    }
}
