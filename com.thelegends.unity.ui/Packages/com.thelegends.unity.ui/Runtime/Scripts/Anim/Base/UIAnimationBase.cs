using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{
    public abstract class UIAnimationBase : MonoBehaviour, IUIPlayable
    {
        [SerializeField]
        private UnityEvent _onAnimationStart;
        public UnityEvent OnAnimationStart => _onAnimationStart;

        [SerializeField]
        private UnityEvent _onAnimationComplete;
        public UnityEvent OnAnimationComplete => _onAnimationComplete;

        // ✅ State tracking
        protected bool _isActive = false;
        protected bool _isPlaying = false;

        // ✅ Interface properties (virtual for override)
        public virtual bool IsActive => _isActive;
        public virtual bool IsPlaying => _isPlaying;

        // ✅ Interface methods
        public abstract void Play();
        public virtual void Stop() 
        {
            _isActive = false;
            _isPlaying = false;
        }
        public virtual void Pause() 
        {
            _isPlaying = false;
            // Keep _isActive = true for paused state
        }
        public virtual void Restart() 
        {
            Stop();
            Play();
        }

        protected virtual void InvokeOnStart()
        {
            _isActive = true;
            _isPlaying = true;
            OnAnimationStart?.Invoke();
        }

        protected virtual void InvokeOnComplete()
        {
            _isActive = false;
            _isPlaying = false;
            OnAnimationComplete?.Invoke();
        }
    }
}
