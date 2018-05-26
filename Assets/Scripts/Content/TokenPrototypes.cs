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
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_GIANT_BEE,
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_QUEEN_BEE,
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_UP,
                    BlocksPathing = false,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    TriggerUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_DOWN,
                    BlocksPathing = false,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    TriggerUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE,
                },
            };
        }
    }
}
