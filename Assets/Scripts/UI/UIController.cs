using Gamepackage;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public DeathNotification DeathNotification;
    public TextLog TextLog;
    public FloatingCombatTextManager FloatingCombatTextManager;
    public InventoryWindow InventoryWindow;
    public LootWindow LootWindow;
    public Tooltip Tooltip;

    public void Init()
    {
        var childUIComponents = GetComponentsInChildren<UIComponent>(true);
        var canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        foreach (var component in childUIComponents)
        {
            component.gameObject.SetActive(false);
        }
        DeathNotification = GetComponentInChildren<DeathNotification>(true);
        TextLog = GetComponentInChildren<TextLog>(true);
        FloatingCombatTextManager = GetComponentInChildren<FloatingCombatTextManager>(true);
        LootWindow = GetComponentInChildren<LootWindow>(true);
        InventoryWindow = GetComponentInChildren<InventoryWindow>(true);
        Tooltip = GetComponentInChildren<Tooltip>(true);
        FloatingCombatTextManager.Show();
        TextLog.ClearText();
        TextLog.Show();
    }

    public void Refresh()
    {
        // The false here indicates that we refresh only active windows
        var childUIComponents = GetComponentsInChildren<UIComponent>(false);

        foreach (var comp in childUIComponents)
        {
            comp.Refresh();
        }
    }
}
