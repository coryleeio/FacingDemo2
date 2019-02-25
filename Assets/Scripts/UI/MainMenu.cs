using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gamepackage
{
    public class MainMenu : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;

            BuildButton("main.menu.buttons.new.game".Localize(), () => {
                SaveUtil.Clear();
                SceneManager.LoadScene((int)Scenes.Loading);
            });

            if(SaveUtil.HasGameToLoad(SaveUtil.GetDefaultSaveLocation()))
            {
                BuildButton("main.menu.buttons.continue".Localize(), () => {

                    SaveUtil.LoadGame();
                    SceneManager.LoadScene((int)Scenes.Loading);
                });
            }

            BuildButton("main.menu.buttons.quit".Localize(), () => {
                if (UnityEngine.Application.isEditor)
                {
                    SaveUtil.Clear();
                    SceneManager.LoadScene((int)Scenes.MainMenu);
                }
                else
                {
                    UnityEngine.Application.Quit();
                }
            });
        }

        public void BuildButton(string buttonName, UnityAction handler)
        {
            var _buttonPrefab = Resources.Load<Button>("UI/ButtonPrefab");
            var buttonGameObject = GameObject.Instantiate<Button>(_buttonPrefab);
            buttonGameObject.name = string.Format("{0}", buttonName);
            var text = buttonGameObject.GetComponentInChildren<Text>();
            text.text = buttonName;
            buttonGameObject.gameObject.transform.SetParent(transform.Find("MainMenuPanel").Find("InnerPanel").transform, false);
            buttonGameObject.onClick.AddListener(handler);
        }
    }
}
