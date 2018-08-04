using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class LootWindow : UIComponent
    {
        private bool active = false;
        public List<Entity> _targets;

        public bool StillHasItems
        {
            get
            {
                if(_targets == null || _targets.Count == 0)
                {
                    return false;
                }
                foreach(var target in _targets)
                {
                    if(target.Inventory.HasAnyItems)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public override void Hide()
        {
            active = false;
            GetComponent<LootWindow>().gameObject.SetActive(false);
            if(Context.UIController.Tooltip != null)
            {
                Context.UIController.Tooltip.Hide();
            }
            Context.UIController.RemoveWindow(this);
        }

        public override void Show()
        {
            active = true;
            GetComponent<LootWindow>().gameObject.SetActive(true);
            Context.UIController.PushWindow(this);
        }

        public void ShowFor(List<Entity> entity)
        {
            _targets = new List<Entity>(entity.Count);
            _targets.AddRange(entity);
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

        public override void Refresh()
        {
            if (_targets == null)
            {
                return;
            }
            var container = GetComponentInChildren<ItemContainer>();
            foreach (Transform child in container.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            foreach (var target in _targets)
            {
                var inventory = target.Inventory;
                var slotPrefab = Resources.Load<InventoryDropSlot>("UI/ItemDropSlot");

                for (var i = 0; i < inventory.Items.Count; i++)
                {
                    if (inventory.Items[i] != null)
                    {
                        var instance = GameObject.Instantiate<InventoryDropSlot>(slotPrefab);
                        instance.Index = i;
                        instance.Entity = target;
                        instance.transform.SetParent(container.transform, false);
                        BuildDraggableItemForPlayerParentToTransform(inventory.Items[i], target, instance.transform);
                    }
                }
            }

            Show();
        }
    }
}
