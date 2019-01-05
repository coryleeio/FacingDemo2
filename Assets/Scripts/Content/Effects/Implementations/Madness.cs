namespace Gamepackage
{
    public class Madness : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.madness.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.madness.description".Localize();
            }
        }

        public override void OnApply(Entity owner)
        {
            base.OnApply(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.madness.apply.message".Localize(), owner.Name));
            if (owner.Behaviour != null)
            {
                owner.Behaviour.ActingTeam = Team.EnemyOfAll;
                owner.Behaviour.LastKnownTargetPosition = null;
                if(owner.Behaviour.IsPlayer)
                {
                    owner.Behaviour.AI = AIType.Archer;
                }
            }
        }

        public override void OnRemove(Entity owner)
        {
            base.OnRemove(owner);
            Context.UIController.TextLog.AddText(string.Format("effect.madness.remove.message".Localize(), owner.Name));
            if (owner.Behaviour != null)
            {
                owner.Behaviour.ActingTeam = owner.Behaviour.OriginalTeam;
                if (owner.Behaviour.IsPlayer)
                {
                    owner.Behaviour.AI = AIType.None;
                }
            }
        }

        public override void HandleStacking(Entity entity)
        {
            StackingStrategies.AddDuration(entity, this);
        }
    }
}
