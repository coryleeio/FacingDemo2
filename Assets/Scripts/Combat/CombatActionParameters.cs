using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    // Describes what happens when you use an item, or your body with a certain interaction type.
    public class CombatActionParameters
    {
        // IF YOU ARE ADDING STUFF ADD IT TO THE COPY CONSTRUCTOR BELOW
        public int BaseDamage;
        public int ClusteringFactor;
        public DamageTypes DamageType;
        public string SkillIdentifier;
        public int NumberOfTurnsToExerciseSkill;
        public string AttackMessagePrefix;
        public int Range;
        public int NumberOfTargetsToPierce;
        public CombatActionTargetingType TargetingType;
        public string ProjectileAppearanceIdentifier;
        public string AccuracyFormula;
        public string BlockChanceFormula;
        public string DodgeChanceFormula;
        public string FailureFormula;
        public string DamageFormula;
        // IF YOU ARE ADDING STUFF ADD IT TO THE COPY CONSTRUCTOR BELOW

        public CombatActionParameters()
        {

        }

        public CombatActionParameters(CombatActionParameters inp)
        {
            BaseDamage = inp.BaseDamage;
            ClusteringFactor = inp.ClusteringFactor;
            DamageType = inp.DamageType;
            SkillIdentifier = inp.SkillIdentifier;
            NumberOfTurnsToExerciseSkill = inp.NumberOfTurnsToExerciseSkill;
            AttackMessagePrefix = inp.AttackMessagePrefix;
            Range = inp.Range;
            NumberOfTargetsToPierce = inp.NumberOfTargetsToPierce;
            TargetingType = inp.TargetingType;
            ProjectileAppearanceIdentifier = inp.ProjectileAppearanceIdentifier;
            AccuracyFormula = inp.AccuracyFormula;
            BlockChanceFormula = inp.BlockChanceFormula;
            DodgeChanceFormula = inp.DodgeChanceFormula;
            FailureFormula = inp.FailureFormula;
            DamageFormula = inp.DamageFormula;
    }

        [JsonIgnore]
        public string AttackHitMessage
        {
            get
            {
                return AttackMessagePrefix + ".hit";
            }
        }

        [JsonIgnore]
        public string AttackMissMessage
        {
            get
            {
                return AttackMessagePrefix + ".miss";
            }
        }

        [JsonIgnore]
        public string AttackBlockedMessage
        {
            get
            {
                return AttackMessagePrefix + ".block";
            }
        }

        [JsonIgnore]
        public string AttackFailedMessage
        {
            get
            {
                return AttackMessagePrefix + ".fail";
            }
        }

        [JsonIgnore]
        public ProjectileAppearanceTemplate ProjectileAppearance
        {
            get
            {
                if (ProjectileAppearanceIdentifier == null)
                {
                    return null;
                }
                return Context.ResourceManager.Load<ProjectileAppearanceTemplate>(ProjectileAppearanceIdentifier);
            }
        }
    }
}
