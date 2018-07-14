using UnityEngine;

namespace Gamepackage
{
    public class LootWindow : UIComponent
    {
        private bool active = false;

        public override void Hide()
        {
            active = false;
            GetComponent<LootWindow>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            active = true;
            GetComponent<LootWindow>().gameObject.SetActive(true);
        }

        public void ShowFor(Entity entity)
        {
            var inventory = entity.Inventory;
            var container = GetComponentInChildren<ItemContainer>();
            foreach (Transform child in container.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            var slotPrefab = Resources.Load<ItemDropSlot>("UI/ItemDropSlot");

            for (var i = 0; i < inventory.Items.Count; i++)
            {
                var instance = GameObject.Instantiate<ItemDropSlot>(slotPrefab);
                instance.Index = i;
                instance.Entity = entity;
                instance.transform.SetParent(container.transform, false);
            }
            Show();
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
