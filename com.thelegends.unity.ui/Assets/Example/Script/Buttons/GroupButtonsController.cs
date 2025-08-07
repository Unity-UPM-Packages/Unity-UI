using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupButtonsController : MonoBehaviour
{
    public ButtonSelectController[] buttonSelectControllers;
    public void HandleInteraction(GameObject source)
    {
        var target = source.GetComponent<ButtonSelectController>();
        foreach (var controller in buttonSelectControllers)
        {
            string btnTxt = controller == target ? "Selected" : "Unselected";
            controller.SetText(btnTxt);
        }
    }
}
