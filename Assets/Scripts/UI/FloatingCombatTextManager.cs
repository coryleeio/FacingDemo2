using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class FloatingCombatTextManager : UIComponent
    {
        public static float LeftRightOffset = 0.15f;
        public void ShowCombatText(string text, Color color, int size, Vector3 worldPos, bool AllowLeftRight)
        {
            LeftRightOffset = LeftRightOffset * -1;
            var prefab = Resources.Load<FloatingText>("Prefabs/UI/FloatingCombatText");
            var floatingTextObject = GameObject.Instantiate<FloatingText>(prefab);
            if(AllowLeftRight)
            {
                floatingTextObject.LeftRightOffset = LeftRightOffset;
            }
            var textComponent = floatingTextObject.GetComponent<Text>();
            textComponent.text = text;
            textComponent.fontSize = size;
            textComponent.color = color;
            textComponent.transform.SetParent(this.transform, false);
            textComponent.transform.position = worldPos;
        }

        public override void Hide()
        {
            GetComponent<FloatingCombatTextManager>().gameObject.SetActive(true);
        }

        public override void Show()
        {
            GetComponent<FloatingCombatTextManager>().gameObject.SetActive(true);
        }

        public override void Refresh()
        {

        }
    }
}
