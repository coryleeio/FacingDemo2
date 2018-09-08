using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class Tooltip : UIComponent
    {
        private static GameObject _mostRecentHover;

        public void Hover(GameObject hoverOver, string input)
        {
            if(input == null || input == "")
            {
                return;
            }
            _mostRecentHover = hoverOver;
            SetDisplayText(input);
            var mousePos = Input.mousePosition;
            transform.position = hoverOver.transform.position;
            Show();
        }

        public void Leave(GameObject obj)
        {
            if (obj == _mostRecentHover)
            {
                Hide();
            }
        }

        private void SetDisplayText(string input)
        {
            GetComponentInChildren<Text>().text = input;
        }

        public override void Hide()
        {
            GetComponent<Tooltip>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<Tooltip>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {
            Hide();
        }
    }
}
