using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState<Root>
    {
        private GameScene _gamePlayScene;
        private ILogSystem _logSystem;
        private IVisibilitySystem _visibilitySystem;
        private IOverlaySystem _overlaySystem;

        public GamePlayState(GameScene gamePlayScene, ILogSystem logSystem, IVisibilitySystem visibilitySystem, IOverlaySystem overlaySystem)
        {
            _gamePlayScene = gamePlayScene;
            _logSystem = logSystem;
            _visibilitySystem = visibilitySystem;
            _overlaySystem = overlaySystem;
        }

        public void Enter(Root owner)
        {
            _gamePlayScene.Load();
            _visibilitySystem.Init();
            _overlaySystem.Init();

            var newOverlay = new Overlay()
            {
                Configs = new System.Collections.Generic.List<OverlayConfig>()
                {
                    new OverlayConfig
                    {
                        Name = "MouseHover",
                        Shape = new Shape(ShapeType.HollowPlus, 5, 5),
                        DefaultColor = new Color(0, 213, 255),
                        OverlayBehaviour = OverlayBehaviour.StationaryNoRotation,
                        RelativeSortOrder = 0,
                    },
                    new OverlayConfig
                    {
                        Name = "MouseHover",
                        Shape = new Shape(ShapeType.Rect, 4, 1),
                        DefaultColor = new Color(112, 112, 0),
                        OverlayBehaviour = OverlayBehaviour.StationaryRotationFollowsCursor,
                        OverlayConstraint = OverlayConstraints.ConstraintedToShapeOfPreviousConfig,
                        RelativeSortOrder = 2,
                        SpriteType = OverlaySpriteType.Circle
                    }
                }
            };
            //_overlaySystem.Activate(newOverlay);
        }

        public void Exit(Root owner)
        {
            _overlaySystem.Clear();
            _visibilitySystem.Clear();
            _gamePlayScene.Unload();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
            _overlaySystem.Process();
        }
    }
}
