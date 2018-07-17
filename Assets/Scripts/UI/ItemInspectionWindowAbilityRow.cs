using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ItemInspectionWindowAbilityRow : MonoBehaviour
    {
        public void Clear()
        {
            Set("");
        }
        public void Set(string input)
        {
            GetComponentInChildren<Text>().text = input;
        }
    }
}
