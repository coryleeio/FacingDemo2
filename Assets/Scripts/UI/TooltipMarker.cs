using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class TooltipMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string hoverString;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hoverString != null)
            {
                Context.UIController.Tooltip.Hover(this.gameObject, hoverString);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.Leave(this.gameObject);
        }
    }
}

