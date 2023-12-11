using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(RendererSortingZ))]
[RequireComponent (typeof(SortingGroup))]
[RequireComponent(typeof(EffectFollowingComponent))]
public class SkillEffectBase : EffectBase
{
    [SerializeField, Tooltip("Mover")]
    protected EasingMover3D Mover;

    [SerializeField, Tooltip("Effect Hide Transforms")]
    protected EffectHideComponent Hide_Comp;

    protected BATTLE_SEND_DATA Send_Data;

    public void SetBattleSendData(BATTLE_SEND_DATA d)
    {
        this.Send_Data = d;
    }

    /// <summary>
    /// 이펙트에서 각각의 일회성/지속성 효과를 구현하는 것이 아닌,
    /// 스킬의 트리거로서의 역할을 하기 위해
    /// 서브로 있는 일회성/지속성 효과를 실행 적용해줘야 함
    /// </summary>
    protected void SkillExec()
    {
        if (Send_Data.Skill == null)
        {
            return;
        }

        ExecOnetimeSkills();
        ExecDurationSkills();
    }
    /// <summary>
    /// 일회성 스킬 이펙트 구현
    /// </summary>
    protected void ExecOnetimeSkills()
    {
        var onetime_list = Send_Data.Skill.GetOnetimeSkillDataList();
        int t_cnt = Send_Data.Targets.Count;
        for (int t = 0; t < t_cnt; t++)
        {
            var target = Send_Data.Targets[t];

            int cnt = onetime_list.Count;
            for (int i = 0; i < cnt; i++)
            {
                var onetime = onetime_list[i];
                string effect_path = onetime.GetEffectPrefab();
                if (string.IsNullOrEmpty(effect_path))
                {
                    onetime.ExecSkill(Send_Data);
                    continue;
                }

                Send_Data.Onetime = onetime;

                PROJECTILE_TYPE ptype = onetime.GetProjectileType();
                var target_trans = target.GetBodyPositionByProjectileType(ptype);
                var target_pos = target_trans.position;

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path);
                effect.SetBattleSendData(Send_Data);
                target_pos.z = target.transform.position.z;
                effect.transform.position = target_pos;
                effect.StartParticle((float)onetime.GetEffectDuration());
            }

        }
        
    }

    /// <summary>
    /// 지속섣 스킬 이펙트 구현
    /// </summary>
    protected void ExecDurationSkills()
    {
        var duration_list = Send_Data.Skill.GetDurationSkillDataList();
        int t_cnt = Send_Data.Targets.Count;
        for (int t = 0; t < t_cnt; t++)
        {
            var target = Send_Data.Targets[t];

            int cnt = duration_list.Count;
            for (int i = 0; i < cnt; i++)
            {
                var duration = duration_list[i];
                string effect_path = duration.GetEffectPrefab();
                if (string.IsNullOrEmpty(effect_path))
                {
                    duration.ExecSkill(Send_Data);
                    continue;
                }
                Send_Data.Duration = duration;

                PROJECTILE_TYPE ptype = duration.GetProjectileType();
                var target_trans = target.GetBodyPositionByProjectileType(ptype);

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path);
                effect.SetBattleSendData(Send_Data);
                var target_pos = target_trans.position;
                target_pos.z = target.transform.position.z;
                effect.transform.position = target_pos;

                float eff_dur = (float)duration.GetEffectDuration();
                if (duration.IsThrowingNode())
                {
                    effect.MoveTarget(target_trans, eff_dur);
                }
                else
                {
                    effect.StartParticle(target_trans, eff_dur, eff_dur == 0);
                    
                }
                
            }
        }

        
    }


    public override void Spawned()
    {
        base.Spawned();
        Send_Data.Reset();
    }
    

}
