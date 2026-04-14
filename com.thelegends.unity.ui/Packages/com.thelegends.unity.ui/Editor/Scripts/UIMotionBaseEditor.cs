using UnityEditor;
using UnityEngine;

namespace TheLegends.Base.UI.Editor
{
    /// <summary>
    /// Custom Inspector for <see cref="UIMotionBase"/> and all its subclasses.
    /// Adds Preview and Reset buttons that work in both Edit Mode and Play Mode,
    /// allowing designers to tune From / To / Ease values without entering Play Mode.
    /// <para>
    /// Edit Mode preview snaps the target component to From or To values directly
    /// using <see cref="SerializedObject"/> reads, then schedules an undo-safe reset
    /// when the Inspector is deselected.
    /// </para>
    /// </summary>
    [CustomEditor(typeof(UIMotionBase), true)]
    public class UIMotionBaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

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
