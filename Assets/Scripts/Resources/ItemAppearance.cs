using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class ItemAppearance : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public Sprite InventorySprite;
        public Dictionary<SpriteAttachment, Sprite> WornItemSpritePerSlot;
    }
}
