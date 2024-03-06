
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
        if (Is_Action && !Is_Pause)
        {
            
            if (CheckParticleComplete())
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
                return;
                //Debug.Log($"파티클 종료 {gameObject.name} [{Delta}]");
            }

            Delta += Time.deltaTime;

            var ec = GetEffectComponent();
            if (ec.Change_Sort_Type != SORTING_TYPE.NONE)
            {
                if (Delta > ec.Change_Sorting_Layer_Delay_Time)
                {
                    if (!Sort_Group.sortingLayerName.Equals(ec.Change_Sort_Type.ToString()))
                    {
                        Sort_Group.sortingLayerName = ec.Change_Sort_Type.ToString();
                    }
                }
            }

            if (Delta > Duration)
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
            }
        }
    }
}
