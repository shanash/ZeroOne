using FluffyDuck.Util;
using UnityEngine;

public class SkillEffect_BuffLight : SkillEffectBase
{
    readonly float START_CURVE_DIST = 3f;

    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);

        var ec = GetEffectComponent();
        ec.Curve?.Move(this.transform.position, target, duration, ec.Start_Curve_Dist, ec.End_Curve_Dist, CurveMoveEndCallback);

    }

    void CurveMoveEndCallback()
    {
        SkillExec();

        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }

    public override void OnPuase()
    {
        base.OnPuase();
        GetEffectComponent().Curve.SetPuase(true);

    }
    public override void OnResume()
    {
        base.OnResume();
        GetEffectComponent().Curve.SetPuase(false);
    }
}
