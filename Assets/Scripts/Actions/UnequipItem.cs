using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class UnequipItem : Action
    {
        [JsonIgnore]
        public Entity Source;

        public Item Item;
        public ItemSlot Slot;
        public int Index = -1;

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
            InventoryUtil.UnequipItemInSlot(Source, Slot, Index);
            Context.UIController.Refresh();
        }
    }
}
