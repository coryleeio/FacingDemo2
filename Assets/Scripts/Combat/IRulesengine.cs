namespace Gamepackage
{
    public interface IRulesEngine
    {
        float CalculateBlockPercentage(CalculatedCombatAction action, Entity Target);
        float CalculateToHitPercentage(CalculatedCombatAction action, Entity Target);
        float CalculateDodgePercentageForDodgeableAttacks(Entity Target);
    }
}
