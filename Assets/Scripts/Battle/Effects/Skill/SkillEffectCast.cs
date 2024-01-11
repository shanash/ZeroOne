
using UnityEngine;

public class SkillEffectCast : SkillEffectBase
{
    public override void StartParticle(float duration, bool loop = false)
    {
        var ec = GetEffectComponent();
        if (ec != null)
        {
            base.StartParticle(ec.Effect_Duration, loop);
        }
        else
        {
            base.StartParticle(duration, loop);
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
