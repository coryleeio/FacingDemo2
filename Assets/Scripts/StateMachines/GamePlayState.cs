using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState<Root>
    {
        public GameScene GamePlayScene { get; set; }
        public ILogSystem LogSystem { get; set; }
        public IVisibilitySystem VisibilitySystem { get; set; }
        public IOverlaySystem OverlaySystem { get; set; }
        public ISpriteSortingSystem SpriteSortingSystem { get; set; }
        public IPathFinder PathFinder { get; set; }
        public IGameStateSystem GameStateSystem { get; set; }
        public IMessageBusSystem MessageBusSystem { get; set; }

        private GameSceneCameraDriver CameraDriver;

        public GamePlayState()
        {

        }

        public void Enter(Root owner)
        {
            SpriteSortingSystem.Init();
            GamePlayScene.Load();
            VisibilitySystem.Init();
            OverlaySystem.Init(GameStateSystem.Game.CurrentLevel.TilesetGrid.GetLength(0), GameStateSystem.Game.CurrentLevel.TilesetGrid.GetLength(1));
            PathFinder.Init(GameStateSystem.Game.CurrentLevel.Domain.Width, GameStateSystem.Game.CurrentLevel.Domain.Height, GridGraph.DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
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

        public void Exit(Root owner)
        {
            OverlaySystem.Clear();
            VisibilitySystem.Clear();
            GamePlayScene.Unload();
            PathFinder.Cleanup();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
            OverlaySystem.Process();
            SpriteSortingSystem.Sort();
            PathFinder.Process();
            CameraDriver.MoveCamera();
            MessageBusSystem.Process();
        }
    }
}
