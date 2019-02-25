using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class PickupItem : Action
    {
        [JsonIgnore]
        public Entity Source;

        [JsonIgnore]
        public List<Entity> Targets = new List<Entity>(0);

        public Item Item;
        public int Index = -1; // if you dont set this the inventory system will find a slot 
        // for the item

        // Source is the person picking up the item
        // Target is the thing having the item taken from it.

        public override bool IsEndable
        {
            get
            {
                return true;
            }
        }

        public override void Enter()
        {
            var level = Context.Game.CurrentLevel;

            base.Enter();
            Assert.IsNotNull(Source);
            Assert.IsTrue(Targets != null && Targets.Count == 1);
            Assert.IsTrue(Source != Targets[0]); // MoveItemInInventory should be used instead

            var source = Source;
            var target = Targets[0];
            InventoryUtil.RemoveWholeItemStack(target, Item);
            ViewFactory.RebuildView(target);
            if(Index == -1)
            {
                InventoryUtil.AddItem(source, Item);
            }
            else
            {
                InventoryUtil.AddItem(source, Item, Index);
            }
            if(source.IsPlayer)
            {
                Context.UIController.TextLog.AddText(string.Format("You have looted {0}{1}", Item.DisplayName, Item.MaxStackSize > 1 ? string.Format(" x {0}", Item.NumberOfItems.ToString()) : ""));
            }
            Context.UIController.Refresh();
            if(!Context.UIController.LootWindow.StillHasItems)
            {
                Context.UIController.LootWindow.Hide();
            }
        }
    }
}
