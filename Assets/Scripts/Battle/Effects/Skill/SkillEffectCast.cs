
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
            Delta += Time.deltaTime * Effect_Speed_Multiple;
            if (Delta > Duration)
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
            }
        }
    }
}
