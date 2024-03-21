using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEasingScale : EffectEasingBase
{
    /// <summary>
    /// 시작전 스케일
    /// </summary>
    Vector3 Start_Scale;
    /// <summary>
    /// 시작전 스케일과 종료 후 스케일(목표 스케일)의 차이
    /// </summary>
    Vector3 Diff_Scale;

    public override void StartMove(EASING_MOVE_TYPE mtype, Action cb = null)
    {
        var found = FindEasingData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Scale = this.transform.localScale;
        Diff_Scale = found.Easing_Vector - Start_Scale;
        base.StartMove(mtype, cb);
    }

    protected override void UpdateEase(EasingFunction.Function func, float weight)
    {
        float ev = func(0.0f, 1.0f, weight);
        Vector3 easing_delta = Diff_Scale * ev;
        this.transform.localScale = Start_Scale + easing_delta;
    }

    /// <summary>
    /// Easing 동작 완료 후 호출되는 함수<br/>
    /// 최종 목표 스케일값에 약간의 오차가 있을 수 있기 때문에<br/>
    /// 오차를 줄이기 위해 목표 스케일로 마지막 업데이트를 해준다.
    /// </summary>
    protected override void UpdatePostDelayEnd()
    {
        var found = FindEasingData(Move_Type);
        if (found != null)
        {
            this.transform.localScale = found.Easing_Vector;
        }
    }

    public override void ResetEase(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }
        if (data[0] is Vector3)
        {
            Vector3 scale = (Vector3)data[0];
            this.transform.localScale = scale;
        }
    }
}
