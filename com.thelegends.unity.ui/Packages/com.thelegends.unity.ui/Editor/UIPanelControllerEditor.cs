#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace TheLegends.Base.UI.Editor
{
    [CustomEditor(typeof(UIPanelController))]
    public class UIPanelControllerEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // ✅ Default inspector content
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // ✅ Add Required Components button
            root.Add(CreateAddRequiredComponentsButton());

            // ✅ Add debug panel
            root.Add(CreateDebugPanel());

            return root;
        }

        VisualElement CreateAddRequiredComponentsButton()
        {
            var box = new Box
            {
                style = {
                    marginTop = 6f,
                    marginBottom = 2f,
                    paddingLeft = 4f,
                    alignItems = Align.Stretch,
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1f,
                }
            };

            box.Add(new Label("Setup")
            {
                style = {
                    marginTop = 5f,
                    marginBottom = 3f,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });

            var addButton = new Button(() => AddRequiredComponents())
            {
                text = "Add Required Components",
                style = {
                    width = 200f,
                    alignSelf = Align.Center,
                    marginTop = 5f,
                    marginBottom = 5f
                }
            };

            box.Add(addButton);

            // ✅ Status info
            var statusLabel = new Label()
            {
                style = {
                    marginTop = 2f,
                    marginBottom = 5f,
                    fontSize = 11f,
                    color = Color.gray
                }
            };

            // ✅ Update status
            box.schedule.Execute(() =>
            {
                var panelController = (UIPanelController)target;
                var hasShow = panelController.ShowAnimationGroup != null;
                var hasHide = panelController.HideAnimationGroup != null;

                if (hasShow && hasHide)
                {
                    statusLabel.text = "✓ All animation groups assigned";
                    statusLabel.style.color = Color.green;
                }
                else if (hasShow || hasHide)
                {
                    statusLabel.text = $"⚠ Missing: {(!hasShow ? "Show" : "")} {(!hasHide ? "Hide" : "")} animation group";
                    statusLabel.style.color = Color.yellow;
                }
                else
                {
                    statusLabel.text = "✗ No animation groups assigned";
                    statusLabel.style.color = Color.red;
                }
            })
            .Every(100);

            box.Add(statusLabel);
            return box;
        }

        VisualElement CreateDebugPanel()
        {
            var box = new Box
            {
                style = {
                    marginTop = 6f,
                    marginBottom = 2f,
                    paddingLeft = 4f,
                    alignItems = Align.Stretch,
                    flexDirection = FlexDirection.Column,
                    flexGrow = 1f,
                }
            };

            box.Add(new Label("Debug")
            {
                style = {
                    marginTop = 5f,
                    marginBottom = 3f,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });

            var buttonGroup = new VisualElement
            {
                style = {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                }
            };

            var showButton = new Button(() => ((UIPanelController)target).Show())
            {
                text = "Show",
                style = { flexGrow = 1f, }
            };

            var hideButton = new Button(() => ((UIPanelController)target).Hide())
            {
                text = "Hide",
                style = { flexGrow = 1f, }
            };

            var showImmediateButton = new Button(() => ((UIPanelController)target).ShowImmediate())
            {
                text = "Show Immediate",
                style = { flexGrow = 1f, }
            };

            var hideImmediateButton = new Button(() => ((UIPanelController)target).HideImmediate())
            {
                text = "Hide Immediate",
                style = { flexGrow = 1f, }
            };

            buttonGroup.Add(showButton);
            buttonGroup.Add(hideButton);
            
            var buttonGroup2 = new VisualElement
            {
                style = {
                    flexDirection = FlexDirection.Row,
                    flexGrow = 1f,
                    marginTop = 2f
                }
            };
            
            buttonGroup2.Add(showImmediateButton);
            buttonGroup2.Add(hideImmediateButton);

            // ✅ Status label
            var statusLabel = new Label("Status: Idle")
            {
                style = {
                    marginTop = 5f,
                    marginBottom = 5f,
                    fontSize = 12f
                }
            };

            // ✅ Update button states and status
            buttonGroup.schedule.Execute(() =>
            {
                var panelController = (UIPanelController)target;
                var isPlayMode = Application.isPlaying;

                // Update button states
                showButton.SetEnabled(isPlayMode);
                hideButton.SetEnabled(isPlayMode);
                showImmediateButton.SetEnabled(isPlayMode);
                hideImmediateButton.SetEnabled(isPlayMode);

                // Update status
                if (isPlayMode)
                {
                    var status = panelController.IsShowing ? "Showing ⏵" : 
                                 panelController.IsHiding ? "Hiding ⏴" : "Idle";
                    statusLabel.text = $"Status: {status}";

                    if (panelController.HasActiveAnimations)
                    {
                        EditorUtility.SetDirty(target);
                        Repaint();
                    }
                }
                else
                {
                    statusLabel.text = "Status: Editor Mode";
                }
            })
            .Every(10);

            box.Add(statusLabel);
            box.Add(buttonGroup);
            box.Add(buttonGroup2);

            return box;
        }

        private void AddRequiredComponents()
        {
            var panelController = (UIPanelController)target;
            
            // ✅ Create Show Animation Group if missing
            if (panelController.ShowAnimationGroup == null)
            {
                CreateAnimationGroup(panelController, "Show Animation Group", true);
            }

            // ✅ Create Hide Animation Group if missing
            if (panelController.HideAnimationGroup == null)
            {
                CreateAnimationGroup(panelController, "Hide Animation Group", false);
            }

            // ✅ Mark as dirty for save
            EditorUtility.SetDirty(panelController);
        }

        private void CreateAnimationGroup(UIPanelController panelController, string groupName, bool isShowGroup)
        {
            // ✅ Create new GameObject for animation group
            var groupGameObject = new GameObject(groupName);
            groupGameObject.transform.SetParent(panelController.transform);
            groupGameObject.transform.localPosition = Vector3.zero;
            groupGameObject.transform.localScale = Vector3.one;

            // ✅ Add UIAnimationGroup component
            var animationGroup = groupGameObject.AddComponent<UIAnimationGroup>();
            
            // ✅ Set default play mode
            var serializedGroup = new SerializedObject(animationGroup);
            var playModeProperty = serializedGroup.FindProperty("_playMode");
            playModeProperty.enumValueIndex = 0; // Parallel for both show and hide
            serializedGroup.ApplyModifiedProperties();

            // ✅ Assign to panel controller using reflection or SerializedProperty
            var serializedPanel = new SerializedObject(panelController);
            var propertyName = isShowGroup ? "_showAnimationGroup" : "_hideAnimationGroup";
            var animationGroupProperty = serializedPanel.FindProperty(propertyName);
            
            if (animationGroupProperty != null)
            {
                animationGroupProperty.objectReferenceValue = animationGroup;
                serializedPanel.ApplyModifiedProperties();
            }

            // ✅ Also set via public method if available
            if (isShowGroup)
            {
                panelController.SetShowAnimationGroup(animationGroup);
            }
            else
            {
                panelController.SetHideAnimationGroup(animationGroup);
            }

            // ✅ Select the newly created object
            Selection.activeGameObject = groupGameObject;
            
            Debug.Log($"Created {groupName} for {panelController.name}");
        }
    }
}
#endif
