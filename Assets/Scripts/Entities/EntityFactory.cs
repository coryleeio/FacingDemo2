
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class EntityFactory
    {

        public static List<Entity> BuildAll(List<string> identifiers)
        {
            var agg = new List<Entity>();
            foreach(var identifier in identifiers)
            {
                agg.Add(Build(identifier));
            }
            return agg;
        }

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
                entity.ViewTemplateIdentifier = "VIEW_HUMAN_WHITE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.PLAYER;
                entity.OriginalTeam = Team.PLAYER;
                entity.AI = AIType.None;
                InventoryUtil.EquipItem(entity, ItemFactory.Build("ITEM_ROBE_OF_WONDERS"));
                InventoryUtil.EquipItem(entity, ItemFactory.Build("ITEM_SANDALS"));
                InventoryUtil.EquipItem(entity, ItemFactory.Build("ITEM_SHORTBOW"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_WAND_OF_LIGHTNING"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_STAFF_OF_FIREBALLS"));

                var HumanoidWeapons = Context.ResourceManager.Load<ProbabilityTable>("LOOT_TABLE_HUMANOID_WEAPONS");
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));

                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_LUCKY_COIN"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_ARROW"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_PURPLE_POTION"));
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
                entity.ViewTemplateIdentifier = "VIEW_MARKER_BLUE";
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
                entity.ViewTemplateIdentifier = "VIEW_BEE";
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
                entity.ViewTemplateIdentifier = "VIEW_SKELETON_WHITE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.Archer;
                var itemIds = new List<string>();

                var HumanoidWeapons = Context.ResourceManager.Load<ProbabilityTable>("LOOT_TABLE_HUMANOID_WEAPONS");
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));

                var HumanoidClothing = Context.ResourceManager.Load<ProbabilityTable>("LOOT_TABLE_HUMANOID_CLOTHING");
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidClothing.Roll()));
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
                entity.ViewTemplateIdentifier = "VIEW_GHOST";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.Archer;
                var itemIds = new List<string>();
                var HumanoidWeapons = Context.ResourceManager.Load<ProbabilityTable>("LOOT_TABLE_HUMANOID_WEAPONS");
                var HumanoidClothing = Context.ResourceManager.Load<ProbabilityTable>("LOOT_TABLE_HUMANOID_CLOTHING");
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidClothing.Roll()));
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
                entity.ViewTemplateIdentifier = "VIEW_GHOST";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.Archer;
                var itemIds = new List<string>();
                var HumanoidWeapons = Context.ResourceManager.Load<ProbabilityTable>("LOOT_TABLE_HUMANOID_WEAPONS");
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(HumanoidWeapons.Roll()));
                foreach (var itemId in itemIds)
                {
                    InventoryUtil.EquipItem(entity, ItemFactory.Build(itemId));
                }
            }

            else if (entity.TemplateIdentifier == "ENTITY_QUEEN_BEE")
            {
                var nameTable = Context.ResourceManager.Load<ProbabilityTable>("NAMETABLE_BEES");
                entity.Name = nameTable.RollAndChooseOne().Localize();
                DefaultBeeBodyAttacks(entity);
                entity.Floating = true;
                entity.Attributes = new Dictionary<Attributes, int>
                {
                    {Attributes.MAX_HEALTH, 15 },
                    {Attributes.VISION_RADIUS, 4 },
                    {Attributes.SHOUT_RADIUS, 4 },
                };
                entity.BlocksPathing = true;
                entity.ViewTemplateIdentifier = "VIEW_LARGE_BEE";
                entity.HasBehaviour = true;
                entity.ActingTeam = Team.Enemy;
                entity.OriginalTeam = Team.Enemy;
                entity.AI = AIType.DumbMelee;
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_LONGSWORD"));
                InventoryUtil.AddItem(entity, ItemFactory.Build("ITEM_PURPLE_POTION"));
            }
            else if (entity.TemplateIdentifier == "ENTITY_STAIRS_UP")
            {
                entity.Name = "entity.stairs.up.name".Localize();
                entity.BlocksPathing = false;
                entity.AlwaysVisible = true;
                entity.ViewTemplateIdentifier = "VIEW_STAIRCASE_UP";
                entity.Trigger = TriggerFactory.Build("TRIGGER_CHANGE_LEVEL_ON_PRESS");
            }
            else if (entity.TemplateIdentifier == "ENTITY_STAIRS_DOWN")
            {
                entity.Name = "entity.stairs.down.name".Localize();
                entity.BlocksPathing = false;
                entity.AlwaysVisible = true;
                entity.ViewTemplateIdentifier = "VIEW_STAIRCASE_DOWN";
                entity.Trigger = TriggerFactory.Build("TRIGGER_CHANGE_LEVEL_ON_PRESS");
            }
            else if (entity.TemplateIdentifier == "ENTITY_GROUND_DROP")
            {
                entity.Name = "";
                entity.BlocksPathing = false;
                entity.ViewTemplateIdentifier = "VIEW_CORPSE";
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
            entity.Position = new Point(0, 0);
            return entity;
        }

        private static void DefaultHumanoidBodyAttacks(Entity ent)
        {
            ent.IsCombatant = true;
            ent.DefaultAttackItem = ItemFactory.Build("ITEM_HUMANOID_FIST");
        }

        private static void DefaultDogBodyAttacks(Entity ent)
        {
            ent.IsCombatant = true;
            ent.DefaultAttackItem = ItemFactory.Build("ITEM_DOG_MAW");
        }

        private static void DefaultBeeBodyAttacks(Entity ent)
        {
            ent.IsCombatant = true;
            ent.DefaultAttackItem = ItemFactory.Build("ITEM_BEE_STINGER");
        }
    }
}
