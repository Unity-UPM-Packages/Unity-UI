using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{

    public class InteractionHandler : MonoBehaviour, IGameObjectInteractionListener, IFloatInteractionListener
    {
        [SerializeField] private UnityEvent<GameObject> onInteractionTriggered;
        [SerializeField] private UnityEvent<float> onFloatInteractionTriggered;

        public void OnFloatInteractionTriggered(float value)
        {
            onFloatInteractionTriggered?.Invoke(value);
        }

        public void OnInteractionTriggered(GameObject source)
        {
            onInteractionTriggered?.Invoke(source);
        }

    }
}
