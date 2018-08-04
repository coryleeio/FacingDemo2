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
        public Queue<TargetableAction> ActionList = new Queue<TargetableAction>();

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
            bool shouldAcceptInput = !Context.UIController.InventoryWindow.isActiveAndEnabled
                                        && !Context.UIController.EscapeMenu.isActiveAndEnabled;
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            var mousePos = MathUtil.GetMousePositionOnMap(Camera.main);
            var hoverIsValidPoint = level.BoundingBox.Contains(mousePos);


            var tileContainsEnemy = false;
            var hoverContainsCombatant = false;
            var hoverContainsAlly = false;

            if (hoverIsValidPoint)
            {
                foreach (var entity in level.Grid[mousePos].EntitiesInPosition)
                {
                    if (entity.IsCombatant)
                    {
                        hoverContainsCombatant = true;
                    }
                    if (entity.Behaviour != null && entity.Behaviour.Team == Team.ENEMY)
                    {
                        tileContainsEnemy = true;
                    }
                    if (entity.IsCombatant && entity.Behaviour != null && entity.Behaviour.Team == Team.PLAYER && !entity.IsPlayer)
                    {
                        hoverContainsAlly = true;
                    }
                }
            }

            var isHoveringOnEnemyCombatant = hoverIsValidPoint && mousePos != player.Position && hoverContainsCombatant && tileContainsEnemy;
            var isHoveringOnAlly = hoverIsValidPoint && mousePos != player.Position && hoverContainsAlly;
            var isAbleToHitHoveringEnemyCombatant = isHoveringOnEnemyCombatant && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);
            var isAbleToSwapWithHoveringAlly = isHoveringOnAlly && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);

            Context.OverlaySystem.SetActivated(MouseHoverOverlay, shouldAcceptInput);
            MouseHoverOverlayConfig.DefaultColor = isHoveringOnEnemyCombatant ? EnemyHoverColor : DefaultHoverColor;
            MouseHoverOverlayConfig.Position = mousePos;
            PathOverlayConfig.Position = mousePos;

            if (player.Body.IsDead)
            {
                Context.UIController.DeathNotification.Show();
                return;
            }

            if (ActionList.Count > 0 && player.Behaviour.NextAction == null && Context.FlowSystem.CurrentPhase == Phase.Player)
            {
                var nextAction = ActionList.Dequeue();

                if (nextAction.GetType() == typeof(Move))
                {
                    var nextActionAsMove = nextAction as Move;

                    if (!level.Grid[nextActionAsMove.TargetPosition].Walkable)
                    {
                        var occupants = level.Grid[nextActionAsMove.TargetPosition].EntitiesInPosition;
                        Entity adjacentFriendlyBlocker = null;
                        foreach (var occupant in occupants)
                        {
                            if (occupant.Behaviour != null && occupant.Behaviour.Team == Team.PLAYER && !occupant.IsPlayer && occupant.BlocksPathing)
                            {
                                adjacentFriendlyBlocker = occupant;
                                break;
                            }
                        }
                        if (adjacentFriendlyBlocker != null)
                        {
                            // Move is blocked by a non ghostly friendly, we can swap instead of move.
                            // we will just create a swap because the move has already been dequeued
                            nextAction = Context.PrototypeFactory.BuildEntityAction<SwapPositionsWithAlly>(player) as SwapPositionsWithAlly;
                            nextAction.Targets.Add(adjacentFriendlyBlocker);
                        }
                        else
                        {
                            // Action is blocked by a non friendly
                            nextAction = null;
                            ActionList.Clear();
                        }
                    }
                }

                player.Behaviour.NextAction = nextAction;
            }

            var points = new List<Point>();
            if (CurrentPath != null)
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
                        Context.PathFinder.StartPath(player.Position, surroundingPositions[0], level.Grid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }
                }
                else
                {
                    waitingForPath = true;

                    if (player.Behaviour.NextAction != null && player.Behaviour.NextAction.GetType() == typeof(Move))
                    {
                        Context.PathFinder.StartPath(((Move)player.Behaviour.NextAction).TargetPosition, mousePos, level.Grid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }
                    else
                    {
                        Context.PathFinder.StartPath(player.Position, mousePos, level.Grid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Context.UIController.HasWindowsOpen)
                {
                    Context.UIController.Pop();
                }
                else
                {
                    Context.UIController.EscapeMenu.Show();
                }
                Context.UIController.ContextMenu.Hide();
                Context.UIController.Tooltip.Hide();
            }

            if (Context.UIController.EscapeMenu.isActiveAndEnabled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                if (Context.UIController.InventoryWindow.isActiveAndEnabled)
                {
                    Context.UIController.ItemInspectionWindow.Hide();
                }
                Context.UIController.InventoryWindow.Toggle();
                Context.UIController.LootWindow.Toggle();
                Context.UIController.Tooltip.Hide();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                QueueWait(level, player);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                var entitiesInPosition = level.Grid[player.Position].EntitiesInPosition;
                var deadEntitiesInPlayerPosition = entitiesInPosition.FindAll((ent) => { return ent.Body != null && ent.Body.IsDead; });
                if (deadEntitiesInPlayerPosition.Count > 0)
                {
                    Context.UIController.LootWindow.ShowFor(deadEntitiesInPlayerPosition);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (shouldAcceptInput)
                {
                    ActionList.Clear();
                    if (isAbleToSwapWithHoveringAlly)
                    {
                        QueueSwapPosition(level, player, mousePos);
                    }
                    else if (isAbleToHitHoveringEnemyCombatant)
                    {
                        QueueAttack(level, player, mousePos);
                    }
                    else
                    {
                        if (CurrentPath != null)
                        {
                            OnPathComplete(CurrentPath);
                        }
                    }
                }
            }
        }

        private void QueueWait(Level level, Entity player)
        {
            var wait = Context.PrototypeFactory.BuildEntityAction<Wait>(player);
            player.Behaviour.NextAction = wait;
        }

        private void QueueSwapPosition(Level level, Entity player, Point mousePos)
        {
            var swapPositions = Context.PrototypeFactory.BuildEntityAction<SwapPositionsWithAlly>(player);
            swapPositions.Targets.Add(level.Grid[mousePos].EntitiesInPosition.Find((x) => { return x.Behaviour != null; }));
            player.Behaviour.NextAction = swapPositions;
        }

        private void QueueAttack(Level level, Entity player, Point mousePos)
        {
            var enemyTarget = level.Grid[mousePos].EntitiesInPosition.Find((t) => t.IsCombatant && t.Body != null && !t.Body.IsDead);
            var attack = Context.PrototypeFactory.BuildEntityAction<MeleeAttack>(player);
            attack.Targets.Add(enemyTarget);
            player.Behaviour.NextAction = attack;
        }

        private void OnPathComplete(Path path)
        {
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            foreach (var node in path.Nodes)
            {
                var move = Context.PrototypeFactory.BuildEntityAction<Move>(player) as Move;
                move.TargetPosition = new Point(node.Position.X, node.Position.Y);
                ActionList.Enqueue(move);
            }
            waitingForPath = false;
        }
    }
}
