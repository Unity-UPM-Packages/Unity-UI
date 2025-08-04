using LitMotion;
using TheLegends.Base.UI;
using UnityEngine;
using UnityEngine.UI;

namespace com.thelegends.unity.ui
{
    public class UIAnimationImageColor : UIAnimationBase
    {
        [Header("Target")]
        [SerializeField] private Image _target;

        [SerializeField]
        private SerializableMotionSettings<Color, NoOptions> _imageSettings = new()
        {
            StartValue = new Color(1.000f, 1.000f, 1.000f, 1.000f),
            EndValue = new Color(1.000f, 1.000f, 1.000f, 1.000f),
            Duration = 1f,
            Ease = Ease.Linear,
            Delay = 0f,
            Loops = 1,
            LoopType = LoopType.Restart
        };

        private MotionHandle _motionHandle;

        protected virtual void Awake()
        {
            if (_target == null)
                _target = GetComponent<Image>();
        }

        public override void Play()
        {
            InvokeOnStart();

            _motionHandle = LMotion.Create(_imageSettings)
                .WithOnComplete(() => InvokeOnComplete()) 
                .Bind(value => _target.color = value);
        }

        public void Stop()
        {
            if (_motionHandle.IsActive())
            {
                _motionHandle.Cancel();
            }
        }
    }
}
