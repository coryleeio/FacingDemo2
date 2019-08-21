using Newtonsoft.Json;
using System.Collections.Generic;

namespace Gamepackage
{
    // Describes what happens when you use an item, or your body with a certain interaction type.
    public class CombatActionParameters
    {
        public int BaseDamage;
        public int ClusteringFactor;
        public DamageTypes DamageType;
        public string AttackMessagePrefix;
        public int Range;
        public int NumberOfTargetsToPierce;
        public CombatActionTargetingType TargetingType;
        public string ProjectileAppearanceIdentifier;
        public List<InteractionProperties> InteractionProperties;
        // IF YOU ARE ADDING STUFF ADD IT TO THE COPY CONSTRUCTOR BELOW

        public CombatActionParameters()
        {

        }

        public CombatActionParameters(CombatActionParameters inp)
        {
            BaseDamage = inp.BaseDamage;
            ClusteringFactor = inp.ClusteringFactor;
            DamageType = inp.DamageType;
            AttackMessagePrefix = inp.AttackMessagePrefix;
            Range = inp.Range;
            NumberOfTargetsToPierce = inp.NumberOfTargetsToPierce;
            TargetingType = inp.TargetingType;
            ProjectileAppearanceIdentifier = inp.ProjectileAppearanceIdentifier;
            InteractionProperties = new List<InteractionProperties>();
            InteractionProperties.AddRange(inp.InteractionProperties);
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
        public bool Unavoidable
        {
            get
            {
                return InteractionProperties.Contains(Gamepackage.InteractionProperties.Unavoidable);
            }
        }

        [JsonIgnore]
        public bool Blockable
        {
            get
            {
                return !InteractionProperties.Contains(Gamepackage.InteractionProperties.IgnoresBlock) && !Unavoidable;
            }
        }

        [JsonIgnore]
        public bool Dodgeable
        {
            get
            {
                return !InteractionProperties.Contains(Gamepackage.InteractionProperties.IgnoresDodge) && !Unavoidable;
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
