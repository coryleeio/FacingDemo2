using UnityEngine;
using UnityEngine.EventSystems;

namespace Gamepackage
{
    public class TooltipMarker : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string Key;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Context.UIController.Tooltip.TooltipShowSimpleText(this.gameObject, DisplayUtil.TooltipFor(Key));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.StopTooltip(this.gameObject);
        }
    }
}

