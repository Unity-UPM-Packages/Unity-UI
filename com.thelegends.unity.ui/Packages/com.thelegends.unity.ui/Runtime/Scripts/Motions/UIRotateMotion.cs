using System;
using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Tweens a <see cref="Transform.localEulerAngles"/> between two Vector3 values.
    /// Commonly used for spin-in/tilt effects on UI elements.
    /// If no target is assigned, automatically uses this GameObject's Transform.
    /// </summary>
    [AddComponentMenu("TheLegends/UI/Motions/Rotate")]
    public sealed class UIRotateMotion : UIMotionBase
    {
        [Header("Target")]
        [Tooltip("Target Transform to animate. If empty, uses this GameObject's Transform.")]
        [SerializeField] private Transform _target;

        [Header("Values")]
        [Tooltip("Euler angles at the start of show animation (and end of hide animation).")]
        [SerializeField] private Vector3 _from = Vector3.zero;

        [Tooltip("Euler angles at the end of show animation (and start of hide animation).")]
        [SerializeField] private Vector3 _to = new Vector3(0f, 0f, 360f);

        private void Awake()
        {
            EnsureTarget();
        }

        /// <inheritdoc/>
        protected override MotionHandle CreateMotion(bool reverse, Action onComplete)
        {
            EnsureTarget();

            Vector3 start = reverse ? _to : _from;
            Vector3 end = reverse ? _from : _to;

            return LMotion.Create(start, end, Duration)
                .WithEase(MotionEase)
                .WithDelay(Delay)
                .WithOnComplete(() => onComplete?.Invoke())
                .Bind(_target, (value, target) => target.localEulerAngles = value);
        }

        /// <inheritdoc/>
        public override void ResetToStart()
        {
            EnsureTarget();
            _target.localEulerAngles = _from;
        }

        /// <inheritdoc/>
        public override void ResetToEnd()
        {
            EnsureTarget();
            _target.localEulerAngles = _to;
        }

        private void EnsureTarget()
        {
            if (_target == null)
            {
                _target = transform;
            }
        }
    }
}
