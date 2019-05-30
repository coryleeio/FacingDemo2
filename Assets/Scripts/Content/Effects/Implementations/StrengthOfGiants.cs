namespace Gamepackage
{
    public class StrengthOfGiants : Effect
    {
        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }
    }
}
