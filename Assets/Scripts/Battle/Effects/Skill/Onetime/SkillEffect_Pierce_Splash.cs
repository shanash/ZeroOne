using UnityEngine;


/// <summary>
/// 타겟을 관통한 후 후방의 다른 적들에게도 추가 피해를 입히는 형식
/// </summary>
public class SkillEffect_Pierce_Splash : SkillEffectBase
{
    public override void StartParticle(float duration, bool loop = false)
    {
        base.StartParticle(duration, loop);

        if (Send_Data.Skill.GetSecondTargetRuleType() == SECOND_TARGET_RULE_TYPE.NONE)
        {
            SkillExec();
        }
        else
        {
            Send_Data.Onetime?.ExecSkill(Send_Data);
            SecondTargetSkillExec();
        }
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
