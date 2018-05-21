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
        public TurnSystem TurnSystem { get; set; }
        private GameSceneCameraDriver CameraDriver;

        public void Enter()
        {
            SpriteSortingSystem.Init();
            GamePlayScene.Load();
            VisibilitySystem.Init();
            OverlaySystem.Init(GameStateManager.Game.CurrentLevel.TilesetGrid.Width, GameStateManager.Game.CurrentLevel.TilesetGrid.Height);
            PathFinder.Init(DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
            CameraDriver = GamePlayScene.GetCamera();
            VisibilitySystem.UpdateVisibility();

            var newOverlay = new Overlay()
            {
                Configs = new System.Collections.Generic.List<OverlayConfig>()
                {
                    new OverlayConfig
                    {
                        Name = "MouseHover",
                        Shape = new Shape(ShapeType.Rect, 1, 1),
                        DefaultColor = new Color(0, 213, 255),
                        OverlayBehaviour = OverlayBehaviour.PositionFollowsCursor,
                        RelativeSortOrder = 0,
                        WalkableTilesOnly = true,
                        ConstrainToLevel = true
                    },
                }
            };
            OverlaySystem.Activate(newOverlay);

            MovementSystem.FollowPath(GameStateManager.Game.CurrentLevel.Player, new List<Point> { new Point(0, 0), new Point(39, 39) });
        }

        public void Process()
        {
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
