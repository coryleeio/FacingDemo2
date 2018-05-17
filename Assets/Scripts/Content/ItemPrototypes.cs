using System.Collections.Generic;

namespace Gamepackage
{
    public class ItemPrototypes
    {
        public static List<ItemPrototype> LoadAll()
        {
            return new List<ItemPrototype>()
            {
                new ItemPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.ITEM_LONGSWORD
                }
            };
        }
    }
}
