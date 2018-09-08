using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ContextMenu : UIComponent
    {
        private List<Button> buttons = new List<Button>();

        public override void Hide()
        {
            GetComponent<ContextMenu>().gameObject.SetActive(false);
            Context.UIController.ClickoutCatcher.Hide();
        }

        public override void Show()
        {
            GetComponent<ContextMenu>().gameObject.SetActive(true);
            Context.UIController.ClickoutCatcher.Show();
        }

        public void ShowForItemAtLocation(Item item, PointerEventData eventData)
        {
            Purge();
            BuildButton("Info", () =>
            {
                Context.UIController.ItemInspectionWindow.ShowFor(item);
            });

            var level = Context.GameStateManager.Game.CurrentLevel;
            var player = level.Player;
            var isWearingItem = player.Inventory.IsWearing(item);
            var hasItemInInventory = player.Inventory.Items.Contains(item);
            var isPickingUpItem = !isWearingItem && !hasItemInInventory;
            var isUsable = item.IsUsable;

            if (hasItemInInventory)
            {
                BuildOnUseButtonIfNeeded(player, item);
                BuildButton("Equip", () =>
                {
                    var action = Context.PrototypeFactory.BuildEntityAction<EquipItem>(player) as EquipItem;
                    action.Item = item;
                    action.Slot = item.SlotsWearable[0];
                    Context.PlayerController.ActionList.Enqueue(action);
                });
            }
            else if (isWearingItem)
            {
                BuildOnUseButtonIfNeeded(player, item);
                BuildButton("Unequip", () =>
                {
                    var action = Context.PrototypeFactory.BuildEntityAction<UnequipItem>(player) as UnequipItem;
                    action.Item = item;
                    action.Slot = player.Inventory.GetItemSlotOfEquippedItem(action.Item);
                    Context.PlayerController.ActionList.Enqueue(action);
                });
            }
            else
            {
                var possibleTargets = level.Grid[player.Position].EntitiesInPosition;

                foreach (var possibleTarget in possibleTargets)
                {
                    if (possibleTarget.Inventory.Items.Contains(item))
                    {
                        BuildButton("Take", () =>
                        {
                            var action = Context.PrototypeFactory.BuildEntityAction<PickupItem>(player) as PickupItem;
                            action.Item = item;
                            action.Targets.Add(possibleTarget);
                            Context.PlayerController.ActionList.Enqueue(action);
                            Context.PlayerController.ActionList.Enqueue(action);
                            Context.UIController.Tooltip.Hide();
                            Context.UIController.Refresh();
                        });
                        break;
                    }
                }
            }
            this.transform.position = eventData.position;
            Show();
        }

        private void BuildOnUseButtonIfNeeded(Entity player, Item item)
        {
            if (item.IsUsable)
            {
                var onUseText = item.CustomOnUseText ?? "Use";
                BuildButton(onUseText, () =>
                {
                    var action = Context.PrototypeFactory.BuildEntityAction<UseItemOnSelf>(player) as UseItemOnSelf;
                    action.Item = item;
                    action.Targets.Add(player);
                    Context.PlayerController.ActionList.Enqueue(action);
                });
            }
        }

        private void Purge()
        {
            foreach (var button in buttons)
            {
                GameObject.Destroy(button.gameObject);
            }
            buttons.Clear();
        }

        public void BuildButton(string buttonName, UnityAction handler)
        {
            var _buttonPrefab = Resources.Load<Button>("UI/ButtonPrefab");
            var buttonGameObject = GameObject.Instantiate<Button>(_buttonPrefab);
            buttonGameObject.name = string.Format("{0}Button", buttonName);
            var text = buttonGameObject.GetComponentInChildren<Text>();
            text.text = buttonName;
            buttonGameObject.gameObject.transform.SetParent(transform, false);
            buttons.Add(buttonGameObject);
            buttonGameObject.onClick.AddListener(() =>
            {
                Hide();
            });
            buttonGameObject.onClick.AddListener(handler);
        }

        public override void Refresh()
        {

        }
    }
}
