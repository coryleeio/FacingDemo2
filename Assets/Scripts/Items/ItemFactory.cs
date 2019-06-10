using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class ItemFactory
    {
        public static Item Build(string uniqueIdentifier)
        {
            var itemTemplate = Context.ResourceManager.Load<ItemTemplate>(uniqueIdentifier);
            var item = new Item()
            {
                TemplateIdentifier = uniqueIdentifier,
                NumberOfItems = Random.Range(itemTemplate.MinStackSize, itemTemplate.MaxStackSize),
                Attributes = new Dictionary<Attributes, int>(),
                EffectsGrantedToOwner = EffectFactory.BuildAll(itemTemplate.EffectsGrantedToOwner),
            };
            foreach (var pair in itemTemplate.TemplateAttributes)
            {
                item.Attributes.Add(pair.Key, pair.Value);
            }

            if (itemTemplate.PossibleEnchantments.Count > 0)
            {
                var enchantmentTemplateIdentifier = MathUtil.ChooseRandomElement<string>(itemTemplate.PossibleEnchantments);
                item.Enchantment = EnchantmentFactory.Build(enchantmentTemplateIdentifier);
                Assert.IsNotNull(item.Enchantment, "Could not resolve enchantment: " + enchantmentTemplateIdentifier);
            }
            return item;
        }
    }
}
