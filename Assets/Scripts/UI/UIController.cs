using Gamepackage;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour, IHasApplicationContext {

    public ApplicationContext Context { get; set; }

    public void InjectContext(ApplicationContext context)
    {
        Context = context;
        var childUIComponents = GetComponentsInChildren<UIComponent>(true);
        foreach(var component in childUIComponents)
        {
            component.InjectContext(context);
            component.gameObject.SetActive(false);
        }
    }

    public void ShowDeathNotification()
    {
        GetComponentInChildren<DeathNotification>(true).gameObject.SetActive(true);
    }
}
