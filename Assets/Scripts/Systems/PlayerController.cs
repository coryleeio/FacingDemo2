using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class PlayerController
    {
        public ApplicationContext Context { get; set; }
        private Overlay MouseHoverOverlay;
        private OverlayConfig MouseHoverOverlayConfig;
        private bool waitingForPath = false;

        private Color DefaultHoverColor = new Color(0, 213, 255);
        private Color EnemyHoverColor = Color.red;

        public void Init()
        {
            MouseHoverOverlayConfig = new OverlayConfig
            {
                Name = "MouseHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { new Point(0, 0) },
                DefaultColor = DefaultHoverColor,
                RelativeSortOrder = 0,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Overlay/Square"),
            };
            MouseHoverOverlay = new Overlay()
            {
                Configs = new List<OverlayConfig>()
                {
                    MouseHoverOverlayConfig,
                }
            };
        }

        public void Process()
        {
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            if (!waitingForPath && player.ActionQueue.Count == 0 && game.IsPlayerTurn)
            {
                Context.OverlaySystem.Activate(MouseHoverOverlay);
                var mousePos = MathUtil.GetMousePositionOnMap(Camera.main);

                MouseHoverOverlayConfig.Position = mousePos;

                if (level.BoundingBox.Contains(mousePos) && mousePos != player.Position && level.TokenGrid[mousePos].Count > 0 && level.TokenGrid[mousePos][0].IsCombatant)
                {
                    MouseHoverOverlayConfig.DefaultColor = EnemyHoverColor;
                    if (Input.GetMouseButtonDown(0))
                    {
                        if(player.Position.IsOrthogonalTo(mousePos))
                        {
                            var attack = Context.PrototypeFactory.BuildTokenAction<Attack>(player);
                            attack.TargetTokenId = level.TokenGrid[mousePos][0].Id;
                            player.ActionQueue.Enqueue(attack);
                            var endTurn = Context.PrototypeFactory.BuildTokenAction<EndTurn>(player);
                            player.ActionQueue.Enqueue(endTurn);
                        }
                        else
                        {
                            Context.PathFinder.StartPath(player.Position, mousePos, Context.GameStateManager.Game.CurrentLevel.TilesetGrid, OnPathComplete);
                            waitingForPath = true;
                        }
                    }
                }
                else
                {
                    MouseHoverOverlayConfig.DefaultColor = DefaultHoverColor;
                    if (Input.GetMouseButtonDown(0))
                    {
                        Context.OverlaySystem.Deactivate(MouseHoverOverlay);
                        Context.PathFinder.StartPath(player.Position, mousePos, Context.GameStateManager.Game.CurrentLevel.TilesetGrid, OnPathComplete);
                        waitingForPath = true;
                    }
                }
            }
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
