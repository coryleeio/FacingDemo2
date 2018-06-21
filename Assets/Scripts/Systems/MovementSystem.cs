using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class MovementSystem
    {
        private float TimeBetweenTiles = 0.5f;

        public void Process()
        {
            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            var entities = level.Entitys;

            foreach (var entity in entities)
            {
                if(entity.Motor == null)
                {
                    continue;
                }

                if(!entity.Motor.IsMoving && entity.Motor.CurrentPath.Count > 0)
                {
                    var nextPos = entity.Motor.CurrentPath.Peek();
                    MoveTo(entity, nextPos);
                }
                if (entity.Motor.IsMoving)
                {
                    entity.Motor.ElapsedMovementTime += Time.deltaTime;
                    if (entity.Motor.ElapsedMovementTime > TimeBetweenTiles)
                    {
                        entity.Motor.ElapsedMovementTime = TimeBetweenTiles;
                    }
                    var lerpPercentarge = entity.Motor.ElapsedMovementTime / TimeBetweenTiles;
                    var targetVectorPos = entity.Motor.LerpTargetPosition;
                    var _lerpPos = Vector2.Lerp(entity.Motor.LerpCurrentPosition.ToVector2(), targetVectorPos.ToVector2(), lerpPercentarge);

                    if (entity.View != null && entity.View.ViewGameObject != null)
                    {
                        entity.View.ViewGameObject.transform.position = _lerpPos;
                    }
                    if (Vector2.Distance(_lerpPos, targetVectorPos.ToVector2()) < 0.005f)
                    {
                        EntityArriveAtPosition(level, entity);

                        if (entity.Motor.CurrentPath.Count > 0)
                        {
                            var oldPos = entity.Motor.CurrentPath.Dequeue();
                            var nextPos = entity.Motor.CurrentPath.Peek();
                            MoveTo(entity, nextPos);
                        }
                        else
                        {
                            if (entity.View != null && entity.View.ViewGameObject != null)
                            {
                                entity.View.ViewGameObject.transform.position = MathUtil.MapToWorld(entity.Motor.MoveTargetPosition);
                            }
                            entity.Motor.IsMoving = false;
                        }
                    }
                }
            }
        }

        private static void EntityArriveAtPosition(Level level, Entity entity)
        {
            // when we arrive give up the lock on our current position
            if (entity.BlocksPathing)
            {
                level.TilesetGrid[entity.Position].Walkable = true;
            }
            level.UnindexEntity(entity, entity.Position);
            entity.Position = entity.Motor.MoveTargetPosition;
            level.IndexEntity(entity, entity.Position);
        }

        public void MoveTo(Entity entity, Point newPosition)
        {
            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            // Reserve the new position as soon as I start walking so nobody else uses it
            // Later when we arrive we will release the lock on our OLD position.
            if (entity.BlocksPathing)
            {
                level.TilesetGrid[newPosition].Walkable = false;
            }
            if(entity.Motor == null)
            {
                throw new NotImplementedException("The thing you are trying to move does not have a movement component.  Check the prototype and see if you forgot to add it.");
            }
            entity.Motor.MoveTargetPosition = newPosition;
            entity.Motor.LerpCurrentPosition = MathUtil.MapToWorld(entity.Position).ToPointf();
            entity.Motor.LerpTargetPosition = MathUtil.MapToWorld(newPosition).ToPointf();
            entity.Motor.ElapsedMovementTime = 0.0f;
            entity.Motor.IsMoving = true;
        }

        public void FollowPath(Entity entity, List<Point> path)
        {
            entity.Motor.CurrentPath.Clear();
            foreach(var point in path)
            {
                entity.Motor.CurrentPath.Enqueue(point);
            }
        }
    }
}
