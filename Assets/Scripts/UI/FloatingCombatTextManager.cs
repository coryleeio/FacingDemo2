using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class FloatingCombatTextManager : UIComponent
    {
        public void ShowCombatText(string text, Color color, int size, Vector3 worldPos)
        {
            var vec2 = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPos);
            var prefab = Resources.Load<FloatingText>("UI/FloatingCombatText");
            var floatingTextObject = GameObject.Instantiate<FloatingText>(prefab);
            var textComponent = floatingTextObject.GetComponent<Text>();
            textComponent.text = text;
            textComponent.fontSize = size;
            textComponent.color = color;
            textComponent.rectTransform.SetParent(this.transform, true);
            textComponent.rectTransform.position = vec2;
        }

        public override void Hide()
        {
            GetComponent<FloatingCombatTextManager>().gameObject.SetActive(true);
        }

        public override void Show()
        {
            GetComponent<FloatingCombatTextManager>().gameObject.SetActive(true);
        }
    }
}
