using System;
using UnityEngine;

namespace Gamepackage
{
    public class UIController : MonoBehaviour
    {
        [NonSerialized]
        public DeathNotification DeathNotification;

        [NonSerialized]
        public TextLog TextLog;

        [NonSerialized]
        public FloatingCombatTextManager FloatingCombatTextManager;

        [NonSerialized]
        public InventoryWindow InventoryWindow;

        [NonSerialized]
        public LootWindow LootWindow;

        [NonSerialized]
        public ItemInspectionWindow ItemInspectionWindow;

        [NonSerialized]
        public ContextMenu ContextMenu;

        [NonSerialized]
        public ClickoutCatcher ClickoutCatcher;

        [NonSerialized]
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
            ItemInspectionWindow = GetComponentInChildren<ItemInspectionWindow>(true);
            ContextMenu = GetComponentInChildren<ContextMenu>(true);
            ClickoutCatcher = GetComponentInChildren<ClickoutCatcher>(true);
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
}

