using FluffyDuck.Util;
using System;
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
    Vector3 Start_Scale = Vector3.one;

    Vector3 Target_Scale = Vector2.zero;

    Vector3 Diff_Scale = Vector2.zero;

    public void SetStartScale(Vector3 start_scale)
    {
        Start_Scale = start_scale;
        this.transform.localScale = Start_Scale;
    }

    /// <summary>
    /// Easing을 이용하여 스케일링 변경
    /// </summary>
    /// <param name="data">변경할 스케일링 Vector2. 현재의 스케일을 기준으로 얼마나 변경할지 정한다. (-)일 경우 스케일 Down이 된다.</param>
    public override void StartEasing(object data, Action cb = null)
    {
        Target_Scale = (Vector3)(data);
        Diff_Scale = Target_Scale - Start_Scale;
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
        Vector3 easingDelta = Diff_Scale * ev;
        //transform.localScale = Start_Scale + new Vector3(easingDelta.x, easingDelta.y, 0f);
        transform.localScale = Start_Scale + easingDelta;
    }

    protected override void UpdatePostDelayEnd()
    {
        transform.localScale = Target_Scale;
    }

    public override void ResetEasing()
    {
        transform.localScale = Vector3.one;
    }
}
