using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class ItemInspectionWindowAttributeRow : MonoBehaviour
    {
        public void Clear()
        {
            Set("", "");
        }

        public void Set(string key, string value)
        {
            var keyComp = transform.Find("KeyPanel").gameObject.GetComponentInChildren<Text>();
            var valComp = transform.Find("ValuePanel").gameObject.GetComponentInChildren<Text>();

            keyComp.text = key;
            valComp.text = value;
        }
    }
}
