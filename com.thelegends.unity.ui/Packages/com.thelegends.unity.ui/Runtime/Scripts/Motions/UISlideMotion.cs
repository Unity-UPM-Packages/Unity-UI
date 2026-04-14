using System;
using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Tweens a <see cref="RectTransform.anchoredPosition"/> between two Vector2 values.
    /// Commonly used for slide-in/slide-out screen transitions.
    /// If no target is assigned, automatically uses the RectTransform on this GameObject.
    /// </summary>
    [AddComponentMenu("TheLegends/UI/Motions/Slide")]
    public sealed class UISlideMotion : UIMotionBase
    {
        [Header("Target")]
        [Tooltip("Target RectTransform to animate. If empty, uses RectTransform on this GameObject.")]
        [SerializeField] private RectTransform _target;

        [Header("Values")]
        [Tooltip("Anchored position at the start of show animation (and end of hide animation).")]
        [SerializeField] private Vector2 _from;

        [Tooltip("Anchored position at the end of show animation (and start of hide animation).")]
        [SerializeField] private Vector2 _to;

        private void Awake()
        {
            EnsureTarget();
        }

        /// <inheritdoc/>
        protected override MotionHandle CreateMotion(bool reverse, Action onComplete)
        {
            EnsureTarget();

            Vector2 start = reverse ? _to : _from;
            Vector2 end = reverse ? _from : _to;

            return LMotion.Create(start, end, Duration)
                .WithEase(MotionEase)
                .WithDelay(Delay)
                .WithOnComplete(() => onComplete?.Invoke())
                .Bind(_target, (value, target) => target.anchoredPosition = value);
        }

        /// <inheritdoc/>
        public override void ResetToStart()
        {
            EnsureTarget();
            _target.anchoredPosition = _from;
        }

        /// <inheritdoc/>
        public override void ResetToEnd()
        {
            EnsureTarget();
            _target.anchoredPosition = _to;
        }

        private void EnsureTarget()
        {
            if (_target == null)
            {
                _target = GetComponent<RectTransform>();
            }
        }
    }
}
