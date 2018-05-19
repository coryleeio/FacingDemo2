using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class MovementSystem
    {
        public PathFinder PathFinder { get; set; }
        private float TimeBetweenTiles = 15f;

        public void Process(List<Token> tokens, Level level, float deltaTime)
        {
            foreach (var token in tokens)
            {
                if(!token.IsMoving && token.CurrentPath.Count > 0)
                {
                    var nextPos = token.CurrentPath.Dequeue();
                    MoveTo(token, nextPos);
                }
                if (token.IsMoving)
                {
                    token.ElapsedMovementTime += deltaTime;
                    if (token.ElapsedMovementTime > TimeBetweenTiles)
                    {
                        token.ElapsedMovementTime = TimeBetweenTiles;
                    }
                    var lerpPercentarge = token.ElapsedMovementTime / TimeBetweenTiles;
                    var targetVectorPos = token.LerpTargetPosition;
                    var _lerpPos = Vector2.Lerp(token.LerpCurrentPosition, targetVectorPos, lerpPercentarge);

                    if (token.View != null)
                    {
                        token.View.transform.position = _lerpPos;
                    }
                    if (Vector2.Distance(_lerpPos, targetVectorPos) < 0.005f)
                    {
                        token.Position = token.TargetPosition;
                        if (token.CurrentPath.Count > 0)
                        {
                            var nextPos = token.CurrentPath.Dequeue();
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

        private void MoveTo(Token token, Point newPosition)
        {
            token.TargetPosition = newPosition;
            token.LerpCurrentPosition = MathUtil.MapToWorld(token.Position);
            token.LerpTargetPosition = MathUtil.MapToWorld(newPosition);
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
