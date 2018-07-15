using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class MoveItemInInventory : TargetableAction
    {
        public Item Item;
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
            Assert.IsTrue(Index != -1);
            var oldIndex = Source.Inventory.Items.IndexOf(Item);
            Source.Inventory.SwapItemPosition(Item, oldIndex, Index);
            Context.UIController.Refresh();
        }
    }
}
