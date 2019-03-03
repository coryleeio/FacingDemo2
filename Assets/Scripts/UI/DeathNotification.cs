using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gamepackage
{
    public class DeathNotification : UIComponent
    {
        public void Start()
        {
            Window.CloseButton.gameObject.SetActive(false);
            DeathText.text = "death.notification.you.have.died".Localize();
            DeathButtonText.text = "death.notification.admit.defeat".Localize();
        }

        private Text DeathText
        {
            get
            {
                return Window.ContentPanel.transform.Find("TextBase").GetComponent<Text>();
            }
        }

        private Text DeathButtonText
        {
            get
            {
                return Window.ContentPanel.transform.Find("Button").Find("TextBase").GetComponent<Text>();
            }
        }

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

