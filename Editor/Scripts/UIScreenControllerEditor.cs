using UnityEditor;
using UnityEngine;

namespace TheLegends.Base.UI.Editor
{
    /// <summary>
    /// Custom Inspector for <see cref="UIScreenController"/>.
    /// Adds Show / Hide test buttons that work during Play Mode, allowing
    /// full orchestration testing (parallel/sequential, lifecycle events) without
    /// writing any test code.
    /// </summary>
    [CustomEditor(typeof(UIScreenController), true)]
    public class UIScreenControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(8f);
            DrawSeparator();
            EditorGUILayout.Space(4f);

            EditorGUILayout.LabelField("▶  Playback Test", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(
                "Buttons below only work in Play Mode.\n" +
                "Use Show / Hide to verify transitions and lifecycle events.",
                MessageType.Info);

            EditorGUILayout.Space(4f);

            bool isPlaying = Application.isPlaying;

            EditorGUI.BeginDisabledGroup(!isPlaying);
            {
                var controller = (UIScreenController)target;
                DrawStateLabel(controller);

                EditorGUILayout.Space(4f);

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.backgroundColor = new Color(0.4f, 0.9f, 0.4f);
                    if (GUILayout.Button("▶  Show", GUILayout.Height(30f)))
                    {
                        controller.Show();
                    }

                    GUI.backgroundColor = new Color(0.9f, 0.4f, 0.4f);
                    if (GUILayout.Button("■  Hide", GUILayout.Height(30f)))
                    {
                        controller.Hide();
                    }

                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                {
                    GUI.backgroundColor = new Color(0.6f, 0.8f, 1f);
                    if (GUILayout.Button("Show Immediate"))
                    {
                        controller.ShowImmediate();
                    }

                    GUI.backgroundColor = new Color(1f, 0.8f, 0.4f);
                    if (GUILayout.Button("Hide Immediate"))
                    {
                        controller.HideImmediate();
                    }

                    GUI.backgroundColor = Color.white;
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();

            if (!isPlaying)
            {
                EditorGUILayout.HelpBox("Enter Play Mode to enable test buttons.", MessageType.None);
            }
        }

        private void DrawStateLabel(UIScreenController controller)
        {
            string stateName = controller.State.ToString();
            Color stateColor = controller.State switch
            {
                UIScreenState.Visible  => new Color(0.3f, 0.9f, 0.3f),
                UIScreenState.Hidden   => new Color(0.6f, 0.6f, 0.6f),
                UIScreenState.Showing  => new Color(0.4f, 0.8f, 1f),
                UIScreenState.Hiding   => new Color(1f, 0.7f, 0.3f),
                _                      => Color.white
            };

            var prevColor = GUI.color;
            GUI.color = stateColor;
            EditorGUILayout.LabelField($"Current State:  {stateName}", EditorStyles.boldLabel);
            GUI.color = prevColor;
        }

        private static void DrawSeparator()
        {
            var rect = EditorGUILayout.GetControlRect(false, 1f);
            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 0.5f));
        }
    }
}
