using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIEaseCanvasGroupAlpha : UIEaseBase
{
    /// <summary>
    /// 시작시 Alpha
    /// </summary>
    float Start_Alpha;
    /// <summary>
    /// 시작과 종료시 Alpha 값의 차이
    /// </summary>
    float Diff_Alpha;

    protected CanvasGroup Canvas_Grp;


    protected override void InitCheckComponent()
    {
        if (Canvas_Grp == null)
            Canvas_Grp = GetComponent<CanvasGroup>();

    }

    public override void StartMove(MOVE_TYPE mtype, Action cb = null)
    {
        var found = FindEaseData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Alpha = Canvas_Grp.alpha;
        Diff_Alpha = found.Ease_Float - Start_Alpha;
        base.StartMove(mtype, cb);
    }


    protected override void OnFadeUpdate(float weight)
    {
        if (EaseFade == EasingFunction.Ease.NotUse)
        {
            return;
        }
        UpdateAlphaFader(EasingFunction.GetEasingFunction(EaseFade), weight);
    }

    void UpdateAlphaFader(EasingFunction.Function func, float weight)
    {
        float ev = func(0.0f, 1.0f, weight);
        float easing_alpah = Diff_Alpha * ev;
        Canvas_Grp.alpha = Start_Alpha + easing_alpah;
    }

    protected override void UpdatePostDelayEnd()
    {
        var found = FindEaseData(Move_Type);
        if (found != null)
        {
            Canvas_Grp.alpha = found.Ease_Float;
        }
    }

    public override void ResetEase(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }
        if (Canvas_Grp != null)
        {
            float alpha = (float)data[0];
            Canvas_Grp.alpha = alpha;
        }
    }
}
