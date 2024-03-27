using FluffyDuck.Util;
using FluffyDuck.UI;
using System;
using UnityEngine;

public class UIEaseRandomMove : UIEaseBase
{
    /// <summary>
    /// 이동전 시작 좌표
    /// </summary>
    Vector2 Start_Position;
    /// <summary>
    /// 이동 전 시작 좌표와 이동 후 도착 좌표간 거리
    /// </summary>
    Vector2 Distance;

    public override void StartMove(MOVE_TYPE mtype, Action cb = null)
    {
        var found = FindEaseData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Position = This_Rect.localPosition;

            
        Distance = (Vector2)UnityEngine.Random.onUnitSphere * found.Ease_Vector;
        base.StartMove(mtype, cb);
    }

    public override void StartMove(MOVE_TYPE mtype, object data, Action cb = null)
    {
        var found = FindEaseData(mtype);
        if (found == null)
        {
            return;
        }
        Start_Position = This_Rect.localPosition;
        TEAM_TYPE ttype = (TEAM_TYPE)data;

        float x, y = 0f;
        if (ttype == TEAM_TYPE.LEFT)
        {
            Vector2 p1 = CommonUtils.Math.AngleToVector(180f - 45f);
            Vector2 p2 = CommonUtils.Math.AngleToVector(180f + 45f);
            x = UnityEngine.Random.Range(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.x, p2.x));
            y = UnityEngine.Random.Range(Mathf.Min(p1.y, p2.y), Mathf.Max(p1.y, p2.y));
            Vector2 direction = new Vector2(x, y);
            Distance = (direction * found.Ease_Vector);
        }
        else
        {
            Vector2 p1 = CommonUtils.Math.AngleToVector(-45f);
            Vector2 p2 = CommonUtils.Math.AngleToVector(45f);
            x = UnityEngine.Random.Range(Mathf.Min(p1.x, p2.x), Mathf.Max(p1.x, p2.x));
            y = UnityEngine.Random.Range(Mathf.Min(p1.y, p2.y), Mathf.Max(p1.y, p2.y));
            Vector2 direction = new Vector2(x, y);
            Distance = (direction * found.Ease_Vector);
        }

        base.StartMove(mtype, data, cb);
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


