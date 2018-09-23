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

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.apply.message".Localize(), owner.Name));
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.remove.message".Localize(), owner.Name));
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            EntityStateChange ctx = new EntityStateChange
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
            ctx.Targets.Add(entity);
            CombatUtil.ApplyEntityStateChange(ctx);
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }
    }
}
