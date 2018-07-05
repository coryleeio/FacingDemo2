using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Inventory
    {
        public List<Item> Items = new List<Item>(0);
        public Dictionary<ItemSlot, Item> EquippedItemBySlot = new Dictionary<ItemSlot, Item>();

        public void EquipItem(Item item)
        {
            Assert.IsNotNull(item);
            if (item.SlotsWearable.Count > 0)
            {
                EquipItemToSlot(item, item.SlotsWearable[0]);
            }
        }

        public Item ItemByIdentifier(UniqueIdentifier identifier)
        {
            foreach (var pair in EquippedItemBySlot)
            {
                if (pair.Value.UniqueIdentifier == identifier)
                {
                    return pair.Value;
                }
            }
            foreach (var item in Items)
            {
                if (item.UniqueIdentifier == item.UniqueIdentifier)
                {
                    return item;
                }
            }
            return null;
        }

        public void RemoveItem(Item item)
        {
            foreach (var pair in EquippedItemBySlot)
            {
                if (pair.Value == item)
                {
                    UnequipItemInSlot(pair.Key);
                }
            }
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
        }

        public void EquipItemToSlot(Item item, ItemSlot slot)
        {
            Assert.IsNotNull(item);
            if (item.SlotsWearable.Contains(slot))
            {
                foreach (var slotCollision in item.SlotsOccupiedByWearing)
                {
                    UnequipItemInSlot(slot);
                }
            }
            if (Items.Contains(item))
            {
                Items.Remove(item);
            }
            EquippedItemBySlot[slot] = item;
        }

        public void UnequipItemInSlot(ItemSlot slot)
        {
            var item = GetItemBySlot(slot);
            if (item != null)
            {
                EquippedItemBySlot.Remove(slot);
                Items.Add(item);
            }
        }

        public Item GetItemBySlot(ItemSlot slot)
        {
            if (EquippedItemBySlot.ContainsKey(slot))
            {
                var item = EquippedItemBySlot[slot];
                return item;
            }
            else
            {
                return null;
            }
        }
    }
}
