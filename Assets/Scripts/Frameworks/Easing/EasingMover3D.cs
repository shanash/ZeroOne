using FluffyDuck.Util;
using System;
using UnityEngine;

public class EasingMover3D : EasingFade
{
    Vector3 Start_Position = Vector3.zero;
    Vector3 Target_Position = Vector3.zero;
    Vector3 Distance = Vector3.zero;
    //Transform Target_Transform;


    /// <summary>
    /// Easing을 이용하여 좌표 이동. 2D 좌표로 이동만 가능.
    /// </summary>
    /// <param name="data">현재 지점에서 목표지점까지의 거리를 Vector2로 지정한다.</param>
    public override void StartEasing(object data, Action cb = null)
    {
        //Target_Transform = (Transform)data;

        Target_Position = (Vector3)data;
        Start_Position = transform.localPosition;
        Distance = Target_Position - Start_Position;
        base.StartEasing(Target_Position, cb);
    }


    protected override void OnFadeUpdate(float weight)
    {
        if (EaseFade == EasingFunction.Ease.NotUse)
        {
            return;
        }
        UpdatePos(EasingFunction.GetEasingFunction(EaseFade), weight);
    }

    void UpdatePos(EasingFunction.Function func, float weight)
    {
        float ev = func(0.0f, 1.0f, weight);
        Vector3 easingDelta = Distance * ev;
        transform.localPosition = Start_Position + easingDelta;
    }

    protected override void UpdatePostDelayEnd()
    {
        transform.localPosition = Target_Position;
    }
}
