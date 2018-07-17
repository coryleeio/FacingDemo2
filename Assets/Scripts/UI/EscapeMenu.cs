using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gamepackage
{
    public class EscapeMenu : UIComponent
    {
        private bool hasInit = false;

        public override void Hide()
        {
            Context.UIController.DarkOverlay.Hide();
            GetComponent<EscapeMenu>().gameObject.SetActive(false);
            Context.UIController.RemoveWindow(this);
        }

        public override void Show()
        {
            if (!hasInit)
            {
                hasInit = true;
                BuildButton("Return to game", () =>
                {

                });
                BuildButton("Restart", () => {
                    Context.GameStateManager.Clear();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
                });
                BuildButton("Save", () =>
                {
                    Context.GameStateManager.SaveGame();
                });
                BuildButton("Load", () =>
                {
                    Context.GameStateManager.LoadGame();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
                });
                BuildButton("Options", () => { });
                BuildButton("Quit game", () =>
                {
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
            Context.UIController.PushWindow(this);
            Context.UIController.DarkOverlay.Show();
            GetComponent<EscapeMenu>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {

        }

        public void BuildButton(string buttonName, UnityAction handler)
        {
            var _buttonPrefab = Resources.Load<Button>("UI/ButtonPrefab");
            var buttonGameObject = GameObject.Instantiate<Button>(_buttonPrefab);
            buttonGameObject.name = string.Format("{0}", buttonName);
            var text = buttonGameObject.GetComponentInChildren<Text>();
            text.text = buttonName;
            buttonGameObject.gameObject.transform.SetParent(transform.Find("InnerPanel").transform, false);
            buttonGameObject.onClick.AddListener(() =>
            {
                Hide();
            });
            buttonGameObject.onClick.AddListener(handler);
        }
    }
}
