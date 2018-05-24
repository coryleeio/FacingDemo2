using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState
    {
        public ApplicationContext Context { get; set; }
        private GameSceneCameraDriver CameraDriver;
        private OverlayConfig MouseOverlay { get; set; }

        public void Enter()
        {
            Context.SpriteSortingSystem.Init();
            Context.GameScene.Load();
            Context.VisibilitySystem.Init();
            Context.OverlaySystem.Init(Context.GameStateManager.Game.CurrentLevel.TilesetGrid.Width, Context.GameStateManager.Game.CurrentLevel.TilesetGrid.Height);
            Context.PathFinder.Init(DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
            Context.FlowSystem.Init();
            CameraDriver = Context.GameScene.GetCamera();
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
            Context.OverlaySystem.Activate(newOverlay);
            Context.MovementSystem.FollowPath(Context.GameStateManager.Game.CurrentLevel.Player, new List<Point> { new Point(0, 0), new Point(39, 39) });
            CameraDriver.JumpToTarget();
        }

        public void Process()
        {
            MouseOverlay.Position = MathUtil.GetMousePositionOnMap(Camera.main);
            Context.OverlaySystem.Process();
            Context.SpriteSortingSystem.Process();
            Context.PathFinder.Process();
            Context.FlowSystem.Process();
            Context.MovementSystem.Process();
            Context.VisibilitySystem.Process();
            CameraDriver.MoveCamera();
        }

        public void Exit()
        {
            Context.OverlaySystem.Clear();
            Context.VisibilitySystem.Clear();
            Context.GameScene.Unload();
            Context.PathFinder.Cleanup();
        }
    }
}
