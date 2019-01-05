using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class PlayerController
    {
        private Overlay MouseHoverOverlay;
        private Overlay AimingOverlay;
        private OverlayConfig MouseHoverOverlayConfig;
        private OverlayConfig PathOverlayConfig;
        private OverlayConfig PossibleAttackPositions;
        private OverlayConfig CurrentProposedAttackPlacement;
        private OverlayConfig CurrentProposedAttackExplosionPlacement;
        private bool waitingForPath = false;

        private Color DefaultHoverColor = new Color(0, 213, 255);
        private Color EnemyHoverColor = Color.red;
        private Path CurrentPath;
        private bool isAiming = false;
        private CombatContext AimingCombatContext = CombatContext.NotSet;

        // Use the capabilities that were established when we started aiming
        // this is important for throwing from inventory, otherwise they will get recalculated in the 
        // button down section of this and cause the primary weapon to be thrown if one exists
        // or it will cause you not to be able to throw if one does not.
        private AttackCapabilities AimingAttackCapabilities;
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

            PossibleAttackPositions = new OverlayConfig()
            {
                Name = "AttackOverlayConfig",
                Position = new Point(0, 0),
                DefaultColor = Color.green,
                RelativeSortOrder = 1,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Overlay/Square"),
            };
            CurrentProposedAttackPlacement = new OverlayConfig
            {
                Name = "AttackPlacementHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { new Point(0, 0) },
                DefaultColor = Color.yellow,
                RelativeSortOrder = 2,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Overlay/Square"),
            };
            CurrentProposedAttackExplosionPlacement = new OverlayConfig
            {
                Name = "ExplosionHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { },
                DefaultColor = Color.red,
                RelativeSortOrder = 3,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Overlay/AoeIndicator"),
            };
            AimingOverlay = new Overlay()
            {
                Configs = new List<OverlayConfig>()
                {
                    PossibleAttackPositions,
                    CurrentProposedAttackPlacement,
                    CurrentProposedAttackExplosionPlacement,
                }
            };
        }

        public void GenerateAttackOverlayOffsets(Point mousePos, Point position, AttackCapability capability, Direction direction)
        {
            if (capability.AttackTargetingType == AttackTargetingType.Line && (direction == Direction.SouthEast || direction == Direction.SouthWest || direction == Direction.NorthEast || direction == Direction.NorthWest))
            {
                var lineOffsets = MathUtil.LineInDirection(position, direction, capability.Range);
                CurrentProposedAttackPlacement.OffsetPoints = lineOffsets;
                PossibleAttackPositions.OffsetPoints = capability.PointsInRange();
            }
            else if (capability.AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                CurrentProposedAttackPlacement.OffsetPoints = new List<Point>()
                {
                    mousePos,
                };
                PossibleAttackPositions.OffsetPoints = capability.PointsInRange();
            }

            if (capability.AttackParameters != null && capability.AttackParameters.ExplosionParameters != null)
            {

                if (capability.PointsInRange().Contains(mousePos))
                {
                    var explosionParameters = capability.AttackParameters.ExplosionParameters;
                    if (capability.AttackTargetingType == AttackTargetingType.Line)
                    {
                        var endpoint = CalculateEndpointOfSkillshot(position, capability, direction);
                        CurrentProposedAttackExplosionPlacement.Position = endpoint;
                        CurrentProposedAttackExplosionPlacement.OffsetPoints = MathUtil.ConvertMapSpaceToLocalMapSpace(endpoint, capability.PointsInExplosionRange(endpoint));
                    }
                    else if(capability.AttackTargetingType == AttackTargetingType.SelectTarget)
                    {
                        CurrentProposedAttackExplosionPlacement.Position = mousePos;
                        CurrentProposedAttackExplosionPlacement.OffsetPoints = MathUtil.ConvertMapSpaceToLocalMapSpace(mousePos, capability.PointsInExplosionRange(mousePos));
                    }
                }
                else
                {
                    CurrentProposedAttackExplosionPlacement.OffsetPoints.Clear();
                }

            }
            else
            {
                CurrentProposedAttackExplosionPlacement.OffsetPoints = new List<Point>();
            }
        }

        private static Point CalculateEndpointOfSkillshot(Point position, AttackCapability capability, Direction direction)
        {
            var pointsInLine = MathUtil.LineInDirection(position, direction, capability.Range);
            var numberOfThingsCanPierce = capability.NumberOfTargetsToPierce;
            var numberOfThingsPierced = 0;

            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var grid = level.Grid;
            var previouslyTraversedPoint = position;
            foreach (var point in pointsInLine)
            {
                numberOfThingsPierced += grid[point].EntitiesInPosition.Count;
                if (numberOfThingsPierced == numberOfThingsCanPierce)
                {
                    return point;
                }
                if (grid[point].TileType != TileType.Floor)
                {
                    return point;
                }
                previouslyTraversedPoint = point;
            }
            return pointsInLine[pointsInLine.Count - 1];
        }

        public void Process()
        {

            bool isAcceptingClickInput = !Context.UIController.EscapeMenu.isActiveAndEnabled &&
                !Context.UIController.LootWindow.isActiveAndEnabled &&
                !Context.UIController.InventoryWindow.isActiveAndEnabled;

            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;

            var mousePos = MathUtil.GetMousePositionOnMap(Camera.main);
            var hoverIsValidPoint = level.BoundingBox.Contains(mousePos);

            var playerAttackCapabilities = new AttackCapabilities(player);

            Context.UIController.TileHoverHint.ShowFor(mousePos);

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
                    if (entity.Behaviour != null && (entity.Behaviour.ActingTeam == Team.Enemy || entity.Behaviour.ActingTeam == Team.EnemyOfAll))
                    {
                        tileContainsEnemy = true;
                    }
                    if (entity.IsCombatant && entity.Behaviour != null && entity.Behaviour.ActingTeam == Team.PLAYER && !entity.IsPlayer)
                    {
                        hoverContainsAlly = true;
                    }
                }
            }

            var isHoveringOnEnemyCombatant = hoverIsValidPoint && mousePos != player.Position && hoverContainsCombatant && tileContainsEnemy;
            var isHoveringOnAlly = hoverIsValidPoint && mousePos != player.Position && hoverContainsAlly;
            AttackCapability meleeAttackCapability = playerAttackCapabilities[CombatContext.Melee];
            var isAbleToHitHoveringEnemyCombatant = isHoveringOnEnemyCombatant && meleeAttackCapability.CanPerform && meleeAttackCapability.IsInRange(mousePos);
            var isAbleToSwapWithHoveringAlly = isHoveringOnAlly && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);
            var hoverDirection = MathUtil.RelativeDirection(player.Position, mousePos);

            Context.OverlaySystem.SetActivated(MouseHoverOverlay, isAcceptingClickInput && !isAiming);
            Context.OverlaySystem.SetActivated(AimingOverlay, isAiming);
            MouseHoverOverlayConfig.DefaultColor = isHoveringOnEnemyCombatant ? EnemyHoverColor : DefaultHoverColor;
            MouseHoverOverlayConfig.Position = mousePos;
            PathOverlayConfig.Position = mousePos;

            if (player.Body.IsDead)
            {
                Context.UIController.DeathNotification.Show();
                return;
            }

            if (isAiming)
            {
                // It is important you use the AIMING attack capabilities
                // otherwise you will throw nothing, or whatever is equipped when throwing 
                // from inventory.
                var aimingAttackCapability = AimingAttackCapabilities[AimingCombatContext];
                GenerateAttackOverlayOffsets(mousePos, player.Position, aimingAttackCapability, hoverDirection);
                if (Input.GetMouseButtonDown(0))
                {
                    if (aimingAttackCapability.CanPerform && aimingAttackCapability.IsInRange(mousePos))
                    {
                        var direction = MathUtil.RelativeDirection(player.Position, mousePos);
                        Attack attack = new Attack(AimingAttackCapabilities, AimingCombatContext, direction, mousePos);
                        player.Behaviour.NextAction = attack;
                    }
                    StopAiming();
                }
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    StopAiming();
                }
                return;
            }

            if (ActionList.Count > 0 && player.Behaviour.NextAction == null && Context.FlowSystem.CurrentlyActingTeam == Team.PLAYER)
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
                            if (occupant.Behaviour != null && occupant.Behaviour.ActingTeam == Team.PLAYER && !occupant.IsPlayer && occupant.BlocksPathing)
                            {
                                adjacentFriendlyBlocker = occupant;
                                break;
                            }
                        }
                        if (adjacentFriendlyBlocker != null)
                        {
                            // Move is blocked by a non ghostly friendly, we can swap instead of move.
                            // we will just create a swap because the move has already been dequeued
                            var swap = new SwapPositionsWithAlly
                            {
                                Source = player
                            };
                            swap.Targets.Add(adjacentFriendlyBlocker);
                            nextAction = swap;
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

            if (Input.GetKeyDown(KeyCode.A))
            {
                StartAiming(playerAttackCapabilities, CombatContext.Melee);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartAiming(playerAttackCapabilities, CombatContext.Ranged);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                StartAiming(playerAttackCapabilities, CombatContext.Thrown);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartAiming(playerAttackCapabilities, CombatContext.Zapped);
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
                Context.UIController.Tooltip.Hide();
            }

            if (Context.UIController.InventoryWindow.isActiveAndEnabled)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                QueueWait(level, player);
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Context.UIController.LootWindow.isActiveAndEnabled)
                {
                    Context.UIController.LootWindow.Hide();
                }
                else
                {
                    var entitiesInPosition = level.Grid[player.Position].EntitiesInPosition;
                    var lootableEntities = entitiesInPosition.FindAll(CombatUtil.LootableEntities);

                    Entity triggerEntity = null;
                    Effect triggerEffect = null;
                    foreach (var entity in entitiesInPosition)
                    {
                        if (entity.Trigger != null)
                        {
                            foreach (var effect in entity.Trigger.Effects)
                            {
                                if (effect.CanTriggerOnPress())
                                {
                                    triggerEntity = entity;
                                    triggerEffect = effect;
                                    break;
                                }
                            }
                            if (triggerEntity == null || triggerEffect == null)
                            {
                                break;
                            }
                        }
                    }

                    if (triggerEntity != null && triggerEffect != null)
                    {
                        QueueTriggerEffect(triggerEntity, triggerEffect, player);
                    }

                    else if (lootableEntities.Count > 0)
                    {
                        Context.UIController.LootWindow.ShowFor(lootableEntities);
                    }
                    Context.UIController.InputHint.Hide();
                }
            }

            if (!isAcceptingClickInput)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                ActionList.Clear();
                if (isAbleToSwapWithHoveringAlly)
                {
                    QueueSwapPosition(level, player, mousePos);
                }
                else if (isAbleToHitHoveringEnemyCombatant)
                {
                    QueueAttack(player, mousePos, playerAttackCapabilities, CombatContext.Melee);
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

        public void StartAiming(AttackCapabilities attackCapabilities, CombatContext combatContext)
        {
            AimingAttackCapabilities = attackCapabilities;
            var attackCapability = attackCapabilities[combatContext];
            if (!isAiming && attackCapability.CanPerform)
            {
                isAiming = true;
                AimingCombatContext = combatContext;
                Context.UIController.InputHint.ShowText(("player.controller.select.direction." + combatContext.ToString().ToLower()).Localize());
            }
            else if (combatContext == CombatContext.Ranged && attackCapability.MainHand != null && (attackCapability.Ammo == null || attackCapability.Ammo.NumberOfItems <= 0))
            {
                var outOfAmmoString = "player.controller.cannot.do.out.of.ammo".Localize();
                Context.UIController.InputHint.ShowText(outOfAmmoString);
            }
            else if (attackCapability.MainHand != null)
            {
                var cannotPerformString = ("player.controller.cannot.do." + combatContext.ToString().ToLower()).Localize();
                Context.UIController.InputHint.ShowText(string.Format(cannotPerformString, attackCapability.MainHand.DisplayName));
            }
            else
            {
                Context.UIController.InputHint.ShowText("player.controller.not.holding".Localize());
            }
        }

        public void StopAiming()
        {
            if (isAiming)
            {
                AimingAttackCapabilities = null;
                isAiming = false;
                Context.UIController.InputHint.Hide();
            }
        }

        private void QueueTriggerEffect(Entity triggerEntity, Effect triggerEffect, Entity player)
        {
            ActionOutcome outcome = new ActionOutcome();
            outcome.Source = triggerEntity;
            outcome.Target = player;
            triggerEffect.TriggerOnPress(outcome);
        }

        private void QueueWait(Level level, Entity player)
        {
            var wait = new Wait
            {
                Source = player
            };
            player.Behaviour.NextAction = wait;
        }

        private void QueueSwapPosition(Level level, Entity player, Point mousePos)
        {
            var swapPositions = new SwapPositionsWithAlly
            {
                Source = player
            };
            swapPositions.Targets.Add(level.Grid[mousePos].EntitiesInPosition.Find((x) => { return x.Behaviour != null; }));
            player.Behaviour.NextAction = swapPositions;
        }

        private void QueueAttack(Entity player, Point mousePos, AttackCapabilities attackCapabilities, CombatContext combatContext)
        {
            var direction = MathUtil.RelativeDirection(player.Position, mousePos);
            Attack attack = new Attack(attackCapabilities, combatContext, direction, mousePos);
            player.Behaviour.NextAction = attack;
        }

        private void OnPathComplete(Path path)
        {
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            foreach (var node in path.Nodes)
            {
                var move = new Move
                {
                    Source = player,
                    TargetPosition = new Point(node.Position.X, node.Position.Y)
                };
                ActionList.Enqueue(move);
            }
            waitingForPath = false;
        }
    }
}
