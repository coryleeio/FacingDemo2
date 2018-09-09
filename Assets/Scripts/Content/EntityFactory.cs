using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class EntityFactory
    {
        public static Entity Build(UniqueIdentifier uniqueIdentifier)
        {
            var entity = new Entity();
            entity.PrototypeIdentifier = uniqueIdentifier;
            entity.Inventory = new Inventory();
            
            // Ensure correct default size - doing this in Inventory causes
            // serialization issues
            for(var i =0; i < 68; i++)
            {
                entity.Inventory.Items.Add(null);
            }

            if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_PONCY)
            {
                entity.Name = "Poncy";
                entity.Body = new Body()
                {
                    Attributes = new Dictionary<Attributes, int>
                    {
                        {Attributes.MAX_HEALTH, 10 },
                    },
                    Attacks = DefaultHumanoidBodyAttacks()
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                };
                entity.Behaviour = new PlayerBehaviour()
                {
                    Team = Team.PLAYER,
                };
                entity.Inventory.EquipItem(ItemFactory.Build(Tables.BanditWeapons.NextSingleItem()));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_LUCKY_COIN));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ARROW));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ANTIDOTE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_MASLOW)
            {
                entity.Name = "Maslow";
                entity.Body = new Body()
                {
                    Attributes = new Dictionary<Attributes, int>
                    {
                        {Attributes.MAX_HEALTH, 45 },
                    },
                    Attacks = DefaultDogBodyAttacks(),
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.PLAYER,
                };
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_GIANT_BEE)
            {
                entity.Name = "Giant Bee";
                entity.Body = new Body()
                {
                    Attributes = new Dictionary<Attributes, int>
                    {
                        {Attributes.MAX_HEALTH, 10 },
                    },
                    Attacks = DefaultBeeBodyAttacks(),
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.ENEMY,
                };
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ARROW));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_QUEEN_BEE)
            {
                entity.Name = "Queen Bee";
                entity.Body = new Body()
                {
                    Attributes = new Dictionary<Attributes, int>
                    {
                        {Attributes.MAX_HEALTH, 15 },
                    },
                    Attacks = DefaultBeeBodyAttacks(),
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.ENEMY,
                };
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_LONGSWORD));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ANTIDOTE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_STAIRS_UP)
            {
                entity.Name = "Stairs (Up)";
                entity.BlocksPathing = false;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_STAIRCASE_UP,
                };
                entity.Trigger = new Trigger()
                {
                    Offsets = new List<Point>() { new Point(0,0) }
                };
                entity.Trigger.Effects.Add(entity, new TraverseStaircase());
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_STAIRS_DOWN)
            {
                entity.Name = "Stairs (Down)";
                entity.BlocksPathing = false;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_STAIRCASE_DOWN,
                };
                entity.Trigger = new Trigger()
                {
                    // params filled out by dungeon generator
                    Offsets = new List<Point>() { new Point(0, 0) }
                };
                entity.Trigger.Effects.Add(entity, new TraverseStaircase());
            }
            else
            {
                throw new NotImplementedException();
            }

            if(entity.Body != null)
            {
                Assert.IsTrue(entity.Body.Attributes.ContainsKey(Attributes.MAX_HEALTH), "Entities must have a value for maximum health if they have a body.");
                entity.Body.Entity = entity; // Needed for the recursive calculation.
                entity.Body.CurrentHealth = entity.Body.CalculateValueOfAttribute(Attributes.MAX_HEALTH);
            }
            return entity;
        }

        private static List<AttackParameters> DefaultHumanoidBodyAttacks()
        {
            return new List<AttackParameters>()
                    {
                        new AttackParameters()
                        {
                            AttackMessage = "{0} punches {1} for {2} points of {3} damage!",
                            Bonus = 0,
                            DyeNumber = 1,
                            DyeSize = 1,
                            DamageType = DamageTypes.BLUDGEONING,
                        },
                    };
        }

        private static List<AttackParameters> DefaultDogBodyAttacks()
        {
            return new List<AttackParameters>()
                    {
                        new AttackParameters()
                        {
                            AttackMessage = "{0} bites {1} for {2} points of {3} damage!",
                            Bonus = 1,
                            DyeNumber = 2,
                            DyeSize = 4,
                            DamageType = DamageTypes.PIERCING,
                        },
                    };
        }

        private static List<AttackParameters> DefaultBeeBodyAttacks()
        {
            return new List<AttackParameters>()
                    {
                        new AttackParameters()
                        {
                            AttackMessage = "{0} strings {1} for {2} points of {3} damage!",
                            Bonus = 0,
                            DyeNumber = 1,
                            DyeSize = 1,
                            DamageType = DamageTypes.PIERCING,
                        },
                    };
        }
    }
}
