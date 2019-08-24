using System.Collections.Generic;

namespace Gamepackage
{
    public interface IRulesEngine
    {
        float CalculateBlockPercentage(CalculatedCombatAction action, Entity Target);
        float CalculateToHitPercentage(CalculatedCombatAction action, Entity Target);
        float CalculateDodgePercentage(CalculatedCombatAction action, Entity Target);
        int CalculateXpForKill(EntityStateChange action, Entity target);
        int CalculateSkillXpForKill(EntityStateChange action, Entity target, int xpForKill, List<Skill> exercisedSkills);
        int CalculateDamage(CombatActionParameters attackParameters);
        CalculatedCombatAction CalculateSimpleDamage(Entity target, string i18nString, int baseDamage, DamageTypes damageType);
    }
}
