
using UnityEngine;

public class SkillEffectCast : SkillEffectBase
{
    public override void StartParticle(float duration, bool loop = false)
    {
        var ec = GetEffectComponent();
        base.StartParticle(ec.Effect_Duration, loop);
    }

    private void Update()
    {
        if (Is_Action)
        {
            if (CheckParticleComplete())
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
                return;
                //Debug.Log($"파티클 종료 {gameObject.name} [{Delta}]");
            }

            Delta += Time.deltaTime;
            if (Delta > Duration)
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
            }
        }
    }
}
