using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class Application : MonoBehaviour
    {
        public StateMachine StateMachine = new StateMachine();
        public GameStateManager GameStateManager { get; set; }
        public VisibilitySystem VisibilitySystem { get; set; }
        public TokenSystem TokenSystem { get; set; }
        public PathFinder PathFinder { get; set; }
        public LoadingResourcesState LoadingResourcesState { get; set; }
        public MainMenuState MainMenuState { get; set; }
        public GamePlayState GamePlayState { get; set; }

        void Start()
        {
            DontDestroyOnLoad(this);
            var container = new TinyIoCContainer();

            // Dont forget to add the injection down below...
            container.Register<Application>(this);
            container.Register<ApplicationContext, ApplicationContext>().AsSingleton();
            container.Register<GamePlayState, GamePlayState>().AsSingleton();
            container.Register<MainMenuState, MainMenuState>().AsSingleton();
            container.Register<LoadingResourcesState, LoadingResourcesState>().AsSingleton();
            container.Register<GameStateManager, GameStateManager>().AsSingleton();
            container.Register<VisibilitySystem, VisibilitySystem>().AsSingleton();
            container.Register<ResourceManager, ResourceManager>().AsSingleton();
            container.Register<PrototypeFactory, PrototypeFactory>().AsSingleton();
            container.Register<TokenSystem, TokenSystem>().AsSingleton();
            container.Register<DungeonGenerator, DungeonGenerator>().AsSingleton();
            container.Register<SpriteSortingSystem, SpriteSortingSystem>().AsSingleton();
            container.Register<OverlaySystem, OverlaySystem>().AsSingleton();
            container.Register<PathFinder, PathFinder>().AsSingleton();
            container.Register<GameScene, GameScene>().AsSingleton();
            container.Register<LoadingScene, LoadingScene>().AsSingleton();
            container.Register<MainMenuScene, MainMenuScene>().AsSingleton();
            container.Register<MovementSystem, MovementSystem>().AsSingleton();
            container.Register<FlowSystem, FlowSystem>().AsSingleton();
            container.Register<DoTriggers, DoTriggers>().AsSingleton();
            container.Register<DoTurn, DoTurn>().AsSingleton();
            container.Register<PlayerController, PlayerController>().AsSingleton();
            container.Register<CombatSystem, CombatSystem>().AsSingleton();
            container.Register<StateMachine>();

            container.BuildUp(container.Resolve<GamePlayState>());
            container.BuildUp(container.Resolve<MainMenuState>());
            container.BuildUp(container.Resolve<LoadingResourcesState>());
            container.BuildUp(container.Resolve<GameStateManager>());
            container.BuildUp(container.Resolve<VisibilitySystem>());
            container.BuildUp(container.Resolve<ResourceManager>());
            container.BuildUp(container.Resolve<PrototypeFactory>());
            container.BuildUp(container.Resolve<TokenSystem>());
            container.BuildUp(container.Resolve<DungeonGenerator>());
            container.BuildUp(container.Resolve<SpriteSortingSystem>());
            container.BuildUp(container.Resolve<OverlaySystem>());
            container.BuildUp(container.Resolve<PathFinder>());
            container.BuildUp(container.Resolve<GameScene>());
            container.BuildUp(container.Resolve<LoadingScene>());
            container.BuildUp(container.Resolve<MainMenuScene>());
            container.BuildUp(container.Resolve<MovementSystem>());
            container.BuildUp(container.Resolve<FlowSystem>());
            container.BuildUp(container.Resolve<DoTriggers>());
            container.BuildUp(container.Resolve<DoTurn>());
            container.BuildUp(container.Resolve<ApplicationContext>());
            container.BuildUp(container.Resolve<PlayerController>());
            container.BuildUp(container.Resolve<CombatSystem>());
            container.BuildUp(this);
            StateMachine.ChangeState(MainMenuState);
        }

        void Update()
        {
            StateMachine.Process();
        }

        void OnDisable()
        {
            PathFinder.Cleanup();
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Start New game"))
            {
                GameStateManager.Clear();
                StateMachine.ChangeState(LoadingResourcesState);
            }

            if (GUI.Button(new Rect(10, 75, 100, 50), "Exit game"))
            {
                GameStateManager.Clear();
                StateMachine.ChangeState(MainMenuState);
            }

            if (GUI.Button(new Rect(10, 140, 100, 50), "Reload current"))
            {
                StateMachine.ChangeState(GamePlayState);
            }

            if (GUI.Button(new Rect(10, 215, 100, 50), "Save"))
            {
                GameStateManager.SaveGame();
            }

            if (GUI.Button(new Rect(10, 290, 100, 50), "Load"))
            {
                GameStateManager.LoadGame();
                StateMachine.ChangeState(GamePlayState);
            }

            if (GUI.Button(new Rect(10, 350, 100, 50), "Reveal"))
            {
                var newVis = new bool[40, 40];
                for (var x = 0; x < 40; x++)
                {
                    for (var y = 0; y < 40; y++)
                    {
                        newVis[x, y] = true;
                    }
                }
                VisibilitySystem.UpdateVisibility(newVis);
            }

            if (Camera.main != null)
            {
                var mouseMapPosition = MathUtil.GetMousePositionOnMap(Camera.main);
                if (GUI.Button(new Rect(150, 10, 100, 50), mouseMapPosition.ToString()))
                {

                }
            }
        }
    }
}