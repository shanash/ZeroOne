using FluffyDuck.Util;
using System;
using UnityEngine;

public class EasingMover : EasingFade
{
    Vector2 Start_Position = Vector2.zero;
    Vector2 Target_Position = Vector2.zero;
    Vector2 Distance = Vector2.zero;


    /// <summary>
    /// Easing을 이용하여 좌표 이동. 2D 좌표로 이동만 가능.
    /// </summary>
    /// <param name="data">현재 지점에서 목표지점까지의 거리를 Vector2로 지정한다.</param>
    public override void StartEasing(object data, Action cb = null)
    {
        Target_Position = (Vector2)data;
        Start_Position = transform.localPosition;
        Distance = Target_Position - Start_Position;
        base.StartEasing(cb);
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
        Vector2 easingDelta = Distance * ev;
        transform.localPosition = Start_Position + easingDelta;
    }

    protected override void UpdatePostDelayEnd()
    {
        transform.localPosition = Target_Position;
    }
}
