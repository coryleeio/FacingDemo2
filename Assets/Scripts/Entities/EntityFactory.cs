
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class EntityFactory
    {
        public static List<Entity> BuildAll(List<string> identifiers, Team team)
        {
            var agg = new List<Entity>();
            foreach (var identifier in identifiers)
            {
                agg.Add(Build(identifier, team));
            }
            return agg;
        }

        public static Entity Build(string uniqueIdentifier, Team team)
        {
            var entity = new Entity();
            var template = Context.ResourceManager.Load<EntityTemplate>(uniqueIdentifier);
            Assert.IsTrue(template != null, "Could not find template: " + uniqueIdentifier);
            return Build(template, team);
        }

        public static Entity Build(EntityTemplate template, Team team)
        {
            var entity = new Entity();
            Assert.IsTrue(template != null, "Could not find template: " + template);
            entity.SpeciesTemplateIdentifier = template.EntityTypeIdentifier;
            entity.Inventory = new Inventory();
            // Ensure correct default size - doing this in Inventory causes
            // serialization issues
            for (var i = 0; i < 68; i++)
            {
                entity.Inventory.Items.Add(null);
            }

            entity.Level = template.Level;
            entity.EntityAcquiredTags = new List<string>();
            entity.EntityInnateTags = new List<string>();
            entity.Name = template.NameList.RollAndChooseOne().Localize();
            entity.Attributes = new Dictionary<Attributes, int>();

            if (entity.Species != null)
            {
                entity.DefaultAttackItem = ItemFactory.Build(entity.Species.DefaultWeaponIdentifier);
                foreach (var pair in entity.Species.TemplateAttributes)
                {
                    entity.Attributes.Add(pair.Key, pair.Value);
                }
                entity.AIClassName = entity.Species.DefaultAIClassName;
            }

            var viewIdentifier = template.ViewTemplateIdentifier;
            entity.BlocksPathing = template.BlocksPathing;


            if (Context.ResourceManager.Contains<ProbabilityTable>(viewIdentifier))
            {
                var table = Context.ResourceManager.Load<ProbabilityTable>(viewIdentifier);
                entity.ViewTemplateIdentifier = table.RollAndChooseOne();
            }
            else
            {
                entity.ViewTemplateIdentifier = viewIdentifier;
            }

            if (template.TriggerTemplateIdentifier != null && template.TriggerTemplateIdentifier != "")
            {
                entity.Trigger = TriggerFactory.Build(template.TriggerTemplateIdentifier);
            }
            foreach (var table in template.EquipmentTables)
            {
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(Context.ResourceManager.Load<ProbabilityTable>(table).Roll()));
            }

            foreach (var table in template.InventoryTables)
            {
                InventoryUtil.AddItems(entity, ItemFactory.BuildAll(Context.ResourceManager.Load<ProbabilityTable>(table).Roll()));
            }

            if (entity.IsCombatant)
            {
                Assert.IsTrue(entity.Attributes.ContainsKey(Attributes.ShoutRadius), "Entities must have a value for SHOUT_RADIUS.");
                Assert.IsTrue(entity.Attributes.ContainsKey(Attributes.ShoutRadius), "Entities must have a value for SHOUT_RADIUS.");
                Assert.IsTrue(entity.Attributes.ContainsKey(Attributes.VisionRadius), "Entities must have a value for VISION_RADIUS.");
                entity.CurrentHealth = entity.CalculateValueOfAttribute(Attributes.MaxHealth);
            }

            entity.Direction = MathUtil.ChooseRandomElement<Direction>(new List<Direction>() { Direction.SouthEast, Direction.SouthWest, Direction.NorthEast, Direction.NorthWest });
            entity.Position = new Point(0, 0);
            entity.ActingTeam = team;
            entity.OriginalTeam = team;
            entity.Xp = Context.Game.CampaignTemplate.XpForLevel[entity.Level];
            return entity;
        }
    }
}
