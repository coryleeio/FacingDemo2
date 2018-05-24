using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState
    {
        public ApplicationContext ApplicationContext { get; set; }
        private GameSceneCameraDriver CameraDriver;
        private OverlayConfig MouseOverlay { get; set; }

        public void Enter()
        {
            ApplicationContext.SpriteSortingSystem.Init();
            ApplicationContext.GameScene.Load();
            ApplicationContext.VisibilitySystem.Init();
            ApplicationContext.OverlaySystem.Init(ApplicationContext.GameStateManager.Game.CurrentLevel.TilesetGrid.Width, ApplicationContext.GameStateManager.Game.CurrentLevel.TilesetGrid.Height);
            ApplicationContext.PathFinder.Init(DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
            CameraDriver = ApplicationContext.GameScene.GetCamera();
            ApplicationContext.VisibilitySystem.UpdateVisibility();
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
                Configs = new List<OverlayConfig>()
                {
                    MouseOverlay
                }
            };
            ApplicationContext.OverlaySystem.Activate(newOverlay);
            ApplicationContext.MovementSystem.FollowPath(ApplicationContext.GameStateManager.Game.CurrentLevel.Player, new List<Point> { new Point(0, 0), new Point(39, 39) });
        }

        public void Process()
        {
            MouseOverlay.Position = MathUtil.GetMousePositionOnMap(Camera.main);
            ApplicationContext.OverlaySystem.Process();
            ApplicationContext.SpriteSortingSystem.Process();
            ApplicationContext.PathFinder.Process();
            ApplicationContext.FlowSystem.Process();
            ApplicationContext.MovementSystem.Process();
            CameraDriver.MoveCamera();
        }

        public void Exit()
        {
            ApplicationContext.OverlaySystem.Clear();
            ApplicationContext.VisibilitySystem.Clear();
            ApplicationContext.GameScene.Unload();
            ApplicationContext.PathFinder.Cleanup();
        }
    }
}
