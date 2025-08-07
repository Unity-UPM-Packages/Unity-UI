using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLegends.Base.UI
{
    [RequireComponent(typeof(UITrigger))]
    public class InteractionConnector : MonoBehaviour
    {
        public List<GameObject> targets = new List<GameObject>();

        private UITrigger _trigger;

        private void Awake()
        {
            _trigger = GetComponent<UITrigger>();
        }

        private void OnEnable()
        {
            _trigger.OnTriggered += HandleTrigger;
            _trigger.OnFloatTriggered += HandleFloatTrigger;
        }

        private void OnDisable()
        {
            _trigger.OnTriggered -= HandleTrigger;
            _trigger.OnFloatTriggered -= HandleFloatTrigger;
        }

        private void HandleTrigger(GameObject source)
        {
            if (targets == null || targets.Count == 0) return;

            foreach (var target in targets)
            {
                if (target == null) continue;

                var listeners = target.GetComponents<IGameObjectInteractionListener>();

                foreach (var listener in listeners)
                {
                    listener.OnInteractionTriggered(source);
                }
            }
        }

        private void HandleFloatTrigger(float value)
        {
            if (targets == null || targets.Count == 0) return;

            foreach (var target in targets)
            {
                if (target == null) continue;

                var floatListeners = target.GetComponents<IFloatInteractionListener>();

                foreach (var listener in floatListeners)
                {
                    listener.OnFloatInteractionTriggered(value);
                }
            }
        }
    }
}
