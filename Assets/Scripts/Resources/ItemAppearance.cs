using UnityEngine;

namespace Gamepackage
{
    public class ItemAppearance : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public Sprite InventorySprite;
    }
}
