using UnityEngine.UI;

namespace Gamepackage
{
    public class TextLog : UIComponent
    {
        public void AddText(string line)
        {
            Text textComponent = GetTextComponent();
            string msgText = textComponent.text;
            if (line != "")
                line += "\n";
            msgText += line;
            UpdateText(msgText);
        }

        private Text GetTextComponent()
        {
            return GetComponentInChildren<Text>();
        }

        public void UpdateText(string text)
        {
            var textComponent = GetTextComponent();
            var scrollRect = GetComponentInChildren<ScrollRect>();
            textComponent.text = text;
            UnityEngine.Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }

        public void ClearText()
        {
            UpdateText("");
        }

        public override void Hide()
        {
            GetComponent<TextLog>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<TextLog>().gameObject.SetActive(true);
        }
    }
}

