using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class MoveItemInInventory : Action
    {
        [JsonIgnore]
        public Entity Source;

        [JsonIgnore]
        public List<Entity> Targets = new List<Entity>(0);
        
        public Item Item;
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
            Assert.IsTrue(Index != -1);
            var oldIndex = Source.Inventory.Items.IndexOf(Item);
            InventoryUtil.SwapItemPosition(Source, Item, oldIndex, Index);
            Context.UIController.Refresh();
        }
    }
}
