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
        }

        private void OnDisable()
        {
            _trigger.OnTriggered -= HandleTrigger;
        }

        private void HandleTrigger(GameObject source)
        {
            if (targets == null || targets.Count == 0) return;

            foreach (var target in targets)
            {
                if (target == null) continue;

                var listeners = target.GetComponents<IInteractionListener>();

                foreach (var listener in listeners)
                {
                    listener.OnInteractionTriggered(source);
                }
            }
        }
    }
}
