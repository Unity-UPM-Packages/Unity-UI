using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TheLegends.Base.UI
{
    [System.Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }
    
    public class InteractionHandler : MonoBehaviour, IInteractionListener
    {
        [SerializeField] private GameObjectEvent onInteractionTriggered;

        public void OnInteractionTriggered(GameObject source)
        {
            onInteractionTriggered?.Invoke(source);
        }

    }
}
