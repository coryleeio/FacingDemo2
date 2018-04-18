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
            _overlaySystem.Activate(new Overlay
            {
                Name = "MouseHover",
                Shape = new Shape(ShapeType.Rect, 5, 1),
                DefaultColor = new Color(0, 213, 255),
                PositionFollowsMouse = false,
                RotationFollowsMouse = true,
            });
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
