using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Poison : Effect
    {
        public int PoisonAmount;

        private static readonly List<Tags> BlockingTags = new List<Tags>() { Gamepackage.Tags.PoisonImmunity, };
        public override List<Tags> TagsThatBlockThisEffect => BlockingTags;

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            var calculated = CombatUtil.CalculateSimpleDamage(
                entity,
                "effect." + LocalizationName + ".tick.message",
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
