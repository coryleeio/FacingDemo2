using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class UnequipItem : TargetableAction
    {
        public Item Item;
        public ItemSlot Slot;
        public int Index = -1;

        public override int TimeCost
        {
            get
            {
                return 0;
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
            Source.Inventory.UnequipItemInSlot(Slot, Index);
            Context.UIController.Refresh();
        }
    }
}
