namespace Gamepackage
{
    public class Domination : Effect
    {
        public override void OnApply(EntityStateChange outcome)
        {
            base.OnApply(outcome);
            if (outcome.Target.Behaviour != null)
            {
                outcome.Target.Behaviour.ActingTeam = outcome.Source.Behaviour.ActingTeam;
                ViewUtils.UpdateHealthBarColor(outcome.Target);
                outcome.Target.Behaviour.LastKnownTargetPosition = null;
                if (outcome.Target.Behaviour.IsPlayer)
                {
                    outcome.Target.Behaviour.AI = AIType.Archer;
                }
            }
        }

        public override void OnRemove(Entity entity)
        {
            base.OnRemove(entity);
            if (entity.Behaviour != null)
            {
                entity.Behaviour.ActingTeam = entity.Behaviour.OriginalTeam;
                ViewUtils.UpdateHealthBarColor(entity);
                if (entity.Behaviour.IsPlayer)
                {
                    entity.Behaviour.AI = AIType.None;
                }
            }
        }

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuplicate(outcome, this);
        }
    }
}
