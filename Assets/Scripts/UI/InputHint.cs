using UnityEngine.UI;

namespace Gamepackage
{
    public class InputHint : UIComponent
    {
        private bool active = false;

        public override void Hide()
        {
            active = false;
            GetComponent<InputHint>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            active = true;
            GetComponent<InputHint>().gameObject.SetActive(true);
        }

        public void ShowText(string text)
        {
            GetComponentInChildren<Text>(true).text = text;
            Show();
        }

        public override void Refresh()
        {

        }

        public void Toggle()
        {
            if (active)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
    }
}
