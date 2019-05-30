namespace Gamepackage
{
    public class Regeneration : Effect
    {
        public int HealAmount;

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            var calculated = CombatUtil.CalculateSimpleDamage(
                entity,
                "effect." + LocalizationName + ".tick.message",
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
