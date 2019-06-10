
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class EntityFactory
    {
        public static Entity Build(string uniqueIdentifier)
        {
            var entity = new Entity();
            entity.TemplateIdentifier = uniqueIdentifier;
            entity.Inventory = new Inventory();

            // Ensure correct default size - doing this in Inventory causes
            // serialization issues
            for (var i = 0; i < 68; i++)
            {
                entity.Inventory.Items.Add(null);
            }

            entity.EntityAcquiredTags = new List<string>();
            entity.EntityInnateTags = new List<string>();

            if (entity.TemplateIdentifier == "ENTITY_PONCY")
            {
                entity.Name = "entity.player.name.default".Localize();
                DefaultHumanoidBodyAttacks(entity);
                entity.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 10 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_HUMAN_WHITE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.PLAYER;
                entity.OriginalTeam = Team.PLAYER;
                entity.AI = AIType.None;
                InventoryUtil.EquipItem(entity, ItemFactory.Build("ITEM_ROBE_OF_WONDERS"));
                InventoryUtil.EquipItem(entity, ItemFactory.Build("ITEM_SANDALS"));
                InventoryUtil.EquipItem(entity, ItemFactory.Build("ITEM_SHORTBOW"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_WAND_OF_LIGHTNING"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_STAFF_OF_FIREBALLS"));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons)));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_LUCKY_COIN"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_ARROW"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_ANTIDOTE"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_MIND_WAND"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_MIND_WAND"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_MIND_WAND"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_SHIELD_OF_AMALURE"));
            }
            else if (entity.TemplateIdentifier == "ENTITY_MASLOW")
            {
                entity.Name = "entity.dog.name.default".Localize();
                DefaultDogBodyAttacks(entity);
                entity.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 45 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_MARKER_BLUE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.PLAYER;
                entity.OriginalTeam = Team.PLAYER;
                entity.AI = AIType.DumbMelee;
            }
            else if (entity.TemplateIdentifier == "ENTITY_GIANT_BEE")
            {
                entity.Name = "entity.bee.name".Localize();
                DefaultBeeBodyAttacks(entity);
                entity.Floating = true;
                entity.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 10 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_BEE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.DumbMelee;
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_ARROW"));
            }
            else if (entity.TemplateIdentifier == "ENTITY_SKELETON")
            {
                entity.Name = "entity.skeleton.name".Localize();
                DefaultHumanoidBodyAttacks(entity);
                entity.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 10 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_SKELETON_WHITE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.Archer;
                var itemIds = new List<string>();
                itemIds.AddRange(Tables.HumanoidClothing.Next());
                itemIds.Add(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
                }
            }
            else if (entity.TemplateIdentifier == "ENTITY_GHOST")
            {
                entity.Name = "entity.ghost.name".Localize();
                DefaultHumanoidBodyAttacks(entity);
                entity.Floating = true;
                entity.Attributes = new Dictionary<Attributes, int>
                {
                   {Attributes.MAX_HEALTH, 10 },
                   {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_GHOST";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.Archer;
                var itemIds = new List<string>();
                itemIds.AddRange(Tables.HumanoidClothing.Next());
                itemIds.Add(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
                }
            }

            else if (entity.TemplateIdentifier == "ENTITY_ANIMATED_WEAPON")
            {
                entity.Name = "entity.animated.weapon.name".Localize();
                DefaultHumanoidBodyAttacks(entity);
                entity.Floating = true;
                entity.Attributes = new Dictionary<Attributes, int>
                {
                   {Attributes.MAX_HEALTH, 10 },
                   {Attributes.VISION_RADIUS, 4 },
                   {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_GHOST";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.Archer;
                var itemIds = new List<string>();
                itemIds.Add(MathUtil.ChooseRandomElement<string>(Tables.HumanoidWeapons));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
                }
            }

            else if (entity.TemplateIdentifier == "ENTITY_QUEEN_BEE")
            {
                entity.Name = (MathUtil.PercentageChanceEventOccurs(10) ? "entity.queen.bee.special.name" : "entity.queen.bee.name").Localize();
                DefaultBeeBodyAttacks(entity);
                entity.Floating = true;
                entity.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 15 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_LARGE_BEE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.DumbMelee;
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_LONGSWORD"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_ANTIDOTE"));
            }
            else if (entity.TemplateIdentifier == "ENTITY_STAIRS_UP")
            {
                entity.Name = "entity.stairs.up.name".Localize();
                entity.BlocksPathing = false;
                entity.AlwaysVisible = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_STAIRCASE_UP";
                entity.Trigger = TriggerFactory.Build("TRIGGER_CHANGE_LEVEL_ON_PRESS");
            }
            else if (entity.TemplateIdentifier == "ENTITY_STAIRS_DOWN")
            {
                entity.Name = "entity.stairs.down.name".Localize();
                entity.BlocksPathing = false;
                entity.AlwaysVisible = true;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_STAIRCASE_DOWN";
                entity.Trigger = TriggerFactory.Build("TRIGGER_CHANGE_LEVEL_ON_PRESS");
            }
            else if (entity.TemplateIdentifier == "ENTITY_GROUND_DROP")
            {
                entity.Name = "";
                entity.BlocksPathing = false;
                entity.ViewPrototypeUniqueIdentifier = "VIEW_CORPSE";
                entity.Trigger = TriggerFactory.Build("TRIGGER_LOOTABLE");
            }

            else
            {
                throw new NotImplementedException();
            }

            if (entity.IsCombatant)
            {
                Assert.IsTrue(entity.Attributes.ContainsKey(Attributes.SHOUT_RADIUS), "Entities must have a value for SHOUT_RADIUS.");
                Assert.IsTrue(entity.Attributes.ContainsKey(Attributes.SHOUT_RADIUS), "Entities must have a value for SHOUT_RADIUS.");
                Assert.IsTrue(entity.Attributes.ContainsKey(Attributes.VISION_RADIUS), "Entities must have a value for VISION_RADIUS.");
                entity.CurrentHealth = entity.CalculateValueOfAttribute(Attributes.MAX_HEALTH);
            }

            entity.Direction = MathUtil.ChooseRandomElement<Direction>(new List<Direction>() { Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest });

            if (entity.Inventory != null)
            {
                var mainHand = InventoryUtil.GetItemBySlot(entity, ItemSlot.MainHand);
                var ammo = InventoryUtil.GetItemBySlot(entity, ItemSlot.Ammo);
                if (mainHand != null)
                {
                    if (mainHand.CanBeUsedInInteractionType(CombatActionType.Ranged) && mainHand.Template.AmmoType == AmmoType.Arrow && ammo == null)
                    {
                        // If we've got a ranged weapon equipped that is a bow, 
                        // but no arrows equipped, go ahead and spawn some default arrows
                        // and equip them
                        InventoryUtil.EquipItem(entity, ItemFactory.Build(MathUtil.ChooseRandomElement<string>(Tables.RandomArrows)));
                    }
                }
            }
            entity.Position = new Point(0, 0);
            return entity;
        }

        private static void DefaultHumanoidBodyAttacks(Entity ent)
        {
            ent.IsCombatant = true;
            ent.CombatActionDescriptors = new Dictionary<CombatActionType, CombatActionDescriptor>()
            {
                { CombatActionType.Melee,  new CombatActionDescriptor()
                {
                    CombatActionParameters = new CombatActionParameters()
                {
                AttackMessagePrefix = "attacks.humanoid.1",
                DyeNumber = 1,
                DyeSize = 1,
                DamageType = DamageTypes.BLUDGEONING,
                Range = 1,
                NumberOfTargetsToPierce = 1,
                TargetingType = CombatActionTargetingType.Line,
                AppliedEffectTemplate = null,
                InteractionProperties = new List<InteractionProperties>(),
                }
                }
            }
            };
        }

        private static void DefaultDogBodyAttacks(Entity ent)
        {
            ent.IsCombatant = true;
            ent.CombatActionDescriptors = new Dictionary<CombatActionType, CombatActionDescriptor>()
            {
                { CombatActionType.Melee,  new CombatActionDescriptor()
                {
                    CombatActionParameters = new CombatActionParameters()
                {
                AttackMessagePrefix = "attacks.dog.1",
                DyeNumber = 2,
                DyeSize = 4,
                DamageType = DamageTypes.PIERCING,
                Range = 1,
                NumberOfTargetsToPierce = 1,
                TargetingType = CombatActionTargetingType.Line,
                AppliedEffectTemplate = null,
                InteractionProperties = new List<InteractionProperties>(),
                }
                }
            }
            };
        }

        private static void DefaultBeeBodyAttacks(Entity ent)
        {
            ent.IsCombatant = true;
            ent.CombatActionDescriptors = new Dictionary<CombatActionType, CombatActionDescriptor>()
            {
                { CombatActionType.Melee,  new CombatActionDescriptor()
                {
                    CombatActionParameters = new CombatActionParameters()
                {
                AttackMessagePrefix = "attacks.stinger.1",
                DyeNumber = 1,
                DyeSize = 1,
                DamageType = DamageTypes.PIERCING,
                AppliedEffectTemplate = "EFFECT_WEAK_POISON",
                Range = 1,
                NumberOfTargetsToPierce = 1,
                TargetingType = CombatActionTargetingType.Line,
                InteractionProperties = new List<InteractionProperties>(),
                }
                }
            }
            };
        }
    }
}
