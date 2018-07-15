using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class EquipmentDropSlot : MonoBehaviour, IDropHandler
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
    }
}
