using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState<Root>
    {
        private GameScene _gamePlayScene;
        private ILogSystem _logSystem;
        private IVisibilitySystem _visibilitySystem;
        private IOverlaySystem _overlaySystem;
        private ISpriteSortingSystem _spriteSortingSystem;
        private IPathFinder _pathFinder;
        private IGameStateSystem _gameStateSystem;

        public GamePlayState(GameScene gamePlayScene, ILogSystem logSystem, IVisibilitySystem visibilitySystem, IOverlaySystem overlaySystem, ISpriteSortingSystem spriteSortingSystem, IPathFinder pathFinder, IGameStateSystem gameStateSystem)
        {
            _gamePlayScene = gamePlayScene;
            _logSystem = logSystem;
            _visibilitySystem = visibilitySystem;
            _overlaySystem = overlaySystem;
            _spriteSortingSystem = spriteSortingSystem;
            _pathFinder = pathFinder;
            _gameStateSystem = gameStateSystem;
        }

        public void Enter(Root owner)
        {
            _spriteSortingSystem.Init();
            _gamePlayScene.Load();
            _visibilitySystem.Init();
            _overlaySystem.Init();
            _pathFinder.Init(_gameStateSystem.Game.CurrentLevel.Domain.Width, _gameStateSystem.Game.CurrentLevel.Domain.Height, GridGraph.DiagonalOptions.DiagonalsWithoutCornerCutting, 5);

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
                    },
                    /*
                    new OverlayConfig
                    {
                        Name = "MouseHover",
                        Shape = new Shape(ShapeType.Rect, 4, 1),
                        DefaultColor = new Color(112, 112, 0),
                        OverlayBehaviour = OverlayBehaviour.StationaryRotationFollowsCursor,
                        OverlayConstraint = OverlayConstraints.ConstraintedToShapeOfPreviousConfig,
                        RelativeSortOrder = 2,
                        SpriteType = OverlaySpriteType.Circle
                    }*/
                }
            };
            _overlaySystem.Activate(newOverlay);
        }

        public void Exit(Root owner)
        {
            _overlaySystem.Clear();
            _visibilitySystem.Clear();
            _gamePlayScene.Unload();
            _pathFinder.Cleanup();
        }

        public void HandleMessage(Message messageToHandle)
        {

        }

        public void Process(Root owner)
        {
            _overlaySystem.Process();
            _spriteSortingSystem.Sort();
            _pathFinder.Process();
        }
    }
}
