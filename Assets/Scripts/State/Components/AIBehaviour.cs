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

        public Point LastKnownPlayerPosition;

        [JsonIgnore]
        UnityEngine.Coroutine runningRoutine;

        public override void FigureOutNextAction()
        {
            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            var player = level.Player;
            NextAction = null;

            if (!ServiceLocator.VisibilitySystem.CanSee(level, Entity, player))
            {
                if (LastKnownPlayerPosition != null && Point.Distance(Entity.Position, LastKnownPlayerPosition) > 2)
                {
                    ServiceLocator.Application.StartCoroutine(MoveToward(LastKnownPlayerPosition));
                    return;
                }
                else
                {
                    LastKnownPlayerPosition = null;
                    NextAction = ServiceLocator.PrototypeFactory.BuildEntityAction<Wait>(Entity);
                    return;
                }
            }
            else
            {
                LastKnownPlayerPosition = new Point(player.Position.X, player.Position.Y);
                if (ServiceLocator.CombatSystem.CanMelee(Entity, player))
                {
                    var attack = ServiceLocator.PrototypeFactory.BuildEntityAction<MeleeAttack>(Entity);
                    attack.Targets.Add(player);
                    NextAction = attack;
                    return;
                }
                else
                {
                    ServiceLocator.Application.StartCoroutine(MoveToward(LastKnownPlayerPosition));
                    return;
                }
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

            while(NextAction == null)
            {
                if(PathsReturned == PathsExpected)
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
                if(runningRoutine != null)
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
