using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamepackage
{
    public class AttributeRow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string Key;

        public void Clear()
        {
            Set("", "");
        }

        public void Set(string key, string value)
        {
            Key = key;
            var keyComp = transform.Find("KeyPanel").gameObject.GetComponentInChildren<Text>();
            var valComp = transform.Find("ValuePanel").gameObject.GetComponentInChildren<Text>();

            keyComp.text = key;
            valComp.text = value;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Context.UIController.Tooltip.TooltipShowSimpleText(this.gameObject, StringUtil.TooltipFor(Key));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Context.UIController.Tooltip.StopTooltip(this.gameObject);
        }
    }
}
