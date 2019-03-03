using Gamepackage;
using UnityEngine;

public abstract class UIComponent : MonoBehaviour
{
    public abstract void Show();
    public abstract void Hide();
    public abstract void Refresh();

    // Not all UIComponents necessary have one of these, but it is fairly common
    // having this here saves a lot of putting this in every component
    public BorderedWindow Window
    {
        get
        {
            return this.transform.GetComponentInChildren<BorderedWindow>(true);
        }
    }
}
