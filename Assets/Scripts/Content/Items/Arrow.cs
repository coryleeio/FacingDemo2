using System.Collections.Generic;

namespace Gamepackage
{
    public class Arrow : Item
    {
        public override UniqueIdentifier UniqueIdentifier
        {
            get
            {
                return UniqueIdentifier.ITEM_ARROW;
            }
        }

        public override UniqueIdentifier ItemAppearanceIdentifier
        {
            get
            {
                return UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN;
            }
        }

        private static List<ItemSlot> WearableSlotsInternal = new List<ItemSlot>() { ItemSlot.Ammo };
        private static List<ItemSlot> SlotsOccupiedInternal = new List<ItemSlot>() { ItemSlot.Ammo };
        private static List<Ability> AbilitiesInternal = new List<Ability>() {  };
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
                return 20;
            }
        }

        public override int DefaultStackSize
        {
            get
            {
                return MathUtil.ChooseRandomIntInRange(5,MaxStackSize / 2 -1);
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
