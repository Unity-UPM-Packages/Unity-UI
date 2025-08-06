using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheLegends.Base.UI
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(GraphicRaycaster))]
    /// <summary>
    /// Panel controller that manages UI panel visibility and animations
    /// Uses UIAnimationGroup components for show/hide animations
    /// </summary>
    public class UIPanelController : MonoBehaviour
    {
        [Header("Panel Info")]
        [SerializeField] private string _panelID;
        public string PanelID => _panelID;

        [Header("Animation Groups")]
        [SerializeField] private UIAnimationGroup _showAnimationGroup;
        [SerializeField] private UIAnimationGroup _hideAnimationGroup;

        [Header("Panel Events")]
        [SerializeField] private UnityEvent _onPanelShowStart;
        [SerializeField] private UnityEvent _onPanelShowComplete;
        [SerializeField] private UnityEvent _onPanelHideStart;
        [SerializeField] private UnityEvent _onPanelHideComplete;

        public UnityEvent OnPanelShowStart => _onPanelShowStart;
        public UnityEvent OnPanelShowComplete => _onPanelShowComplete;
        public UnityEvent OnPanelHideStart => _onPanelHideStart;
        public UnityEvent OnPanelHideComplete => _onPanelHideComplete;

        private Canvas _canvas;

        #region Properties

        /// <summary>
        /// Reference to the show animation group
        /// </summary>
        public UIAnimationGroup ShowAnimationGroup => _showAnimationGroup;

        /// <summary>
        /// Reference to the hide animation group
        /// </summary>
        public UIAnimationGroup HideAnimationGroup => _hideAnimationGroup;

        /// <summary>
        /// Whether the panel is currently showing (has show animations playing)
        /// </summary>
        public bool IsShowing => _showAnimationGroup != null && _showAnimationGroup.IsPlaying;

        /// <summary>
        /// Whether the panel is currently hiding (has hide animations playing)
        /// </summary>
        public bool IsHiding => _hideAnimationGroup != null && _hideAnimationGroup.IsPlaying;

        /// <summary>
        /// Whether any panel animations are currently active
        /// </summary>
        public bool HasActiveAnimations => IsShowing || IsHiding;

        #endregion

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            _canvas = GetComponent<Canvas>();

            // ✅ Subscribe to animation group events
            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.OnGroupStart.AddListener(OnShowAnimationStart);
                _showAnimationGroup.OnGroupComplete.AddListener(OnShowAnimationComplete);
            }

            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.OnGroupStart.AddListener(OnHideAnimationStart);
                _hideAnimationGroup.OnGroupComplete.AddListener(OnHideAnimationComplete);
            }
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
            // ✅ Cleanup event subscriptions
            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.OnGroupStart.RemoveListener(OnShowAnimationStart);
                _showAnimationGroup.OnGroupComplete.RemoveListener(OnShowAnimationComplete);
            }

            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.OnGroupComplete.RemoveListener(OnHideAnimationStart);
                _hideAnimationGroup.OnGroupComplete.RemoveListener(OnHideAnimationComplete);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Show the panel with animations
        /// </summary>
        public virtual void Show()
        {
            _canvas.enabled = true;

            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.Play();
            }
            else
            {
                // ✅ No animation group, fire events immediately
                OnPanelShowStart?.Invoke();
                OnPanelShowComplete?.Invoke();
            }
        }

        /// <summary>
        /// Hide the panel with animations
        /// </summary>
        public virtual void Hide()
        {
            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.Play();
            }
            else
            {
                // ✅ No animation group, hide immediately
                _canvas.enabled = false;
                OnPanelHideStart?.Invoke();
                OnPanelHideComplete?.Invoke();
            }
        }

        /// <summary>
        /// Show the panel immediately without animations
        /// </summary>
        public virtual void ShowImmediate()
        {
            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.Stop();
            }

            _canvas.enabled = true;
            OnPanelShowStart?.Invoke();
            OnPanelShowComplete?.Invoke();
        }

        /// <summary>
        /// Hide the panel immediately without animations
        /// </summary>
        public virtual void HideImmediate()
        {
            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.Stop();
            }

            _canvas.enabled = false;
            OnPanelHideStart?.Invoke();
            OnPanelHideComplete?.Invoke();
        }

        /// <summary>
        /// Stop all panel animations
        /// </summary>
        public virtual void StopAllAnimations()
        {
            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.Stop();
            }

            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.Stop();
            }
        }

        #endregion

        #region Animation Event Handlers

        private void OnShowAnimationStart()
        {
            OnPanelShowStart?.Invoke();
        }

        private void OnShowAnimationComplete()
        {
            OnPanelShowComplete?.Invoke();
        }

        private void OnHideAnimationStart()
        {
            OnPanelHideStart?.Invoke();
        }

        private void OnHideAnimationComplete()
        {
            _canvas.enabled = false;
            OnPanelHideComplete?.Invoke();
        }

        #endregion

        #region Utility Methods

        /// <summary>
        /// Set the show animation group
        /// </summary>
        public void SetShowAnimationGroup(UIAnimationGroup animationGroup)
        {
            // ✅ Unsubscribe from old group
            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.OnGroupStart.RemoveListener(OnShowAnimationStart);
                _showAnimationGroup.OnGroupComplete.RemoveListener(OnShowAnimationComplete);
            }

            _showAnimationGroup = animationGroup;

            // ✅ Subscribe to new group
            if (_showAnimationGroup != null)
            {
                _showAnimationGroup.OnGroupStart.AddListener(OnShowAnimationStart);
                _showAnimationGroup.OnGroupComplete.AddListener(OnShowAnimationComplete);
            }
        }

        /// <summary>
        /// Set the hide animation group
        /// </summary>
        public void SetHideAnimationGroup(UIAnimationGroup animationGroup)
        {
            // ✅ Unsubscribe from old group
            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.OnGroupStart.RemoveListener(OnHideAnimationStart);
                _hideAnimationGroup.OnGroupComplete.RemoveListener(OnHideAnimationComplete);
            }

            _hideAnimationGroup = animationGroup;

            // ✅ Subscribe to new group
            if (_hideAnimationGroup != null)
            {
                _hideAnimationGroup.OnGroupStart.AddListener(OnHideAnimationStart);
                _hideAnimationGroup.OnGroupComplete.AddListener(OnHideAnimationComplete);
            }
        }

        #endregion
    }
}
