using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class InventoryUtil
    {

        public static void TryEquipItems(Entity entity, List<Item> items)
        {
            if(items == null)
            {
                return;
            }

            foreach(var item in items)
            {
                if(item !=null)
                {
                    EquipItem(entity, item);
                }
            }
        }


        public static void EquipItem(Entity entity, Item item)
        {
            Assert.IsNotNull(item);
            if (item.Template.SlotsWearable.Count > 0)
            {
                EquipItemToSlot(entity, item, item.Template.SlotsWearable[0]);
            }
        }

        public static List<Item> GetEquippedItems(Entity entity)
        {
            var aggregate = new List<Item>();
            if(entity.Inventory != null)
            {
                foreach(var pair in entity.Inventory.EquippedItemBySlot)
                {
                    if(!aggregate.Contains(pair.Value))
                    {
                        aggregate.Add(pair.Value);
                    }
                }
            }
            return aggregate;
        }

        public static void AddItem(Entity entity, Item item)
        {
            AddItem(entity, item, -1);
        }

        public static void AddItems(Entity entity, List<Item> items)
        {
            foreach(var item in items)
            {
                AddItem(entity, item, -1);
            }
        }

        public static void AddItem(Entity entity, Item item, int position)
        {
            var targetPosition = position;
            var inventory = entity.Inventory;
            if (position == -1 || (inventory.Items[position] != null && !inventory.Items[position].CanStack(item)))
            {
                targetPosition = FindFirstAvailablePositionForItem(entity, item);
            }

            if (inventory.Items[targetPosition] != null)
            {
                if (inventory.Items[targetPosition].CanStack(item))
                {
                    inventory.Items[targetPosition].NumberOfItems += item.NumberOfItems;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                inventory.Items[targetPosition] = item;
            }
        }

        private static int FindFirstAvailablePositionForItem(Entity entity, Item item)
        {
            var firstNullIndex = -1;
            for (var i = 0; i < entity.Inventory.Items.Count; i++)
            {
                if (entity.Inventory.Items[i] != null && entity.Inventory.Items[i].CanStack(item))
                {
                    return i;
                }
                if (entity.Inventory.Items[i] == null && firstNullIndex == -1)
                {
                    firstNullIndex = i;
                }
            }
            Assert.IsTrue(firstNullIndex != -1); // need to handle full inventory
            return firstNullIndex;
        }

        public static void SwapItemPosition(Entity entity, Item item, int oldIndex, int newIndex)
        {
            var inventory = entity.Inventory;
            RemoveWholeItemStack(entity, item);
            if (inventory.Items[newIndex] != null)
            {
                inventory.Items[oldIndex] = inventory.Items[newIndex];
            }
            inventory.Items[newIndex] = item;
        }

        public static Item ItemByIdentifier(Entity entity, string identifier)
        {
            var inventory = entity.Inventory;
            foreach (var pair in inventory.EquippedItemBySlot)
            {
                if (pair.Value.TemplateIdentifier == identifier)
                {
                    return pair.Value;
                }
            }
            foreach (var item in inventory.Items)
            {
                if (item != null && item.TemplateIdentifier == identifier)
                {
                    return item;
                }
            }
            return null;
        }

        public static void RemoveWholeItemStack(Entity entity, Item item)
        {
            // If you modify the dictionary while iterating it,
            // it will fail silently sometimes due to a undetected race condition
            ItemSlot found = default(ItemSlot);
            var inventory = entity.Inventory;
            foreach (var pair in inventory.EquippedItemBySlot)
            {
                if (pair.Value == item)
                {
                    found = pair.Key;
                }
            }
            if (found != default(ItemSlot))
            {
                UnequipItemInSlot(entity, found);
            }
            if (inventory.Items.Contains(item))
            {
                var ind = inventory.Items.IndexOf(item);
                inventory.Items[ind] = null;
            }
            if (entity != null && entity.EntityType.Identifier == "ENTITY_TYPE_GROUND_DROP")
            {
                // Remove ground drop entities if their last item is looted
                if (Context.Game != null && Context.Game.CurrentLevel != null)
                {
                    Context.EntitySystem.Deregister(entity, Context.Game.CurrentLevel);
                }
            }
        }

        public static void EquipItemToSlot(Entity entity, Item item, ItemSlot slot)
        {
            Assert.IsNotNull(item);
            Assert.IsNotNull(entity.Inventory);
            var inventory = entity.Inventory;
            var itemsThatMustBeRemoved = new List<Item>();

            foreach (var pair in inventory.EquippedItemBySlot)
            {
                foreach (var slotOccupied in item.Template.SlotsOccupiedByWearing)
                {
                    var slotOfEquippedItem = pair.Key;
                    var equippedItem = pair.Value;
                    if (equippedItem.Template.SlotsOccupiedByWearing.Contains(slotOccupied))
                    {
                        if (!itemsThatMustBeRemoved.Contains(equippedItem))
                        {
                            itemsThatMustBeRemoved.Add(equippedItem);
                        }
                    }
                }
            }
            foreach (var itemThatMustBeRemoved in itemsThatMustBeRemoved)
            {
                UnequipItemInSlot(entity, GetItemSlotOfEquippedItem(entity, itemThatMustBeRemoved));
            }

            RemoveWholeItemStack(entity, item);
            inventory.EquippedItemBySlot[slot] = item;
            if(item.IsEnchanted)
            {
                foreach (var effect in item.Enchantment.WornEffects)
                {
                    effect.EffectImpl.OnApplySelf(effect, entity);
                }
            }

            if (entity.ViewGameObject != null)
            {
                ViewFactory.RebuildView(entity);
            }
        }

        public static void UnequipItemInSlot(Entity entity, ItemSlot slot, int IndexToMoveTo = -1)
        {
            var item = GetItemBySlot(entity, slot);
            if (item != null)
            {
                entity.Inventory.EquippedItemBySlot.Remove(slot);
                AddItem(entity, item, IndexToMoveTo);
                if(item.IsEnchanted)
                {
                    foreach (var effect in item.Enchantment.WornEffects)
                    {
                        effect.EffectImpl.OnRemove(effect, entity);
                    }
                }
                if (entity.ViewGameObject != null)
                {
                    ViewFactory.RebuildView(entity);
                }
            }
        }

        public static bool IsWearing(Entity entity, Item item)
        {
            foreach (var slot in item.Template.SlotsWearable)
            {
                var itemInSlot = GetItemBySlot(entity, slot);
                if (itemInSlot != null && itemInSlot == item)
                {
                    return true;
                }
            }
            return false;
        }

        public static ItemSlot GetItemSlotOfEquippedItem(Entity entity, Item item)
        {
            foreach (var slot in item.Template.SlotsWearable)
            {
                var itemInSlot = GetItemBySlot(entity, slot);
                if (itemInSlot == item)
                {
                    return slot;
                }
            }
            throw new NotImplementedException("Must not be equipped after all..");
        }

        public static Item GetMainHandOrDefaultWeapon(Entity entity)
        {
            var item = GetItemBySlot(entity, ItemSlot.MainHand);
            if(item == null)
            {
                return entity.DefaultAttackItem;
            }
            return item;
        }

        public static Item GetItemBySlot(Entity entity, ItemSlot slot)
        {
            if (entity.Inventory.EquippedItemBySlot.ContainsKey(slot))
            {
                var item = entity.Inventory.EquippedItemBySlot[slot];
                return item;
            }
            else
            {
                return null;
            }
        }

        public static int NumberOfItems(Entity entity)
        {
            if(entity.Inventory == null)
            {
                return 0;
            }
            else
            {
                var count = 0;
                foreach (var item in entity.Inventory.Items)
                {
                    if (item != null)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public static List<Item> ChooseRandomItemsFromInventory(Entity entity, int numberToChoose)
        {
            var validItems = new List<Item>();
            var inventory = entity.Inventory;
            foreach (var item in inventory.Items)
            {
                if (item != null)
                {
                    validItems.Add(item);
                }
            }
            foreach (var pair in inventory.EquippedItemBySlot)
            {
                validItems.Add(pair.Value);
            }
            return MathUtil.ChooseNRandomElements(numberToChoose, validItems);
        }

        public static int NumberOfEquippedItems(Entity entity)
        {
            if(entity.Inventory == null)
            {
                return 0;
            }
            else
            {
                var count = 0;
                foreach (var pair in entity.Inventory.EquippedItemBySlot)
                {
                    if (pair.Value != null)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        public static bool HasAnyItems(Entity entity)
        {
            return entity.Inventory != null && (NumberOfEquippedItems(entity) > 0 || NumberOfItems(entity) > 0);
        }
    }
}
