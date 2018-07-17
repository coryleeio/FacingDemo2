using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gamepackage
{
    public class MainMenu : MonoBehaviour
    {
        public void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;

            BuildButton("New Game", () => {
                Context.GameStateManager.Clear();
                Context.Application.StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
            });

            if(Context.GameStateManager.HasGameToLoad())
            {
                BuildButton("Continue", () => {

                    Context.GameStateManager.LoadGame();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
                });
            }

            BuildButton("Quit", () => {
                if (UnityEngine.Application.isEditor)
                {
                    Context.GameStateManager.Clear();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
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
