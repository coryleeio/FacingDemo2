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
            var level = Context.GameStateManager.Game.CurrentLevel;
            var target = FindTarget(Entity);
            NextAction = null;

            if (target == null)
            {
                DefaultBehaviour();
                return;
            }

            if (!Context.VisibilitySystem.CanSee(level, Entity, target))
            {
                DefaultBehaviour();
            }
            else
            {
                // If we see the target move toward it or attack him
                if (CombatUtil.CanMelee(Entity, target))
                {
                    var attack = Context.PrototypeFactory.BuildEntityAction<MeleeAttack>(Entity);
                    attack.Targets.Add(target);
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
            if(Entity.Behaviour.Team == Team.PLAYER)
            {
                // Default for allies is follow player
                var player = Context.GameStateManager.Game.CurrentLevel.Player;
                Context.Application.StartCoroutine(MoveToward(player.Position));
            }
            else
            {
                // Default for monsters is waiting
                NextAction = Context.PrototypeFactory.BuildEntityAction<Wait>(Entity);
            }
        }

        IEnumerator MoveToward(Point targetPosition)
        {
            NextAction = null;
            PathsReturned = 0;

            var level = Context.GameStateManager.Game.CurrentLevel;
            var PointsAroundTarget = MathUtil.OrthogonalPoints(targetPosition).FindAll((p) => { return level.Grid[p].Walkable; });
            var PointsAroundMe = MathUtil.OrthogonalPoints(Entity.Position).FindAll((p) => { return level.Grid[p].Walkable && Point.DistanceSquared(p, targetPosition) < Point.DistanceSquared(Entity.Position, targetPosition); });

            PathsExpected = PointsAroundTarget.Count + PointsAroundMe.Count;
            PointsAroundMe.Sort(new PointDistanceComparer()
            {
                Source = Entity.Position
            });

            foreach (var pointAroundTarget in PointsAroundTarget)
            {
                Context.PathFinder.StartPath(Entity.Position, pointAroundTarget, Context.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            foreach (var pointAroundMe in PointsAroundMe)
            {
                Context.PathFinder.StartPath(Entity.Position, pointAroundMe, Context.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            while (NextAction == null)
            {
                if (PathsReturned == PathsExpected)
                {
                    NextAction = Context.PrototypeFactory.BuildEntityAction<Wait>(Entity);
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
                var move = Context.PrototypeFactory.BuildEntityAction<Move>(Entity) as Move;
                move.TargetPosition = new Point(path.Nodes[0].Position.X, path.Nodes[0].Position.Y);
                NextAction = move;
            }
        }
    }
}
