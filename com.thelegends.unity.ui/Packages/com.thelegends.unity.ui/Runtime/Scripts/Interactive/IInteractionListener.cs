using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public interface IInteractionListener
    {
        void OnInteractionTriggered(GameObject source);
    }
}
