using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

        // Use the capabilities that were established when we started aiming
        // this is important for throwing from inventory, otherwise they will get recalculated in the 
        // button down section of this and cause the primary weapon to be thrown if one exists
        // or it will cause you not to be able to throw if one does not.
        private CombatActionType AimedInteractionType;
        private Item AimedItem;
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
                Sprite = Resources.Load<Sprite>("SpritesManual/Square"),
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
                Sprite = Resources.Load<Sprite>("SpritesManual/Dot"),
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
                Sprite = Resources.Load<Sprite>("SpritesManual/Square"),
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
                Sprite = Resources.Load<Sprite>("SpritesManual/Square"),
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
                Sprite = Resources.Load<Sprite>("SpritesManual/AoeIndicator"),
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

        public void GenerateAttackOverlayOffsets(Entity entity, CombatActionType InteractionType, Item item, Point mousePos)
        {
            var direction = MathUtil.RelativeDirection(entity.Position, mousePos);
            var position = entity.Position;
            var InteractionTypeParameters = CombatUtil.CombatActionParametersResolve(entity, InteractionType, item);
            var grid = Context.Game.CurrentLevel.Grid;
            if (InteractionTypeParameters.CombatActionParameters.TargetingType == CombatActionTargetingType.Line && (direction == Direction.SouthEast || direction == Direction.SouthWest || direction == Direction.NorthEast || direction == Direction.NorthWest))
            {
                var lineOffsets = MathUtil.LineInDirection(position, direction, InteractionTypeParameters.CombatActionParameters.Range);
                CurrentProposedAttackPlacement.OffsetPoints = lineOffsets;
            }
            else if (InteractionTypeParameters.CombatActionParameters.TargetingType == CombatActionTargetingType.SelectTarget)
            {
                CurrentProposedAttackPlacement.OffsetPoints = new List<Point>()
                {
                    mousePos,
                };
            }

            PossibleAttackPositions.OffsetPoints = CombatUtil.PointsInRange(grid, position, InteractionTypeParameters.CombatActionParameters.Range, InteractionTypeParameters.CombatActionParameters.TargetingType);

            if (InteractionTypeParameters != null && InteractionTypeParameters.ExplosionParameters != null)
            {

                if (PossibleAttackPositions.OffsetPoints.Contains(mousePos))
                {
                    var explosionParameters = InteractionTypeParameters.ExplosionParameters;

                    if (InteractionTypeParameters.CombatActionParameters.TargetingType == CombatActionTargetingType.Line)
                    {
                        var endPoint = CombatUtil.CalculateEndpointOfLineSkillshot(position, InteractionTypeParameters.CombatActionParameters, direction);
                        CurrentProposedAttackExplosionPlacement.Position = endPoint;
                        CurrentProposedAttackExplosionPlacement.OffsetPoints = MathUtil.ConvertMapSpaceToLocalMapSpace(endPoint, CombatUtil.PointsInExplosionRange(explosionParameters, endPoint));
                    }
                    else if (InteractionTypeParameters.CombatActionParameters.TargetingType == CombatActionTargetingType.SelectTarget)
                    {
                        CurrentProposedAttackExplosionPlacement.Position = mousePos;
                        CurrentProposedAttackExplosionPlacement.OffsetPoints = MathUtil.ConvertMapSpaceToLocalMapSpace(mousePos, CombatUtil.PointsInExplosionRange(explosionParameters, mousePos));
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

        public void Process()
        {
            if(Context.UIController == null || Context.UIController.EscapeMenu == null)
            {
                // bail out if we are changing scenes
                return;
            }
            bool isAcceptingClickInput = !Context.UIController.EscapeMenu.isActiveAndEnabled &&
                !Context.UIController.LootWindow.isActiveAndEnabled &&
                !Context.UIController.InventoryWindow.isActiveAndEnabled &&
                !Context.UIController.DialogController.isActiveAndEnabled;

            var game = Context.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            var item = InventoryUtil.GetMainHandOrDefaultWeapon(player);
            var meleeCombatActionParameters = CombatUtil.CombatActionParametersResolve(player, CombatActionType.Melee, item);

            var mousePos = MathUtil.GetMousePositionOnMap(Camera.main);
            var hoverIsValidPoint = level.BoundingBox.Contains(mousePos);

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
                    if (entity.IsCombatant && (entity.ActingTeam == Team.Enemy || entity.ActingTeam == Team.EnemyOfAll))
                    {
                        tileContainsEnemy = true;
                    }
                    if (entity.IsCombatant && entity.ActingTeam == Team.Player && !entity.IsPlayer)
                    {
                        hoverContainsAlly = true;
                    }
                }
            }

            var isHoveringOnEnemyCombatant = hoverIsValidPoint && mousePos != player.Position && hoverContainsCombatant && tileContainsEnemy;
            var isHoveringOnAlly = hoverIsValidPoint && mousePos != player.Position && hoverContainsAlly;

            var isAbleToHitHoveringEnemyCombatant = isHoveringOnEnemyCombatant &&
                CombatUtil.CanAttackWithItem(player, CombatActionType.Melee, item) &&
                CombatUtil.InRangeOfAttack(level.Grid, player.Position, meleeCombatActionParameters.CombatActionParameters.Range, meleeCombatActionParameters.CombatActionParameters.TargetingType, mousePos);
            var isAbleToSwapWithHoveringAlly = isHoveringOnAlly && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);

            Context.OverlaySystem.SetActivated(MouseHoverOverlay, isAcceptingClickInput && !isAiming);
            Context.OverlaySystem.SetActivated(AimingOverlay, isAiming);
            MouseHoverOverlayConfig.DefaultColor = isHoveringOnEnemyCombatant ? EnemyHoverColor : DefaultHoverColor;
            MouseHoverOverlayConfig.Position = mousePos;
            PathOverlayConfig.Position = mousePos;

            if (player.IsDead)
            {
                Context.UIController.DeathNotification.Show();
                return;
            }

            if (isAiming)
            {
                // It is important you use the AIMING attack capabilities
                // otherwise you will throw nothing, or whatever is equipped when throwing 
                // from inventory.
                GenerateAttackOverlayOffsets(player, AimedInteractionType, AimedItem, mousePos);
                if (Input.GetMouseButtonDown(0))
                {
                    var aimingCombatActionParameters = CombatUtil.CombatActionParametersResolve(player, AimedInteractionType, AimedItem);
                    if (CombatUtil.CanAttackWithItem(player, AimedInteractionType, AimedItem) &&
                        CombatUtil.InRangeOfAttack(level.Grid, player.Position,
                        aimingCombatActionParameters.CombatActionParameters.Range,
                        aimingCombatActionParameters.CombatActionParameters.TargetingType,
                        mousePos))
                    {
                        var calculatedAttack = CombatUtil.CalculateAttack(level.Grid, player, AimedInteractionType, AimedItem, mousePos, aimingCombatActionParameters);
                        CombatAction attack = new CombatAction(calculatedAttack);
                        player.NextAction = attack;
                    }
                    StopAiming();
                }
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
                {
                    StopAiming();
                }
                return;
            }

            if (ActionList.Count > 0 && player.NextAction == null && Context.FlowSystem.CurrentlyActingTeam == Team.Player)
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
                            if (occupant.IsCombatant && occupant.ActingTeam == Team.Player && !occupant.IsPlayer && occupant.BlocksPathing)
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

                player.NextAction = nextAction;
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

                    // If I am hovering over an enemy that is far away, enqueue a path
                    // to move to the closest square adjacent to that enemy.
                    if (surroundingPositions.Count > 0)
                    {
                        surroundingPositions.Sort(delegate (Point p1, Point p2)
                        {
                            return Point.Distance(player.Position, p1).CompareTo(Point.Distance(player.Position, p2));
                        });
                        waitingForPath = true;
                        StartPathPlayerController(level, player.Position, surroundingPositions[0]);
                    }
                }
                else
                {
                    waitingForPath = true;

                    if (player.NextAction != null && player.NextAction.GetType() == typeof(Move))
                    {
                        // If the player is currently moving into a target tile, Enqueue a path from there to their mouse hover
                        StartPathPlayerController(level, ((Move)player.NextAction).TargetPosition, mousePos);
                    }
                    else
                    {
                        // Enqueue a path from where the player is to their mouse hover.
                        StartPathPlayerController(level, player.Position, mousePos);
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                StartAiming(CombatActionType.Melee, item);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                StartAiming(CombatActionType.Ranged, item);
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                StartAiming(CombatActionType.Thrown, item);
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartAiming(CombatActionType.Zapped, item);
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
                    var grid = level.Grid;
                    var entities = level.Entitys;
                    var entitiesInPositionOfMove = grid[player.Position];
                    var pressTriggers = entities.FindAll(Predicates.PressTriggers);
                    foreach (var trigger in pressTriggers)
                    {
                        if (Trigger.EntityInTrigger(player, trigger))
                        {
                            if (trigger.Trigger.CanPerform(player, trigger))
                            {
                                var action = trigger.Trigger.TriggerAction;
                                action.Perform(trigger, player, trigger.Trigger.Data);
                                Context.UIController.InputHint.Hide();
                            }
                        }
                    }

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
                    QueueAttack(player, CombatActionType.Melee, item, mousePos);
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

        private void StartPathPlayerController(Level level, Point from, Point to)
        {
            // We always use the level.GridWithoutPlayerUnits in the player controller for finding paths
            // because we want to path through our friendly units and automatically swap places with them instead of having to get directly adjacent
            // and click on them
            CurrentPath = Context.PathFinder.FindPath(from, to, level.GridWithoutPlayerUnits);
            waitingForPath = false;
        }

        public void StartAiming(CombatActionType InteractionType, Item item)
        {
            AimedInteractionType = InteractionType;
            AimedItem = item;
            var player = Context.Game.CurrentLevel.Player;
            if (player == null)
            {
                return;
            }
            Assert.AreNotEqual(CombatActionType.NotSet, InteractionType);
            var ammo = CombatUtil.AmmoResolve(player, InteractionType, item);
            if (!isAiming && CombatUtil.CanAttackWithItem(player, InteractionType, item))
            {
                isAiming = true;
                Context.UIController.InputHint.ShowText(("player.controller.select.direction." + InteractionType.ToString().ToLower()).Localize());
            }
            else if (InteractionType == CombatActionType.Ranged && item != null && (ammo == null || ammo.NumberOfItems <= 0))
            {
                var outOfAmmoString = "player.controller.cannot.do.out.of.ammo".Localize();
                Context.UIController.InputHint.ShowText(outOfAmmoString);
            }
            else if (item != null)
            {
                var cannotPerformString = ("player.controller.cannot.do." + InteractionType.ToString().ToLower()).Localize();
                Context.UIController.InputHint.ShowText(string.Format(cannotPerformString, item.Name.Localize()));
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
                AimedInteractionType = CombatActionType.NotSet;
                AimedItem = null;
                isAiming = false;
                Context.UIController.InputHint.Hide();
            }
        }

        private void QueueWait(Level level, Entity player)
        {
            var wait = new Wait
            {
                Source = player
            };
            player.NextAction = wait;
        }

        private void QueueSwapPosition(Level level, Entity player, Point mousePos)
        {
            var swapPositions = new SwapPositionsWithAlly
            {
                Source = player
            };
            swapPositions.Targets.Add(level.Grid[mousePos].EntitiesInPosition.Find((x) => { return x.IsCombatant; }));
            player.NextAction = swapPositions;
        }

        private void QueueAttack(Entity player, CombatActionType InteractionType, Item item, Point mousePos)
        {
            var grid = Context.Game.CurrentLevel.Grid;
            var InteractionTypeParameters = CombatUtil.CombatActionParametersResolve(player, InteractionType, item);
            var calculatedAttack = CombatUtil.CalculateAttack(grid, player, InteractionType, item, mousePos, InteractionTypeParameters);
            CombatAction attack = new CombatAction(calculatedAttack);
            player.NextAction = attack;
        }

        private void OnPathComplete(Path path)
        {
            var game = Context.Game;
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
