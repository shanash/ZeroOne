
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect_Parabola_Bullet : SkillEffectBase
{
    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);
        var ec = GetEffectComponent();
        
        var this_pos = this.transform.position;
        var target_pos = target.position;

        ec.Parabola.Move(this_pos, target_pos, ec.Parabola_Height, ec.Projectile_Velocity, MoveEndCallback);
    }

    void MoveEndCallback()
    {
        SkillExec();
        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }

    public override void OnPuase()
    {
        base.OnPuase();
        //  parabola pause todo
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.Parabola?.SetPuase(true);
        }
    }
    public override void OnResume()
    {
        base.OnResume();
        //  parabola resume todo
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.Parabola?.SetPuase(false);
        }
    }
}
