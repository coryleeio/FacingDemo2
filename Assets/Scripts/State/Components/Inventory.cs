using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Inventory
    {
        public List<Item> Items = new List<Item>();
        public Dictionary<ItemSlot, Item> EquippedItemBySlot = new Dictionary<ItemSlot, Item>();

        [JsonIgnore]
        public Entity Entity;

        public virtual void Rewire(Entity entity)
        {
            Entity = entity;
        }

        public void EquipItem(Item item)
        {
            Assert.IsNotNull(item);
            if (item.SlotsWearable.Count > 0)
            {
                EquipItemToSlot(item, item.SlotsWearable[0]);
            }
        }

        public void AddItem(Item item)
        {
            AddItem(item, -1);
        }

        public void AddItem(Item item, int position)
        {
            var targetPosition = position;

            if (position == -1 || (Items[position] != null && !Items[position].CanStack(item)))
            {
                targetPosition = FindFirstAvailablePositionForItem(item);
            }

            if (Items[targetPosition] != null)
            {
                if (Items[targetPosition].CanStack(item))
                {
                    Items[targetPosition].NumberOfItems += item.NumberOfItems;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                Items[targetPosition] = item;
            }
        }

        private int FindFirstAvailablePositionForItem(Item item)
        {
            var firstNullIndex = -1;
            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i] != null && Items[i].CanStack(item))
                {
                    return i;
                }
                if (Items[i] == null && firstNullIndex == -1)
                {
                    firstNullIndex = i;
                }
            }
            Assert.IsTrue(firstNullIndex != -1); // need to handle full inventory
            return firstNullIndex;
        }

        public void SwapItemPosition(Item item, int oldIndex, int newIndex)
        {
            RemoveItemStack(item);
            if (Items[newIndex] != null)
            {
                Items[oldIndex] = Items[newIndex];
            }
            Items[newIndex] = item;
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
                if (item != null && item.UniqueIdentifier == item.UniqueIdentifier)
                {
                    return item;
                }
            }
            return null;
        }

        public void ConsumeItem(Item item)
        {
            if (item.NumberOfItems > 1)
            {
                item.NumberOfItems--;
            }
            else
            {
                RemoveItemStack(item);
            }
        }

        public void RemoveItemStack(Item item)
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
                var ind = Items.IndexOf(item);
                Items[ind] = null;
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
            RemoveItemStack(item);
            EquippedItemBySlot[slot] = item;
            if (Entity != null)
            {
                foreach (var effect in item.Effects.Values)
                {
                    effect.OnApply(Entity);
                }
            }
        }

        public void UnequipItemInSlot(ItemSlot slot, int IndexToMoveTo = -1)
        {
            var item = GetItemBySlot(slot);
            if (item != null)
            {
                EquippedItemBySlot.Remove(slot);
                AddItem(item, IndexToMoveTo);
                if (Entity != null)
                {
                    foreach (var effect in item.Effects.Values)
                    {
                        effect.OnRemove(Entity);
                    }
                }
            }
        }

        public bool IsWearing(Item item)
        {
            foreach (var slot in item.SlotsWearable)
            {
                var itemInSlot = GetItemBySlot(slot);
                if (itemInSlot != null && itemInSlot == item)
                {
                    return true;
                }
            }
            return false;
        }

        public ItemSlot GetItemSlotOfEquippedItem(Item item)
        {
            foreach (var slot in item.SlotsWearable)
            {
                var itemInSlot = GetItemBySlot(slot);
                if (itemInSlot == item)
                {
                    return slot;
                }
            }
            throw new NotImplementedException("Must not be equipped after all..");
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

        public int NumberOfItems
        {
            get
            {
                var count = 0;
                foreach (var item in Items)
                {
                    if (item != null)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public List<Item> ChooseRandomItemsFromInventory(int numberToChoose)
        {
            var validItems = new List<Item>();
            foreach (var item in Items)
            {
                if (item != null)
                {
                    validItems.Add(item);
                }
            }
            foreach(var pair in EquippedItemBySlot)
            {
                validItems.Add(pair.Value);
            }
            return MathUtil.ChooseNRandomElements(numberToChoose, validItems);
        }

        public int NumberOfEquippedItems
        {
            get
            {
                var count = 0;
                foreach (var pair in EquippedItemBySlot)
                {
                    if (pair.Value != null)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public bool HasAnyItems
        {
            get
            {
                return NumberOfEquippedItems > 0 || NumberOfItems > 0;
            }
        }
    }
}
