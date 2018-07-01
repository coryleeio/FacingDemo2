using KDSharp.DistanceFunctions;
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
            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            var target = FindTarget(Entity);
            NextAction = null;

            if (target == null)
            {
                DefaultBehaviour();
                return;
            }

            if (!ServiceLocator.VisibilitySystem.CanSee(level, Entity, target))
            {
                DefaultBehaviour();
            }
            else
            {
                // If we see the target move toward it or attack him
                if (ServiceLocator.CombatSystem.CanMelee(Entity, target))
                {
                    var attack = ServiceLocator.PrototypeFactory.BuildEntityAction<MeleeAttack>(Entity);
                    attack.Targets.Add(target);
                    NextAction = attack;
                }
                else
                {
                    ServiceLocator.Application.StartCoroutine(MoveToward(target.Position));
                }
            }
        }

        public static NearestNeighbour<Entity> NearestTargets(Entity entity)
        {
            var entitySystem = ServiceLocator.EntitySystem;
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
            var entitySystem = ServiceLocator.EntitySystem;
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
            if(Entity.Behaviour.Team == Team.PLAYER)
            {
                // Default for allies is follow player
                var player = ServiceLocator.GameStateManager.Game.CurrentLevel.Player;
                ServiceLocator.Application.StartCoroutine(MoveToward(player.Position));
            }
            else
            {
                // Default for monsters is waiting
                NextAction = ServiceLocator.PrototypeFactory.BuildEntityAction<Wait>(Entity);
            }
        }

        IEnumerator MoveToward(Point targetPosition)
        {
            NextAction = null;
            PathsReturned = 0;

            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            var PointsAroundTarget = MathUtil.OrthogonalPoints(targetPosition).FindAll((p) => { return level.Grid[p].Walkable; });
            var PointsAroundMe = MathUtil.OrthogonalPoints(Entity.Position).FindAll((p) => { return level.Grid[p].Walkable && Point.DistanceSquared(p, targetPosition) < Point.DistanceSquared(Entity.Position, targetPosition); });

            PathsExpected = PointsAroundTarget.Count + PointsAroundMe.Count;
            PointsAroundMe.Sort(new PointDistanceComparer()
            {
                Source = Entity.Position
            });

            foreach (var pointAroundTarget in PointsAroundTarget)
            {
                ServiceLocator.PathFinder.StartPath(Entity.Position, pointAroundTarget, ServiceLocator.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            foreach (var pointAroundMe in PointsAroundMe)
            {
                ServiceLocator.PathFinder.StartPath(Entity.Position, pointAroundMe, ServiceLocator.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            while (NextAction == null)
            {
                if (PathsReturned == PathsExpected)
                {
                    NextAction = ServiceLocator.PrototypeFactory.BuildEntityAction<Wait>(Entity);
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
                    ServiceLocator.Application.StopCoroutine(runningRoutine);
                    runningRoutine = null;
                }
                var move = ServiceLocator.PrototypeFactory.BuildEntityAction<Move>(Entity) as Move;
                move.TargetPosition = new Point(path.Nodes[0].Position.X, path.Nodes[0].Position.Y);
                NextAction = move;
            }
        }
    }
}
