using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class ItemAppearances
    {
        public static List<ItemAppearance> LoadAll()
        {
            return new List<ItemAppearance>()
            {
                new ItemAppearance()
                {
                    UniqueIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD,
                    InventorySprite=Resources.Load<Sprite>("Tilesets/Stone/StoneFloor"),
                },
                new ItemAppearance()
                {
                    UniqueIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN,
                    InventorySprite=Resources.Load<Sprite>("Tilesets/Stone/StoneFloor"),
                },
                new ItemAppearance()
                {
                    UniqueIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW,
                    InventorySprite=Resources.Load<Sprite>("Tilesets/Stone/StoneFloor"),
                },
            };
        }
    }
}
