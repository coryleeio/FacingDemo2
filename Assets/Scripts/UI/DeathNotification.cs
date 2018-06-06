using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class DeathNotification : UIComponent
    {

        public void RestartGame()
        {
            ServiceLocator.Application.StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
        }
    }
}

