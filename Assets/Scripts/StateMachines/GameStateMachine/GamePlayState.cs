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

        private GameSceneCameraDriver CameraDriver;

        public GamePlayState()
        {

        }

        public void Enter()
        {
            SpriteSortingSystem.Init();
            GamePlayScene.Load();
            VisibilitySystem.Init();
            OverlaySystem.Init(GameStateManager.Game.CurrentLevel.TilesetGrid.SizeX, GameStateManager.Game.CurrentLevel.TilesetGrid.SizeY);
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
        }

        public void Process()
        {
            OverlaySystem.Process();
            SpriteSortingSystem.Sort();
            PathFinder.Process();
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
