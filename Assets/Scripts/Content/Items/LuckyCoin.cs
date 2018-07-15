using System.Collections.Generic;

namespace Gamepackage
{
    public class LuckyCoin : Item
    {
        public override UniqueIdentifier UniqueIdentifier
        {
            get
            {
                return UniqueIdentifier.ITEM_LUCKY_COIN;
            }
        }

        public override UniqueIdentifier ItemAppearanceIdentifier
        {
            get
            {
                return UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN;
            }
        }

        public override string DisplayName
        {
            get
            {
                return "Lucky Coin";
            }
        }

        private static List<ItemSlot> WearableSlotsInternal = new List<ItemSlot>() { };
        private static List<ItemSlot> SlotsOccupiedInternal = new List<ItemSlot>() { };
        private static List<Ability> AbilitiesInternal = new List<Ability>() { AbilityFactory.Build(UniqueIdentifier.ABILITY_LUCKY_COIN_LIFE_SAVE) };
        private static Dictionary<Attributes, int> AttributesInternal = new Dictionary<Attributes, int>() { };
        private static List<AttackParameters> ThrowParametersInternal = new List<AttackParameters>() { };
        private static List<AttackParameters> AttackParametersInternal = new List<AttackParameters>() { };

        public override List<ItemSlot> SlotsWearable
        {
            get
            {
                return WearableSlotsInternal;
            }
        }

        public override List<ItemSlot> SlotsOccupiedByWearing
        {
            get
            {
                return SlotsOccupiedInternal;
            }
        }

        public override int MaxStackSize
        {
            get
            {
                return 1;
            }
        }

        public override int DefaultStackSize
        {
            get
            {
                return 1;
            }
        }



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
