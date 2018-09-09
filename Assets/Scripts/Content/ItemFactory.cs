using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class ItemFactory
    {
        public static void SetDefaults(Item item)
        {
            item.DisplayName = "No name";
            item.Description = "No set description";
            item.SlotsWearable = new List<ItemSlot>(0);
            item.SlotsOccupiedByWearing = new List<ItemSlot>(0);
            item.NumberOfItems = 1;
            item.MaxStackSize = 1;
            item.Attributes = new Dictionary<Attributes, int>(0);
            item.Effects = new EffectList();
            item.AttackParameters = new List<AttackParameters>(0);
            item.ThrowParameters = new List<AttackParameters>(0);
            item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW;
            item.ExactNumberOfChargesRemaining = 0;
            item.HasUnlimitedCharges = false;
            item.DestroyWhenAllChargesAreConsumed = false;
        }

        public static Item Build(UniqueIdentifier uniqueIdentifier)
        {
            var item = new Item();
            SetDefaults(item);
            item.UniqueIdentifier = uniqueIdentifier;
            if (uniqueIdentifier == UniqueIdentifier.ITEM_LONGSWORD)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD;
                item.DisplayName = "Longsword";
                item.Description = "This is a simple longsword.";
                item.AttackParameters.Add(new AttackParameters()
                {
                    AttackMessage = "{0} slashes {1} for {2} points of {3} damage!",
                    Bonus = 0,
                    DyeNumber = 1,
                    DyeSize = 8,
                    DamageType = DamageTypes.SLASHING,
                });
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
                item.Effects.Add(null, EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ANTIDOTE)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ANTIDOTE;
                item.DisplayName = "Antidote";
                item.Description = "A bubbling vial of viscous green liquid.";
                item.CustomOnUseText = "Drink";
                item.OnUseText = "You quaff a potion";
                item.ExactNumberOfChargesRemaining = 1;
                item.DestroyWhenAllChargesAreConsumed = true;
                item.Effects.Add(null, EffectFactory.Build(UniqueIdentifier.EFFECT_CURE_POISON));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_LUCKY_COIN)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN;
                item.DisplayName = "Lucky Coin";
                item.Description = "This coin is particularly lucky, and is probably deserving of a better description.";
                item.Effects.Add(null, EffectFactory.Build(UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ARROW)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW;
                item.DisplayName = "Arrow";
                item.Description = "People have been using this to kill each other literally forever. The only thing they used before this was a rock. Arrows are considered by many to be more civil.";
                item.MaxStackSize = 20;
                item.NumberOfItems = MathUtil.ChooseRandomIntInRange(5, item.MaxStackSize / 2 - 1);
                item.SlotsWearable.Add(ItemSlot.Ammo);
                item.SlotsOccupiedByWearing.Add(ItemSlot.Ammo);
            }
            else
            {
                throw new NotImplementedException();
            }

            return item;
        }
    }
}
