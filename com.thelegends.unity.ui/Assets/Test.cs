using System.Collections;
using System.Collections.Generic;
using TheLegends.Base.UI;
using UnityEngine;

public class Test : MonoBehaviour
{
    public UIAnimationGroup[] uIAnimationGroups;

    void Start()
    {
        A();
    }

    public void A()
    {
        foreach (var group in uIAnimationGroups)
        {
            group.Play();
        }
    }

    public void B(float value)
    {
        Debug.Log("value: " + value);
    }
    
}
