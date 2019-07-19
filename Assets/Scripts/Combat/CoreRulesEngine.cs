using System;
using UnityEngine;

namespace Gamepackage
{
    public class CoreRulesEngine : IRulesEngine
    {
        public float CalculateBlockPercentage(CalculatedCombatAction action, Entity Target)
        {
            if (!action.ResolvedCombatActionParameters.CombatActionParameters.Blockable)
            {
                return 0.0f;
            }
            return 10.0f;
        }

        public float CalculateDodgePercentageForDodgeableAttacks(Entity entity)
        {
            return 5.0f;
        }

        public float CalculateAccuracyPercent(CalculatedCombatAction action)
        {
            return 95f;
        }

        public float CalculateToHitPercentage(CalculatedCombatAction action, Entity Target)
        {
            if (action.ResolvedCombatActionParameters.CombatActionParameters.Unavoidable)
            {
                return 100.0f;
            }
            var dodgeable = action.ResolvedCombatActionParameters.CombatActionParameters.Dodgeable;
            var dodgePercent = dodgeable ? CalculateDodgePercentageForDodgeableAttacks(Target) : 0.0f;
            var accuracyPercentage = CalculateAccuracyPercent(action);
            var toHit = accuracyPercentage * ((100.0f - dodgePercent) * .01f);
            return toHit;
        }

        public int CalculateXpForKill(EntityStateChange result, Entity target)
        {
            var campaignTemplate = Context.Game.CampaignTemplate;
            float.TryParse(campaignTemplate.Settings[Settings.GlobalXpModifier.ToString()], out float xpMod);
            var xpAwardedForKillOfLevel = campaignTemplate.XpAwardedForKillingEntityOfLevel[target.Level];
            return (int)(xpAwardedForKillOfLevel * xpMod); ;
        }
    }
}
