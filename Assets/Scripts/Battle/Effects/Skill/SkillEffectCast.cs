
using UnityEngine;

public class SkillEffectCast : SkillEffectBase
{

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
