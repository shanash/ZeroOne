using System.Collections;
using UnityEngine;


/// <summary>
/// 일반적인 날아가는 투사체.
/// 투사체가 날아가며 타겟까지 도달한 후 스킬 트리거 발생
/// 도착 후 트리거로 인한 효과를 보여줄 수 있음.
/// </summary>
public class SkillEffect_Normal_Bullet : SkillEffectBase
{

    public override void MoveTarget(Transform target, float duration)
    {
        var ec = GetEffectComponent();
        if (ec != null)
        {
            base.MoveTarget(target, duration);
            switch (ec.Throwing_Type)
            {
                case THROWING_TYPE.LINEAR:
                    ec.Mover.SetEasing(FluffyDuck.Util.EasingFunction.Ease.Linear, 0, duration);
                    ec.Mover.StartEasing(Target_Transform, MoveEndCallback);
                    break;
                case THROWING_TYPE.PARABOLA:
                    ec.Parabola.Move(this.transform.position, target.position, ec.Parabola_Height, ec.Projectile_Velocity, MoveEndCallback);
                    break;
                case THROWING_TYPE.BEZIER:
                    ec.Curve.Move(this.transform.position, target, duration, ec.Start_Curve_Dist, ec.End_Curve_Dist, MoveEndCallback);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
            
        }
        
    }

    /// <summary>
    /// 이동 완료 후 콜백
    /// </summary>
    void MoveEndCallback()
    {
        SkillExec();

        var ec = GetEffectComponent();
        if (ec != null && ec.Use_Hide_Transforms)
        {
            ec.ShowObjects(false);
            StartCoroutine(WaitDelay(ec.Hide_After_Delay_Time));
        }
        else
        {
            Finish_Callback?.Invoke(this);
            UnusedEffect();
        }
    }

    IEnumerator WaitDelay(float delay)
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
            switch (ec.Throwing_Type)
            {
                case THROWING_TYPE.LINEAR:
                    ec.Mover?.OnPause();
                    break;
                case THROWING_TYPE.PARABOLA:
                    ec.Parabola?.SetPuase(true);
                    break;
                case THROWING_TYPE.BEZIER:
                    ec.Curve?.SetPuase(true);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }
    }

    public override void OnResume()
    {
        base.OnResume();
        
        var ec = GetEffectComponent();
        if (ec != null)
        {
            switch (ec.Throwing_Type)
            {
                case THROWING_TYPE.LINEAR:
                    ec.Mover?.OnResume();
                    break;
                case THROWING_TYPE.PARABOLA:
                    ec.Parabola?.SetPuase(false);
                    break;
                case THROWING_TYPE.BEZIER:
                    ec.Curve?.SetPuase(false);
                    break;
                default:
                    Debug.Assert(false);
                    break;
            }
        }

    }

    public override void Spawned()
    {
        base.Spawned();
        var ec = GetEffectComponent();
        if (ec != null && ec.Use_Hide_Transforms)
        {
            ec.ShowObjects(true);
        }
    }

}
