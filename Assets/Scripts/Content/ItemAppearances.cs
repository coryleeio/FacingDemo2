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
            };
        }

        public static List<ItemAppearance> LoadAll()
        {
            var retVal = new List<ItemAppearance>();
            var longsword = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD, "Sprites/Longsword");
            retVal.Add(longsword);

            var shortbow = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_BOW, "Sprites/Bow");
            retVal.Add(shortbow);

            var luckyCoin = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN, "Sprites/LuckyCoin");
            retVal.Add(luckyCoin);

            var greenPotion = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_GREEN_POTION, "Sprites/GreenPotion");
            retVal.Add(greenPotion);

            var arrow = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_ARROW, "Sprites/Arrow");
            retVal.Add(arrow);

            var actionStaff = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_ACTION_STAFF, "Sprites/ActionStaff");
            retVal.Add(actionStaff);

            var swirlStaff = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_SWIRL_STAFF, "Sprites/SwirlStaff");
            retVal.Add(swirlStaff);

            return retVal;
        }
    }
}
