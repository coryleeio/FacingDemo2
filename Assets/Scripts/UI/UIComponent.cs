using Gamepackage;
using UnityEngine;

public class UIComponent : MonoBehaviour, IHasApplicationContext
{
    protected ApplicationContext Context { get; set; }
    public void InjectContext(ApplicationContext context)
    {
        Context = context;
    }
}
