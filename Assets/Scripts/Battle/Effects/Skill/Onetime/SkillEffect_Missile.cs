using System.Collections;
using UnityEngine;


/// <summary>
/// 투사체가 날아가며 타겟까지 도달 한 후 스킬 트리거 발생
/// 도착시 연기가 잠시 남아야 하기 때문에 도착 후 일부 이미지 감추면서 대기해야 하는 기능 있음.
/// 타겟 위치에 도착시 스킬 트리거 발생시켜줌
/// </summary>
public class SkillEffect_Missile : SkillEffectBase
{
    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);
        var ec = GetEffectComponent();
        ec.Mover.SetEasing(FluffyDuck.Util.EasingFunction.Ease.Linear, 0, duration / Effect_Speed_Multiple);
        ec.Mover.StartEasing(Target_Transform, MoveEndCallback);
        
    }

    /// <summary>
    /// 이동 완료 후 콜백
    /// </summary>
    void MoveEndCallback()
    {
        SkillExec();
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.ShowObjects(false);
            StartCoroutine(Wait(ec.Hide_After_Delay_Time));
        }
        
    }

    IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }


    public override void OnPuase()
    {
        base.OnPuase();
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.Mover?.OnPause();
        }
        
    }

    public override void OnResume()
    {
        base.OnResume();
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.Mover?.OnResume();
        }
    }

    public override void Spawned()
    {
        base.Spawned();
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.ShowObjects(true);
        }
    }

}
