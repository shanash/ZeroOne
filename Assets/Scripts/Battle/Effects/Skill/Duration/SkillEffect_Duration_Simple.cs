using UnityEngine;

public class SkillEffect_Duration_Simple : SkillEffectBase
{

    public override void StartParticle(Transform target, float duration, bool loop = false)
    {
        base.StartParticle(target, duration, loop);

        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.SetFollowingTarget(target);
        }
        //  add duration skill data
        Send_Data.Duration?.SetSkillEffect(this);
        Send_Data.Duration?.ExecSkill(Send_Data);

    }

    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);

        Is_Loop = duration == 0f;
        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.SetFollowingTarget(target);
        }

        Send_Data.Duration?.SetSkillEffect(this);
        Send_Data.Duration?.ExecSkill(Send_Data);
    }



    public override void Spawned()
    {
        base.Spawned();
    }

    public override void Despawned()
    {
        base.Despawned();

        var ec = GetEffectComponent();
        if (ec != null)
        {
            ec.ResetFollowingTarget();
        }
    }
}
