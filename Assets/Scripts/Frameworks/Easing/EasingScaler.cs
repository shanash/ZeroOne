using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 단순한 유형의 스케일링 유틸
/// ScaleUp은 +Vector2, ScaleDown 은 -Vector2를 사용한다.
/// 스케일의 변경은 현재의 OriginLocalScale을 기준으로 변경된 값 만큼 +- 된다.
/// Vector2.one에서 ScaleUp Vector2.one을 적용할 경우 결과는 Vector2(2, 2)로 된다.
/// Vector2.one에서 ScaleDown -Vector2.one을 적용할 경우 결과는 Vector2.zero로 된다.
/// </summary>
public class EasingScaler : EasingFade
{
    Vector3 OriginLocalScale = Vector3.one;

    Vector2 ScaleData = Vector2.zero;
    
    private void Awake()
    {
        SaveOriginScale();
    }
    void SaveOriginScale()
    {
        OriginLocalScale = transform.localScale;
    }

    /// <summary>
    /// Easing을 이용하여 스케일링 변경
    /// </summary>
    /// <param name="data">변경할 스케일링 Vector2. 현재의 스케일을 기준으로 얼마나 변경할지 정한다. (-)일 경우 스케일 Down이 된다.</param>
    public override void StartEasing(object data, Action cb = null)
    {
        ScaleData = (Vector2)(data);
        SaveOriginScale();
        base.StartEasing(data, cb);
    }

    protected override void OnFadeUpdate(float weight)
    {
        if (EaseFade == EasingFunction.Ease.NotUse)
        {
            return;
        }
        UpdateScaler(EasingFunction.GetEasingFunction(EaseFade), weight);
    }

    private void UpdateScaler(EasingFunction.Function func, float weight)
    {
        float ev = func(0.0f, 1.0f, weight);
        Vector2 easingDelta = ScaleData * ev;
        transform.localScale = OriginLocalScale + new Vector3(easingDelta.x, easingDelta.y, 0f);
    }

    protected override void UpdatePostDelayEnd()
    {
        transform.localScale = OriginLocalScale + (Vector3)ScaleData;
    }
}
