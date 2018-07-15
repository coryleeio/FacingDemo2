using UnityEngine;

namespace Gamepackage
{
    public class LootWindow : UIComponent
    {
        private bool active = false;
        private Entity target;

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
            target = entity;
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

        public override void Refresh()
        {
            if (target == null)
            {
                return;
            }
            var inventory = target.Inventory;
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
                instance.Entity = target;
                instance.transform.SetParent(container.transform, false);
            }
            Show();
        }
    }
}
