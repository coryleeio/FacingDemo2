using System.Collections.Generic;

namespace Gamepackage
{
    public abstract class Weapon : Item
    {
        private static List<ItemSlot> WearableSlotsInternal = new List<ItemSlot>() { ItemSlot.Weapon };
        private static List<ItemSlot> SlotsOccupiedInternal = new List<ItemSlot>() { ItemSlot.Weapon };

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
    }
}
