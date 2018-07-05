using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class DeathNotification : UIComponent
    {
        public void RestartGame()
        {
            Context.Application.StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
        }

        public override void Hide()
        {
            GetComponent<DeathNotification>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<DeathNotification>().gameObject.SetActive(true);
        }
    }
}

