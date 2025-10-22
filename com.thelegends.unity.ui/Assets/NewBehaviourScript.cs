using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using TheLegends.Base.UI;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public void ShowToast()
    {
        UIToatsController.Show("ajkshfkjahfjkashfjkashfjkashfjsakhf", 0.5f, ToastPosition.BottomCenter);
    }

    public void ShowLoading()
    {
        UILoadingController.Show(3f, () =>
        {
            Debug.Log("Loading Complete");
        });
    }
}
