using System;

namespace Gamepackage
{
    public static class Filters
    {
        public static Predicate<Entity> HittableEntities = (t) => t.IsCombatant && t.IsCombatant && !t.IsDead;
        public static Predicate<Point> NonoccupiedTiles = (t) => { return Context.Game.CurrentLevel.Grid[t].Walkable && Context.Game.CurrentLevel.Grid[t].EntitiesInPosition.FindAll(HittableEntities).Count == 0; };
        public static Predicate<Entity> LootableEntities = (ent) => { return ent.TemplateIdentifier == "ENTITY_GROUND_DROP" || (ent.IsCombatant && ent.IsDead && InventoryUtil.HasAnyItems(ent)); };
        public static Predicate<Point> FloorTiles = (pointOnLevel) => { return Context.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType == TileType.Floor; };
        public static Predicate<Point> VisibleTiles = (pointOnLevel) => { return Context.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType != TileType.Empty; };
        public static Predicate<Entity> StepTriggers = (ent) => ent.Trigger != null && ent.Trigger.Template.TriggerMode == TriggerMode.Step;
        public static Predicate<Entity> PressTriggers = (ent) => ent.Trigger != null && ent.Trigger.Template.TriggerMode == TriggerMode.Press;
    }
}
