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
                    StartingMaxHealth = 10,
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                    Traits = new List<Traits>(){ Traits.Combatant },
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_GIANT_BEE,
                    StartingMaxHealth = 1,
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                    Traits = new List<Traits>(){ Traits.Combatant },
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_QUEEN_BEE,
                    StartingMaxHealth = 3,
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                    Traits = new List<Traits>(){ Traits.Combatant },
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_UP,
                    StartingMaxHealth = 1,
                    BlocksPathing = false,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    TriggerUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE
                },
                new TokenPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_DOWN,
                    StartingMaxHealth = 1,
                    BlocksPathing = false,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    TriggerUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE,
                },
            };
        }
    }
}
