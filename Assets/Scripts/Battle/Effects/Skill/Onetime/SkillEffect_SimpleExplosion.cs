
using UnityEngine;

/// <summary>
/// 단순 이펙트만 보여주기 위한 기능
/// 차후 SkillEffect_NormalHeal 이런 것들과 같이 사용도 가능
/// 어차피 1회성 스킬 효과 데이터가 없으면 스킬 적용을 하지 않음.
/// </summary>
public class SkillEffect_SimpleExplosion : SkillEffectBase
{
    public override void StartParticle(float duration, bool loop)
    {
        var ec = GetEffectComponent();
        base.StartParticle(ec.Effect_Duration, loop);

        Send_Data.Onetime?.ExecSkill(Send_Data);
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
