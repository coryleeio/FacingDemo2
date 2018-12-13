using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class EntityFactory
    {
        private static Body BuildBody(List<AttackParameters> attackParameters)
        {
            Body body = new Body
            {
                MeleeRange = 1,
                MeleeTargetsPierced = 1,
                MeleeParameters = attackParameters,
            };
            return body;
        }

        public static Entity Build(UniqueIdentifier uniqueIdentifier)
        {
            var entity = new Entity();
            entity.PrototypeIdentifier = uniqueIdentifier;
            entity.Inventory = new Inventory();

            // Ensure correct default size - doing this in Inventory causes
            // serialization issues
            for (var i = 0; i < 68; i++)
            {
                entity.Inventory.Items.Add(null);
            }

            if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_PONCY)
            {
                entity.Name = "entity.player.name.default".Localize();
                entity.Body = BuildBody(DefaultHumanoidBodyAttacks());
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 10 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_HUMAN_WHITE,
                };
                entity.Behaviour = new PlayerBehaviour()
                {
                    Team = Team.PLAYER,
                };
                entity.Inventory.EquipItem(ItemFactory.Build(UniqueIdentifier.ITEM_ROBE_OF_WONDERS));
                entity.Inventory.EquipItem(ItemFactory.Build(UniqueIdentifier.ITEM_SANDALS));
                entity.Inventory.EquipItem(ItemFactory.Build(UniqueIdentifier.ITEM_SHORTBOW));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_WAND_OF_LIGHTNING));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_STAFF_OF_FIREBALLS));
                entity.Inventory.AddItem(ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                entity.Inventory.AddItem(ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                entity.Inventory.AddItem(ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                entity.Inventory.AddItem(ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_LUCKY_COIN));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ARROW));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ANTIDOTE));
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_SHIELD_OF_AMALURE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_MASLOW)
            {
                entity.Name = "entity.dog.name.default".Localize();
                entity.Body = BuildBody(DefaultDogBodyAttacks());
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 45 },
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
                entity.Name = "entity.bee.name".Localize();
                entity.Body = BuildBody(DefaultBeeBodyAttacks());
                entity.Body.Floating = true;
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                   {Attributes.MAX_HEALTH, 10 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_BEE,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.ENEMY,
                };
                entity.Inventory.AddItem(ItemFactory.Build(UniqueIdentifier.ITEM_ARROW));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_SKELETON)
            {
                entity.Name = "entity.skeleton.name".Localize();
                entity.Body = BuildBody(DefaultHumanoidBodyAttacks());
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                   {Attributes.MAX_HEALTH, 10 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_SKELETON_WHITE,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.ENEMY,
                };
                var itemIds = new List<UniqueIdentifier>();
                itemIds.AddRange(Tables.HumanoidClothing.Next());
                itemIds.Add(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    entity.Inventory.EquipItem(ItemFactory.Build(itemId));
                }
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_GHOST)
            {
                entity.Name = "entity.ghost.name".Localize();
                entity.Body = BuildBody(DefaultHumanoidBodyAttacks());
                entity.Body.Floating = true;
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                   {Attributes.MAX_HEALTH, 10 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_GHOST,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.ENEMY,
                };
                var itemIds = new List<UniqueIdentifier>();
                itemIds.AddRange(Tables.HumanoidClothing.Next());
                itemIds.Add(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    entity.Inventory.EquipItem(ItemFactory.Build(itemId));
                }
            }

            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_ANIMATED_WEAPON)
            {
                entity.Name = "entity.animated.weapon.name".Localize();
                entity.Body = BuildBody(DefaultHumanoidBodyAttacks());
                entity.Body.Floating = true;
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                   {Attributes.MAX_HEALTH, 10 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_GHOST,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Team = Team.ENEMY,
                };
                var itemIds = new List<UniqueIdentifier>();
                itemIds.Add(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    entity.Inventory.EquipItem(ItemFactory.Build(itemId));
                }
            }

            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_QUEEN_BEE)
            {
                entity.Name = "entity.queen.bee.name".Localize();
                entity.Body = BuildBody(DefaultBeeBodyAttacks());
                entity.Body.Floating = true;
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 15 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_BEE,
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
                entity.Name = "entity.stairs.up.name".Localize();
                entity.BlocksPathing = false;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_STAIRCASE_UP,
                };
                entity.Trigger = new Trigger()
                {
                    Offsets = new List<Point>() { new Point(0, 0) }
                };
                entity.Trigger.Effects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_TRAVERSE_STAIRCASE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_STAIRS_DOWN)
            {
                entity.Name = "entity.stairs.down.name".Localize();
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
                entity.Trigger.Effects.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_TRAVERSE_STAIRCASE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_GROUND_DROP)
            {
                entity.Name = "";
                entity.BlocksPathing = false;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_CORPSE,
                };
            }

            else
            {
                throw new NotImplementedException();
            }

            if (entity.Body != null)
            {
                Assert.IsTrue(entity.Body.Attributes.ContainsKey(Attributes.MAX_HEALTH), "Entities must have a value for maximum health if they have a body.");
                entity.Body.Entity = entity; // Needed for the recursive calculation.
                entity.Body.CurrentHealth = entity.CalculateValueOfAttribute(Attributes.MAX_HEALTH);
            }

            entity.Direction = MathUtil.ChooseRandomElement<Direction>(new List<Direction>() { Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest });
            return entity;
        }

        private static List<AttackParameters> DefaultHumanoidBodyAttacks()
        {
            return new List<AttackParameters>()
                    {
                        new AttackParameters()
                        {
                            AttackMessage = "attacks.humanoid.1".Localize(),
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
                            AttackMessage = "attacks.dog.1".Localize(),
                            Bonus = 1,
                            DyeNumber = 2,
                            DyeSize = 4,
                            DamageType = DamageTypes.PIERCING,
                        },
                    };
        }

        private static List<AttackParameters> DefaultBeeBodyAttacks()
        {
            var effectList = new List<Effect>();
            effectList.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON, new List<CombatContext>() { CombatContext.Melee }));
            var attackParameters = new AttackParameters()
            {
                AttackMessage = "attacks.bee.1".Localize(),
                Bonus = 0,
                DyeNumber = 1,
                DyeSize = 1,
                DamageType = DamageTypes.PIERCING,
                AttackSpecificEffects = effectList,
            };
            return new List<AttackParameters>()
            {
                attackParameters,
            };
        }
    }
}
