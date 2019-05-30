using System.Collections.Generic;

namespace Gamepackage
{
    public class PoisonImmunity : Effect
    {
        private static readonly List<Tags> TagsGranted = new List<Tags>() { Gamepackage.Tags.PoisonImmunity, };
        public override List<Tags> TagsAppliedToEntity => TagsGranted;

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }
    }
}
