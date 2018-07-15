using System.Collections.Generic;

namespace Gamepackage
{
    public class Longsword : Weapon
    {
        public override UniqueIdentifier UniqueIdentifier
        {
            get
            {
                return UniqueIdentifier.ITEM_LONGSWORD;
            }
        }

        public override UniqueIdentifier ItemAppearanceIdentifier
        {
            get
            {
                return UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD;
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Longsword";
            }
        }

        private static Dictionary<Attributes, int> AttributesInternal = new Dictionary<Attributes, int>()
        {
        };
        private static List<Ability> AbilitiesInternal = new List<Ability>() { };
        private static List<AttackParameters> ThrowParametersInternal = new List<AttackParameters>() { };
        private static List<AttackParameters> AttackParametersInternal = new List<AttackParameters>() {
            new AttackParameters()
            {
                AttackMessage = "{0} slashes {1} for {2} points of {3} damage!",
                Bonus = 0,
                DyeNumber = 1,
                DyeSize = 8,
                DamageType = DamageTypes.SLASHING,
            },
        };

        #region Boilerplate

        public override Dictionary<Attributes, int> Attributes
        {
            get
            {
                return AttributesInternal;
            }
        }

        public override List<Ability> Abilities
        {
            get
            {
                return AbilitiesInternal;
            }
        }

        public override List<AttackParameters> AttackParameters
        {
            get
            {
                return AttackParametersInternal;
            }
        }

        public override List<AttackParameters> ThrowParameters
        {
            get
            {
                return ThrowParametersInternal;
            }
        }

        #endregion
    }
}
