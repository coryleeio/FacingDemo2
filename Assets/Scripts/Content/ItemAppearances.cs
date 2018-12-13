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
                WornItemSpritePerSlot = new Dictionary<SpriteAttachment, Sprite>(),
            };
        }

        public static List<ItemAppearance> LoadAll()
        {
            var retVal = new List<ItemAppearance>();
            var longsword = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD, "Sprites/Longsword");
            longsword.WornItemSpritePerSlot.Add(SpriteAttachment.MainHandFront, Resources.Load<Sprite>("Sprites/Longsword"));
            retVal.Add(longsword);

            var shortbow = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_BOW, "Sprites/Bow");
            shortbow.WornItemSpritePerSlot.Add(SpriteAttachment.MainHandFront, Resources.Load<Sprite>("Sprites/Bow"));
            retVal.Add(shortbow);

            var luckyCoin = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN, "Sprites/LuckyCoin");
            retVal.Add(luckyCoin);

            var greenPotion = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_GREEN_POTION, "Sprites/GreenPotion");
            retVal.Add(greenPotion);

            var redPotion = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_RED_POTION, "Sprites/RedPotion");
            retVal.Add(redPotion);

            var purplePotion = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_PURPLE_POTION, "Sprites/PurplePotion");
            retVal.Add(purplePotion);

            var bluePotion = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_BLUE_POTION, "Sprites/BluePotion");
            retVal.Add(bluePotion);

            var arrow = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_ARROW, "Sprites/Arrow");
            retVal.Add(arrow);

            var actionStaff = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_ACTION_STAFF, "Sprites/ActionStaff");
            actionStaff.WornItemSpritePerSlot.Add(SpriteAttachment.MainHandFront, Resources.Load<Sprite>("Sprites/ActionStaff"));
            retVal.Add(actionStaff);

            var dagger = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_DAGGER, "Sprites/Dagger");
            dagger.WornItemSpritePerSlot.Add(SpriteAttachment.MainHandFront, Resources.Load<Sprite>("Sprites/Dagger"));
            retVal.Add(dagger);

            var mace = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_MACE, "Sprites/Mace");
            mace.WornItemSpritePerSlot.Add(SpriteAttachment.MainHandFront, Resources.Load<Sprite>("Sprites/Mace"));
            retVal.Add(mace);

            var swirlStaff = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_SWIRL_STAFF, "Sprites/SwirlStaff");
            swirlStaff.WornItemSpritePerSlot.Add(SpriteAttachment.MainHandFront, Resources.Load<Sprite>("Sprites/SwirlStaff"));
            retVal.Add(swirlStaff);

            var shieldOfAmalure = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_SHIELD_OF_AMALURE, "Sprites/ShieldOfAmalureSE");
            shieldOfAmalure.WornItemSpritePerSlot.Add(SpriteAttachment.OffHandFrontSE, Resources.Load<Sprite>("Sprites/ShieldOfAmalureSE"));
            shieldOfAmalure.WornItemSpritePerSlot.Add(SpriteAttachment.OffHandFrontNE, Resources.Load<Sprite>("Sprites/ShieldOfAmalureNE"));
            retVal.Add(shieldOfAmalure);

            var robeOfWonders = BuildDefaultItemAppearance(UniqueIdentifier.ITEM_APPEARANCE_ROBE_OF_WONDERS, "Sprites/RobeOfWondersFrontSE");
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.ChestFrontNE, Resources.Load<Sprite>("Sprites/RobeOfWondersFrontNE"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.ChestFrontSE, Resources.Load<Sprite>("Sprites/RobeOfWondersFrontSE"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.HelmetBackSE, Resources.Load<Sprite>("Sprites/RobeOfWondersHoodBackSE"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.HelmetFrontNE, Resources.Load<Sprite>("Sprites/RobeOfWondersHoodFrontNE"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.HelmetFrontSE, Resources.Load<Sprite>("Sprites/RobeOfWondersHoodFrontSE"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.LeftArmFrontSE, Resources.Load<Sprite>("Sprites/RobeOfWondersLeftArmFront"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.LeftArmFrontNE, Resources.Load<Sprite>("Sprites/RobeOfWondersLeftArmFront"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.RightArmFrontSE, Resources.Load<Sprite>("Sprites/RobeOfWondersRightArmFront"));
            robeOfWonders.WornItemSpritePerSlot.Add(SpriteAttachment.RightArmFrontNE, Resources.Load<Sprite>("Sprites/RobeOfWondersRightArmFront"));
            retVal.Add(robeOfWonders);

            return retVal;
        }
    }
}
