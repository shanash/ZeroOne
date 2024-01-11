
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect_Bezier_Bullet : SkillEffectBase
{
    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);
        var ec = GetEffectComponent();
        
        var this_pos = this.transform.position;
        var target_pos = target.position;
        var center_pos = (this_pos + target_pos) * 0.5f;
        center_pos.y += ec.Curve_Height;

        List<Vector3> pos_list = new List<Vector3>();
        pos_list.Add(this_pos);
        pos_list.Add(center_pos);
        pos_list.Add(target_pos);

        ec.Curve.Move(pos_list, duration, 0, CurveEndCallback);
    }

    void CurveEndCallback(object data)
    {
        SkillExec();
        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }

    public override void OnPuase()
    {
        base.OnPuase();
        //  curve pause todo
    }
    public override void OnResume()
    {
        base.OnResume();
        //  curve resume todo
    }
}
