using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class ItemAppearance
    {
        public string Identifier { get; set; }
        public Sprite InventorySprite;
        public Sprite GroundSprite;
        public Dictionary<SpriteAttachment, Sprite> WornItemSpritePerSlot;
    }
}
