using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ܼ��� ������ �����ϸ� ��ƿ
/// ScaleUp�� +Vector2, ScaleDown �� -Vector2�� ����Ѵ�.
/// �������� ������ ������ OriginLocalScale�� �������� ����� �� ��ŭ +- �ȴ�.
/// Vector2.one���� ScaleUp Vector2.one�� ������ ��� ����� Vector2(2, 2)�� �ȴ�.
/// Vector2.one���� ScaleDown -Vector2.one�� ������ ��� ����� Vector2.zero�� �ȴ�.
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
    /// Easing�� �̿��Ͽ� �����ϸ� ����
    /// </summary>
    /// <param name="data">������ �����ϸ� Vector2. ������ �������� �������� �󸶳� �������� ���Ѵ�. (-)�� ��� ������ Down�� �ȴ�.</param>
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
