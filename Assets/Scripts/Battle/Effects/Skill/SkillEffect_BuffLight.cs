using FluffyDuck.Util;
using UnityEngine;

public class SkillEffect_BuffLight : SkillEffectBase
{
    [SerializeField, Tooltip("Bezier")]
    BezierMove3D Curve;

    readonly float START_CURVE_DIST = 3f;

    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);

        Curve.Move(this.transform.position, target, duration, START_CURVE_DIST, 0, CurveMoveEndCallback);
    }

    void CurveMoveEndCallback(object obj)
    {
        SkillExec();

        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }

    public override void OnPuase()
    {
        base.OnPuase();
        Curve.SetPuase(true);
    }
    public override void OnResume()
    {
        base.OnResume();
        Curve.SetPuase(false);
    }
}
