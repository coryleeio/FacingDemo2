using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class Root : MonoBehaviour
    {
        public GameStateMachine StateMachine { get; set; }
        public IGameStateSystem GameStateSystem { get; set; }
        public IVisibilitySystem VisibilitySystem { get; set; }
        public ITokenSystem TokenSystem { get; set; }
        public IPathFinder PathFinder { get; set; }

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
            container.Register<ITurnSystem, TurnSystem>().AsSingleton();
            container.Register<ILogSystem, LogSystem>().AsSingleton();
            container.Register<ICombatSystem, CombatSystem>().AsSingleton();
            container.Register<IDialogueSystem, DialogueSystem>().AsSingleton();
            container.Register<IGameStateSystem, GameStateSystem>().AsSingleton();
            container.Register<IItemSystem, ItemSystem>().AsSingleton();
            container.Register<ILogSystem, LogSystem>().AsSingleton();
            container.Register<IMessageBusSystem, MessageBusSystem>().AsSingleton();
            container.Register<IMovementSystem, MovementSystem>().AsSingleton();
            container.Register<ITriggerSystem, TriggerSystem>().AsSingleton();
            container.Register<IVisibilitySystem, VisibilitySystem>().AsSingleton();
            container.Register<IModSystem, ModSystem>().AsSingleton();
            container.Register<IResourceManager, ResourceManager>().AsSingleton();
            container.Register<IPrototypeFactory, PrototypeFactory>().AsSingleton();
            container.Register<ITokenSystem, TokenSystem>().AsSingleton();
            container.Register<IDungeonGenerator, DungeonGenerator>().AsSingleton();
            container.Register<ISpriteSortingSystem, SpriteSortingSystem>().AsSingleton();
            container.Register<IOverlaySystem, OverlaySystem>().AsSingleton();
            container.Register<IPathFinder, PathFinder>().AsSingleton();
            container.Register<IPlayerController, PlayerController>().AsSingleton();

            container.BuildUp(container.Resolve<AdvancingTime>());
            container.BuildUp(container.Resolve<TurnStateMachine>());
            container.BuildUp(container.Resolve<GamePlayState>());
            container.BuildUp(container.Resolve<MainMenuState>());
            container.BuildUp(container.Resolve<LoadingResourcesState>());
            container.BuildUp(container.Resolve<GameStateMachine>());
            container.BuildUp(container.Resolve<ITurnSystem>());
            container.BuildUp(container.Resolve<ILogSystem>());
            container.BuildUp(container.Resolve<ICombatSystem>());
            container.BuildUp(container.Resolve<IDialogueSystem>());
            container.BuildUp(container.Resolve<IGameStateSystem>());
            container.BuildUp(container.Resolve<IItemSystem>());
            container.BuildUp(container.Resolve<ILogSystem>());
            container.BuildUp(container.Resolve<IMessageBusSystem>());
            container.BuildUp(container.Resolve<IMovementSystem>());
            container.BuildUp(container.Resolve<ITriggerSystem>());
            container.BuildUp(container.Resolve<IVisibilitySystem>());
            container.BuildUp(container.Resolve<IModSystem>());
            container.BuildUp(container.Resolve<IResourceManager>());
            container.BuildUp(container.Resolve<IPrototypeFactory>());
            container.BuildUp(container.Resolve<ITokenSystem>());
            container.BuildUp(container.Resolve<IDungeonGenerator>());
            container.BuildUp(container.Resolve<ISpriteSortingSystem>());
            container.BuildUp(container.Resolve<IOverlaySystem>());
            container.BuildUp(container.Resolve<IPathFinder>());
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
                GameStateSystem.Clear();
                StateMachine.ChangeState(StateMachine.LoadingResourcesState);
            }

            if (GUI.Button(new Rect(10, 75, 100, 50), "Exit game"))
            {
                GameStateSystem.Clear();
                StateMachine.ChangeState(StateMachine.MainMenuState);
            }

            if (GUI.Button(new Rect(10, 140, 100, 50), "Reload current"))
            {
                StateMachine.ChangeState(StateMachine.GamePlayState);
            }

            if (GUI.Button(new Rect(10, 215, 100, 50), "Save"))
            {
                GameStateSystem.SaveGame();
            }

            if (GUI.Button(new Rect(10, 290, 100, 50), "Load"))
            {
                GameStateSystem.LoadGame();
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