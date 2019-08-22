namespace Gamepackage
{
    public class Regeneration : EffectImpl
    {
        public override void Tick(Effect state, Entity entity)
        {
            base.Tick(state, entity);
            int.TryParse(state.Data["HealAmount"], out int HealAmount);
            var calculated = Context.RulesEngine.CalculateSimpleDamage(
                entity,
                state.Template.LocalizationPrefix,
                HealAmount,
                DamageTypes.HEALING
            );
            CombatUtil.ApplyAttackInstantly(calculated);
        }
    }
}
