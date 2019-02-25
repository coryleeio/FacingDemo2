using System;

namespace Gamepackage
{
    public static class Filters
    {
        public static Predicate<Effect> AppliedEffects = (effectInQuestion) => { return effectInQuestion is AppliedEffect; };
        public static Predicate<Entity> HittableEntities = (t) => t.IsCombatant && t.Body != null && !t.Body.IsDead;
        public static Predicate<Point> NonoccupiedTiles = (t) => { return Context.Game.CurrentLevel.Grid[t].Walkable && Context.Game.CurrentLevel.Grid[t].EntitiesInPosition.FindAll(HittableEntities).Count == 0; };
        public static Predicate<Entity> LootableEntities = (ent) => { return ent.PrototypeIdentifier == UniqueIdentifier.ENTITY_GROUND_DROP || (ent.Body != null && ent.Body.IsDead && InventoryUtil.HasAnyItems(ent)); };
        public static Predicate<Point> FloorTiles = (pointOnLevel) => { return Context.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType == TileType.Floor; };
        public static Predicate<Point> VisibleTiles = (pointOnLevel) => { return Context.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType != TileType.Empty; };
    }
}
