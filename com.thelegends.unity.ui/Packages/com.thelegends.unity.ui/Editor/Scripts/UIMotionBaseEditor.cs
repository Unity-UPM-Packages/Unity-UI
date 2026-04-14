using UnityEditor;
using UnityEngine;

namespace TheLegends.Base.UI.Editor
{
    /// <summary>
    /// Custom Inspector for <see cref="UIMotionBase"/> and all its subclasses.
    /// <para>
    /// Conditional timing fields: when a <see cref="UIMotionPreset"/> is assigned,
    /// local Duration / Ease / Delay fields are hidden to prevent confusion — the preset values are shown instead.
    /// When no preset is assigned, local fields are shown for override.
    /// </para>
    /// <para>
    /// Preview buttons: Edit Mode snaps to From/To values with Undo support.
    /// Play Mode runs the actual LitMotion tween.
    /// </para>
    /// </summary>
    [CustomEditor(typeof(UIMotionBase), true)]
    public class UIMotionBaseEditor : UnityEditor.Editor
    {
        // Names of serialized fields that are preset-overridable
        private static readonly string[] TimingFieldNames = { "_duration", "_ease", "_delay" };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var presetProp = serializedObject.FindProperty("_preset");
            bool hasPreset = presetProp.objectReferenceValue != null;

            DrawPropertiesWithConditionalTiming(presetProp, hasPreset);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(8f);
            DrawSeparator();
            EditorGUILayout.Space(4f);

            EditorGUILayout.LabelField("▶  Motion Preview", EditorStyles.boldLabel);

            var motion = (UIMotionBase)target;

            if (Application.isPlaying)
            {
                DrawPlayModeButtons(motion);
            }
            else
            {
                DrawEditModeButtons(motion);
            }
        }

        private void DrawPropertiesWithConditionalTiming(SerializedProperty presetProp, bool hasPreset)
        {
            var iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                // Always skip the internal script reference field
                if (iterator.name == "m_Script")
                {
                    continue;
                }

                // Skip local timing fields when a preset is in control
                if (hasPreset && IsTimingField(iterator.name))
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);

                // When preset changes, re-evaluate and show an info box immediately after the preset field
                if (iterator.name == "_preset")
                {
                    hasPreset = presetProp.objectReferenceValue != null;

                    if (hasPreset)
                    {
                        var preset = (UIMotionPreset)presetProp.objectReferenceValue;
                        EditorGUILayout.HelpBox(
                            $"Using preset values — Duration: {preset.Duration}s  |  Ease: {preset.Ease}  |  Delay: {preset.Delay}s\n" +
                            "Remove the preset to override timing locally.",
                            MessageType.None);
                    }
                }
            }
        }

        private static bool IsTimingField(string fieldName)
        {
            for (int i = 0; i < TimingFieldNames.Length; i++)
            {
                if (TimingFieldNames[i] == fieldName)
                {
                    return true;
                }
            }

            return false;
        }

        private void DrawPlayModeButtons(UIMotionBase motion)
        {
            EditorGUILayout.HelpBox(
                "Play Mode: Runs actual LitMotion tween — previews full animation including easing.",
                MessageType.Info);

            EditorGUILayout.Space(4f);

            EditorGUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = new Color(0.4f, 0.9f, 0.4f);
                if (GUILayout.Button("▶  Play Forward", GUILayout.Height(28f)))
                {
                    motion.Play();
                }

                GUI.backgroundColor = new Color(1f, 0.6f, 0.3f);
                if (GUILayout.Button("◀  Play Reverse", GUILayout.Height(28f)))
                {
                    motion.PlayReverse();
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = new Color(0.6f, 0.8f, 1f);
                if (GUILayout.Button("⏮  Reset to Start"))
                {
                    motion.Kill();
                    motion.ResetToStart();
                }

                GUI.backgroundColor = new Color(1f, 0.8f, 0.4f);
                if (GUILayout.Button("⏭  Reset to End"))
                {
                    motion.Kill();
                    motion.ResetToEnd();
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();

            if (motion.IsPlaying)
            {
                var prevColor = GUI.color;
                GUI.color = new Color(0.4f, 1f, 0.6f);
                EditorGUILayout.LabelField("● Playing...", EditorStyles.boldLabel);
                GUI.color = prevColor;

                // Trigger repaint so label refreshes while animation is active
                Repaint();
            }
        }

        private void DrawEditModeButtons(UIMotionBase motion)
        {
            EditorGUILayout.HelpBox(
                "Edit Mode: Instantly snaps to From / To values for quick visual tuning.\n" +
                "Enter Play Mode to preview the actual animated tween.",
                MessageType.Info);

            EditorGUILayout.Space(4f);

            EditorGUILayout.BeginHorizontal();
            {
                GUI.backgroundColor = new Color(0.6f, 0.8f, 1f);
                if (GUILayout.Button("⏮  Snap to Start", GUILayout.Height(28f)))
                {
                    Undo.RecordObject(motion, "Preview Motion Start");
                    motion.ResetToStart();
                    EditorUtility.SetDirty(motion);
                }

                GUI.backgroundColor = new Color(0.4f, 0.9f, 0.4f);
                if (GUILayout.Button("⏭  Snap to End", GUILayout.Height(28f)))
                {
                    Undo.RecordObject(motion, "Preview Motion End");
                    motion.ResetToEnd();
                    EditorUtility.SetDirty(motion);
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSeparator()
        {
            var rect = EditorGUILayout.GetControlRect(false, 1f);
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
        }
    }
}
