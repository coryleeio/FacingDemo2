using Gamepackage;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour {

    public void Init()
    {
        var childUIComponents = GetComponentsInChildren<UIComponent>(true);
        foreach (var component in childUIComponents)
        {
            component.gameObject.SetActive(false);
        }
    }

    public void ShowDeathNotification()
    {
        GetComponentInChildren<DeathNotification>(true).gameObject.SetActive(true);
    }
}
