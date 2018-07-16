using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Gamepackage
{
    public class InventoryWindow : UIComponent
    {
        private bool active = false;

        public override void Hide()
        {
            active = false;
            GetComponent<InventoryWindow>().gameObject.SetActive(false);
        }

        public override void Refresh()
        {
            var player = Context.GameStateManager.Game.CurrentLevel.Player;
            var inventory = player.Inventory;
            var container = GetComponentInChildren<ItemContainer>();
            foreach (Transform child in container.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            var slotPrefab = Resources.Load<InventoryDropSlot>("UI/ItemDropSlot");
            for (var i = 0; i < inventory.Items.Count; i++)
            {
                var slotInstance = GameObject.Instantiate<InventoryDropSlot>(slotPrefab);
                slotInstance.Index = i;
                slotInstance.Entity = player;
                slotInstance.transform.SetParent(container.transform, false);

                if (inventory.Items[i] != null)
                {
                    var itemInSlot = inventory.Items[i];
                    BuildDraggableItemForPlayerParentToTransform(itemInSlot, player, slotInstance.transform);
                }
            }
            var itemSlotsToEquipmentDropSlots = new Dictionary<ItemSlot, EquipmentDropSlot>();
            var allSlots = GetComponentsInChildren<EquipmentDropSlot>();

            foreach (var slot in allSlots)
            {
                if (slot.Slot != ItemSlot.None)
                {
                    slot.Player = player;
                    itemSlotsToEquipmentDropSlots[slot.Slot] = slot;
                    foreach (Transform child in slot.transform)
                    {
                        GameObject.Destroy(child.gameObject);
                    }
                }
            }

            foreach (var enumVal in Enum.GetValues(typeof(ItemSlot)))
            {
                var castVal = (ItemSlot)enumVal;
                if (castVal != ItemSlot.None)
                {
                    Assert.IsTrue(itemSlotsToEquipmentDropSlots.ContainsKey(castVal));
                    var dropableSlot = itemSlotsToEquipmentDropSlots[castVal];

                    // Ensure that after a failed drop the grid got reactivated since these slots are not
                    // recreated.
                    var grid = dropableSlot.GetComponent<GridLayoutGroup>();
                    grid.enabled = true;

                    if (inventory.EquippedItemBySlot.ContainsKey(castVal))
                    {
                        var item = inventory.EquippedItemBySlot[castVal];
                        if (item != null)
                        {
                            var itemEquippedToSlot = inventory.EquippedItemBySlot[castVal];
                            BuildDraggableItemForPlayerParentToTransform(itemEquippedToSlot, player, dropableSlot.transform);
                        }
                    }
                }
            }
        }

        private static void BuildDraggableItemForPlayerParentToTransform(Item item, Entity player, Transform parentTransform)
        {
            var draggablePrefab = Resources.Load<DraggableItem>("UI/DraggableItem");
            var draggableInstance = GameObject.Instantiate<DraggableItem>(draggablePrefab);
            draggableInstance.transform.SetParent(parentTransform, false);
            draggableInstance.Source = player;
            draggableInstance.Item = item;
            var stackCounter = draggableInstance.GetComponentInChildren<Text>();
            stackCounter.gameObject.SetActive(item.MaxStackSize > 1);
            stackCounter.text = item.NumberOfItems.ToString();
            var spr = draggableInstance.GetComponent<Image>();
            spr.sprite = item.ItemAppearance.InventorySprite;
        }

        public override void Show()
        {
            active = true;
            GetComponent<InventoryWindow>().gameObject.SetActive(true);
            Context.UIController.PushWindow(this);
            Refresh();
        }

        public void Toggle()
        {
            if (active)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}
