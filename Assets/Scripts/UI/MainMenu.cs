using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gamepackage
{
    public class MainMenu : UIComponent
    {
        public void Start()
        {
            Window.CloseButton.gameObject.SetActive(false);
            BuildButton("main.menu.buttons.new.game".Localize(), () =>
            {
                SaveUtil.Clear();
                SceneManager.LoadScene((int)Scenes.Loading);
            });

            if (SaveUtil.HasGameToLoad(SaveUtil.GetDefaultSaveLocation()))
            {
                BuildButton("main.menu.buttons.continue".Localize(), () =>
                {

                    SaveUtil.LoadGame();
                    SceneManager.LoadScene((int)Scenes.Loading);
                });
            }

            BuildButton("main.menu.buttons.quit".Localize(), () =>
            {
                if (Application.isEditor)
                {
                    SaveUtil.Clear();
                    SceneManager.LoadScene((int)Scenes.MainMenu);
                }
                else
                {
                    Application.Quit();
                }
            });
        }

        public void BuildButton(string buttonName, UnityAction handler)
        {
            var window = this.transform.GetComponentInChildren<BorderedWindow>();
            var _buttonPrefab = Resources.Load<Button>("Prefabs/UI/MenuButton");
            var buttonGameObject = GameObject.Instantiate<Button>(_buttonPrefab);
            buttonGameObject.name = string.Format("{0}", buttonName);
            var text = buttonGameObject.GetComponentInChildren<Text>();
            text.text = buttonName;
            buttonGameObject.gameObject.transform.SetParent(window.ContentPanel.transform, false);
            buttonGameObject.onClick.AddListener(handler);
        }

        public override void Hide()
        {
            GetComponent<MainMenu>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<MainMenu>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {

        }
    }
}
