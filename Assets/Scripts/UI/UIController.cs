using Gamepackage;
using UnityEngine;

public class UIController : MonoBehaviour {

    public DeathNotification DeathNotification;
    public TextLog TextLog;

    public void Init()
    {
        var childUIComponents = GetComponentsInChildren<UIComponent>(true);
        var canvas = GetComponent<Canvas>();
        foreach (var component in childUIComponents)
        {
            component.gameObject.SetActive(false);
        }
        DeathNotification = GetComponentInChildren<DeathNotification>(true);
        TextLog = GetComponentInChildren<TextLog>(true);
        TextLog.ClearText();
        TextLog.Show();
    }
}
