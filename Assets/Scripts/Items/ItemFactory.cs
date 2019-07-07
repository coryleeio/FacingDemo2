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
            };

            if (itemTemplate.PossibleEnchantments != null)
            {
                var enchantmentTemplateIdentifiers = itemTemplate.PossibleEnchantments.Roll();
                if(enchantmentTemplateIdentifiers.Count > 1)
                {
                    throw new NotImplementedException("Items can only have one enchantment");
                }
                else if(enchantmentTemplateIdentifiers.Count == 1)
                {
                    var enchantmentTemplateIdentifier = enchantmentTemplateIdentifiers[0];
                    if(enchantmentTemplateIdentifier != null && enchantmentTemplateIdentifier != "")
                    {
                        item.Enchantment = EnchantmentFactory.Build(enchantmentTemplateIdentifier);
                        Assert.IsNotNull(item.Enchantment, "Could not resolve enchantment: " + enchantmentTemplateIdentifier);
                    }
                }
            }
            return item;
        }
    }
}
