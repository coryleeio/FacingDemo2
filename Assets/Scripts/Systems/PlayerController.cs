using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class PlayerController
    {
        private Overlay MouseHoverOverlay;
        private OverlayConfig MouseHoverOverlayConfig;
        private OverlayConfig PathOverlayConfig;
        private bool waitingForPath = false;

        private Color DefaultHoverColor = new Color(0, 213, 255);
        private Color EnemyHoverColor = Color.red;
        private Path CurrentPath;
        public Queue<Action> ActionList = new Queue<Action>();

        public void Init()
        {
            CurrentPath = null;
            waitingForPath = false;

            MouseHoverOverlayConfig = new OverlayConfig
            {
                Name = "MouseHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { new Point(0, 0) },
                DefaultColor = DefaultHoverColor,
                RelativeSortOrder = 1,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Overlay/Square"),
            };
            PathOverlayConfig = new OverlayConfig
            {
                Name = "PathHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { new Point(0, 0) },
                DefaultColor = Color.green,
                RelativeSortOrder = 0,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Dot"),
            };
            MouseHoverOverlay = new Overlay()
            {
                Configs = new List<OverlayConfig>()
                {
                    MouseHoverOverlayConfig,
                    PathOverlayConfig,
                }
            };
        }

        public void Process()
        {
            var game = ServiceLocator.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            var mousePos = MathUtil.GetMousePositionOnMap(Camera.main);
            var isValidPoint = level.BoundingBox.Contains(mousePos);
            var isHoveringOnEnemyCombatant = isValidPoint && mousePos != player.Position && level.Grid[mousePos].EntitiesInPosition.Count > 0 && level.Grid[mousePos].EntitiesInPosition[0].IsCombatant && level.Grid[mousePos].EntitiesInPosition[0].Behaviour.Team == Team.ENEMY;
            var isHoveringOnAlly = isValidPoint && mousePos != player.Position && level.Grid[mousePos].EntitiesInPosition.Count > 0 && level.Grid[mousePos].EntitiesInPosition[0].IsCombatant && level.Grid[mousePos].EntitiesInPosition[0].Behaviour.Team == Team.PLAYER && !level.Grid[mousePos].EntitiesInPosition[0].Behaviour.IsPlayer;

            var isAbleToHitHoveringEnemyCombatant = isHoveringOnEnemyCombatant && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);
            var isAbleToSwapWithHoveringAlly = isHoveringOnAlly && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);

            ServiceLocator.OverlaySystem.SetActivated(MouseHoverOverlay, true);
            MouseHoverOverlayConfig.DefaultColor = isHoveringOnEnemyCombatant ? EnemyHoverColor : DefaultHoverColor;
            MouseHoverOverlayConfig.Position = mousePos;
            PathOverlayConfig.Position = mousePos;

            if (player.Body.IsDead)
            {
                ServiceLocator.UIController.DeathNotification.Show();
                return;
            }

            if(ActionList.Count > 0 && player.Behaviour.NextAction == null && ServiceLocator.FlowSystem.CurrentPhase == Phase.Player)
            {
                var nextAction = ActionList.Dequeue();

                if(nextAction.GetType() == typeof(Move))
                {
                    var nextActionAsMove = nextAction as Move;

                    if(!level.Grid[nextActionAsMove.TargetPosition].Walkable)
                    {
                        nextAction = null;
                        ActionList.Clear();
                    }
                }

                player.Behaviour.NextAction = nextAction;
            }

            var points = new List<Point>();
            if(CurrentPath != null)
            {
                foreach (var node in CurrentPath.Nodes)
                {
                    points.Add(node.Position);
                }
            }

            PathOverlayConfig.OffsetPoints = MathUtil.ConvertMapSpaceToLocalMapSpace(mousePos, points);

            if (!waitingForPath)
            {
                if (isHoveringOnEnemyCombatant && !isAbleToHitHoveringEnemyCombatant)
                {
                    var surroundingPositions = MathUtil.OrthogonalPoints(mousePos).FindAll((p) => { return level.Grid[p].Walkable; });

                    if (surroundingPositions.Count > 0)
                    {
                        surroundingPositions.Sort(delegate (Point p1, Point p2)
                        {
                            return Point.Distance(player.Position, p1).CompareTo(Point.Distance(player.Position, p2));
                        });
                        waitingForPath = true;
                        ServiceLocator.PathFinder.StartPath(player.Position, surroundingPositions[0], level.Grid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }
                }
                else
                {
                    waitingForPath = true;

                    if(player.Behaviour.NextAction != null && player.Behaviour.NextAction.GetType() == typeof(Move))
                    {
                        ServiceLocator.PathFinder.StartPath(((Move)player.Behaviour.NextAction).TargetPosition, mousePos, level.Grid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }
                    else
                    {
                        ServiceLocator.PathFinder.StartPath(player.Position, mousePos, level.Grid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }


                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                ActionList.Clear();
                if(isAbleToSwapWithHoveringAlly)
                {
                    QueueSwapPosition(level, player, mousePos);
                }
                else if (isAbleToHitHoveringEnemyCombatant)
                {
                    QueueAttack(level, player, mousePos);
                }
                else
                {
                    if(CurrentPath != null)
                    {
                        OnPathComplete(CurrentPath);
                    }
                }
            }
        }

        private void QueueSwapPosition(Level level, Entity player, Point mousePos)
        {
            var swapPositions = ServiceLocator.PrototypeFactory.BuildEntityAction<SwapPositionsWithAlly>(player);
            swapPositions.Targets.Add(level.Grid[mousePos].EntitiesInPosition.Find((x) => { return x.Behaviour != null; } ));
            player.Behaviour.NextAction = swapPositions;
        }

        private void QueueAttack(Level level, Entity player, Point mousePos)
        {
            var attack = ServiceLocator.PrototypeFactory.BuildEntityAction<MeleeAttack>(player);
            attack.Targets.Add(level.Grid[mousePos].EntitiesInPosition[0]);
            player.Behaviour.NextAction = attack;
        }

        private void OnPathComplete(Path path)
        {
            var game = ServiceLocator.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            foreach (var node in path.Nodes)
            {
                var move = ServiceLocator.PrototypeFactory.BuildEntityAction<Move>(player) as Move;
                move.TargetPosition = new Point(node.Position.X, node.Position.Y);
                ActionList.Enqueue(move);
            }
            waitingForPath = false;
        }
    }
}
