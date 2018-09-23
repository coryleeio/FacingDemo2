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
                BuildButton("escape.menu.buttons.return".Localize(), () =>
                {

                });
                BuildButton("escape.menu.buttons.restart".Localize(), () => {
                    Context.GameStateManager.Clear();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
                });
                BuildButton("escape.menu.buttons.save".Localize(), () =>
                {
                    Context.GameStateManager.SaveGame();
                });
                BuildButton("escape.menu.buttons.load".Localize(), () =>
                {
                    Context.GameStateManager.LoadGame();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
                });
                BuildButton("escape.menu.buttons.quit.to.main.menu".Localize(), () => {
                    Context.GameStateManager.Clear();
                    Context.Application.StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
                });
                BuildButton("escape.menu.buttons.quit.game".Localize(), () =>
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
