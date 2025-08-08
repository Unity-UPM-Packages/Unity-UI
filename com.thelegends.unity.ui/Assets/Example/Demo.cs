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
    public Button backBtn;

    public void OnEnable()
    {
        showShowCaseBtn.onClick.AddListener(OnShowShowCaseClicked);
        hideShowCaseBtn.onClick.AddListener(OnHideShowCaseClicked);
        backBtn.onClick.AddListener(OnBackClicked);
    }

    private void OnBackClicked()
    {
        UIManager.Instance.HidePanel("Interactive");
        UIManager.Instance.ShowPanel("ShowCase");
    }

    private void OnHideShowCaseClicked()
    {
        UIManager.Instance.HidePanel("ShowCase");
        UIManager.Instance.ShowPanel("Interactive");
    }

    private void OnShowShowCaseClicked()
    {
        UIManager.Instance.ShowPanel("ShowCase");
    }
    
}
