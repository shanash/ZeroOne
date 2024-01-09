using FluffyDuck.Util;
using System;
using UnityEngine;

public class EasingCanvasGroupAlpha : EasingFade
{
    public struct EasingAlphaData
    {
        public float Dest_Alpha;
        public CanvasGroup Dest_CanvasGroup;
    }

    EasingAlphaData Alpha_Data;

    float Origin_Alpha = 0;
    /// <summary>
    /// Easing을 이용하여 알파 변경
    /// </summary>
    /// <param name="data">목표로 할 알파값</param>
    public override void StartEasing(object data, Action cb = null)
    {
        Alpha_Data = (EasingAlphaData)(data);
        Origin_Alpha = Alpha_Data.Dest_CanvasGroup.alpha;
        base.StartEasing(data, cb);
    }

    protected override void OnFadeUpdate(float weight)
    {
        if (EaseFade == EasingFunction.Ease.NotUse)
        {
            return;
        }
        UpdateAlphaFader(EasingFunction.GetEasingFunction(EaseFade), weight);
    }

    private void UpdateAlphaFader(EasingFunction.Function func, float weight)
    {
        float ev = func(0.0f, 1.0f, weight);
        float easing_alpha = (Alpha_Data.Dest_Alpha - Origin_Alpha) * ev;
        Alpha_Data.Dest_CanvasGroup.alpha = Origin_Alpha + easing_alpha;
    }

    protected override void UpdatePostDelayEnd()
    {
        Alpha_Data.Dest_CanvasGroup.alpha = Alpha_Data.Dest_Alpha;
    }
}
