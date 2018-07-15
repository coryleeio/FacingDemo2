using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class ItemDropSlot : MonoBehaviour, IDropHandler
    {
        public int Index;
        public Entity Entity;

        public void OnDrop(PointerEventData eventData)
        {
            if(Entity == InventoryDraggable.CurrentDraggable.Source)
            {
                // Moving into same inventory, this is just a reorg
                // so we just allow this to happen for free.
                var step = new Step();
                var action = Context.PrototypeFactory.BuildEntityAction<MoveItemInInventory>(Entity) as MoveItemInInventory;
                action.Item = InventoryDraggable.CurrentDraggable.Item;
                action.Index = Index;
                step.Actions.AddFirst(action);
                Context.FlowSystem.Steps.AddFirst(step);
            }
            else
            {
                // Moving from one inventory into another, looting is not a free action, so 
                // we pass this to the player controller.
                var step = new Step();
                var action = Context.PrototypeFactory.BuildEntityAction<PickupItem>(Entity) as PickupItem;
                action.Item = InventoryDraggable.CurrentDraggable.Item;
                action.Index = Index;
                // Take the item from the drop
                action.Targets.Add(InventoryDraggable.CurrentDraggable.Source); 
                step.Actions.AddFirst(action);
                Context.FlowSystem.Steps.AddFirst(step);
            }
        }
    }
}
