using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 지역 전체를 뒤덮는 이펙트 효과
/// 
/// </summary>
public class SkillEffect_AllRound : SkillEffectBase
{
    public override void StartParticle(float duration, bool loop = false)
    {
        base.StartParticle(duration, loop);

        SkillExec();

    }
    private void Update()
    {
        if (Is_Action)
        {
            Delta += Time.deltaTime;
            if (Delta > Duration)
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
            }
        }
    }

}
