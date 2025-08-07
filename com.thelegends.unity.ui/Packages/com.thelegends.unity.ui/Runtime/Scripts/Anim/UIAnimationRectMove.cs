using LitMotion;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class UIAnimationRectMove : UIAnimationLitMotionBase<Vector3>
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

        protected virtual void Awake()
        {
            if (_target == null)
                _target = GetComponent<RectTransform>();
        }

        protected override MotionHandle CreateMotion()
        {
            return LMotion.Create(_positionSettings)
                .WithOnComplete(() => InvokeOnComplete())
                .Bind(value => _target.anchoredPosition = value);
        }

        protected override SerializableMotionSettings<Vector3, NoOptions> GetMotionSettings()
        {
            return _positionSettings;
        }
    }
}

