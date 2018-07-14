using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gamepackage
{
    public class InventoryDraggable : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            var canvas = GetComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 9999;
            GetComponentInParent<GridLayoutGroup>().enabled = false;
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GetComponentInParent<GridLayoutGroup>().enabled = true;
            var canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.blocksRaycasts = true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
