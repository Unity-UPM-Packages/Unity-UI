using System;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class UITrigger : MonoBehaviour
    {
        public event Action<GameObject> OnTriggered;

        public void Trigger()
        {
            OnTriggered?.Invoke(this.gameObject);
        }
    }
}
