using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class Root : MonoBehaviour
    {
        public GameStateMachine StateMachine { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public VisibilitySystem VisibilitySystem { get; set; }
        public TokenSystem TokenSystem { get; set; }
        public PathFinder PathFinder { get; set; }

        void Start()
        {
            DontDestroyOnLoad(this);
            var container = new TinyIoCContainer();

            // Dont forget to add the injection down below...
            container.Register<Root>(this);
            container.Register<GamePlayState, GamePlayState>().AsSingleton();
            container.Register<MainMenuState, MainMenuState>().AsSingleton();
            container.Register<LoadingResourcesState, LoadingResourcesState>().AsSingleton();
            container.Register<GameStateMachine, GameStateMachine>().AsSingleton();
            container.Register<TurnStateMachine, TurnStateMachine>().AsSingleton();
            container.Register<AdvancingTime, AdvancingTime>().AsSingleton();
            container.Register<DoTurn, DoTurn>().AsSingleton();
            container.Register<GatherInput, GatherInput>().AsSingleton();
            container.Register<TurnSystem, TurnSystem>().AsSingleton();
            container.Register<Logger, Logger>().AsSingleton();
            container.Register<GameStateManager, GameStateManager>().AsSingleton();
            container.Register<Logger, Logger>().AsSingleton();
            container.Register<VisibilitySystem, VisibilitySystem>().AsSingleton();
            container.Register<ResourceManager, ResourceManager>().AsSingleton();
            container.Register<PrototypeFactory, PrototypeFactory>().AsSingleton();
            container.Register<TokenSystem, TokenSystem>().AsSingleton();
            container.Register<DungeonGenerator, DungeonGenerator>().AsSingleton();
            container.Register<SpriteSortingSystem, SpriteSortingSystem>().AsSingleton();
            container.Register<OverlaySystem, OverlaySystem>().AsSingleton();
            container.Register<PathFinder, PathFinder>().AsSingleton();


            container.BuildUp(container.Resolve<TurnStateMachine>());
            container.BuildUp(container.Resolve<AdvancingTime>());
            container.BuildUp(container.Resolve<DoTurn>());
            container.BuildUp(container.Resolve<GatherInput>());
            container.BuildUp(container.Resolve<GamePlayState>());
            container.BuildUp(container.Resolve<MainMenuState>());
            container.BuildUp(container.Resolve<LoadingResourcesState>());
            container.BuildUp(container.Resolve<GameStateMachine>());
            container.BuildUp(container.Resolve<TurnSystem>());
            container.BuildUp(container.Resolve<Logger>());
            container.BuildUp(container.Resolve<GameStateManager>());
            container.BuildUp(container.Resolve<VisibilitySystem>());
            container.BuildUp(container.Resolve<ResourceManager>());
            container.BuildUp(container.Resolve<PrototypeFactory>());
            container.BuildUp(container.Resolve<TokenSystem>());
            container.BuildUp(container.Resolve<DungeonGenerator>());
            container.BuildUp(container.Resolve<SpriteSortingSystem>());
            container.BuildUp(container.Resolve<OverlaySystem>());
            container.BuildUp(container.Resolve<PathFinder>());
            container.BuildUp(this);
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
                StateMachine.ChangeState(StateMachine.LoadingResourcesState);
            }

            if (GUI.Button(new Rect(10, 75, 100, 50), "Exit game"))
            {
                GameStateManager.Clear();
                StateMachine.ChangeState(StateMachine.MainMenuState);
            }

            if (GUI.Button(new Rect(10, 140, 100, 50), "Reload current"))
            {
                StateMachine.ChangeState(StateMachine.GamePlayState);
            }

            if (GUI.Button(new Rect(10, 215, 100, 50), "Save"))
            {
                GameStateManager.SaveGame();
            }

            if (GUI.Button(new Rect(10, 290, 100, 50), "Load"))
            {
                GameStateManager.LoadGame();
                StateMachine.ChangeState(StateMachine.GamePlayState);
            }

            if (GUI.Button(new Rect(10, 350, 100, 50), "Reveal"))
            {
                var newVis = new bool[40, 40];
                for(var x = 0; x < 40; x++)
                {
                    for(var y = 0; y < 40; y++)
                    {
                        newVis[x,y] = true;
                    }
                }
                VisibilitySystem.UpdateVisibility(newVis);
            }

            if(Camera.main != null)
            {
                var mouseMapPosition = MathUtil.GetMousePositionOnMap(Camera.main);
                if (GUI.Button(new Rect(150, 10, 100, 50), mouseMapPosition.ToString()))
                {

                }
            }
        }
    }
}