using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Application : MonoBehaviour
    {
        public ApplicationStateMachine StateMachine = new ApplicationStateMachine();

        void Start()
        {
            DontDestroyOnLoad(this);
            Context.Application = this;
            StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
        }

        void Update()
        {
            StateMachine.Process();
        }

        void OnDisable()
        {
            Context.PathFinder.Cleanup();
        }
    }
}