using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{
    public abstract class UIAnimationBase : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent _onAnimationStart;
        public UnityEvent OnAnimationStart => _onAnimationStart;

        [SerializeField]
        private UnityEvent _onAnimationComplete;
        public UnityEvent OnAnimationComplete => _onAnimationComplete;

        public abstract void Play();

        protected virtual void InvokeOnStart()
        {
            OnAnimationStart?.Invoke();
        }

        protected virtual void InvokeOnComplete()
        {
            OnAnimationComplete?.Invoke();
        }
    }
}
