#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace TheLegends.Base.UI.Editor
{
    [CustomEditor(typeof(UIAnimationGroup))]
    public class UIAnimationGroupEditor : UnityEditor.Editor
    {
        private Button _addButton;
        private AddAnimationDropdown _dropdown;

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // ✅ Default inspector content
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // ✅ Add Animation button
            root.Add(CreateAddAnimationButton());

            // ✅ Add debug panel
            root.Add(CreateDebugPanel());

            return root;
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

            var playButton = new Button(() => ((IUIPlayable)target).Play())
            {
                text = "Play",
                style = { flexGrow = 1f, }
            };

            var restartButton = new Button(() => ((IUIPlayable)target).Restart())
            {
                text = "Restart",
                style = { flexGrow = 1f, }
            };

            var pauseButton = new Button(() => ((IUIPlayable)target).Pause())
            {
                text = "Pause",
                style = { flexGrow = 1f, }
            };

            var stopButton = new Button(() => ((IUIPlayable)target).Stop())
            {
                text = "Stop",
                style = { flexGrow = 1f, }
            };

            buttonGroup.Add(playButton);
            buttonGroup.Add(restartButton);
            buttonGroup.Add(pauseButton);
            buttonGroup.Add(stopButton);

            // ✅ Status label
            var statusLabel = new Label("Status: Stopped")
            {
                style = {
                    marginTop = 5f,
                    marginBottom = 5f,
                    fontSize = 12f
                }
            };

            // ✅ Update button states and status (following LitMotion logic)
            buttonGroup.schedule.Execute(() =>
            {
                var playable = (IUIPlayable)target;
                var animationGroup = (UIAnimationGroup)target;
                var isPlayMode = Application.isPlaying;

                // Update button states - Play always enabled for resume
                playButton.SetEnabled(isPlayMode);                        // Play always enabled (can resume)
                restartButton.SetEnabled(isPlayMode && playable.IsActive); // Restart when active
                pauseButton.SetEnabled(isPlayMode && playable.IsActive);   // Pause when active  
                stopButton.SetEnabled(isPlayMode && playable.IsActive);    // Stop when active

                // Update status
                if (isPlayMode)
                {
                    var status = playable.IsPlaying ? "Playing ⏵" : 
                                 playable.IsActive ? "Paused ⏸" : "Stopped ⏹";
                    var animationCount = animationGroup.Animations?.Count ?? 0;
                    statusLabel.text = $"Status: {status} | Animations: {animationCount}";

                    if (playable.IsPlaying)
                    {
                        EditorUtility.SetDirty(target);
                        Repaint();
                    }
                }
                else
                {
                    var animationCount = animationGroup.Animations?.Count ?? 0;
                    statusLabel.text = $"Status: Editor Mode | Animations: {animationCount}";
                }
            })
            .Every(10);

            box.Add(statusLabel);
            box.Add(buttonGroup);

            return box;
        }

        VisualElement CreateAddAnimationButton()
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

            box.Add(new Label("Add Animation")
            {
                style = {
                    marginTop = 5f,
                    marginBottom = 3f,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            });

            _addButton = new Button()
            {
                text = "Add Animation...",
                name = "AddAnimationButton",
                style = {
                    width = 200f,
                    alignSelf = Align.Center,
                    marginTop = 5f,
                    marginBottom = 5f
                }
            };
            
            // Create dropdown instance once (like LitMotion)
            _dropdown = new AddAnimationDropdown(target as UIAnimationGroup, Vector2.zero);
            _addButton.clicked += () => _dropdown.Show(_addButton.worldBound);

            box.Add(_addButton);
            return box;
        }
    }
}
#endif