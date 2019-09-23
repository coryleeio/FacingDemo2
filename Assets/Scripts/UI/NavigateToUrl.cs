using UnityEngine;
namespace Gamepackage
{
    public class NavigateToUrl : MonoBehaviour
    {
        public string URL;
        public void OpenURL()
        {
            Application.OpenURL(URL);
        }
    }
}
