using System;
using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Tweens a <see cref="RectTransform.anchoredPosition"/> to create slide-in and slide-out transitions.
    /// <para>
    /// Enter and exit directions are configured independently via <see cref="SlideDirection"/>.
    /// Screen-relative directions (Left, Right, Up, Down) compute their offset dynamically from
    /// the root Canvas size plus the element's own size, ensuring correct behaviour across all resolutions.
    /// </para>
    /// <para>
    /// The element's <b>rest position</b> (the anchoredPosition set in the Editor) is captured once
    /// on first use and serves as the fully-visible anchor point for all offset calculations.
    /// </para>
    /// </summary>
    [AddComponentMenu("TheLegends/UI/Motions/Slide")]
    public sealed class UISlideMotion : UIMotionBase
    {
        [Header("Target")]
        [Tooltip("Target RectTransform to animate. If empty, uses RectTransform on this GameObject.")]
        [SerializeField] private RectTransform _target;

        [Header("Show — Enter")]
        [Tooltip("Direction the element slides FROM when showing. Custom = use manual offset below.")]
        [SerializeField] private SlideDirection _showFrom = SlideDirection.Left;

        [Tooltip("Manual offset applied from the rest position. Only used when Show direction is Custom.")]
        [SerializeField] private Vector2 _showFromCustomOffset = new Vector2(-500f, 0f);

        [Header("Hide — Exit")]
        [Tooltip("Direction the element slides TO when hiding. Custom = use manual offset below.")]
        [SerializeField] private SlideDirection _hideTo = SlideDirection.Right;

        [Tooltip("Manual offset applied from the rest position. Only used when Hide direction is Custom.")]
        [SerializeField] private Vector2 _hideToCustomOffset = new Vector2(500f, 0f);

        // 10% safety buffer ensures the element clears the screen edge regardless of anchor/pivot setup.
        private const float OffsetSafetyBuffer = 1.1f;

        private RectTransform _canvasRect;
        private Vector2 _restPosition;
        private bool _isInitialized;

        /// <inheritdoc/>
        protected override MotionHandle CreateMotion(bool reverse, Action onComplete)
        {
            EnsureInitialized();

            Vector2 start;
            Vector2 end;

            if (!reverse)
            {
                start = ResolvePosition(_showFrom, _showFromCustomOffset);
                end = _restPosition;
            }
            else
            {
                start = _restPosition;
                end = ResolvePosition(_hideTo, _hideToCustomOffset);
            }

            return LMotion.Create(start, end, Duration)
                .WithEase(MotionEase)
                .WithDelay(Delay)
                .WithOnComplete(() => onComplete?.Invoke())
                .Bind(_target, (value, target) => target.anchoredPosition = value);
        }

        /// <inheritdoc/>
        public override void ResetToStart()
        {
            EnsureInitialized();
            _target.anchoredPosition = ResolvePosition(_showFrom, _showFromCustomOffset);
        }

        /// <inheritdoc/>
        public override void ResetToEnd()
        {
            EnsureInitialized();
            _target.anchoredPosition = _restPosition;
        }

        /// <summary>
        /// Lazy initializer — safe to call before Awake due to potential sibling component ordering.
        /// Captures the designed rest position and caches the Canvas reference exactly once.
        /// </summary>
        private void EnsureInitialized()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            if (_target == null)
            {
                _target = GetComponent<RectTransform>();
            }

            // Capture the designer-placed position as the fully-visible anchor point
            _restPosition = _target.anchoredPosition;
            CacheCanvasRect();
        }

        private void CacheCanvasRect()
        {
            var canvas = GetComponentInParent<Canvas>();

            if (canvas != null)
            {
                _canvasRect = canvas.rootCanvas.GetComponent<RectTransform>();
            }
        }

        /// <summary>
        /// Resolves the target anchoredPosition for a given direction.
        /// Screen-relative directions compute an offset from the rest position using
        /// the Canvas and element dimensions to guarantee full off-screen placement.
        /// </summary>
        private Vector2 ResolvePosition(SlideDirection direction, Vector2 customOffset)
        {
            if (direction == SlideDirection.Custom)
            {
                return _restPosition + customOffset;
            }

            if (_canvasRect == null)
            {
                // Fallback: large offset that works on most mobile resolutions
                Debug.LogWarning($"[UISlideMotion] Canvas not found on {name}. Using fallback offset.", this);
                return _restPosition + GetFallbackOffset(direction);
            }

            float canvasHalfW = _canvasRect.rect.width * 0.5f;
            float canvasHalfH = _canvasRect.rect.height * 0.5f;
            float elementHalfW = _target.rect.width * 0.5f;
            float elementHalfH = _target.rect.height * 0.5f;

            // Buffer ensures element center travels past edge by adding its own half-size
            float bufferX = (canvasHalfW + elementHalfW) * OffsetSafetyBuffer;
            float bufferY = (canvasHalfH + elementHalfH) * OffsetSafetyBuffer;

            Vector2 delta;

            switch (direction)
            {
                case SlideDirection.Left:  delta = new Vector2(-bufferX, 0f);  break;
                case SlideDirection.Right: delta = new Vector2( bufferX, 0f);  break;
                case SlideDirection.Up:    delta = new Vector2(0f,  bufferY);  break;
                case SlideDirection.Down:  delta = new Vector2(0f, -bufferY);  break;
                default:                   delta = Vector2.zero;               break;
            }

            return _restPosition + delta;
        }

        private static Vector2 GetFallbackOffset(SlideDirection direction)
        {
            const float FallbackDistance = 2000f;

            switch (direction)
            {
                case SlideDirection.Left:  return new Vector2(-FallbackDistance, 0f);
                case SlideDirection.Right: return new Vector2( FallbackDistance, 0f);
                case SlideDirection.Up:    return new Vector2(0f,  FallbackDistance);
                case SlideDirection.Down:  return new Vector2(0f, -FallbackDistance);
                default:                   return Vector2.zero;
            }
        }
    }
}
