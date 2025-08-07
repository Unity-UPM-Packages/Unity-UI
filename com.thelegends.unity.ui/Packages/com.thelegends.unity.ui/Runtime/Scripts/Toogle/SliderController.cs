using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TheLegends.Base.UI
{
    public class SliderController : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private UnityEvent<float> _onValueChanged;
        public UnityEvent<float> OnValueChanged => _onValueChanged;

        void OnEnable()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float value)
        {
            OnValueChanged?.Invoke(value);
        }

        public void Awake()
        {
            _slider = GetComponent<Slider>();

            if (_slider == null)
            {
                Debug.LogError("ToggleController requires a Slider component to function properly.");
                return;
            }

        }

        public void SetValue(float value)
        {
            if (_slider == null)
            {
                Debug.LogError("SliderController: Slider component is not assigned.");
                return;
            }

            _slider.value = Mathf.Clamp01(value);
            Debug.Log($"Slider value set to: {_slider.value}");
        }

    }
}
