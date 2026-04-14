using UnityEditor;
using UnityEngine;

namespace TheLegends.Base.UI.Editor
{
    /// <summary>
    /// Custom Inspector for <see cref="UISlideMotion"/>.
    /// Extends <see cref="UIMotionBaseEditor"/> to conditionally show the custom offset Vector2 fields
    /// only when the corresponding direction is set to <see cref="SlideDirection.Custom"/>.
    /// The Offset Multiplier is hidden when both directions are Custom (it has no effect in that case).
    /// </summary>
    [CustomEditor(typeof(UISlideMotion))]
    public class UISlideMotionEditor : UIMotionBaseEditor
    {
        // Slide-specific field names excluded from the base auto-iterator to avoid double-drawing.
        private static readonly string[] SlideFieldExclusions =
        {
            "_target",
            "_showFrom", "_showFromCustomOffset",
            "_hideTo", "_hideToCustomOffset"
        };

        private SerializedProperty _presetProp;
        private SerializedProperty _targetProp;
        private SerializedProperty _showFromProp;
        private SerializedProperty _showFromCustomOffsetProp;
        private SerializedProperty _hideToProp;
        private SerializedProperty _hideToCustomOffsetProp;

        private void OnEnable()
        {
            _presetProp                 = serializedObject.FindProperty("_preset");
            _targetProp                 = serializedObject.FindProperty("_target");
            _showFromProp               = serializedObject.FindProperty("_showFrom");
            _showFromCustomOffsetProp   = serializedObject.FindProperty("_showFromCustomOffset");
            _hideToProp                 = serializedObject.FindProperty("_hideTo");
            _hideToCustomOffsetProp     = serializedObject.FindProperty("_hideToCustomOffset");
        }

        /// <summary>
        /// Draws UIMotionBase fields (preset, order, timing) plus slide-specific direction
        /// fields with conditional custom offset and multiplier visibility.
        /// </summary>
        protected override void DrawMotionFields()
        {
            bool hasPreset = _presetProp.objectReferenceValue != null;

            // ── Base class: Preset & Timing (slide fields excluded to avoid double-draw) ──
            DrawPropertiesWithPresetConditional(_presetProp, hasPreset, SlideFieldExclusions);

            // ── Target ───────────────────────────────────────────────────────────────────
            // PropertyField draws [Header("Target")] automatically from the MonoBehaviour attribute
            EditorGUILayout.PropertyField(_targetProp);

            // ── Show — Enter ─────────────────────────────────────────────────────────────
            // PropertyField draws [Header("Show — Enter")] automatically
            EditorGUILayout.PropertyField(_showFromProp);

            var showFrom = (SlideDirection)_showFromProp.enumValueIndex;

            if (showFrom == SlideDirection.Custom)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_showFromCustomOffsetProp);
                EditorGUI.indentLevel--;
            }

            // ── Hide — Exit ──────────────────────────────────────────────────────────────
            EditorGUILayout.PropertyField(_hideToProp);

            var hideTo = (SlideDirection)_hideToProp.enumValueIndex;

            if (hideTo == SlideDirection.Custom)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(_hideToCustomOffsetProp);
                EditorGUI.indentLevel--;
            }
        }
    }
}
