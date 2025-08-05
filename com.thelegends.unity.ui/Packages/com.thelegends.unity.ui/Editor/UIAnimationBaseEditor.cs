#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace TheLegends.Base.UI.Editor
{
    [CustomEditor(typeof(UIAnimationBase), true)]
    public class UIAnimationBaseEditor : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();

            // ✅ Default inspector content
            InspectorElement.FillDefaultInspector(root, serializedObject, this);

            // ✅ Only show debug controls for IUIPlayable implementations
            if (target is IUIPlayable)
            {
                root.Add(CreateDebugPanel());
            }

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
                    statusLabel.text = $"Status: {status}";

                    // ✅ Auto-refresh inspector while playing
                    if (playable.IsPlaying)
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

            return box;
        }
    }
}
#endif