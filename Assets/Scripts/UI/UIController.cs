using System;
using System.Collections.Generic;
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
        public EscapeMenu EscapeMenu;

        [NonSerialized]
        public ContextMenu ContextMenu;

        [NonSerialized]
        public ClickoutCatcher ClickoutCatcher;

        [NonSerialized]
        public DarkOverlay DarkOverlay;

        [NonSerialized]
        public Tooltip Tooltip;

        private Stack<UIComponent> WindowStack = new Stack<UIComponent>();

        public void Init()
        {
            WindowStack.Clear();
            var childUIComponents = GetComponentsInChildren<UIComponent>(true);
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            foreach (var component in childUIComponents)
            {
                component.gameObject.SetActive(true); // force all children to activate
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
            DarkOverlay = GetComponentInChildren<DarkOverlay>(true);
            Tooltip = GetComponentInChildren<Tooltip>(true);
            EscapeMenu = GetComponentInChildren<EscapeMenu>(true);

            FloatingCombatTextManager.Show();
            TextLog.ClearText();
            TextLog.Show();
        }

        public void Pop()
        {
            if (WindowStack.Count > 0)
            {
                var win = WindowStack.Pop();
                win.Hide();
            }
        }

        public bool HasWindowsToPop()
        {
            return WindowStack.Count > 0;
        }

        public void PushWindow(UIComponent uIComponent)
        {
            WindowStack.Push(uIComponent);
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

