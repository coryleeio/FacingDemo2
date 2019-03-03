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
            var action = new EquipItem
            {
                Source = Player,
                Item = DraggableItem.CurrentDraggable.Item,
                Slot = Slot
            };
            if (action.Item.SlotsWearable.Contains(Slot))
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
            var item = InventoryUtil.GetItemBySlot(Player, Slot);
            if(item == null)
            {
                Context.UIController.Tooltip.TooltipShowSimpleText(this.gameObject, StringUtil.DisplayValueForSlot(Slot));
            }
            else
            {
                Context.UIController.Tooltip.TooltipShowSimpleText(this.gameObject, String.Format("{0} ({1})", item.DisplayName, StringUtil.DisplayValueForSlot(Slot)));
            }
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.StopTooltip(this.gameObject);
        }
    }
}
