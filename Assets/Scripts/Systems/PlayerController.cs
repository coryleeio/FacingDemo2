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

        public void Init()
        {
            MouseHoverOverlayConfig = new OverlayConfig
            {
                Name = "MouseHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { new Point(0, 0) },
                DefaultColor = new Color(0, 213, 255),
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
                MouseHoverOverlayConfig.Position = MathUtil.GetMousePositionOnMap(Camera.main);
                if (Input.GetMouseButtonDown(0))
                {
                    Context.OverlaySystem.Deactivate(MouseHoverOverlay);
                    Context.PathFinder.StartPath(player.Position, MathUtil.GetMousePositionOnMap(Camera.main), Context.GameStateManager.Game.CurrentLevel.TilesetGrid, OnPathComplete);
                    waitingForPath = true;
                }
            }
        }

        private void OnPathComplete(Path path)
        {
            var game = Context.GameStateManager.Game;
            var level = game.CurrentLevel;
            var player = level.Player;
            foreach(var node in path.Nodes)
            {
                var move = Context.PrototypeFactory.BuildTokenAction<Move>(player);
                move.TargetLocation = new Point(node.Position.X, node.Position.Y);

                var endTurn = Context.PrototypeFactory.BuildTokenAction<EndTurn>(player);

                player.ActionQueue.Enqueue(move);
                player.ActionQueue.Enqueue(endTurn);
            }
            waitingForPath = false;
        }
    }
}
