using System;
using UnityEngine;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Main controller for a UI screen. Attach to any RectTransform that represents a screen.
    /// Automatically collects all <see cref="IUIMotion"/> components on the same GameObject
    /// and orchestrates them during Show/Hide transitions.
    /// <para>
    /// Supports both <see cref="UITransitionMode.Parallel"/> (all motions at once) and
    /// <see cref="UITransitionMode.Sequential"/> (motions in order) for Show and Hide independently.
    /// </para>
    /// <para>
    /// Fires lifecycle events: <see cref="OnShowing"/>, <see cref="OnShown"/>,
    /// <see cref="OnHiding"/>, <see cref="OnHidden"/>.
    /// </para>
    /// </summary>
    [AddComponentMenu("TheLegends/UI/Screen Controller")]
    public class UIScreenController : MonoBehaviour, IUIScreenController
    {
        [Header("Transition Settings")]
        [Tooltip("How motions execute when showing the screen.")]
        [SerializeField] private UITransitionMode _showMode = UITransitionMode.Parallel;

        [Tooltip("How motions execute when hiding the screen.")]
        [SerializeField] private UITransitionMode _hideMode = UITransitionMode.Parallel;

        [Tooltip("If true, the screen starts hidden (all motions reset to start, GameObject deactivated).")]
        [SerializeField] private bool _hideOnAwake = true;

        private IUIMotion[] _motions;
        private int _completedCount;

        /// <inheritdoc/>
        public UIScreenState State { get; private set; } = UIScreenState.Hidden;

        /// <inheritdoc/>
        public event Action OnShowing;

        /// <inheritdoc/>
        public event Action OnShown;

        /// <inheritdoc/>
        public event Action OnHiding;

        /// <inheritdoc/>
        public event Action OnHidden;

        /// <summary>
        /// Collects all motion components and optionally hides the screen on startup.
        /// </summary>
        protected virtual void Awake()
        {
            CollectMotions();

            if (_hideOnAwake)
            {
                HideImmediate();
            }
        }

        /// <inheritdoc/>
        public void Show()
        {
            if (State == UIScreenState.Visible || State == UIScreenState.Showing)
            {
                return;
            }

            KillAllMotions();
            gameObject.SetActive(true);
            State = UIScreenState.Showing;
            OnShowing?.Invoke();

            if (_motions == null || _motions.Length == 0)
            {
                CompleteShow();
                return;
            }

            ExecuteMotions(_showMode, false, CompleteShow);
        }

        /// <inheritdoc/>
        public void Hide()
        {
            if (State == UIScreenState.Hidden || State == UIScreenState.Hiding)
            {
                return;
            }

            KillAllMotions();
            State = UIScreenState.Hiding;
            OnHiding?.Invoke();

            if (_motions == null || _motions.Length == 0)
            {
                CompleteHide();
                return;
            }

            ExecuteMotions(_hideMode, true, CompleteHide);
        }

        /// <summary>
        /// Instantly hides the screen without animation. Resets all motions to their start values
        /// and deactivates the GameObject. Does NOT fire lifecycle events.
        /// Use for initialization and non-interactive state resets.
        /// </summary>
        public void HideImmediate()
        {
            KillAllMotions();

            if (_motions != null)
            {
                for (int i = 0; i < _motions.Length; i++)
                {
                    _motions[i].ResetToStart();
                }
            }

            State = UIScreenState.Hidden;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Instantly shows the screen without animation. Sets all motions to their end values
        /// and activates the GameObject. Does NOT fire lifecycle events.
        /// Use for initialization and non-interactive state resets.
        /// </summary>
        public void ShowImmediate()
        {
            KillAllMotions();
            gameObject.SetActive(true);

            if (_motions != null)
            {
                for (int i = 0; i < _motions.Length; i++)
                {
                    _motions[i].ResetToEnd();
                }
            }

            State = UIScreenState.Visible;
        }

        /// <summary>
        /// Sets the show transition mode at runtime.
        /// </summary>
        /// <param name="mode">The transition mode to use when showing.</param>
        public void SetShowMode(UITransitionMode mode)
        {
            _showMode = mode;
        }

        /// <summary>
        /// Sets the hide transition mode at runtime.
        /// </summary>
        /// <param name="mode">The transition mode to use when hiding.</param>
        public void SetHideMode(UITransitionMode mode)
        {
            _hideMode = mode;
        }

        /// <summary>
        /// Cleans up all active motions and clears event subscribers on destroy.
        /// </summary>
        protected virtual void OnDestroy()
        {
            KillAllMotions();
            OnShowing = null;
            OnShown = null;
            OnHiding = null;
            OnHidden = null;
        }

        private void CollectMotions()
        {
            _motions = GetComponents<IUIMotion>();
            Array.Sort(_motions, (a, b) => a.Order.CompareTo(b.Order));
        }

        private void CompleteShow()
        {
            State = UIScreenState.Visible;
            OnShown?.Invoke();
        }

        private void CompleteHide()
        {
            State = UIScreenState.Hidden;
            OnHidden?.Invoke();
            gameObject.SetActive(false);
        }

        private void ExecuteMotions(UITransitionMode mode, bool reverse, Action onAllComplete)
        {
            if (mode == UITransitionMode.Parallel)
            {
                ExecuteParallel(reverse, onAllComplete);
            }
            else
            {
                // Hide Sequential starts from the last motion (LIFO) to naturally mirror the show order.
                int startIndex = reverse ? _motions.Length - 1 : 0;
                ExecuteSequential(reverse, startIndex, onAllComplete);
            }
        }

        private void ExecuteParallel(bool reverse, Action onAllComplete)
        {
            _completedCount = 0;
            int total = _motions.Length;

            for (int i = 0; i < _motions.Length; i++)
            {
                if (reverse)
                {
                    _motions[i].PlayReverse(() => HandleMotionCompleted(total, onAllComplete));
                }
                else
                {
                    _motions[i].Play(() => HandleMotionCompleted(total, onAllComplete));
                }
            }
        }

        private void HandleMotionCompleted(int total, Action onAllComplete)
        {
            _completedCount++;

            if (_completedCount >= total)
            {
                onAllComplete?.Invoke();
            }
        }

        private void ExecuteSequential(bool reverse, int index, Action onAllComplete)
        {
            bool outOfBounds = reverse ? index < 0 : index >= _motions.Length;

            if (outOfBounds)
            {
                onAllComplete?.Invoke();
                return;
            }

            int nextIndex = reverse ? index - 1 : index + 1;

            if (reverse)
            {
                _motions[index].PlayReverse(() => ExecuteSequential(true, nextIndex, onAllComplete));
            }
            else
            {
                _motions[index].Play(() => ExecuteSequential(false, nextIndex, onAllComplete));
            }
        }

        private void KillAllMotions()
        {
            if (_motions == null)
            {
                return;
            }

            for (int i = 0; i < _motions.Length; i++)
            {
                _motions[i].Kill();
            }
        }
    }
}
