using UnityEngine;
using UnityEngine.UI;

public class XCloseButton : MonoBehaviour
{
    private UIComponent Closable;

    public void WireUp(UIComponent uIComponent)
    {
        this.Closable = uIComponent;
        Button.onClick.AddListener(() => { uIComponent.Hide(); });
    }

    private Button Button
    {
        get
        {
            return transform.GetComponent<Button>();
        }
    }
}
