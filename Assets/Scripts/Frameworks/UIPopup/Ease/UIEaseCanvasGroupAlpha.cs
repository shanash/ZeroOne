using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEaseCanvasGroupAlpha : UIEaseBase
{
    [SerializeField, Tooltip("Move In 최종 Alpha 값")]
    float In_End_Alpha;
    [SerializeField, Tooltip("Move Out 최종 Alpha 값 ")]
    float Out_End_Alpha;

    /// <summary>
    /// 시작시 Alpha
    /// </summary>
    float Start_Alpha;
    /// <summary>
    /// 시작과 종료시 Alpha 값의 차이
    /// </summary>
    float Diff_Alpha;

    protected CanvasGroup Canvas_Grp;

    private void Awake()
    {
        CheckCanvasGroup();
    }

    void CheckCanvasGroup()
    {
        if (Canvas_Grp == null)
            Canvas_Grp = GetComponent<CanvasGroup>();
    }

    public override void StartMoveIn(Action cb = null)
    {
        Start_Alpha = Canvas_Grp.alpha;
        Diff_Alpha = In_End_Alpha - Start_Alpha;
        base.StartMoveIn(cb);
    }

    public override void StartMoveOut(Action cb = null)
    {
        Start_Alpha = Canvas_Grp.alpha;
        Diff_Alpha = Out_End_Alpha - Start_Alpha;
        base.StartMoveOut(cb);
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
        if (Move_Type == MOVE_TYPE.MOVE_IN)
        {
            Canvas_Grp.alpha = In_End_Alpha;
        }
        else
        {
            Canvas_Grp.alpha = Out_End_Alpha;
        }
    }
}
