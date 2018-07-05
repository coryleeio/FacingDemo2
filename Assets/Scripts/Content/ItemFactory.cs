using System;

namespace Gamepackage
{
    public class ItemFactory
    {
        public static Item Build(UniqueIdentifier uniqueIdentifier)
        {
            if(uniqueIdentifier == UniqueIdentifier.ITEM_LONGSWORD)
            {
                return new Longsword();
            }
            else if (uniqueIdentifier == UniqueIdentifier.ITEM_LUCKY_COIN)
            {
                return new LuckyCoin();
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
