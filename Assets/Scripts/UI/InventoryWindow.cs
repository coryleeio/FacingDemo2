using UnityEngine;
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
            var slotPrefab = Resources.Load<ItemDropSlot>("UI/ItemDropSlot");
            for (var i = 0; i < inventory.Items.Count; i++)
            {
                var slotInstance = GameObject.Instantiate<ItemDropSlot>(slotPrefab);
                slotInstance.Index = i;
                slotInstance.Entity = player;
                slotInstance.transform.SetParent(container.transform, false);

                if (inventory.Items[i] != null)
                {
                    var itemInSlot = inventory.Items[i];
                    var draggablePrefab = Resources.Load<InventoryDraggable>("UI/InventoryDraggable");
                    var draggableInstance = GameObject.Instantiate<InventoryDraggable>(draggablePrefab);
                    draggableInstance.transform.SetParent(slotInstance.transform, false);
                    draggableInstance.Source = player;
                    draggableInstance.Item = inventory.Items[i];
                    var stackCounter = draggableInstance.GetComponentInChildren<Text>();
                    stackCounter.gameObject.SetActive(itemInSlot.MaxStackSize > 1);
                    stackCounter.text = itemInSlot.NumberOfItems.ToString();
                    var spr = draggableInstance.GetComponent<Image>();
                    spr.sprite = itemInSlot.ItemAppearance.InventorySprite;
                }
            }
        }

        public override void Show()
        {
            active = true;
            GetComponent<InventoryWindow>().gameObject.SetActive(true);
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
