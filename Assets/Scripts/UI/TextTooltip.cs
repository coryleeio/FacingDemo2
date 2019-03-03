using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class TextTooltip : MonoBehaviour
    {
        private Text _text;
        private Text Text
        {
            get
            {
                if (_text == null)
                {
                    _text = GetComponentInChildren<Text>(true);
                }
                return _text;
            }
        }

        public void ShowText(string text)
        {
            Text.text = text;
        }
    }
}
