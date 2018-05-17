using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public static class TokenPrototypes 
    {
        public static List<TokenPrototype> LoadAll()
        {
            return new List<TokenPrototype>()
            {
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_PONCY,
                    BlocksPathing = true,
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_GIANT_BEE,
                    BlocksPathing = true,
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_QUEEN_BEE,
                    BlocksPathing = true,
                },
            };
        }
    }
}
