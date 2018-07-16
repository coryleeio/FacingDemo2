using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamepackage
{
    public class DraggableItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static DraggableItem CurrentDraggable;

        [NonSerialized]
        public Entity Source;
        public Item Item;

        public void OnBeginDrag(PointerEventData eventData)
        {
            CurrentDraggable = this;
            var canvas = GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 9999;
            GetComponentInParent<GridLayoutGroup>().enabled = false;
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponentInParent<GridLayoutGroup>().enabled = true;
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var shiftLeftClicked = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && eventData.button == PointerEventData.InputButton.Left);
            var rightClicked = eventData.button == PointerEventData.InputButton.Right;

            var isWearingItem = Source.Inventory.IsWearing(Item);
            var hasItemInInventory = Source.Inventory.Items.Contains(Item);
            var isPickingUpItem = !isWearingItem && !hasItemInInventory;

            if(rightClicked)
            {
                Context.UIController.ContextMenu.ShowForItemAtLocation(Item, eventData);
            }
            if (shiftLeftClicked)
            {
                if(isWearingItem)
                {
                    var action = Context.PrototypeFactory.BuildEntityAction<UnequipItem>(Source) as UnequipItem;
                    action.Item = Item;
                    action.Slot = Source.Inventory.GetItemSlotOfEquippedItem(action.Item);
                    Context.PlayerController.ActionList.Enqueue(action);
                    Context.UIController.Refresh();
                }
                else if(hasItemInInventory)
                {
                    var action = Context.PrototypeFactory.BuildEntityAction<EquipItem>(Source) as EquipItem;
                    action.Item = Item;
                    if(Item.SlotsWearable.Count > 0)
                    {
                        action.Slot = Item.SlotsWearable[0];
                        Context.PlayerController.ActionList.Enqueue(action);
                    }
                    Context.UIController.Refresh();
                }
                else if(isPickingUpItem)
                {
                    // picking up the item
                    var action = Context.PrototypeFactory.BuildEntityAction<PickupItem>(Source) as PickupItem;
                    action.Item = Item;
                    
                    // player
                    action.Source = Context.GameStateManager.Game.CurrentLevel.Player;
                    
                    // takes from source (which is the draggable source)
                    action.Targets.Add(Source);

                    Context.PlayerController.ActionList.Enqueue(action);
                    Context.UIController.Refresh();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Context.UIController.Tooltip.Hover(this.gameObject, Item.DisplayName.ToString());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.Leave(this.gameObject);
        }
    }
}
