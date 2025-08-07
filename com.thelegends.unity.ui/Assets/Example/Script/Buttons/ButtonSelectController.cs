using System;
using System.Collections;
using System.Collections.Generic;
using TheLegends.Base.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectController : MonoBehaviour
{
    public Button selectButton;
    public UITrigger trigger;
    public TMP_Text textMesh;

    void Awake()
    {
        selectButton = GetComponent<Button>();
    }

    void OnEnable()
    {
        selectButton.onClick.AddListener(OnSelectButtonClicked);
    }

    void OnDisable()
    {
        selectButton.onClick.RemoveListener(OnSelectButtonClicked);
    }

    private void OnSelectButtonClicked()
    {
        trigger?.Trigger();
    }

    public void SetText(string text)
    {
        if (textMesh != null)
        {
            textMesh.text = text;
        }
    }
}
