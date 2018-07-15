using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class EquipItem : TargetableAction
    {
        public Item Item;
        public ItemSlot Slot;

        public override int TimeCost
        {
            get
            {
                return 250;
            }
        }

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        public override void Enter()
        {
            base.Enter();
            Assert.IsNotNull(Source);
            Assert.IsNotNull(Item);
            Assert.IsTrue(Slot != ItemSlot.None);
            Source.Inventory.EquipItemToSlot(Item, Slot);
            Context.UIController.Refresh();
        }
    }
}
