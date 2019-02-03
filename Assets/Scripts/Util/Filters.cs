using System;

namespace Gamepackage
{
    public static class Filters
    {
        public static Predicate<Effect> AppliedEffects = (effectInQuestion) => { return effectInQuestion is AppliedEffect; };
        public static Predicate<Entity> HittableEntities = (t) => t.IsCombatant && t.Body != null && !t.Body.IsDead;
        public static Predicate<Point> NonoccupiedTiles = (t) => { return Context.GameStateManager.Game.CurrentLevel.Grid[t].Walkable && Context.GameStateManager.Game.CurrentLevel.Grid[t].EntitiesInPosition.FindAll(HittableEntities).Count == 0; };
        public static Predicate<Entity> LootableEntities = (ent) => { return ent.PrototypeIdentifier == UniqueIdentifier.ENTITY_GROUND_DROP || (ent.Body != null && ent.Body.IsDead && ent.Inventory.HasAnyItems); };
        public static Predicate<Point> FloorTiles = (pointOnLevel) => { return Context.GameStateManager.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.GameStateManager.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType == TileType.Floor; };
        public static Predicate<Point> VisibleTiles = (pointOnLevel) => { return Context.GameStateManager.Game.CurrentLevel.BoundingBox.Contains(pointOnLevel) && Context.GameStateManager.Game.CurrentLevel.Grid[pointOnLevel.X, pointOnLevel.Y].TileType != TileType.Empty; };
    }
}
