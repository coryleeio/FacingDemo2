using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public class ItemFactory
    {
        public static void SetDefaults(Item item)
        {
            item.DisplayName = "item.no.name".Localize();
            item.Description = "item.no.description".Localize();
            item.SlotsWearable = new List<ItemSlot>(0);
            item.SlotsOccupiedByWearing = new List<ItemSlot>(0);
            item.NumberOfItems = 1;
            item.MaxStackSize = 1;
            item.Attributes = new Dictionary<Attributes, int>(0);
            item.Effects = new List<Effect>();
            item.AttackParameters = new List<AttackParameters>(0);
            item.ThrowParameters = new List<AttackParameters>(0);
            item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW;
            item.ExactNumberOfChargesRemaining = 0;
            item.HasUnlimitedCharges = false;
            item.DestroyWhenAllChargesAreConsumed = false;
            item.IsUsable = false;
        }

        public static Item Build(UniqueIdentifier uniqueIdentifier)
        {
            var item = new Item();
            SetDefaults(item);
            item.UniqueIdentifier = uniqueIdentifier;
            if (uniqueIdentifier == UniqueIdentifier.ITEM_LONGSWORD)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LONGSWORD;
                item.DisplayName = "item.longsword.name".Localize();
                item.Description = "item.longsword.description".Localize();
                item.AttackParameters.Add(new AttackParameters()
                {
                    AttackMessage = "attacks.slashing.1".Localize(),
                    Bonus = 0,
                    DyeNumber = 1,
                    DyeSize = 8,
                    DamageType = DamageTypes.SLASHING,
                });
                item.SlotsWearable.Add(ItemSlot.MainHand);
                item.SlotsOccupiedByWearing.Add(ItemSlot.MainHand);
                item.Effects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON));
                item.Effects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_STRENGTH_OF_GIANTS));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ANTIDOTE)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ANTIDOTE;
                item.DisplayName = "item.antidote.name".Localize();
                item.Description = "item.antidote.description".Localize();
                item.CustomOnUseText = "item.antidote.action".Localize();
                item.OnUseText = "item.antidote.on.use".Localize();
                item.ExactNumberOfChargesRemaining = 1;
                item.DestroyWhenAllChargesAreConsumed = true;
                item.Effects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_POISON_IMMUNITY));
                item.IsUsable = true;
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_LUCKY_COIN)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_LUCKY_COIN;
                item.DisplayName = "item.lucky.coin.name".Localize();
                item.Description = "item.lucky.coin.description".Localize();
                item.Effects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_LUCKY_COIN_LIFE_SAVE));
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_ARROW)
            {
                item.ItemAppearanceIdentifier = UniqueIdentifier.ITEM_APPEARANCE_ARROW;
                item.DisplayName = "item.arrow.name".Localize();
                item.Description = "item.arrow.description".Localize();
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
