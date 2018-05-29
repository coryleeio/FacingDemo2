using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class MovementSystem
    {
        public ApplicationContext Context { get; set; }
        private float TimeBetweenTiles = 0.01f;

        public void Process()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            var entities = level.Entitys;

            foreach (var entity in entities)
            {
                if(entity.MovementComponent == null)
                {
                    continue;
                }

                if(!entity.MovementComponent.IsMoving && entity.MovementComponent.CurrentPath.Count > 0)
                {
                    var nextPos = entity.MovementComponent.CurrentPath.Peek();
                    MoveTo(entity, nextPos);
                }
                if (entity.MovementComponent.IsMoving)
                {
                    entity.MovementComponent.ElapsedMovementTime += Time.deltaTime;
                    if (entity.MovementComponent.ElapsedMovementTime > TimeBetweenTiles)
                    {
                        entity.MovementComponent.ElapsedMovementTime = TimeBetweenTiles;
                    }
                    var lerpPercentarge = entity.MovementComponent.ElapsedMovementTime / TimeBetweenTiles;
                    var targetVectorPos = entity.MovementComponent.LerpTargetPosition;
                    var _lerpPos = Vector2.Lerp(entity.MovementComponent.LerpCurrentPosition.ToVector2(), targetVectorPos.ToVector2(), lerpPercentarge);

                    if (entity.View != null)
                    {
                        entity.View.transform.position = _lerpPos;
                    }
                    if (Vector2.Distance(_lerpPos, targetVectorPos.ToVector2()) < 0.005f)
                    {
                        EntityArriveAtPosition(level, entity);

                        if (entity.MovementComponent.CurrentPath.Count > 0)
                        {
                            var oldPos = entity.MovementComponent.CurrentPath.Dequeue();
                            var nextPos = entity.MovementComponent.CurrentPath.Peek();
                            MoveTo(entity, nextPos);
                        }
                        else
                        {
                            if (entity.View != null)
                            {
                                entity.View.transform.position = MathUtil.MapToWorld(entity.MovementComponent.TargetPosition);
                            }
                            entity.MovementComponent.IsMoving = false;
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
            entity.Position = entity.MovementComponent.TargetPosition;
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
            if(entity.MovementComponent == null)
            {
                throw new NotImplementedException("The thing you are trying to move does not have a movement component.  Check the prototype and see if you forgot to add it.");
            }
            entity.MovementComponent.TargetPosition = newPosition;
            entity.MovementComponent.LerpCurrentPosition = MathUtil.MapToWorld(entity.Position).ToPointf();
            entity.MovementComponent.LerpTargetPosition = MathUtil.MapToWorld(newPosition).ToPointf();
            entity.MovementComponent.ElapsedMovementTime = 0.0f;
            entity.MovementComponent.IsMoving = true;
        }

        public void FollowPath(Entity entity, List<Point> path)
        {
            entity.MovementComponent.CurrentPath.Clear();
            foreach(var point in path)
            {
                entity.MovementComponent.CurrentPath.Enqueue(point);
            }
        }
    }
}
