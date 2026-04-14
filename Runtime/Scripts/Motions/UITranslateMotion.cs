using System;
using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Tweens a <see cref="RectTransform.anchoredPosition"/> between two explicitly defined positions.
    /// <para>
    /// Unlike <see cref="UISlideMotion"/>, this component is entirely screen-agnostic and is intended
    /// for arbitrary position transitions — for example, a sidebar panel shifting 200px to reveal a sub-menu,
    /// or a card sliding from one grid cell to another.
    /// </para>
    /// <para>
    /// <see cref="ResetToStart"/> snaps to <c>_from</c> (hidden state).
    /// <see cref="ResetToEnd"/> snaps to <c>_to</c> (visible state).
    /// </para>
    /// </summary>
    [AddComponentMenu("TheLegends/UI/Motions/Translate")]
    public sealed class UITranslateMotion : UIMotionBase
    {
        [Header("Target")]
        [Tooltip("Target RectTransform to animate. If empty, uses the RectTransform on this GameObject.")]
        [SerializeField] private RectTransform _target;

        [Header("Positions")]
        [Tooltip("Start anchoredPosition — where the element is when the screen is hidden.")]
        [SerializeField] private Vector2 _from;

        [Tooltip("End anchoredPosition — where the element is when the screen is fully visible.")]
        [SerializeField] private Vector2 _to;

        /// <inheritdoc/>
        protected override MotionHandle CreateMotion(bool reverse, Action onComplete)
        {
            EnsureTarget();

            Vector2 start = reverse ? _to : _from;
            Vector2 end   = reverse ? _from : _to;

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
