using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class AbilityRow : MonoBehaviour
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
