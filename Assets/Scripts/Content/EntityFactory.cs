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
                MeleeAttackTypeParameters = new AttackTypeParameters()
                {
                    Range = 1,
                    NumberOfTargetsToPierce = 1,
                    AttackParameters = attackParameters,
                },
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

            entity.EntityAcquiredTags = new List<Tags>();
            entity.EntityInnateTags = new List<Tags>();

            if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_PONCY)
            {
                entity.Name = "entity.player.name.default".Localize();
                entity.Body = BuildBody(DefaultHumanoidBodyAttacks());
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 10 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_HUMAN_WHITE,
                };
                entity.Behaviour = new Behaviour()
                {
                    AI = AIType.None,
                    ActingTeam = Team.PLAYER,
                    OriginalTeam = Team.PLAYER,
                };
                InventoryUtil.EquipItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_ROBE_OF_WONDERS));
                InventoryUtil.EquipItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_SANDALS));
                InventoryUtil.EquipItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_SHORTBOW));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_WAND_OF_LIGHTNING));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_STAFF_OF_FIREBALLS));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_LUCKY_COIN));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_ARROW));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_ANTIDOTE));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_WAND_OF_MADNESS));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_WAND_OF_CHARM));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_WAND_OF_DOMINATION));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_SHIELD_OF_AMALURE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_MASLOW)
            {
                entity.Name = "entity.dog.name.default".Localize();
                entity.Body = BuildBody(DefaultDogBodyAttacks());
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 45 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                };
                entity.Behaviour = new Behaviour()
                {
                    AI = AIType.DumbMelee,
                    ActingTeam = Team.PLAYER,
                    OriginalTeam = Team.PLAYER,
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
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_BEE,
                };
                entity.Behaviour = new Behaviour()
                {
                    AI = AIType.DumbMelee,
                    ActingTeam = Team.Enemy,
                    OriginalTeam = Team.Enemy,
                };
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_ARROW));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_SKELETON)
            {
                entity.Name = "entity.skeleton.name".Localize();
                entity.Body = BuildBody(DefaultHumanoidBodyAttacks());
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 10 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_SKELETON_WHITE,
                };
                entity.Behaviour = new Behaviour()
                {
                    ActingTeam = Team.Enemy,
                    OriginalTeam = Team.Enemy,
                    AI = AIType.Archer,
                };
                var itemIds = new List<UniqueIdentifier>();
                itemIds.AddRange(Tables.HumanoidClothing.Next());
                itemIds.Add(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
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
                   {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_GHOST,
                };
                entity.Behaviour = new Behaviour()
                {
                    ActingTeam = Team.Enemy,
                    OriginalTeam = Team.Enemy,
                    AI = AIType.Archer,
                };
                var itemIds = new List<UniqueIdentifier>();
                itemIds.AddRange(Tables.HumanoidClothing.Next());
                itemIds.Add(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
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
                   {Attributes.VISION_RADIUS, 4 },
                   {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_GHOST,
                };
                entity.Behaviour = new Behaviour()
                {
                    ActingTeam = Team.Enemy,
                    OriginalTeam = Team.Enemy,
                    AI = AIType.Archer,
                };
                var itemIds = new List<UniqueIdentifier>();
                itemIds.Add(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
                }
            }

            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_QUEEN_BEE)
            {
                entity.Name = (MathUtil.PercentageChanceEventOccurs(10) ? "entity.queen.bee.special.name" : "entity.queen.bee.name").Localize();
                entity.Body = BuildBody(DefaultBeeBodyAttacks());
                entity.Body.Floating = true;
                entity.Body.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 15 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.Spine,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_LARGE_BEE,
                };
                entity.Behaviour = new Behaviour()
                {
                    ActingTeam = Team.Enemy,
                    OriginalTeam = Team.Enemy,
                    AI = AIType.DumbMelee,
                };
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_LONGSWORD));
                InventoryUtil.AddItem(entity, ItemFactory.Build(UniqueIdentifier.ITEM_ANTIDOTE));
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_STAIRS_UP)
            {
                entity.Name = "entity.stairs.up.name".Localize();
                entity.BlocksPathing = false;
                entity.AlwaysVisible = true;
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
                entity.AlwaysVisible = true;
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
                Assert.IsTrue(entity.Body.Attributes.ContainsKey(Attributes.MAX_HEALTH), "Entities must have a value for MAX_HEALTH.");
                Assert.IsTrue(entity.Body.Attributes.ContainsKey(Attributes.SHOUT_RADIUS), "Entities must have a value for SHOUT_RADIUS.");
                Assert.IsTrue(entity.Body.Attributes.ContainsKey(Attributes.VISION_RADIUS), "Entities must have a value for VISION_RADIUS.");
                entity.Body.CurrentHealth = entity.CalculateValueOfAttribute(Attributes.MAX_HEALTH);
            }

            entity.Direction = MathUtil.ChooseRandomElement<Direction>(new List<Direction>() { Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest });

            if (entity.Inventory != null)
            {
                var mainHand = InventoryUtil.GetItemBySlot(entity, ItemSlot.MainHand);
                var ammo = InventoryUtil.GetItemBySlot(entity, ItemSlot.Ammo);
                if (mainHand != null)
                {
                    if (mainHand.CanBeUsedInAttackType(AttackType.Ranged) && mainHand.AmmoType == AmmoType.Arrow && ammo == null)
                    {
                        // If we've got a ranged weapon equipped that is a bow, 
                        // but no arrows equipped, go ahead and spawn some default arrows
                        // and equip them
                        InventoryUtil.EquipItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<UniqueIdentifier>(Tables.RandomArrows)));
                    }
                }
            }
            entity.Position = new Point(0, 0);
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
            effectList.Add(EffectFactory.Build(UniqueIdentifier.EFFECT_APPLIED_WEAK_POISON, new List<AttackType>() { AttackType.Melee }));
            var attackParameters = new AttackParameters()
            {
                AttackMessage = "attacks.stinger.1".Localize(),
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
