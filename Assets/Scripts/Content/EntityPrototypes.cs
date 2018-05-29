using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public static class EntityPrototypes 
    {
        public static List<EntityPrototype> LoadAll()
        {
            return new List<EntityPrototype>()
            {
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_PONCY,
                    CombatantComponent = new CombatantComponent()
                    {
                        CurrentHealth = 10,
                        MaxHealth = 10,
                    },
                    MovementComponent = new MovementComponent(),
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                    Traits = new List<Traits>(){ Traits.Combatant },
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_GIANT_BEE,
                    CombatantComponent = new CombatantComponent()
                    {
                        CurrentHealth = 1,
                        MaxHealth = 1,
                    },
                    MovementComponent = new MovementComponent(),
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                    Traits = new List<Traits>(){ Traits.Combatant },
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_QUEEN_BEE,
                    CombatantComponent = new CombatantComponent()
                    {
                        CurrentHealth = 3,
                        MaxHealth = 3,
                    },
                    MovementComponent = new MovementComponent(),
                    BlocksPathing = true,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                    Traits = new List<Traits>(){ Traits.Combatant },
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_UP,
                    CombatantComponent = new CombatantComponent()
                    {
                        CurrentHealth = 1,
                        MaxHealth = 1,
                    },
                    BlocksPathing = false,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    TriggerComponent = new TriggerComponent()
                    {
                        TriggerActionPrototypeUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE,
                    },
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_DOWN,
                    CombatantComponent = new CombatantComponent()
                    {
                        CurrentHealth = 1,
                        MaxHealth = 1,
                    },
                    BlocksPathing = false,
                    ViewType = ViewType.StaticSprite,
                    ViewUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    TriggerComponent = new TriggerComponent()
                    {
                        TriggerActionPrototypeUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE,
                    },
                },
            };
        }
    }
}
