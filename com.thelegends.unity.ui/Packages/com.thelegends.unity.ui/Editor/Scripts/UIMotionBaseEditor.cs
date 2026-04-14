using UnityEditor;
using UnityEngine;

namespace TheLegends.Base.UI.Editor
{
    /// <summary>
    /// Custom Inspector for <see cref="UIMotionBase"/> and all its subclasses.
    /// <para>
    /// Conditional timing fields: when a <see cref="UIMotionPreset"/> is assigned,
    /// local Duration / Ease / Delay fields are hidden and replaced by a readable preset summary.
    /// </para>
    /// <para>
    /// Subclasses can override <see cref="DrawMotionFields"/> to add custom conditional field logic
    /// while still inheriting shared timing and preview button behaviour.
    /// </para>
    /// </summary>
    [CustomEditor(typeof(UIMotionBase), true)]
    public class UIMotionBaseEditor : UnityEditor.Editor
    {
        private static readonly string[] TimingFieldNames = { "_duration", "_ease", "_delay" };

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawMotionFields();
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

        /// <summary>
        /// Draws the motion component's serialized fields.
        /// Override in subclasses to add custom conditional field visibility.
        /// </summary>
        protected virtual void DrawMotionFields()
        {
            var presetProp = serializedObject.FindProperty("_preset");
            bool hasPreset = presetProp.objectReferenceValue != null;

            DrawPropertiesWithPresetConditional(presetProp, hasPreset);
        }

        /// <summary>
        /// Iterates all serialized properties and hides timing fields when a preset is assigned.
        /// </summary>
        /// <param name="extraExclusions">
        /// Optional additional field names to skip during iteration.
        /// Use in subclass editors to prevent auto-drawing fields that will be drawn manually.
        /// </param>
        protected void DrawPropertiesWithPresetConditional(SerializedProperty presetProp, bool hasPreset, string[] extraExclusions = null)
        {
            var iterator = serializedObject.GetIterator();
            bool enterChildren = true;

            while (iterator.NextVisible(enterChildren))
            {
                enterChildren = false;

                if (iterator.name == "m_Script")
                {
                    continue;
                }

                if (hasPreset && IsTimingField(iterator.name))
                {
                    continue;
                }

                if (IsExcluded(iterator.name, extraExclusions))
                {
                    continue;
                }

                EditorGUILayout.PropertyField(iterator, true);

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

        /// <summary>Draws the Play Mode test buttons (actual LitMotion tween).</summary>
        protected void DrawPlayModeButtons(UIMotionBase motion)
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

                Repaint();
            }
        }

        /// <summary>Draws the Edit Mode snap buttons (instant position only, no tween).</summary>
        protected void DrawEditModeButtons(UIMotionBase motion)
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

        /// <summary>Draws a thin horizontal separator line.</summary>
        protected static void DrawSeparator()
        {
            var rect = EditorGUILayout.GetControlRect(false, 1f);
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
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

        private static bool IsExcluded(string fieldName, string[] exclusions)
        {
            if (exclusions == null)
            {
                return false;
            }

            for (int i = 0; i < exclusions.Length; i++)
            {
                if (exclusions[i] == fieldName)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
