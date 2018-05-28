using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class MovementSystem
    {
        public ApplicationContext Context { get; set; }
        private float TimeBetweenTiles = 0.001f;

        public void Process()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            var entities = level.Entitys;

            foreach (var entity in entities)
            {
                if(!entity.IsMoving && entity.CurrentPath.Count > 0)
                {
                    var nextPos = entity.CurrentPath.Peek();
                    MoveTo(entity, nextPos);
                }
                if (entity.IsMoving)
                {
                    entity.ElapsedMovementTime += Time.deltaTime;
                    if (entity.ElapsedMovementTime > TimeBetweenTiles)
                    {
                        entity.ElapsedMovementTime = TimeBetweenTiles;
                    }
                    var lerpPercentarge = entity.ElapsedMovementTime / TimeBetweenTiles;
                    var targetVectorPos = entity.LerpTargetPosition;
                    var _lerpPos = Vector2.Lerp(entity.LerpCurrentPosition.ToVector2(), targetVectorPos.ToVector2(), lerpPercentarge);

                    if (entity.View != null)
                    {
                        entity.View.transform.position = _lerpPos;
                    }
                    if (Vector2.Distance(_lerpPos, targetVectorPos.ToVector2()) < 0.005f)
                    {
                        EntityArriveAtPosition(level, entity);

                        if (entity.CurrentPath.Count > 0)
                        {
                            var oldPos = entity.CurrentPath.Dequeue();
                            var nextPos = entity.CurrentPath.Peek();
                            MoveTo(entity, nextPos);
                        }
                        else
                        {
                            if (entity.View != null)
                            {
                                entity.View.transform.position = MathUtil.MapToWorld(entity.TargetPosition);
                            }
                            entity.IsMoving = false;
                        }
                    }
                }
            }
        }

        private static void EntityArriveAtPosition(Level level, Entity entity)
        {
            // when we arrive give up the lock on our current position
            if (entity.EntityPrototype.BlocksPathing)
            {
                level.TilesetGrid[entity.Position].Walkable = true;
            }
            level.UnindexEntity(entity, entity.Position);
            entity.Position = entity.TargetPosition;
            level.IndexEntity(entity, entity.Position);
        }

        public void MoveTo(Entity entity, Point newPosition)
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            // Reserve the new position as soon as I start walking so nobody else uses it
            // Later when we arrive we will release the lock on our OLD position.
            if (entity.EntityPrototype.BlocksPathing)
            {
                level.TilesetGrid[newPosition].Walkable = false;
            }
            entity.TargetPosition = newPosition;
            entity.LerpCurrentPosition = MathUtil.MapToWorld(entity.Position).ToPointf();
            entity.LerpTargetPosition = MathUtil.MapToWorld(newPosition).ToPointf();
            entity.ElapsedMovementTime = 0.0f;
            entity.IsMoving = true;
        }

        public void FollowPath(Entity entity, List<Point> path)
        {
            entity.CurrentPath.Clear();
            foreach(var point in path)
            {
                entity.CurrentPath.Enqueue(point);
            }
        }
    }
}
