using System;
using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Tweens a <see cref="CanvasGroup.alpha"/> between two values.
    /// Commonly used for fade-in/fade-out screen transitions.
    /// If no target is assigned, automatically uses the CanvasGroup on this GameObject.
    /// </summary>
    [AddComponentMenu("TheLegends/UI/Motions/Fade")]
    public sealed class UIFadeMotion : UIMotionBase
    {
        [Header("Target")]
        [Tooltip("Target CanvasGroup to animate. If empty, uses CanvasGroup on this GameObject.")]
        [SerializeField] private CanvasGroup _target;

        [Header("Values")]
        [Tooltip("Alpha value at the start of show animation (and end of hide animation).")]
        [SerializeField] private float _from;

        [Tooltip("Alpha value at the end of show animation (and start of hide animation).")]
        [SerializeField] private float _to = 1f;

        private void Awake()
        {
            EnsureTarget();
        }

        /// <inheritdoc/>
        protected override MotionHandle CreateMotion(bool reverse, Action onComplete)
        {
            EnsureTarget();

            float start = reverse ? _to : _from;
            float end = reverse ? _from : _to;

            return LMotion.Create(start, end, Duration)
                .WithEase(MotionEase)
                .WithDelay(Delay)
                .WithOnComplete(() => onComplete?.Invoke())
                .Bind(_target, (value, target) => target.alpha = value);
        }

        /// <inheritdoc/>
        public override void ResetToStart()
        {
            EnsureTarget();
            _target.alpha = _from;
        }

        /// <inheritdoc/>
        public override void ResetToEnd()
        {
            EnsureTarget();
            _target.alpha = _to;
        }

        private void EnsureTarget()
        {
            if (_target == null)
            {
                _target = GetComponent<CanvasGroup>();
            }
        }
    }
}
