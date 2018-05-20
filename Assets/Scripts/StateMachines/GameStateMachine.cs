using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GameStateMachine : StateMachine
    {
        public GamePlayState GamePlayState { get; set; }
        public MainMenuState MainMenuState { get; set; }
        public LoadingResourcesState LoadingResourcesState { get; set; }
    }

    public class LoadingResourcesState : IStateMachineState
    {
        public LoadingScene LoadingScene { get; set; }
        public Logger Logger { get; set; }
        public ResourceManager ResourceManager { get; set; }
        public PrototypeFactory PrototypeFactory { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public DungeonGenerator DungeonGenerator { get; set; }
        public Root Root { get; set; }

        public LoadingResourcesState(
        )
        {
        }

        public void Enter()
        {
            LoadingScene.Load();
            Root.StartCoroutine(LoadPrototypes(Root));
        }

        IEnumerator LoadPrototypes(Root owner)
        {
            ResourceManager.LoadAllPrototypes();
            if (GameStateManager.Game == null)
            {
                GameStateManager.NewGame();
                DungeonGenerator.GenerateDungeon();
            }
            owner.StateMachine.ChangeState(owner.StateMachine.GamePlayState);
            yield return new WaitForEndOfFrame();
        }

        public void Process()
        {
        }

        public void Exit()
        {
            LoadingScene.Unload();
        }
    }

    public class MainMenuState : IStateMachineState
    {
        public MainMenuScene MainMenuScene { get; set; }
        public Logger LogSystem { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public TokenSystem TokenSystem { get; set; }

        public MainMenuState()
        {
        }

        public void Enter()
        {
            GameStateManager.Clear();
            TokenSystem.Clear();
            MainMenuScene.Load();
        }

        public void Process()
        {
        }

        public void Exit()
        {
            MainMenuScene.Unload();
        }
    }

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

        public GamePlayState()
        {

        }

        public void Enter()
        {
            SpriteSortingSystem.Init();
            GamePlayScene.Load();
            VisibilitySystem.Init();
            TurnSystem.Init();
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
            SpriteSortingSystem.Sort();
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