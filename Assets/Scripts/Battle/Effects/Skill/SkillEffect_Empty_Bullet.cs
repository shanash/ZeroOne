using System.Collections;
using UnityEngine;

public class SkillEffect_Empty_Bullet : SkillEffectBase
{
    public override void MoveTarget(Vector3 target, float duration)
    {
        var ec = GetEffectComponent();
        if (ec != null)
        {
            base.MoveTarget(target, duration);
            switch (ec.Throwing_Type)
            {
                case THROWING_TYPE.LINEAR:
                    ec.Mover.SetEasing(FluffyDuck.Util.EasingFunction.Ease.Linear, 0, duration / Effect_Speed_Multiple);
                    ec.Mover.StartEasing(target, MoveEndCallback);
                    break;
                case THROWING_TYPE.PARABOLA:
                    {
                        float dist = Vector3.Distance(this.transform.position, target);
                        float height = ec.Parabola_Height;
                        float velocity = ec.Projectile_Velocity;
                        if (dist < 5)
                        {
                            height = ec.Parabola_Height * 0.2f;
                            velocity = ec.Projectile_Velocity * 0.5f * Effect_Speed_Multiple;
                        }
                        else if (dist < 10)
                        {
                            height = ec.Parabola_Height * 0.5f;
                            velocity = ec.Projectile_Velocity * 0.7f * Effect_Speed_Multiple;
                        }
                        else
                        {
                            height = ec.Parabola_Height;
                            velocity = ec.Projectile_Velocity * Effect_Speed_Multiple;
                        }
                        ec.Parabola.Move(this.transform.position, target, height, velocity, MoveEndCallback);
                    }
                    break;
                case THROWING_TYPE.BEZIER:
                    ec.Curve.Move(this.transform.position, target, duration / Effect_Speed_Multiple, ec.Start_Curve_Dist, ec.End_Curve_Dist, MoveEndCallback);
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
    }

}