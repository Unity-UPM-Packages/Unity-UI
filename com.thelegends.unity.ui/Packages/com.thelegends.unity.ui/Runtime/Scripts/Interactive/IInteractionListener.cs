using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public interface IGameObjectInteractionListener
    {
        void OnInteractionTriggered(GameObject source);
    }

    public interface IFloatInteractionListener
    {
        void OnFloatInteractionTriggered(float value);
    }
}
