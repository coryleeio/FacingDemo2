using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class EquipmentDropSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public ItemSlot Slot;

        [NonSerialized]
        public Entity Player;

        public void OnDrop(PointerEventData eventData)
        {
            var action = Context.PrototypeFactory.BuildEntityAction<EquipItem>(Player) as EquipItem;
            action.Item = DraggableItem.CurrentDraggable.Item;
            action.Slot = Slot;
            if(action.Item.SlotsWearable.Contains(Slot))
            {
                DraggableItem.CurrentDraggable.transform.SetParent(this.transform);
                Context.PlayerController.ActionList.Enqueue(action);
            }
            else
            {
                Context.UIController.Refresh();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var item = Player.Inventory.GetItemBySlot(Slot);
            if(item == null)
            {
                Context.UIController.Tooltip.Hover(this.gameObject, Slot.ToString());
            }
            else
            {
                Context.UIController.Tooltip.Hover(this.gameObject, String.Format("{0} ({1})", item.DisplayName, Slot.ToString()));
            }
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.Leave(this.gameObject);
        }
    }
}
