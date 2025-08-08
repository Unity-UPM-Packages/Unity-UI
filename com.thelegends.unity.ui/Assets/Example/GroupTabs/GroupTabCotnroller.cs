using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTabCotnroller : MonoBehaviour
{
    public GameObject[] tabs;
    public void HandleInteraction(GameObject source)
    {
        var target = source.GetComponent<ButtonSelectController>();
       
        foreach (var tab in tabs)
        {
            tab.SetActive(tab.name == target.tabName);
        }
    }
}
