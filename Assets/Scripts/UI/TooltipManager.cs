using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class TooltipManager : UIComponent
    {
        private static GameObject _mostRecentHover;

        private TextTooltip _textTooltip;
        private TextTooltip TextTooltip
        {
            get
            {
                if(_textTooltip == null)
                {
                    _textTooltip = GetComponentInChildren<TextTooltip>(true);
                }
                return _textTooltip;
            }
        }

        public void TooltipShowSimpleText(GameObject hoverOver, string input)
        {
            Debug.Log("SHow" + hoverOver.name);
            if(input == null || input == "")
            {
                return;
            }
            _mostRecentHover = hoverOver;
            var mousePos = Input.mousePosition;
            transform.position = hoverOver.transform.position;
            Show();
            TextTooltip.ShowText(input);
        }

        public void StopTooltip(GameObject obj)
        {
            if (obj == _mostRecentHover)
            {
                Debug.Log(obj);
                Hide();
            }
        }

        public override void Hide()
        {
            GetComponent<TooltipManager>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<TooltipManager>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {
            Hide();
        }
    }
}
