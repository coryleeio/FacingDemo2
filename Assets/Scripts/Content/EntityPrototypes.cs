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
                    Name = "Poncy",
                    Body = new Body()
                    {
                        CurrentHealth = 10,
                        MaxHealth = 10,
                    },
                    Motor = new Motor(),
                    BlocksPathing = true,
                    ViewComponent = new View()
                    {
                        ViewType = ViewType.StaticSprite,
                        ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                    },
                    TurnComponent = new Behaviour()
                    {
                        BehaviourImplUniqueIdentifier = UniqueIdentifier.BEHAVIOUR_PLAYER,
                    }
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_GIANT_BEE,
                    Name = "Giant Bee",
                    Body = new Body()
                    {
                        CurrentHealth = 1,
                        MaxHealth = 1,
                    },
                    Motor = new Motor(),
                    BlocksPathing = true,
                    ViewComponent = new View()
                    {
                        ViewType = ViewType.StaticSprite,
                        ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                    },
                    TurnComponent = new Behaviour()
                    {
                        BehaviourImplUniqueIdentifier = UniqueIdentifier.BEHAVIOUR_BRUTE,
                    }
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_QUEEN_BEE,
                    Name = "Queen Bee",
                    Body = new Body()
                    {
                        CurrentHealth = 3,
                        MaxHealth = 3,
                    },
                    Motor = new Motor(),
                    BlocksPathing = true,
                    ViewComponent = new View()
                    {
                        ViewType = ViewType.StaticSprite,
                        ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                    },
                    TurnComponent = new Behaviour()
                    {
                        BehaviourImplUniqueIdentifier = UniqueIdentifier.BEHAVIOUR_BRUTE,
                    }
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_UP,
                    Name = "Stairs (Up)",
                    BlocksPathing = false,
                    ViewComponent = new View()
                    {
                        ViewType = ViewType.StaticSprite,
                        ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    },
                    Trigger = new Trigger()
                    {
                        TriggerActionPrototypeUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE,
                    },
                },
                new EntityPrototype()
                {
                    UniqueIdentifier = UniqueIdentifier.TOKEN_STAIRS_DOWN,
                    Name = "Stairs (Down)",
                    BlocksPathing = false,
                    ViewComponent = new View()
                    {
                        ViewType = ViewType.StaticSprite,
                        ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                    },
                    Trigger = new Trigger()
                    {
                        TriggerActionPrototypeUniqueIdentifier = UniqueIdentifier.TRIGGER_TRAVERSE_STAIRCASE,
                    },
                },
            };
        }
    }
}
