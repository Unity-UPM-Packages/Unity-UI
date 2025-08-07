using System.Collections;
using System.Collections.Generic;
using TheLegends.Base.UI;
using UnityEngine;

public class ShowCaseAnimPlay : MonoBehaviour
{
    public UIAnimationGroup[] uIAnimationGroups;

    public void PlayAll()
    {
        foreach (var group in uIAnimationGroups)
        {
            group.Play();
        }
    }
    
}
