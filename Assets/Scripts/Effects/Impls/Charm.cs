using System.Collections.Generic;

namespace Gamepackage
{
    public class Charm : EffectImpl
    {
        public override void OnApplyOther(Effect state, EntityStateChange outcome)
        {
            base.OnApplyOther(state, outcome);
            if (outcome.Target.IsCombatant)
            {
                outcome.Target.ActingTeam = outcome.Source.ActingTeam;
                ViewUtils.UpdateHealthBarColor(outcome.Target);
                outcome.Target.LastKnownTargetPosition = null;
                if (outcome.Target.IsPlayer)
                {
                    outcome.Target.AIClassName = "Gamepackage.Archer";
                    outcome.Target.AI = null;
                }
            }
        }

        public override void OnRemove(Effect state, Entity entity)
        {
            base.OnRemove(state, entity);
            if (entity.IsCombatant)
            {
                entity.ActingTeam = entity.OriginalTeam;
                ViewUtils.UpdateHealthBarColor(entity);
                if (entity.IsPlayer)
                {
                    entity.AIClassName = null;
                    entity.AI = null;
                }
            }
        }
    }
}
