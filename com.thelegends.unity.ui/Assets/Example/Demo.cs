using System;
using System.Collections;
using System.Collections.Generic;
using TheLegends.Base.UI;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public Button showShowCaseBtn;
    public Button hideShowCaseBtn;

    public void OnEnable()
    {
        showShowCaseBtn.onClick.AddListener(OnShowShowCaseClicked);
        hideShowCaseBtn.onClick.AddListener(OnHideShowCaseClicked);
    }

    private void OnHideShowCaseClicked()
    {
        UIManager.Instance.HidePanel("ShowCase");
    }

    private void OnShowShowCaseClicked()
    {
        UIManager.Instance.ShowPanel("ShowCase");
    }
    
}
