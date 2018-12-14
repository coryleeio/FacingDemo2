using KDSharp.KDTree;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class AIBehaviour : Behaviour
    {
        public override bool IsPlayer
        {
            get
            {
                return false;
            }
        }

        [JsonIgnore]
        public List<Point> PointsAroundTarget = new List<Point>();

        [JsonIgnore]
        public List<Point> PointsAroundMe = new List<Point>();

        public int PathsExpected = 0;
        public int PathsReturned = 0;

        [JsonIgnore]
        Coroutine runningRoutine;

        public override void FigureOutNextAction()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            var target = FindTarget(entity);
            var capabilities = new AttackCapabilities(entity);
            var meleeCapabilities = capabilities[CombatContext.Melee];
            var thrownCapabilities = capabilities[CombatContext.Thrown];
            var rangedCapabilities = capabilities[CombatContext.Ranged];
            var ZappedCapabilities = capabilities[CombatContext.Zapped];
            NextAction = null;

            if (target == null)
            {
                DefaultBehaviour();
                return;
            }

            if (!Context.VisibilitySystem.CanSee(level, entity, target))
            {
                DefaultBehaviour();
            }
            else
            {
                if (meleeCapabilities.CanPerform && meleeCapabilities.IsInRange(target))
                {
                    var direction = MathUtil.RelativeDirection(entity.Position, target.Position);
                    var attack = new Attack(capabilities, CombatContext.Melee, direction);
                    NextAction = attack;
                }
                else if (ZappedCapabilities.CanPerform && ZappedCapabilities.IsInRange(target) && ZappedCapabilities.HasAClearShot(target.Position))
                {
                    var direction = MathUtil.RelativeDirection(entity.Position, target.Position);
                    var attack = new Attack(capabilities, CombatContext.Zapped, direction);
                    NextAction = attack;
                }
                else if(thrownCapabilities.CanPerform && thrownCapabilities.IsInRange(target) && thrownCapabilities.HasAClearShot(target.Position) && thrownCapabilities.MainHand != null && thrownCapabilities.MainHand.NumberOfItems > 1)
                {
                    var direction = MathUtil.RelativeDirection(entity.Position, target.Position);
                    var attack = new Attack(capabilities, CombatContext.Thrown, direction);
                    NextAction = attack;
                }
                else if (rangedCapabilities.CanPerform && rangedCapabilities.IsInRange(target) && rangedCapabilities.HasAClearShot(target.Position))
                {
                    var direction = MathUtil.RelativeDirection(entity.Position, target.Position);
                    var attack = new Attack(capabilities, CombatContext.Ranged, direction);
                    NextAction = attack;
                }

                else
                {
                    Context.Application.StartCoroutine(MoveToward(target.Position));
                }
            }
        }

        public static NearestNeighbour<Entity> NearestTargets(Entity entity)
        {
            var entitySystem = Context.EntitySystem;
            KDTree<Entity> relevantTree = null;

            if (entity.Behaviour.Team == Team.PLAYER)
            {
                // If I am a member of the player's team - search for enemies.
                relevantTree = entitySystem.EnemyTeamTree;
            }
            else if (entity.Behaviour.Team == Team.ENEMY)
            {
                // If I am a member of the enemy team - search for players.
                relevantTree = entitySystem.PlayerTeamTree;
            }
            else if (entity.Behaviour.Team == Team.NEUTRAL)
            {
                // Neutrals dont have enemies.
            }
            else
            {
                throw new NotImplementedException();
            }
            return relevantTree.NearestNeighbors(new double[2] { entity.Position.X, entity.Position.Y }, 10);
        }

        private static Entity FindTarget(Entity entity)
        {
            var entitySystem = Context.EntitySystem;
            var possibleTargets = NearestTargets(entity);

            if (possibleTargets == null)
            {
                return null;
            }
            foreach (var target in possibleTargets)
            {
                return target;
            }
            return null;
        }

        private void DefaultBehaviour()
        {
            if (entity.Behaviour.Team == Team.PLAYER)
            {
                // Default for allies is follow player
                var player = Context.GameStateManager.Game.CurrentLevel.Player;
                Context.Application.StartCoroutine(MoveToward(player.Position));
            }
            else
            {
                // Default for monsters is waiting
                var wait = new Wait
                {
                    Source = entity
                };
                NextAction = wait;
            }
        }

        IEnumerator MoveToward(Point targetPosition)
        {
            NextAction = null;
            PathsReturned = 0;

            var level = Context.GameStateManager.Game.CurrentLevel;
            var PointsAroundTarget = MathUtil.OrthogonalPoints(targetPosition).FindAll((p) => { return level.Grid[p].Walkable; });
            var PointsAroundMe = MathUtil.OrthogonalPoints(entity.Position).FindAll((p) => { return level.Grid[p].Walkable && Point.DistanceSquared(p, targetPosition) < Point.DistanceSquared(entity.Position, targetPosition); });

            PathsExpected = PointsAroundTarget.Count + PointsAroundMe.Count;
            PointsAroundMe.Sort(new PointDistanceComparer()
            {
                Source = entity.Position
            });

            foreach (var pointAroundTarget in PointsAroundTarget)
            {
                Context.PathFinder.StartPath(entity.Position, pointAroundTarget, Context.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            foreach (var pointAroundMe in PointsAroundMe)
            {
                Context.PathFinder.StartPath(entity.Position, pointAroundMe, Context.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            while (NextAction == null)
            {
                if (PathsReturned == PathsExpected)
                {
                    var wait = new Wait
                    {
                        Source = entity
                    };
                    NextAction = wait;
                    break; // done
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public void ReceivePath(Path path)
        {
            PathsReturned++;
            if (path.Nodes.Count > 0)
            {
                if (runningRoutine != null)
                {
                    Context.Application.StopCoroutine(runningRoutine);
                    runningRoutine = null;
                }
                var move = new Move
                {
                    Source = entity,
                    TargetPosition = new Point(path.Nodes[0].Position.X, path.Nodes[0].Position.Y)
                };
                NextAction = move;
            }
        }
    }
}
