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
            var isLeftClick = eventData.button == PointerEventData.InputButton.Left;
            var shiftLeftClicked = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) && isLeftClick);
            var rightClicked = eventData.button == PointerEventData.InputButton.Right;

            var player = Context.Game.CurrentLevel.Player;


            var isWearingItem = InventoryUtil.IsWearing(player, Item);
            var hasItemInInventory = player.Inventory.Items.Contains(Item);
            var isPickingUpItem = !isWearingItem && !hasItemInInventory;

            if (rightClicked)
            {
                Context.UIController.ContextMenu.ShowForItemAtLocation(Item, eventData);
            }
            if (shiftLeftClicked)
            {
                if (isWearingItem)
                {
                    var action = new UnequipItem
                    {
                        Source = Source,
                        Item = Item
                    };
                    action.Slot = InventoryUtil.GetItemSlotOfEquippedItem(Source, action.Item);
                    Context.PlayerController.ActionList.Enqueue(action);
                    Context.UIController.Refresh();
                }
                else if (hasItemInInventory)
                {
                    var action = new EquipItem
                    {
                        Source = Source,
                        Item = Item
                    };
                    if (Item.SlotsWearable.Count > 0)
                    {
                        action.Slot = Item.SlotsWearable[0];
                        Context.PlayerController.ActionList.Enqueue(action);
                    }
                    Context.UIController.Refresh();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (isLeftClick && isPickingUpItem)
            {
                // picking up the item
                var action = new PickupItem
                {
                    Source = player,
                    Item = Item
                };

                // takes from source (which is the draggable source)
                action.Targets.Add(Source);

                Context.PlayerController.ActionList.Enqueue(action);
                Context.UIController.Tooltip.Hide();
                Context.UIController.Refresh();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Context.UIController.Tooltip.TooltipShowSimpleText(this.gameObject, Item.DisplayName.ToString());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.StopTooltip(this.gameObject);
        }
    }
}
