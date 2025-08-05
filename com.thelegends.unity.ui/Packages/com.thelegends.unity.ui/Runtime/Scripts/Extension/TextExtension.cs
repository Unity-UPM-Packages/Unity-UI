using System;
using System.Collections;
using System.Collections.Generic;
using LitMotion;
using TMPro;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public static class TextExtension
    {
        private static void DONumber(this TMP_Text textMeshPro, float startValue, float endValue, float duration, string format = "#0", Action actionOnDone = null)
        {
            LMotion.Create(startValue, endValue, duration)
                .WithEase(Ease.OutCubic)
                .WithOnComplete(() =>
                {
                    actionOnDone?.Invoke();
                })
                .Bind(value =>
                {
                    textMeshPro.text = value.ToString(format);
                });
        }

        public static void DONumber(this TMP_Text textMeshPro, float startValue, float endValue, float duration, Action actionOnDone = null)
        {
            textMeshPro.DONumber(startValue, endValue, duration, "#0", actionOnDone);
        }
    }
}
