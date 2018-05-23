using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState
    {
        public GameScene GamePlayScene { get; set; }
        public Logger LogSystem { get; set; }
        public VisibilitySystem VisibilitySystem { get; set; }
        public OverlaySystem OverlaySystem { get; set; }
        public SpriteSortingSystem SpriteSortingSystem { get; set; }
        public PathFinder PathFinder { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public MovementSystem MovementSystem { get; set; }
        public FlowSystem TurnSystem { get; set; }
        private GameSceneCameraDriver CameraDriver;
        private OverlayConfig MouseOverlay { get; set; }

        public void Enter()
        {
            SpriteSortingSystem.Init();
            GamePlayScene.Load();
            VisibilitySystem.Init();
            OverlaySystem.Init(GameStateManager.Game.CurrentLevel.TilesetGrid.Width, GameStateManager.Game.CurrentLevel.TilesetGrid.Height);
            PathFinder.Init(DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
            CameraDriver = GamePlayScene.GetCamera();
            VisibilitySystem.UpdateVisibility();
            MouseOverlay = new OverlayConfig
            {
                Name = "MouseHover",
                Position = new Point(0, 0),
                OffsetPoints = new List<Point>() { new Point(0,0)},
                DefaultColor = new Color(0, 213, 255),
                RelativeSortOrder = 0,
                WalkableTilesOnly = true,
                ConstrainToLevel = true,
                Sprite = Resources.Load<Sprite>("Overlay/Square"),
            };
            var newOverlay = new Overlay()
            {
                Configs = new System.Collections.Generic.List<OverlayConfig>()
                {
                    MouseOverlay
                }
            };
            OverlaySystem.Activate(newOverlay);

            MovementSystem.FollowPath(GameStateManager.Game.CurrentLevel.Player, new List<Point> { new Point(0, 0), new Point(39, 39) });
        }

        public void Process()
        {
            MouseOverlay.Position = MathUtil.GetMousePositionOnMap(Camera.main);
            OverlaySystem.Process();
            SpriteSortingSystem.Process();
            PathFinder.Process();
            TurnSystem.Process();
            MovementSystem.Process();
            CameraDriver.MoveCamera();
        }

        public void Exit()
        {
            OverlaySystem.Clear();
            VisibilitySystem.Clear();
            GamePlayScene.Unload();
            PathFinder.Cleanup();
        }
    }
}
