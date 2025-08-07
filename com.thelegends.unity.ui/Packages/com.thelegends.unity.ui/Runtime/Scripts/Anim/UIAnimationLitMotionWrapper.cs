using LitMotion.Animation;
using UnityEngine;
namespace TheLegends.Base.UI
{
    [RequireComponent(typeof(LitMotionAnimation))]
    public class UIAnimationLitMotionWrapper : UIAnimationBase
    {
        [SerializeField] LitMotionAnimation litMotionAnimation;

        private bool _wasPlaying = false;
        private bool _wasPaused = false;

        void OnEnable()
        {
            OnAnimationComplete.AddListener(OnComplete);
        }
        
        private void OnComplete()
        {
            litMotionAnimation.Complete();
        }

        void Awake()
        {
            if (litMotionAnimation == null)
            {
                litMotionAnimation = GetComponent<LitMotionAnimation>();
            }

            if (litMotionAnimation == null)
            {
                Debug.LogError("LitMotionAnimation component is missing on " + gameObject.name);
            }
        }

        void OnValidate()
        {
            if (litMotionAnimation == null)
            {
                litMotionAnimation = GetComponent<LitMotionAnimation>();
            }

            if (litMotionAnimation == null)
            {
                Debug.LogWarning("LitMotionAnimation component is missing on " + gameObject.name + ". Please assign it in the inspector.");
            }
        }

        public override void Play()
        {
            if (litMotionAnimation == null) return;

            if (_wasPaused)
            {
                // Resume from pause
                litMotionAnimation.Play();
                _wasPaused = false;
                _isPlaying = true;
            }
            else
            {
                // Fresh start
                InvokeOnStart();
                litMotionAnimation.Play();
                _wasPlaying = true;
                _isPlaying = true;
                _isActive = true;
            }
        }

        public override void Pause()
        {
            if (litMotionAnimation == null || !_isActive) return;

            litMotionAnimation.Pause();
            _wasPaused = true;
            _isPlaying = false;

            base.Pause();
        }

        public override void Stop()
        {
            if (litMotionAnimation == null) return;

            litMotionAnimation.Stop();
            _wasPlaying = false;
            _wasPaused = false;

            base.Stop();
        }

        public override void Restart()
        {
            Stop();
            _wasPaused = false; // Ensure clean restart
            Play();
        }

        private void Update()
        {
            if (!_isActive || _wasPaused) return;

            if (_wasPlaying && !litMotionAnimation.IsPlaying)
            {
                _wasPlaying = false;
                InvokeOnComplete();
            }

            _isPlaying = litMotionAnimation.IsPlaying;
        }

        public override bool IsActive => litMotionAnimation != null && litMotionAnimation.IsActive;
        public override bool IsPlaying => litMotionAnimation != null && litMotionAnimation.IsPlaying && !_wasPaused;

        private void OnDisable()
        {
            _wasPlaying = false;
            OnAnimationComplete.RemoveListener(OnComplete);
        }
    }
}
