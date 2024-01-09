using UnityEngine;

public class SkillEffect_Duration_Simple : SkillEffectBase
{
    EffectFollowingComponent Following;


    public override void StartParticle(Transform target, float duration, bool loop = false)
    {
        base.StartParticle(target, duration, loop);

        Following?.SetTarget(target);

        //  add duration skill data
        Send_Data.Duration?.SetSkillEffect(this);
        Send_Data.Duration?.ExecSkill(Send_Data);

    }

    public override void MoveTarget(Transform target, float duration)
    {
        base.MoveTarget(target, duration);

        Is_Loop = duration == 0f;
        Following?.SetTarget(target);

        Send_Data.Duration?.SetSkillEffect(this);
        Send_Data.Duration?.ExecSkill(Send_Data);
    }



    public override void Spawned()
    {
        base.Spawned();

        if (Following == null)
        {
            Following = GetComponent<EffectFollowingComponent>();
        }
    }

    public override void Despawned()
    {
        base.Despawned();

        Following?.ResetComponent();
    }
}
