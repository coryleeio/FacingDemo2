using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class ItemAppearances
    {
        public static GameObject BuildDefaultProjectile(Sprite sprite)
        {
            GameObject myGameObject = new GameObject();
            myGameObject.name = string.Format("{0} Generated Projectile Prefab", sprite.name);
            myGameObject.hideFlags = HideFlags.HideInHierarchy;
            var spriteRenderer = myGameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 30000; // Always in front
            spriteRenderer.sprite = sprite;
            myGameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            var defaultMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            spriteRenderer.material = defaultMaterial;

            GameObject.DontDestroyOnLoad(myGameObject);
            myGameObject.transform.position = MathUtil.Offscreen;
            return myGameObject;
        }

        public static ItemAppearance BuildDefaultItemAppearance(UniqueIdentifier uniqueIdentifier, string inventorySpriteLocation)
        {
            return new ItemAppearance()
            {
                UniqueIdentifier = uniqueIdentifier,
                InventorySprite = Resources.Load<Sprite>(inventorySpriteLocation),
                MeleeProjectileTravelTime = 0.25f,
                MeleeProjectilePrefab = BuildDefaultProjectile(Resources.Load<Sprite>(inventorySpriteLocation)),
                RangedProjectilePrefab = BuildDefaultProjectile(Resources.Load<Sprite>(inventorySpriteLocation)),
                ThrownProjectilePrefab = BuildDefaultProjectile(Resources.Load<Sprite>(inventorySpriteLocation)),
                ZappedProjectilePrefab = BuildDefaultProjectile(Resources.Load<Sprite>(inventorySpriteLocation)),
                ShouldSpinWhenThrown = true,
            };
        }

        public static List<ItemAppearance> LoadAll()
        {
            var retVal = new List<ItemAppearance>();
            var longsword = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD, "Sprites/Longsword");
            retVal.Add(longsword);

            var luckyCoin = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN, "Sprites/LuckyCoin");
            retVal.Add(luckyCoin);

            var greenPotion = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_GREEN_POTION, "Sprites/GreenPotion");
            retVal.Add(greenPotion);

            var arrow = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_ARROW, "Sprites/Arrow");
            arrow.ShouldSpinWhenThrown = false;
            retVal.Add(arrow);

            return retVal;
        }
    }
}
