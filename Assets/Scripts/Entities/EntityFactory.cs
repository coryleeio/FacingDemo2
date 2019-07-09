
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


            Assert.IsTrue(entity.Template != null, "Could not find template: " + entity.TemplateIdentifier);

            entity.Name = entity.Template.NameList.RollAndChooseOne().Localize();
            entity.IsCombatant = entity.Template.IsCombatant;
            entity.DefaultAttackItem = ItemFactory.Build(entity.Template.DefaultWeaponIdentifier);
            entity.Attributes = new Dictionary<Attributes, int>();

            foreach(var pair in entity.Template.TemplateAttributes)
            {
                entity.Attributes.Add(pair.Key, pair.Value);
            }
            entity.BlocksPathing = entity.Template.BlocksPathing;

            var viewIdentifier = entity.Template.ViewTemplateIdentifier;

            if(Context.ResourceManager.Contains<ProbabilityTable>(viewIdentifier))
            {
                var table = Context.ResourceManager.Load<ProbabilityTable>(viewIdentifier);
                entity.ViewTemplateIdentifier = table.RollAndChooseOne();
            }
            else
            {
                entity.ViewTemplateIdentifier = viewIdentifier;
            }

            entity.AIClassName = entity.Template.AIClassName;
            entity.Floating = entity.Template.isFloating == FloatingState.IsFloating;
            entity.CastsShadow = entity.Template.CastsShadow == ShadowCastState.CastsShadow;
            entity.AlwaysVisible = entity.Template.IsAlwaysVisible;

            if(entity.AlwaysVisible)
            {
                Debug.Log("Hit");
            }

            if(entity.Template.Trigger != null && entity.Template.Trigger != "")
            {
                entity.Trigger = TriggerFactory.Build(entity.Template.Trigger);
            }
            foreach (var table in entity.Template.EquipmentTables)
            {
                InventoryUtil.TryEquipItems(entity, ItemFactory.BuildAll(Context.ResourceManager.Load<ProbabilityTable>(table).Roll()));
            }

            foreach (var table in entity.Template.InventoryTables)
            {
                InventoryUtil.AddItems(entity, ItemFactory.BuildAll(Context.ResourceManager.Load<ProbabilityTable>(table).Roll()));
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
            entity.ActingTeam = team;
            entity.OriginalTeam = team;
            return entity;
        }
    }
}
