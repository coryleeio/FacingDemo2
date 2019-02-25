using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public class DeathNotification : UIComponent
    {
        public void RestartGame()
        {
            SceneManager.LoadScene((int)Scenes.MainMenu);
        }

        public override void Hide()
        {
            GetComponent<DeathNotification>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<DeathNotification>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {
        }
    }
}

