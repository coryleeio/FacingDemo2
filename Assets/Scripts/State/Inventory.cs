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
    }
}
