using Spine.Unity;
using UnityEngine;

namespace Gamepackage
{
    public class Poison : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.poison.name";
            }
        }

        public override string Description
        {
            get
            {
                return "effect.poison.description";
            }
        }

        public int PoisonAmount;

        public override void OnApply(EntityStateChange outcome)
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
            var calculated = CombatUtil.CalculateSimpleDamage(
                entity,
                "effect.poison.tick.message",
                PoisonAmount,
                DamageTypes.POISON
            );
            CombatUtil.ApplyAttackInstantly(calculated);
        }

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
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
