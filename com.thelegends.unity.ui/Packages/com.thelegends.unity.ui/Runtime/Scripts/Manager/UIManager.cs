using System.Collections;
using System.Collections.Generic;
using TheLegends.Base.UnitySingleton;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class UIManager : Singleton<UIManager>
    {

        private Dictionary<string, UIPanelController> _panelRegistry = new Dictionary<string, UIPanelController>();

        public void RegisterPanel(UIPanelController panel)
        {
            if (panel == null || string.IsNullOrEmpty(panel.PanelID))
            {
                return;
            }

            if (!_panelRegistry.ContainsKey(panel.PanelID))
            {
                _panelRegistry[panel.PanelID] = panel;
            }
            else
            {
                if (_panelRegistry[panel.PanelID] != panel)
                {
                    Debug.LogWarning($"A panel with ID '{panel.PanelID}' is already registered. Overwriting the reference.");
                    _panelRegistry[panel.PanelID] = panel;
                }
            }
        }

        public void UnregisterPanel(UIPanelController panel)
        {
            if (panel == null || string.IsNullOrEmpty(panel.PanelID))
            {
                return;
            }

            if (_panelRegistry.ContainsKey(panel.PanelID) && _panelRegistry[panel.PanelID] == panel)
            {
                _panelRegistry.Remove(panel.PanelID);
            }
        }

        public void ShowPanel(string panelID)
        {
            if (string.IsNullOrEmpty(panelID))
            {
                Debug.LogError("ShowPanel called with a null or empty panelID.");
                return;
            }

            if (_panelRegistry.TryGetValue(panelID, out UIPanelController panel))
            {
                panel.Show();
            }
            else
            {
                Debug.LogWarning($"Panel with ID '{panelID}' could not be found. Is it active and registered?");
            }
        }

        public void HidePanel(string panelID)
        {
            if (string.IsNullOrEmpty(panelID))
            {
                Debug.LogError("HidePanel called with a null or empty panelID.");
                return;
            }

            if (_panelRegistry.TryGetValue(panelID, out UIPanelController panel))
            {
                panel.Hide();
            }
            else
            {
                Debug.LogWarning($"Panel with ID '{panelID}' could not be found.");
            }
        }

        public UIPanelController GetPanel(string panelID)
        {
            if (string.IsNullOrEmpty(panelID)) return null;

            _panelRegistry.TryGetValue(panelID, out UIPanelController panel);
            return panel;
        }

        public bool IsPanelShown(string panelID)
        {
            if (string.IsNullOrEmpty(panelID)) return false;

            if (_panelRegistry.TryGetValue(panelID, out UIPanelController panel))
            {
                return panel.gameObject.activeSelf;
            }
            return false;
        }
    }
}
