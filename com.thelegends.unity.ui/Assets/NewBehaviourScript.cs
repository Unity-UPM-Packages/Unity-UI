using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using TheLegends.Base.UI;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public void A()
    {
        // UILoadingController.SetProgress(0.2f, null);
        UILoadingController.Show(5, null);
    }
}
