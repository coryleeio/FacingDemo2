using Newtonsoft.Json;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class PickupItem : TargetableAction
    {
        public Item Item;
        public int Index = -1; // if you dont set this the inventory system will find a slot 
        // for the item

        // Source is the person picking up the item
        // Target is the thing having the item taken from it.

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
            var level = Context.GameStateManager.Game.CurrentLevel;

            base.Enter();
            Assert.IsNotNull(Source);
            Assert.IsTrue(Targets != null && Targets.Count == 1);
            Assert.IsTrue(Source != Targets[0]); // MoveItemInInventory should be used instead

            var source = Source;
            var target = Targets[0];
            target.Inventory.RemoveItemStack(Item);
            if(Index == -1)
            {
               source.Inventory.AddItem(Item);
            }
            else
            {
                source.Inventory.AddItem(Item, Index);
            }
            Context.UIController.Refresh();
            if(target.Inventory.NumberOfItems == 0)
            {
                Context.EntitySystem.Deregister(target, level);
                if(target.View != null)
                {
                    UnityEngine.GameObject.Destroy(target.View.ViewGameObject);
                }
                Context.UIController.LootWindow.Hide();
            }
        }
    }
}
