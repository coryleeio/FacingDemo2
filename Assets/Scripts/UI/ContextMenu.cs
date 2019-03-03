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
            BuildButton("context.menu.buttons.info".Localize(), () =>
            {
                Context.UIController.ItemInspectionWindow.ShowFor(item);
            });

            var level = Context.Game.CurrentLevel;
            var player = level.Player;
            var isWearingItem = InventoryUtil.IsWearing(player, item);
            var hasItemInInventory = player.Inventory.Items.Contains(item);
            var isPickingUpItem = !isWearingItem && !hasItemInInventory;
            var isUsable = item.IsUsable;
            var isEquippable = item.SlotsWearable.Count > 0;


            if (hasItemInInventory && item.IsUsable)
            {
                var onUseText = item.CustomOnUseText ?? "context.menu.buttons.use.default".Localize();
                BuildButton(onUseText, () =>
                {
                    var grid = Context.Game.CurrentLevel.Grid;
                    var attackTypeParameters = CombatUtil.AttackTypeParametersResolve(player, AttackType.ApplyToSelf, item);
                    var attackParameters = CombatUtil.AttackParametersResolve(player, AttackType.ApplyToSelf, item);
                    var calculatedAttack = CombatUtil.CalculateAttack(grid, player, AttackType.ApplyToSelf, item, player.Position, attackTypeParameters, attackParameters);
                    Attack attack = new Attack(calculatedAttack);
                    Context.PlayerController.ActionList.Enqueue(attack);
                });
            }

            if ((hasItemInInventory || isWearingItem) && item.CanBeUsedInAttackType(AttackType.Thrown))
            {
                BuildButton("context.menu.buttons.throw".Localize(), () =>
                {
                    Context.PlayerController.StartAiming(AttackType.Thrown, item);
                    Context.UIController.InventoryWindow.Hide();
                });
            }

            if (hasItemInInventory && isEquippable)
            {
                BuildButton("context.menu.buttons.equip".Localize(), () =>
                {
                    var action = new EquipItem
                    {
                        Source = player,
                        Item = item,
                        Slot = item.SlotsWearable[0]
                    };
                    Context.PlayerController.ActionList.Enqueue(action);
                });
            }
            if (isWearingItem)
            {
                BuildButton("context.menu.buttons.unequip".Localize(), () =>
                {
                    var action = new UnequipItem
                    {
                        Source = player,
                        Item = item
                    };
                    action.Slot = InventoryUtil.GetItemSlotOfEquippedItem(player, action.Item);
                    Context.PlayerController.ActionList.Enqueue(action);
                });
            }
            if (!isWearingItem && !hasItemInInventory)
            {
                var possibleTargets = level.Grid[player.Position].EntitiesInPosition;

                foreach (var possibleTarget in possibleTargets)
                {
                    if (possibleTarget.Inventory.Items.Contains(item))
                    {
                        BuildButton("context.menu.buttons.take".Localize(), () =>
                        {
                            var action = new PickupItem
                            {
                                Source = player,
                                Item = item
                            };
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
            this.transform.position = eventData.pointerPressRaycast.gameObject.transform.position;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
            Show();
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
            var _buttonPrefab = Resources.Load<Button>("Prefabs/UI/ContextMenuButton");
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
