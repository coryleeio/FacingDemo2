using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class EquipItem : Action
    {
        [JsonIgnore]
        public override Entity Source
        {
            get; set;
        }
        public Item Item;
        public ItemSlot Slot;

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
            InventoryUtil.EquipItemToSlot(Source, Item, Slot);
            Context.UIController.Refresh();
        }
    }
}
