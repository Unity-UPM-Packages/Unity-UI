using System.Collections;
using System.Collections.Generic;
using LitMotion;
using TheLegends.Base.UI;
using TMPro;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class PlayableText : UIAnimationLitMotionBase<float>
    {
        [SerializeField]
        private TMP_Text _textMeshPro;
        [SerializeField]
        private string _format = "F0";

        [Space(10)]
        [SerializeField]
        private SerializableMotionSettings<float, NoOptions> _textSettings = new()
        {
            StartValue = 0f,
            EndValue = 0f,
            Duration = 1f,
            Ease = Ease.Linear,
            Delay = 0f,
            Loops = 1,
            LoopType = LoopType.Restart
        };


        protected virtual void Awake()
        {
            if (_textMeshPro == null)
                _textMeshPro = GetComponent<TMP_Text>();
        }

        public void SetFormat(string format)
        {
            _format = format;
        }

        protected override MotionHandle CreateMotion()
        {
            return LMotion.Create(_textSettings)
                .WithOnComplete(() => InvokeOnComplete())
                .Bind(value => _textMeshPro.text = value.ToString(_format));
        }

        protected override SerializableMotionSettings<float, NoOptions> GetMotionSettings()
        {
            return _textSettings;
        }

        public SerializableMotionSettings<float, NoOptions> GetTextMotionSettings()
        {
            return _textSettings;
        }

        public void SetTextMotionSettings(SerializableMotionSettings<float, NoOptions> settings)
        {
            if (IsPlaying)
            {
                Stop();
            }

            _textSettings = settings;
        }
        
    }
}
