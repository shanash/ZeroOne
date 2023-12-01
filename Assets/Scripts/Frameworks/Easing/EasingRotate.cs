using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasingRotate : EasingFade
{
    Quaternion OriginRotate = Quaternion.identity;

    Vector3 RotateDelta = Vector3.zero;


    public override void StartEasing(object data, Action cb = null)
    {
        RotateDelta = (Vector3)data;
        base.StartEasing(data, cb);
    }

    protected override void OnFadeUpdate(float weight)
    {
        if (EaseFade == EasingFunction.Ease.NotUse)
        {
            return;
        }
        UpdateRot(EasingFunction.GetEasingFunction(EaseFade), weight);
    }

    void UpdateRot(EasingFunction.Function func, float weight)
    {
        float ev = func(0.0f, 1.0f, weight);
        Vector3 easeDelta = RotateDelta * ev;
        transform.localRotation = Quaternion.Euler(easeDelta);
    }
}
