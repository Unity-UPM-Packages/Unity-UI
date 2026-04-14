using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Shareable timing configuration for UI motions.
    /// Create via Assets menu: TheLegends > UI > Motion Preset.
    /// Multiple motion components can reference the same preset to ensure consistent timing.
    /// When assigned to a motion component, the preset values override the component's local timing fields.
    /// </summary>
    [CreateAssetMenu(fileName = "New UIMotionPreset", menuName = "TheLegends/UI/Motion Preset")]
    public class UIMotionPreset : ScriptableObject
    {
        [Tooltip("Duration of the animation in seconds.")]
        [SerializeField] private float _duration = 0.3f;

        [Tooltip("Easing function applied to the animation curve.")]
        [SerializeField] private Ease _ease = Ease.OutCubic;

        [Tooltip("Delay before the animation starts, in seconds.")]
        [SerializeField] private float _delay;

        /// <summary>Duration of the animation in seconds.</summary>
        public float Duration => _duration;

        /// <summary>Easing function applied to the animation curve.</summary>
        public Ease Ease => _ease;

        /// <summary>Delay before the animation starts, in seconds.</summary>
        public float Delay => _delay;
    }
}
