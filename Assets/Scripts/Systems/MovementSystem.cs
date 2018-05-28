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
            var tokens = level.Tokens;

            foreach (var token in tokens)
            {
                if(!token.IsMoving && token.CurrentPath.Count > 0)
                {
                    var nextPos = token.CurrentPath.Peek();
                    MoveTo(token, nextPos);
                }
                if (token.IsMoving)
                {
                    token.ElapsedMovementTime += Time.deltaTime;
                    if (token.ElapsedMovementTime > TimeBetweenTiles)
                    {
                        token.ElapsedMovementTime = TimeBetweenTiles;
                    }
                    var lerpPercentarge = token.ElapsedMovementTime / TimeBetweenTiles;
                    var targetVectorPos = token.LerpTargetPosition;
                    var _lerpPos = Vector2.Lerp(token.LerpCurrentPosition.ToVector2(), targetVectorPos.ToVector2(), lerpPercentarge);

                    if (token.View != null)
                    {
                        token.View.transform.position = _lerpPos;
                    }
                    if (Vector2.Distance(_lerpPos, targetVectorPos.ToVector2()) < 0.005f)
                    {
                        TokenArriveAtPosition(level, token);

                        if (token.CurrentPath.Count > 0)
                        {
                            var oldPos = token.CurrentPath.Dequeue();
                            var nextPos = token.CurrentPath.Peek();
                            MoveTo(token, nextPos);
                        }
                        else
                        {
                            if (token.View != null)
                            {
                                token.View.transform.position = MathUtil.MapToWorld(token.TargetPosition);
                            }
                            token.IsMoving = false;
                        }
                    }
                }
            }
        }

        private static void TokenArriveAtPosition(Level level, Token token)
        {
            // when we arrive give up the lock on our current position
            if (token.TokenPrototype.BlocksPathing)
            {
                level.TilesetGrid[token.Position].Walkable = true;
            }
            level.UnindexToken(token, token.Position);
            token.Position = token.TargetPosition;
            level.IndexToken(token, token.Position);
        }

        public void MoveTo(Token token, Point newPosition)
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            // Reserve the new position as soon as I start walking so nobody else uses it
            // Later when we arrive we will release the lock on our OLD position.
            if (token.TokenPrototype.BlocksPathing)
            {
                level.TilesetGrid[newPosition].Walkable = false;
            }
            token.TargetPosition = newPosition;
            token.LerpCurrentPosition = MathUtil.MapToWorld(token.Position).ToPointf();
            token.LerpTargetPosition = MathUtil.MapToWorld(newPosition).ToPointf();
            token.ElapsedMovementTime = 0.0f;
            token.IsMoving = true;
        }

        public void FollowPath(Token token, List<Point> path)
        {
            token.CurrentPath.Clear();
            foreach(var point in path)
            {
                token.CurrentPath.Enqueue(point);
            }
        }
    }
}
