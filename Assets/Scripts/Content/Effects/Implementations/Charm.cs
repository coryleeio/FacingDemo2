namespace Gamepackage
{
    public class Charm : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.charm.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.charm.description".Localize();
            }
        }

        public override void OnApply(ActionOutcome outcome)
        {
            base.OnApply(outcome);
            Context.UIController.TextLog.AddText(string.Format("effect.domination.apply.message".Localize(), outcome.Target.Name));
            if (outcome.Target.Behaviour != null)
            {
                outcome.Target.Behaviour.ActingTeam = outcome.Source.Behaviour.ActingTeam;
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
            Context.UIController.TextLog.AddText(string.Format("effect.domination.remove.message".Localize(), entity.Name));
            if (entity.Behaviour != null)
            {
                entity.Behaviour.ActingTeam = entity.Behaviour.OriginalTeam;
                if (entity.Behaviour.IsPlayer)
                {
                    entity.Behaviour.AI = AIType.None;
                }
            }
        }

        public override void HandleStacking(ActionOutcome outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }
    }
}
