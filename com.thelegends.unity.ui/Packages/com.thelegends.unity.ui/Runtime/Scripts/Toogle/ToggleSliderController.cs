
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LitMotion;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{
    public class ToggleSliderController : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private UnityEvent<float> _onToggleChanged;
        public UnityEvent<float> OnToggleChanged => _onToggleChanged;
        public bool IsOn => _slider != null && Mathf.Approximately(_slider.value, _slider.maxValue);
        private float _currentTargetValue = 1f;

        public void Awake()
        {
            _slider = GetComponent<Slider>();

            if (_slider == null)
            {
                Debug.LogError("ToggleController requires a Slider component to function properly.");
                return;
            }

            _slider.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Toggle();
        }

        private void Toggle()
        {
            if (_slider == null)
            {
                Debug.LogError("ToggleController: Slider component is not assigned.");
                return;
            }

            // Determine new value based on current state
            float newValue = IsOn ? _slider.minValue : _slider.maxValue;

            SetValue(newValue);
        }


        public void SetValue(float value)
        {
            if (_slider == null)
            {
                Debug.LogError("ToggleController: Slider component is not assigned.");
                return;
            }

            float targetValue = value >= 0.005f ? _slider.maxValue : _slider.minValue;
            if (!Mathf.Approximately(_currentTargetValue, targetValue))
            {
                _currentTargetValue = targetValue;

                OnToggleChanged?.Invoke(targetValue);
                LMotion.Create(_slider.value, targetValue, 0.3f)
                    .WithEase(Ease.OutCubic)
                    .Bind(x => _slider.value = x);

            }
        }

        public void SetToggle(bool isOn)
        {
            float targetValue = isOn ? _slider.maxValue : _slider.minValue;
            SetValue(targetValue);
        }

    }
}
