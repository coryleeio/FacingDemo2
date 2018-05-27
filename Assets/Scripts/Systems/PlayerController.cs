using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class PlayerController
    {
        public ApplicationContext Context { get; set; }
        private Overlay MouseHoverOverlay;
        private OverlayConfig MouseHoverOverlayConfig;
        private OverlayConfig PathOverlayConfig;
        private bool waitingForPath = false;

        private Color DefaultHoverColor = new Color(0, 213, 255);
        private Color EnemyHoverColor = Color.red;
        private Path CurrentPath;

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
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            var mousePos = MathUtil.GetMousePositionOnMap(Camera.main);
            var isValidPoint = level.BoundingBox.Contains(mousePos);
            var isHoveringOnEnemyCombatant = isValidPoint && mousePos != player.Position && level.TokenGrid[mousePos].Count > 0 && level.TokenGrid[mousePos][0].IsCombatant;
            var isAbleToHitHoveringEnemyCombatant = isHoveringOnEnemyCombatant && player.Position.IsOrthogonalTo(mousePos) && player.Position.IsAdjacentTo(mousePos);
            Context.OverlaySystem.SetActivated(MouseHoverOverlay, true);
            MouseHoverOverlayConfig.DefaultColor = isHoveringOnEnemyCombatant ? EnemyHoverColor : DefaultHoverColor;
            MouseHoverOverlayConfig.Position = mousePos;
            PathOverlayConfig.Position = mousePos;

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
                    var surroundingPositions = MathUtil.OrthogonalPoints(mousePos).FindAll((p) => { return level.TilesetGrid[p].Walkable; });

                    if (surroundingPositions.Count > 0)
                    {
                        surroundingPositions.Sort(delegate (Point p1, Point p2)
                        {
                            return Point.Distance(player.Position, p1).CompareTo(Point.Distance(player.Position, p2));
                        });
                        waitingForPath = true;
                        Context.PathFinder.StartPath(player.Position, surroundingPositions[0], level.TilesetGrid, (path) =>
                        {
                            CurrentPath = path;
                            waitingForPath = false;
                        });
                    }
                }
                else
                {
                    waitingForPath = true;
                    Context.PathFinder.StartPath(player.Position, mousePos, level.TilesetGrid, (path) =>
                    {
                        CurrentPath = path;
                        waitingForPath = false;
                    });
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                player.ActionQueue.Clear();
                if (isHoveringOnEnemyCombatant && isAbleToHitHoveringEnemyCombatant)
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

        private void QueueAttack(Level level, Token player, Point mousePos)
        {
            var attack = Context.PrototypeFactory.BuildTokenAction<Attack>(player);
            attack.TargetTokenId = level.TokenGrid[mousePos][0].Id;
            player.ActionQueue.Enqueue(attack);
            var endTurn = Context.PrototypeFactory.BuildTokenAction<EndTurn>(player);
            player.ActionQueue.Enqueue(endTurn);
        }

        private void OnPathComplete(Path path)
        {
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            foreach (var node in path.Nodes)
            {
                var move = Context.PrototypeFactory.BuildTokenAction<Move>(player);
                move.TargetLocation = new Point(node.Position.X, node.Position.Y);
                player.ActionQueue.Enqueue(move);

                var endTurn = Context.PrototypeFactory.BuildTokenAction<EndTurn>(player);
                player.ActionQueue.Enqueue(endTurn);
            }
            waitingForPath = false;
        }
    }
}
