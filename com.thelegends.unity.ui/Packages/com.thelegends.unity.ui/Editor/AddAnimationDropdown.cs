#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TheLegends.Base.UI.Editor
{
    /// <summary>
    /// Dropdown for adding animations to UIAnimationGroup
    /// Similar to Unity's AddComponent dropdown
    /// </summary>
    public class AddAnimationDropdown : PopupWindowContent
    {
        private readonly UIAnimationGroup _targetGroup;
        private readonly List<Type> _animationTypes;
        private string _searchText = "";
        private Vector2 _scrollPosition;
        private bool _focusSearchField = true;

        public AddAnimationDropdown(UIAnimationGroup targetGroup, Vector2 buttonPosition)
        {
            _targetGroup = targetGroup;
            _animationTypes = GetAnimationTypes();
        }

        public void Show(Rect buttonRect)
        {
            UnityEditor.PopupWindow.Show(buttonRect, this);
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 350); // Smaller width to match button
        }

        public override void OnGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            
            // Title bar (like LitMotion)
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Animation", EditorStyles.toolbarButton, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();
            
            // Search field with icon (like LitMotion)
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label(EditorGUIUtility.IconContent("Search Icon"), GUILayout.Width(16), GUILayout.Height(16));
            
            GUI.SetNextControlName("SearchField");
            _searchText = GUILayout.TextField(_searchText, EditorStyles.toolbarSearchField, GUILayout.ExpandWidth(true));
            
            if (_focusSearchField)
            {
                GUI.FocusControl("SearchField");
                _focusSearchField = false;
            }
            
            GUILayout.EndHorizontal();
            
            // Animation list with proper alignment
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUIStyle.none, GUI.skin.verticalScrollbar);
            
            var filteredTypes = GetFilteredTypes();
            
            if (filteredTypes.Count == 0)
            {
                GUILayout.Label("No animations found", EditorStyles.centeredGreyMiniLabel);
            }
            else
            {
                // Use proper spacing and alignment like LitMotion
                GUILayout.Space(2);
                
                foreach (var type in filteredTypes)
                {
                    // Create button style similar to LitMotion
                    var buttonStyle = new GUIStyle(EditorStyles.toolbarButton)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        padding = new RectOffset(8, 8, 4, 4),
                        margin = new RectOffset(0, 0, 0, 0)
                    };
                    
                    if (GUILayout.Button(GetDisplayName(type), buttonStyle, GUILayout.ExpandWidth(true), GUILayout.Height(20)))
                    {
                        AddAnimation(type);
                        editorWindow.Close();
                        break;
                    }
                }
            }
            
            GUILayout.EndScrollView();
            
            GUILayout.EndArea();
        }

        private List<Type> GetAnimationTypes()
        {
            var types = new List<Type>();
            
            // Get all assemblies
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var assembly in assemblies)
            {
                try
                {
                    var assemblyTypes = assembly.GetTypes()
                        .Where(t => typeof(IUIPlayable).IsAssignableFrom(t) && 
                                   !t.IsAbstract && 
                                   !t.IsInterface &&
                                   t != typeof(UIAnimationGroup) &&
                                   t.Namespace != null &&
                                   (t.Namespace.Contains("TheLegends") || t.Namespace.Contains("com.thelegends")))
                        .ToList();
                    
                    types.AddRange(assemblyTypes);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed to get types from assembly {assembly.FullName}: {ex.Message}");
                }
            }
            
            return types.OrderBy(t => GetDisplayName(t)).ToList();
        }

        private List<Type> GetFilteredTypes()
        {
            if (string.IsNullOrEmpty(_searchText))
                return _animationTypes;
            
            return _animationTypes
                .Where(t => GetDisplayName(t).IndexOf(_searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();
        }

        private string GetDisplayName(Type type)
        {
            // Remove common prefixes
            var name = type.Name;
            
            if (name.StartsWith("UIAnimation"))
                name = name.Substring("UIAnimation".Length);
            
            // Add spaces before capital letters
            name = System.Text.RegularExpressions.Regex.Replace(name, "([a-z])([A-Z])", "$1 $2");
            
            return name;
        }

        private void AddAnimation(Type animationType)
        {
            if (_targetGroup == null) return;
            
            // Create new GameObject with the animation component
            var newGameObject = new GameObject(GetDisplayName(animationType));
            newGameObject.transform.SetParent(_targetGroup.transform);
            newGameObject.transform.localPosition = Vector3.zero;
            newGameObject.transform.localScale = Vector3.one;

            // Add the animation component
            var animationComponent = newGameObject.AddComponent(animationType) as UIAnimationBase;
            
            if (animationComponent != null)
            {
                // Add to the group
                _targetGroup.AddAnimation(animationComponent);
                
                // Select the new object
                Selection.activeGameObject = newGameObject;
                
                // Mark as dirty
                EditorUtility.SetDirty(_targetGroup);
            }
        }
    }
}
#endif 