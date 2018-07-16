using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ContextMenu : UIComponent
    {
        public override void Hide()
        {
            GetComponent<ContextMenu>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<ContextMenu>().gameObject.SetActive(true);
        }

        public void ShowFor(Item item)
        {
            Show();
        }

        public void BuildButton(string buttonName, UnityAction handler)
        {
            var _buttonPrefab = Resources.Load<Button>("Prefabs/UI/ContextMenuButton");
            var buttonContainer = transform.Find("ButtonContainer");
            var buttonGameObject = GameObject.Instantiate<Button>(_buttonPrefab);
            buttonGameObject.name = string.Format("{0}Button", buttonName);
            var text = buttonGameObject.GetComponentInChildren<Text>();
            text.text = buttonName;
            buttonGameObject.gameObject.transform.SetParent(buttonContainer.transform, false);
            buttonGameObject.onClick.AddListener(handler);
        }

        public override void Refresh()
        {

        }
    }
}
