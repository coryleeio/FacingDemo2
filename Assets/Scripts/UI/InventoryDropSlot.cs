using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class InventoryDropSlot : MonoBehaviour, IDropHandler
    {
        public int Index;

        [NonSerialized]
        public Entity Entity;

        public void OnDrop(PointerEventData eventData)
        {
            DraggableItem.CurrentDraggable.transform.SetParent(this.transform);
            if (Entity == DraggableItem.CurrentDraggable.Source)
            {
                if (Entity.Inventory.IsWearing(DraggableItem.CurrentDraggable.Item))
                {
                    // Unequip item and move it to slot in inventory
                    // this is not a free action so we send it to the player controller
                    var action = Context.PrototypeFactory.BuildEntityAction<UnequipItem>(Entity) as UnequipItem;
                    action.Item = DraggableItem.CurrentDraggable.Item;
                    action.Index = Index;
                    action.Slot = Entity.Inventory.GetItemSlotOfEquippedItem(action.Item);
                    Context.PlayerController.ActionList.Enqueue(action);
                }
                else
                {
                    // Moving into same inventory, this is just a reorg
                    // so we just allow this to happen for free.
                    var step = new Step();
                    var action = Context.PrototypeFactory.BuildEntityAction<MoveItemInInventory>(Entity) as MoveItemInInventory;
                    action.Item = DraggableItem.CurrentDraggable.Item;
                    action.Index = Index;
                    step.Actions.AddFirst(action);
                    Context.FlowSystem.Steps.AddFirst(step);
                }
            }
            else
            {
                // Moving from one inventory into another, looting is not a free action, so 
                // we pass this to the player controller.
                var action = Context.PrototypeFactory.BuildEntityAction<PickupItem>(Entity) as PickupItem;
                action.Item = DraggableItem.CurrentDraggable.Item;
                action.Index = Index;
                // Take the item from the drop
                action.Targets.Add(DraggableItem.CurrentDraggable.Source);
                Context.PlayerController.ActionList.Enqueue(action);
            }
        }
    }
}
