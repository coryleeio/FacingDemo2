using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class ClickoutCatcher : UIComponent, IPointerClickHandler
    {
        public override void Hide()
        {
            GetComponent<ClickoutCatcher>().gameObject.SetActive(false);
        }

        public override void Show()
        {
            GetComponent<ClickoutCatcher>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(Context.UIController.ContextMenu != null)
            {
                Context.UIController.ContextMenu.Hide();
            }
        }
    }
}
