using Mono.Data.Sqlite;
using System.Data;
using System.IO;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class Root : MonoBehaviour
    {
        public GameStateMachine StateMachine;
        private IGameStateSystem _gameStateSystem;
        private IVisibilitySystem _visibilitySystem;
        private ITokenSystem _tokenSystem;
        private IPathFinder _pathFinder;

        void Start()
        {
            DontDestroyOnLoad(this);
            var container = new TinyIoCContainer();
            container.Register<Root>(this);

            container.Register<GamePlayState, GamePlayState>().AsSingleton();
            container.Register<MainMenuState, MainMenuState>().AsSingleton();
            container.Register<LoadingResourcesState, LoadingResourcesState>().AsSingleton();
            container.Register<GameStateMachine, GameStateMachine>().AsSingleton();
            container.Register<ILogSystem, LogSystem>().AsSingleton();
            container.Register<ICombatSystem, CombatSystem>().AsSingleton();
            container.Register<IDialogueSystem, DialogueSystem>().AsSingleton();
            container.Register<IGameStateSystem, GameStateSystem>().AsSingleton();
            container.Register<IItemSystem, ItemSystem>().AsSingleton();
            container.Register<ILogSystem, LogSystem>().AsSingleton();
            container.Register<IMessageBusSystem, MessageBusSystem>().AsSingleton();
            container.Register<IMovementSystem, MovementSystem>().AsSingleton();
            container.Register<ITriggerSystem, TriggerSystem>().AsSingleton();
            container.Register<ITurnSystem, TurnSystem>().AsSingleton();
            container.Register<IVisibilitySystem, VisibilitySystem>().AsSingleton();
            container.Register<IModSystem, ModSystem>().AsSingleton();
            container.Register<IResourceManager, ResourceManager>().AsSingleton();
            container.Register<IPrototypeFactory, PrototypeFactory>().AsSingleton();
            container.Register<ITokenSystem, TokenSystem>().AsSingleton();
            container.Register<IDungeonGenerator, DungeonGenerator>().AsSingleton();
            container.Register<ISpriteSortingSystem, SpriteSortingSystem>().AsSingleton();
            container.Register<IOverlaySystem, OverlaySystem>().AsSingleton();
            container.Register<IPathFinder, PathFinder>().AsSingleton();
            _pathFinder = container.Resolve<IPathFinder>();
            _gameStateSystem = container.Resolve<IGameStateSystem>();
            _tokenSystem = container.Resolve<ITokenSystem>();
            _visibilitySystem = container.Resolve<IVisibilitySystem>();
            StateMachine = container.Resolve<GameStateMachine>();
        }

        void Update()
        {
            StateMachine.Process();
        }

        void OnDisable()
        {
            _pathFinder.Cleanup();
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Start game"))
            {
                StateMachine.ChangeState(StateMachine.LoadingResourcesState);
            }

            if (GUI.Button(new Rect(10, 75, 100, 50), "Exit game"))
            {
                StateMachine.ChangeState(StateMachine.MainMenuState);
            }

            if (GUI.Button(new Rect(10, 140, 100, 50), "Next Level"))
            {
                StateMachine.ChangeState(StateMachine.GamePlayState);
            }

            if (GUI.Button(new Rect(10, 215, 100, 50), "Save"))
            {
                _gameStateSystem.SaveGame();
            }

            if (GUI.Button(new Rect(10, 290, 100, 50), "Load"))
            {
                _gameStateSystem.LoadGame();
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
                _visibilitySystem.UpdateVisibility(newVis);
            }

            var mouseMapPosition = MathUtil.GetMousePositionOnMap(Camera.main);
            if(GUI.Button(new Rect(150, 10, 100, 50), mouseMapPosition.ToString()))
            {

            }
        }
    }
}