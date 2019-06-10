using UnityEngine;

namespace Gamepackage
{
    public class Poison : EffectImpl
    {
        public override void Tick(Effect state, Entity entity)
        {
            base.Tick(state, entity);
            int.TryParse(state.Data["PoisonAmount"], out int PoisonAmount);
            var calculated = CombatUtil.CalculateSimpleDamage(
                entity,
                state.Template.LocalizationPrefix,
                PoisonAmount,
                DamageTypes.POISON
            );
            CombatUtil.ApplyAttackInstantly(calculated);
        }

        public override void ApplyPersistentVisualEffects(Entity owner)
        {
            base.ApplyPersistentVisualEffects(owner);
            ViewUtils.ApplyColorToEntity(owner, Color.green);
        }

        public override void RemovePersistantVisualEffects(Entity owner)
        {
            base.RemovePersistantVisualEffects(owner);
            ViewUtils.ApplyColorToEntity(owner, Color.white);
        }
    }
}
