using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{
    public class UIPanelController : MonoBehaviour
    {
        [SerializeField]
        private string _panelID;

        public string PanelID => _panelID;

        [SerializeField]
        private List<UIAnimationBase> _showAnimations;

        [SerializeField]
        private List<UIAnimationBase> _hideAnimations;

        [SerializeField]
        private UnityEvent _onPanelShowStart;
        public UnityEvent OnPanelShowStart => _onPanelShowStart;

        [SerializeField]
        private UnityEvent _onPanelHideStart;
        public UnityEvent OnPanelHideStart => _onPanelHideStart;

        [SerializeField]
        private UnityEvent _onPanelShowComplete;
        public UnityEvent OnPanelShowComplete => _onPanelShowComplete;

        [SerializeField]
        private UnityEvent _onPanelHideComplete;
        public UnityEvent OnPanelHideComplete => _onPanelHideComplete;

        private Canvas _canvas;
        private int _activeShowAnimations = 0;
        private int _activeHideAnimations = 0;

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        protected virtual void OnEnable()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.RegisterPanel(this);
            }
        }

        protected virtual void OnDisable()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UnregisterPanel(this);
            }
        }

        protected virtual void OnDestroy()
        {
            // Cleanup any remaining animation listeners
            CleanupShowAnimationListeners();
            CleanupHideAnimationListeners();
        }

        public virtual void Show()
        {
            _canvas.enabled = true;
            OnPanelShowStart?.Invoke();

            if (_showAnimations == null || _showAnimations.Count == 0)
            {
                OnPanelShowComplete?.Invoke();
                return;
            }

            _activeShowAnimations = _showAnimations.Count;

            foreach (var anim in _showAnimations)
            {
                // Subscribe to animation completion
                anim.OnAnimationComplete.AddListener(OnShowAnimationComplete);
                anim.Play();
            }
        }

        public virtual void Hide()
        {
            OnPanelHideStart?.Invoke();

            if (_hideAnimations == null || _hideAnimations.Count == 0)
            {
                _canvas.enabled = false;
                OnPanelHideComplete?.Invoke();
                return;
            }

            _activeHideAnimations = _hideAnimations.Count;

            foreach (var anim in _hideAnimations)
            {
                // Subscribe to animation completion
                anim.OnAnimationComplete.AddListener(OnHideAnimationComplete);
                anim.Play();
            }
        }

        private void OnShowAnimationComplete()
        {
            _activeShowAnimations--;
            
            if (_activeShowAnimations <= 0)
            {
                // All show animations completed
                CleanupShowAnimationListeners();
                OnPanelShowComplete?.Invoke();
            }
        }

        private void OnHideAnimationComplete()
        {
            _activeHideAnimations--;
            
            if (_activeHideAnimations <= 0)
            {
                // All hide animations completed
                CleanupHideAnimationListeners();
                _canvas.enabled = false;
                OnPanelHideComplete?.Invoke();
            }
        }

        private void CleanupShowAnimationListeners()
        {
            if (_showAnimations != null)
            {
                foreach (var anim in _showAnimations)
                {
                    anim.OnAnimationComplete.RemoveListener(OnShowAnimationComplete);
                }
            }
        }

        private void CleanupHideAnimationListeners()
        {
            if (_hideAnimations != null)
            {
                foreach (var anim in _hideAnimations)
                {
                    anim.OnAnimationComplete.RemoveListener(OnHideAnimationComplete);
                }
            }
        }
    }
}
