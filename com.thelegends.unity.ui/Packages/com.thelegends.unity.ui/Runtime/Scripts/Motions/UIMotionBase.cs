using System;
using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Abstract base class for all UI motion components.
    /// Handles shared timing logic (duration, ease, delay), preset overrides,
    /// execution order, and LitMotion handle lifecycle management.
    /// Subclasses only need to implement <see cref="CreateMotion"/> and reset methods.
    /// </summary>
    public abstract class UIMotionBase : MonoBehaviour, IUIMotion
    {
        [Header("Preset (Optional)")]
        [Tooltip("If assigned, timing values (Duration, Ease, Delay) are read from the preset instead of local fields.")]
        [SerializeField] private UIMotionPreset _preset;

        [Header("Timing")]
        [Tooltip("Execution priority for Sequential mode. Lower values execute first.")]
        [SerializeField] private int _order;

        [Tooltip("Duration of the animation in seconds. Overridden by preset if assigned.")]
        [SerializeField] private float _duration = 0.3f;

        [Tooltip("Easing function. Overridden by preset if assigned.")]
        [SerializeField] private Ease _ease = Ease.OutCubic;

        [Tooltip("Delay before the animation starts. Overridden by preset if assigned.")]
        [SerializeField] private float _delay;

        private MotionHandle _handle;

        /// <inheritdoc/>
        public bool IsPlaying => _handle.IsActive();

        /// <inheritdoc/>
        public int Order => _order;

        /// <summary>Effective duration, resolved from preset or local field.</summary>
        protected float Duration => _preset != null ? _preset.Duration : _duration;

        /// <summary>Effective easing, resolved from preset or local field.</summary>
        protected Ease MotionEase => _preset != null ? _preset.Ease : _ease;

        /// <summary>Effective delay, resolved from preset or local field.</summary>
        protected float Delay => _preset != null ? _preset.Delay : _delay;

        /// <inheritdoc/>
        public float TotalDuration => Delay + Duration;

        /// <inheritdoc/>
        public void Play(Action onComplete = null)
        {
            Kill();
            _handle = CreateMotion(false, onComplete);
        }

        /// <inheritdoc/>
        public void PlayReverse(Action onComplete = null)
        {
            Kill();
            _handle = CreateMotion(true, onComplete);
        }

        /// <inheritdoc/>
        public void Kill()
        {
            if (_handle.IsActive())
            {
                _handle.Cancel();
            }
        }

        /// <summary>
        /// Cancels any active tween when the component is destroyed
        /// to prevent callbacks on destroyed objects.
        /// </summary>
        protected virtual void OnDestroy()
        {
            Kill();
        }

        /// <summary>
        /// Creates the LitMotion tween for this motion.
        /// </summary>
        /// <param name="reverse">
        /// If false, animates From → To (show direction).
        /// If true, animates To → From (hide direction).
        /// </param>
        /// <param name="onComplete">Optional callback invoked when the tween finishes.</param>
        /// <returns>A <see cref="MotionHandle"/> for the created tween.</returns>
        protected abstract MotionHandle CreateMotion(bool reverse, Action onComplete);

        /// <inheritdoc/>
        public abstract void ResetToStart();

        /// <inheritdoc/>
        public abstract void ResetToEnd();
    }
}
