using LitMotion;
using TheLegends.Base.UI;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public PlayableText playableText;
    private float currentValue = 0f;

    public UIAnimationGroup groupAnim;


    public void Extension()
    {
        textMeshPro.DONumber(0f, 1000, 0.5f);
    }

    public void Playble()
    {
        var nextValue = currentValue + 1000f;
        DoNumber(currentValue, nextValue);
        currentValue = nextValue;
        playableText.Play();
    }

    private void DoNumber(float startValue, float endValue)
    {
        var settings = playableText.GetTextMotionSettings();

        var newSettings = new SerializableMotionSettings<float, NoOptions>
        {
            StartValue = startValue,
            EndValue = endValue,
            Duration = settings.Duration,
            Ease = settings.Ease,
            Delay = settings.Delay,
            Loops = settings.Loops,
            LoopType = settings.LoopType
        };

        playableText.SetTextMotionSettings(newSettings);
    }

    public void GroupAnim()
    {
        var nextValue = currentValue + 1000f;
        DoNumber(currentValue, nextValue);
        currentValue = nextValue;
        groupAnim.Play();
    }

    public void Log()
    {
        Debug.Log("Log");
    }
    
}
