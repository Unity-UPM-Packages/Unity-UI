using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class UIAnimationRectMove : UIAnimationBase
    {
        [Header("Target")]
        [SerializeField] private RectTransform _target;

        [SerializeField]
        private SerializableMotionSettings<Vector3, NoOptions> _positionSettings = new()
        {
            StartValue = Vector3.zero,
            EndValue = Vector3.zero,
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
                _target = GetComponent<RectTransform>();
        }

        public override void Play()
        {
            InvokeOnStart();

            _motionHandle = LMotion.Create(_positionSettings)
                .WithOnComplete(() => InvokeOnComplete())
                .Bind(value => _target.anchoredPosition = value);
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
