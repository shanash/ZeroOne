using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectEasingSlide : EffectEasingBase
{
    /// <summary>
    /// 이동전 시작 좌표
    /// </summary>
    Vector3 Start_Position;
    /// <summary>
    /// 이동전 시작 좌표와 이동후 도착 좌표간 거리
    /// </summary>
    Vector3 Distance;

    public override void StartMove(EASING_MOVE_TYPE mtype, Action cb = null)
    {
        var found = FindEasingData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Position = this.transform.localPosition;
        Distance = found.Easing_Vector - Start_Position;
        base.StartMove(mtype, cb);
    }

    protected override void UpdateEase(EasingFunction.Function func, float weight)
    {
        float ev = func(0f, 1f, weight);
        Vector3 easing_delta = Distance * ev;
        this.transform.localPosition = Start_Position + easing_delta;
    }

    protected override void UpdatePostDelayEnd()
    {
        var found = FindEasingData(Move_Type);
        if (found != null)
        {
            this.transform.localPosition = found.Easing_Vector;
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
            Vector3 pos = (Vector3)data[0];
            this.transform.localPosition = pos;
        }
    }
}
