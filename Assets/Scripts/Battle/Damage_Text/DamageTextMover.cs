using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum DAMAGE_DIRECTION
{
    NONE = 0,
    LEFT,
    RIGHT,
    UP,
    DOWN,
}
public class DamageTextMover : UIEaseBase
{
    /// <summary>
    /// 이동전 시작 좌표
    /// </summary>
    Vector2 Start_Position;
    /// <summary>
    /// 이동 전 시작 좌표와 이동 후 도착 좌표간 거리
    /// </summary>
    Vector2 Distance;

    public override void StartMove(MOVE_TYPE mtype, object data, Action cb = null)
    {
        var found = FindEaseData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Position = This_Rect.localPosition;
        DAMAGE_DIRECTION dir = (DAMAGE_DIRECTION)data;

        Vector2 direction = GetCalcDirectionVector(dir);
        Distance = (direction * found.Ease_Vector);

        base.StartMove(mtype, data, cb);
    }

    public override void StartMove(MOVE_TYPE mtype, object data1, object data2, Action cb = null)
    {

        var found = FindEaseData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Position = This_Rect.localPosition;
        DAMAGE_DIRECTION dir = (DAMAGE_DIRECTION)data1;
        float x_scale = (float)data2;

        Vector2 direction = GetCalcDirectionVector(dir);

        Distance = (direction * found.Ease_Vector);
        if (x_scale > 1f)
        {
            Distance.x *= x_scale;
        }

        base.StartMove(mtype, data1, data2, cb);
    }

    Vector2 GetCalcDirectionVector(DAMAGE_DIRECTION dir)
    {
        float x = 0f;
        float y = 0f;
        if (dir == DAMAGE_DIRECTION.UP)
        {
            Vector2 p1 = CommonUtils.Math.AngleToVector(90f - 45f);
            Vector2 p2 = CommonUtils.Math.AngleToVector(90f + 45f);
            x = UnityEngine.Random.Range(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.x, p2.x));
            y = UnityEngine.Random.Range(Mathf.Min(p1.y, p2.y), Mathf.Max(p1.y, p2.y));
        }
        else if (dir == DAMAGE_DIRECTION.RIGHT)
        {
            Vector2 p1 = CommonUtils.Math.AngleToVector(-45f);
            Vector2 p2 = CommonUtils.Math.AngleToVector(45f);
            x = UnityEngine.Random.Range(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.x, p2.x));
            y = UnityEngine.Random.Range(Mathf.Min(p1.y, p2.y), Mathf.Max(p1.y, p2.y));

        }
        else if (dir == DAMAGE_DIRECTION.DOWN)
        {
            Vector2 p1 = CommonUtils.Math.AngleToVector(270f - 45f);
            Vector2 p2 = CommonUtils.Math.AngleToVector(270f + 45f);
            x = UnityEngine.Random.Range(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.x, p2.x));
            y = UnityEngine.Random.Range(Mathf.Min(p1.y, p2.y), Mathf.Max(p1.y, p2.y));
        }
        else if (dir == DAMAGE_DIRECTION.LEFT)
        {
            Vector2 p1 = CommonUtils.Math.AngleToVector(180f - 45f);
            Vector2 p2 = CommonUtils.Math.AngleToVector(180f + 45f);
            x = UnityEngine.Random.Range(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.x, p2.x));
            y = UnityEngine.Random.Range(Mathf.Min(p1.y, p2.y), Mathf.Max(p1.y, p2.y));
        }

        return new Vector2(x, y);
    }


    protected override void UpdateEase(EasingFunction.Function func, float weight)
    {
        float ev = func(0f, 1f, weight);
        Vector2 easing_delta = Distance * ev;
        This_Rect.localPosition = Start_Position + easing_delta;
    }

    protected override void UpdatePostDelayEnd()
    {
        var found = FindEaseData(Move_Type);
        if (found != null)
        {
            This_Rect.localPosition = Start_Position + Distance;
        }
    }
}
