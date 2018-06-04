using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathNotification : UIComponent {

    public void RestartGame()
    {
        Context.Application.StateMachine.ChangeState(Context.MainMenuState);
    }
}
