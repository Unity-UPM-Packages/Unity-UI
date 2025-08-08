using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheLegends.Base.UI;
using TMPro;
using LitMotion;

public class CountNumberTxtController : MonoBehaviour
{
    private int currentValue = 0;
    public TMP_Text textMesh;
    public UIAnimationGroup animationGroup;

    public void StartDoNumber()
    {
        LMotion.Create(0, 1, 1f)
            .WithOnLoopComplete((x) =>
            {
                IncrementValue(1); // Chạy sau mỗi loop iteration
            })
            .WithLoops(-1, LoopType.Restart)
            .RunWithoutBinding();
    }

    public void IncrementValue(int increment)
    {
        var nextValue = currentValue + increment;
        textMesh.DONumber(currentValue, nextValue, 0.5f, () =>
        {
            animationGroup.Play();
        });

        currentValue = nextValue;
    }

}
