using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{
    /// <summary>
    /// Component that manages a group of UIAnimationBase animations
    /// Supports both parallel and sequential playback modes
    /// </summary>
    [AddComponentMenu("UI/Animation Group")]
    public class UIAnimationGroup : MonoBehaviour, IUIPlayable
    {
        public enum PlayMode
        {
            Parallel,    // All animations play simultaneously
            Sequential   // Animations play one after another
        }

        [Header("Animation Group Settings")]
        [SerializeField] private PlayMode _playMode = PlayMode.Parallel;
        [SerializeField] private List<UIAnimationBase> _animations = new List<UIAnimationBase>();

        [Header("Events")]
        [SerializeField] private UnityEvent _onGroupStart;
        [SerializeField] private UnityEvent _onGroupComplete;

        // ✅ State tracking
        private int _activeAnimations = 0;
        private int _completedAnimations = 0;
        private bool _isPlaying = false;
        private Queue<UIAnimationBase> _sequentialQueue = new Queue<UIAnimationBase>();

        // ✅ Interface properties
        public bool IsActive => _activeAnimations > 0;
        public bool IsPlaying => _isPlaying;

        // ✅ Public properties
        public PlayMode CurrentPlayMode => _playMode;
        public List<UIAnimationBase> Animations => _animations;
        public UnityEvent OnGroupStart => _onGroupStart;
        public UnityEvent OnGroupComplete => _onGroupComplete;

        #region IUIPlayable Implementation

        public void Play()
        {
            if (_animations == null || _animations.Count == 0) 
            {
                Debug.LogWarning($"[UIAnimationGroup] No animations assigned to {gameObject.name}");
                return;
            }

            _isPlaying = true;
            _activeAnimations = 0;
            _completedAnimations = 0;

            // ✅ Fire start events
            _onGroupStart?.Invoke();

            switch (_playMode)
            {
                case PlayMode.Parallel:
                    PlayParallel();
                    break;
                case PlayMode.Sequential:
                    PlaySequential();
                    break;
            }
        }

        public void Stop()
        {
            foreach (var animation in _animations)
            {
                if (animation != null && animation is IUIPlayable playable)
                {
                    playable.Stop();
                }
            }

            _isPlaying = false;
            _activeAnimations = 0;
            _sequentialQueue.Clear();
            CleanupListeners();
        }

        public void Pause()
        {
            foreach (var animation in _animations)
            {
                if (animation != null && animation is IUIPlayable playable)
                {
                    playable.Pause();
                }
            }
        }

        public void Restart()
        {
            Stop();
            Play();
        }

        #endregion

        #region Private Methods

        private void PlayParallel()
        {
            foreach (var animation in _animations)
            {
                if (animation != null)
                {
                    _activeAnimations++;

                    // ✅ Subscribe to completion
                    animation.OnAnimationComplete.AddListener(OnParallelAnimationComplete);
                    animation.Play();
                }
            }

            // ✅ Handle case where no valid animations
            if (_activeAnimations == 0)
            {
                OnAllAnimationsComplete();
            }
        }

        private void PlaySequential()
        {
            _sequentialQueue.Clear();
            
            // ✅ Add valid animations to queue
            foreach (var animation in _animations)
            {
                if (animation != null)
                {
                    _sequentialQueue.Enqueue(animation);
                }
            }

            if (_sequentialQueue.Count > 0)
            {
                _activeAnimations = 1;
                PlayNextSequentialAnimation();
            }
            else
            {
                OnAllAnimationsComplete();
            }
        }

        private void PlayNextSequentialAnimation()
        {
            if (_sequentialQueue.Count > 0)
            {
                var nextAnimation = _sequentialQueue.Dequeue();
                nextAnimation.OnAnimationComplete.AddListener(OnSequentialAnimationComplete);
                nextAnimation.Play();
            }
            else
            {
                OnAllAnimationsComplete();
            }
        }

        private void OnParallelAnimationComplete()
        {
            _completedAnimations++;
            if (_completedAnimations >= _activeAnimations)
            {
                OnAllAnimationsComplete();
            }
        }

        private void OnSequentialAnimationComplete()
        {
            _completedAnimations++;
            
            if (_sequentialQueue.Count > 0)
            {
                // ✅ Play next animation in sequence
                PlayNextSequentialAnimation();
            }
            else
            {
                OnAllAnimationsComplete();
            }
        }

        private void OnAllAnimationsComplete()
        {
            _isPlaying = false;
            _activeAnimations = 0;

            // ✅ Cleanup listeners
            CleanupListeners();

            // ✅ Fire completion events
            _onGroupComplete?.Invoke();
        }

        private void CleanupListeners()
        {
            foreach (var animation in _animations)
            {
                if (animation != null)
                {
                    animation.OnAnimationComplete.RemoveListener(OnParallelAnimationComplete);
                    animation.OnAnimationComplete.RemoveListener(OnSequentialAnimationComplete);
                }
            }
        }

        #endregion

        #region Unity Lifecycle

        private void OnDestroy()
        {
            CleanupListeners();
        }

        private void OnValidate()
        {
            // ✅ Remove null references in editor
            if (_animations != null)
            {
                _animations.RemoveAll(anim => anim == null);
            }
        }

        #endregion

        #region Public Utility Methods

        /// <summary>
        /// Add an animation to the group
        /// </summary>
        public void AddAnimation(UIAnimationBase animation)
        {
            if (animation != null && !_animations.Contains(animation))
            {
                _animations.Add(animation);
            }
        }

        /// <summary>
        /// Remove an animation from the group
        /// </summary>
        public void RemoveAnimation(UIAnimationBase animation)
        {
            if (animation != null)
            {
                _animations.Remove(animation);
            }
        }

        /// <summary>
        /// Clear all animations from the group
        /// </summary>
        public void ClearAnimations()
        {
            Stop();
            _animations.Clear();
        }

        /// <summary>
        /// Get the total estimated duration of all animations
        /// </summary>
        public float GetEstimatedDuration()
        {
            if (_animations == null || _animations.Count == 0) return 0f;

            float totalDuration = 0f;

            switch (_playMode)
            {
                case PlayMode.Parallel:
                    // Return the longest animation duration
                    foreach (var animation in _animations)
                    {
                        if (animation != null && animation is IUIAnimationDuration durationProvider)
                        {
                            totalDuration = Mathf.Max(totalDuration, durationProvider.Duration);
                        }
                    }
                    break;

                case PlayMode.Sequential:
                    // Sum all animation durations
                    foreach (var animation in _animations)
                    {
                        if (animation != null && animation is IUIAnimationDuration durationProvider)
                        {
                            totalDuration += durationProvider.Duration;
                        }
                    }
                    break;
            }

            return totalDuration;
        }

        #endregion
    }

    /// <summary>
    /// Optional interface for animations that can provide their duration
    /// </summary>
    public interface IUIAnimationDuration
    {
        float Duration { get; }
    }
}