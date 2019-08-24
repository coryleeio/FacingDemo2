using System.Collections.Generic;

namespace Gamepackage
{
    public class CoreRulesEngine : IRulesEngine
    {
        // These are just the strings that the standard rules engine uses added to an enum for strong typing.
        // Strings are used for comparisons even though it is slower so that modders can add
        // a new rules engine and new formulas
        enum SupportedFormulas
        {
            BLOCK_STANDARD,
            BLOCK_IGNORE,
            ACCURACY_GUARANTEED,
            ACCURACY_STANDARD,
            DODGE_IGNORE,
            DODGE_STANDARD,
            DAMAGE_STANDARD,
            NO_FAILURE,
        }

        public float CalculateBlockPercentage(CalculatedCombatAction action, Entity Target)
        {
            var formula = action.ResolvedCombatActionParameters.CombatActionParameters.BlockChanceFormula;
            if (formula == SupportedFormulas.BLOCK_IGNORE.ToString())
            {
                return 0.0f;
            }
            else if (formula == SupportedFormulas.BLOCK_STANDARD.ToString())
            {
                return 10.0f;
            }
            else
            {
                throw new NotImplementedException("Could not find formula: " + formula);
            }
        }

        public float CalculateDodgePercentage(CalculatedCombatAction action, Entity Target)
        {
            string formula = action.ResolvedCombatActionParameters.CombatActionParameters.DodgeChanceFormula;
            if (formula == SupportedFormulas.DODGE_IGNORE.ToString())
            {
                return 0.0f;
            }
            else if (formula == SupportedFormulas.DODGE_STANDARD.ToString())
            {
                return 5.0f;
            }
            else
            {
                throw new NotImplementedException("Could not find formula: " + formula);
            }
        }

        // This exists separately from to hit because the player might want to know their
        // base accuracy percentage without the enemies defenses factored in.
        public float CalculateAccuracyPercent(CalculatedCombatAction action)
        {
            string formula = action.ResolvedCombatActionParameters.CombatActionParameters.AccuracyFormula;
            if (formula == SupportedFormulas.ACCURACY_GUARANTEED.ToString())
            {
                return 100.0f;
            }
            else if (formula == SupportedFormulas.ACCURACY_STANDARD.ToString())
            {
                return 95f;
            }
            else
            {
                throw new NotImplementedException("Could not find formula: " + formula);
            }
        }

        public float CalculateToHitPercentage(CalculatedCombatAction action, Entity Target)
        {
            string formula = action.ResolvedCombatActionParameters.CombatActionParameters.AccuracyFormula;

            // When we are guaranteed a hit by accuracy ignore this calculation.
            // accuracy also does this, but this is necessary because a 100% accuracy is not
            // necessarily a guaranteed hit after you factor in enemy defenses, but an attack
            // with guaranteed accuracy should just always hit and ignore all defenses.
            if (formula == SupportedFormulas.ACCURACY_GUARANTEED.ToString())
            {
                return 100.0f;
            }
            var accuracyPercentage = CalculateAccuracyPercent(action);
            var dodgePercent = CalculateDodgePercentage(action, Target);
            var toHit = accuracyPercentage * ((100.0f - dodgePercent) * .01f);
            return toHit;
        }

        public int CalculateSkillXpForKill(EntityStateChange result, Entity target, int xpForKill, List<Skill> exercisedSkills)
        {
            var campaignTemplate = Context.Game.CampaignTemplate;
            float.TryParse(campaignTemplate.Settings[Settings.GlobalSkillXpModifier.ToString()], out float xpMod);
            var xpPerSkill = xpForKill / exercisedSkills.Count;
            return (int)(xpPerSkill * xpMod);
        }

        public int CalculateXpForKill(EntityStateChange result, Entity target)
        {
            var campaignTemplate = Context.Game.CampaignTemplate;
            float.TryParse(campaignTemplate.Settings[Settings.GlobalXpModifier.ToString()], out float xpMod);
            var xpAwardedForKillOfLevel = campaignTemplate.XpAwardedForKillingEntityOfLevel[target.Level];
            return (int)(xpAwardedForKillOfLevel * xpMod);
        }

        public int CalculateDamage(CombatActionParameters attackParameters)
        {
            var ret = 0;

            if (attackParameters.BaseDamage == 0 || attackParameters.ClusteringFactor == 0)
            {
                return 0;
            }

            for (var numDyeRolled = 0; numDyeRolled < attackParameters.ClusteringFactor; numDyeRolled++)
            {
                ret = UnityEngine.Random.Range(1, attackParameters.BaseDamage);
            }

            ret /= attackParameters.ClusteringFactor;

            if (attackParameters.DamageType == DamageTypes.HEALING)
            {
                ret = ret *= -1;
            }
            return ret;
        }

        // Sometimes the game needs to calculate undodgeable damage for things like fire poison etc.
        // since you need to know what formulas to use in that calculation you need to define that in the rules engine.
        public CalculatedCombatAction CalculateSimpleDamage(Entity target, string i18nString, int baseDamage, DamageTypes damageType)
        {
            var resolved = new ResolvedCombatActionDescriptor()
            {
                CombatActionParameters = new DerivedCombatActionParameters()
                {
                    Range = 1,
                    AttackMessagePrefix = i18nString,
                    ClusteringFactor = 1,
                    BaseDamage = baseDamage,
                    DamageType = damageType,
                    ProjectileAppearanceIdentifier = "PROJECTILE_APPEARANCE_NONE",
                    NumberOfTargetsToPierce = 1,
                    TargetingType = CombatActionTargetingType.SelectTarget,
                    AccuracyFormula = SupportedFormulas.ACCURACY_GUARANTEED.ToString(),
                    BlockChanceFormula = SupportedFormulas.BLOCK_IGNORE.ToString(),
                    DamageFormula = SupportedFormulas.DAMAGE_STANDARD.ToString(),
                    DodgeChanceFormula = SupportedFormulas.DODGE_STANDARD.ToString(),
                    FailureFormula = SupportedFormulas.NO_FAILURE.ToString(),
                },
                ExplosionParameters = null
            };
            var calculated = CombatUtil.CalculateAttack(Context.Game.CurrentLevel.Grid,
                    null,
                    CombatActionType.ApplyToOther,
                    null,
                    target.Position,
                    resolved
            );
            return calculated;
        }
    }
}
