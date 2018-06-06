using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class ApplicationStateMachine : StateMachine
    {
        public static GamePlayState GamePlayState = new GamePlayState();
        public static LoadingResourcesState LoadingResourcesState  = new LoadingResourcesState();
        public static MainMenuState MainMenuState  = new MainMenuState();
    }
}
