using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class BorderedWindow : MonoBehaviour
    {
        public void SetHeader(string localizationKey)
        {
            var headerPanel = HeaderPanel;
            headerPanel.SetActive(true);
            headerPanel.transform.GetComponentInChildren<Text>().text = localizationKey.Localize();
        }

        public GameObject ContentPanel
        {
            get
            {
                return this.transform.Find("ContentPanel").gameObject;
            }
        }

        public GameObject HeaderPanel
        {
            get
            {
                return this.transform.Find("HeaderPanel").gameObject;
            }
        }

        public XCloseButton CloseButton
        {
            get
            {
                return this.transform.GetComponentInChildren<XCloseButton>(true);
            }
        }
    }
}
