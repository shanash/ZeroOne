using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// 세컨 타겟 스킬<br/>
    /// 기준 타겟을 포함하여 새로운 타겟을 설정하고, 해당 타겟에 2차 일회성 효과를 적용한다.
    /// </summary>
    protected void SecondTargetSkillExec()
    {
        if (Send_Data.Skill == null)
        {
            return;
        }
        if (Send_Data.Targets.Count == 0)
        {
            return;
        }

        List<HeroBase_V2> targets = new List<HeroBase_V2>();

        var skill = Send_Data.Skill;
        var first_target = Send_Data.Targets.First();
        TeamManager_V2 team_mng = first_target.GetTeamManager();
        team_mng.FindSecondTargetInRange(first_target, skill.GetSecondTargetRuleType(), (float)skill.GetSecondTargetRange(), skill.GetMaxSecondTargetCount(), ref targets);

        ExecSecondTargetOnetimeSkills(targets);
        ExecSecondTargetDurationSkills(targets);
    }

    void ExecSecondTargetOnetimeSkills(List<HeroBase_V2> targets)
    {
        var onetime_list = Send_Data.Skill.GetSecondTargetOnetimeSkillDataList();
        int t_cnt = targets.Count;
        for (int t = 0; t < t_cnt; t++)
        {
            var target = targets[t];

            var sdata = Send_Data.Clone();
            sdata.ClearTargets();
            sdata.AddTarget(target);

            int cnt = onetime_list.Count;
            for (int i = 0; i < cnt; i++)
            {
                var onetime = onetime_list[i];
                sdata.Onetime = onetime;

                string effect_path = onetime.GetEffectPrefab();
                if (string.IsNullOrEmpty(effect_path))
                {
                    onetime.ExecSkill(sdata);
                    continue;
                }

                PROJECTILE_TYPE ptype = onetime.GetProjectileType();
                var target_trans = target.GetBodyPositionByProjectileType(ptype);
                var target_pos = target_trans.position;

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path);
                effect.SetBattleSendData(sdata);
                target_pos.z = target.transform.position.z;
                effect.transform.position = target_pos;
                effect.StartParticle((float)onetime.GetEffectDuration());
            }

        }
    }

    void ExecSecondTargetDurationSkills(List<HeroBase_V2> targets)
    {
        var duration_list = Send_Data.Skill.GetSecondTargetDurationSkillDataList();
        int t_cnt = targets.Count;
        for (int t = 0; t < t_cnt; t++)
        {
            var target = targets[t];

            var sdata = Send_Data.Clone();
            sdata.ClearTargets();
            sdata.AddTarget(target);

            int cnt = duration_list.Count;
            for (int i = 0; i < cnt; i++)
            {
                var duration = duration_list[i];
                sdata.Duration = duration;

                string effect_path = duration.GetEffectPrefab();
                if (string.IsNullOrEmpty(effect_path))
                {
                    duration.ExecSkill(sdata);
                    continue;
                }

                PROJECTILE_TYPE ptype = duration.GetProjectileType();
                var target_trans = target.GetBodyPositionByProjectileType(ptype);

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path);
                effect.SetBattleSendData(sdata);
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

    /// <summary>
    /// 이펙트에서 각각의 일회성/지속성 효과를 구현하는 것이 아닌,<br/>
    /// 스킬의 트리거로서의 역할을 하기 위해<br/>
    /// 서브로 있는 일회성/지속성 효과를 실행 적용해줘야 함
    /// </summary>
    protected void SkillExec()
    {
        if (Send_Data.Skill == null)
        {
            return;
        }

        List<HeroBase_V2> targets = new List<HeroBase_V2>();

        var skill = Send_Data.Skill;

        if (skill.GetSecondTargetRuleType() != SECOND_TARGET_RULE_TYPE.NONE)
        {
            var first_target = Send_Data.Targets.First();
            TeamManager_V2 team_mng = first_target.GetTeamManager();
            team_mng.FindSecondTargetInRange(first_target, skill.GetSecondTargetRuleType(), (float)skill.GetSecondTargetRange(), skill.GetMaxSecondTargetCount(), ref targets);
        }
        else
        {
            targets.AddRange(Send_Data.Targets);
        }

        ExecOnetimeSkills(targets);
        ExecDurationSkills(targets);
    }
    /// <summary>
    /// 일회성 스킬 이펙트 구현
    /// </summary>
    protected void ExecOnetimeSkills(List<HeroBase_V2> targets)
    {
        var onetime_list = Send_Data.Skill.GetOnetimeSkillDataList();
        int t_cnt = targets.Count;
        for (int t = 0; t < t_cnt; t++)
        {
            var target = targets[t];

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
    protected void ExecDurationSkills(List<HeroBase_V2> targets)
    {
        var duration_list = Send_Data.Skill.GetDurationSkillDataList();
        int t_cnt = targets.Count;
        for (int t = 0; t < t_cnt; t++)
        {
            var target = targets[t];

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
