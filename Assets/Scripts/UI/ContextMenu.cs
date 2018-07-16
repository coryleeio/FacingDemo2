using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ContextMenu : UIComponent
    {
        private List<Button> buttons = new List<Button>();

        public override void Hide()
        {
            GetComponent<ContextMenu>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<ContextMenu>().gameObject.SetActive(true);
        }

        public void ShowForItemAtLocation(Item item, PointerEventData eventData)
        {
            Purge();
            BuildButton("Info", () => {
                Context.UIController.ItemInspectionWindow.ShowFor(item);
            });
            this.transform.position = eventData.position;
            Show();
        }

        private void Purge()
        {
            foreach(var button in buttons)
            {
                GameObject.Destroy(button.gameObject);
            }
            buttons.Clear();
        }

        public void BuildButton(string buttonName, UnityAction handler)
        {
            var _buttonPrefab = Resources.Load<Button>("UI/ButtonPrefab");
            var buttonGameObject = GameObject.Instantiate<Button>(_buttonPrefab);
            buttonGameObject.name = string.Format("{0}Button", buttonName);
            var text = buttonGameObject.GetComponentInChildren<Text>();
            text.text = buttonName;
            buttonGameObject.gameObject.transform.SetParent(transform, false);
            buttons.Add(buttonGameObject);
            buttonGameObject.onClick.AddListener(() =>
            {
                Hide();
            });
            buttonGameObject.onClick.AddListener(handler);
        }

        public override void Refresh()
        {

        }
    }
}
