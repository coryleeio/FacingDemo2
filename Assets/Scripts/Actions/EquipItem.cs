using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class EquipItem : Action
    {
        [JsonIgnore]
        public Entity Source;
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
            Source.Inventory.EquipItemToSlot(Item, Slot);
            Context.ViewFactory.BuildView(Source);
            Context.UIController.Refresh();
        }
    }
}
