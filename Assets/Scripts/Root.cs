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
            container.Register<IPrototypeSystem, PrototypeSystem>().AsSingleton();
            container.Register<IPrototypeFactory, PrototypeFactory>().AsSingleton();
            container.Register<ITokenSystem, TokenSystem>().AsSingleton();
            StateMachine = container.Resolve<GameStateMachine>();
        }

        void Update()
        {
            StateMachine.Process();
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
        }
    }
}