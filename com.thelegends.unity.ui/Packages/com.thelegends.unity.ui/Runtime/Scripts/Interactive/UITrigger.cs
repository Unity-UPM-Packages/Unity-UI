using System;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class UITrigger : MonoBehaviour
    {
        public event Action<GameObject> OnTriggered;
        public event Action<float> OnFloatTriggered;

        public void Trigger()
        {
            OnTriggered?.Invoke(this.gameObject);
        }

        public void TriggerFloat(float value)
        {
            OnFloatTriggered?.Invoke(value);
        }
    }
}
