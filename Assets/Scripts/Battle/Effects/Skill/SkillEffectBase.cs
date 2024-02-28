using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(RendererSortingZ))]
[RequireComponent(typeof(SortingGroup))]
public class SkillEffectBase : EffectBase
{
 
    protected BATTLE_SEND_DATA Send_Data;

    protected EffectComponent Effect_Comp;

    protected RendererSortingZ Sorting_Z;


    public void SetBattleSendData(BATTLE_SEND_DATA d)
    {
        this.Send_Data = d;
    }

    public override void SetEffectSpeedMultiple(float multiple)
    {
        base.SetEffectSpeedMultiple(multiple);

        var ec = GetEffectComponent();
        if (ec != null)
        {
            this.Duration = ec.Effect_Duration / Effect_Speed_Multiple;
        }
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

        ExecSecondTargetOnetimeSkills_V2(targets);
        ExecSecondTargetDurationSkills_V2(targets);
    }

    

    void ExecSecondTargetOnetimeSkills_V2(List<HeroBase_V2> targets)
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
            for (int o = 0; o < cnt; o++)
            {
                var onetime = onetime_list[o];
                string effect_path = onetime.GetEffectPrefab();
                if (string.IsNullOrEmpty(effect_path))
                {
                    onetime.ExecSkill(Send_Data);
                    continue;
                }
                sdata.Onetime = onetime;

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path, sdata.Caster.GetUnitScale());
                effect.SetBattleSendData(sdata);

                var ec = GetEffectComponent();
                var target_trans = ec.GetTargetReachPosition(target);
                var target_pos = target_trans.position;
                target_pos.z = target.transform.position.z;
                effect.transform.position = target_pos;
                effect.StartParticle(ec.Effect_Duration);
            }
        }
    }

    void ExecSecondTargetDurationSkills_V2(List<HeroBase_V2> targets)
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

            for (int d = 0; d < cnt; d++)
            {
                var duration = duration_list[d];
                string effect_path = duration.GetEffectPrefab();
                if (string.IsNullOrEmpty(effect_path))
                {
                    duration.ExecSkill(sdata);
                    continue;
                }

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path, target.GetUnitScale());
                effect.SetBattleSendData(sdata);

                var ec = GetEffectComponent();
                var target_trans = ec.GetTargetReachPosition(target);
                var target_pos = target_trans.position;
                effect.transform.position = target_pos;
                if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE)
                {
                    float distance = Vector3.Distance(this.transform.position, target_trans.position);
                    float flying_time = 0f;
                    if (ec.Projectile_Duration > 0f)
                    {
                        flying_time = ec.Projectile_Duration;
                    }
                    else
                    {
                        flying_time = distance / ec.Projectile_Velocity;
                    }
                    effect.MoveTarget(target_trans, flying_time);
                }
                else if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                {
                    effect.StartParticle(target_trans, ec.Effect_Duration, ec.Is_Loop);
                }
                else
                {
                    Debug.Assert(false);
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

        ExecOnetimeSkills_V2(targets);
        ExecDurationSkills_V2(targets);
    }

    /// <summary>
    /// 일회성 스킬 이펙트 구현
    /// </summary>
    protected void ExecOnetimeSkills_V2(List<HeroBase_V2> targets)
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

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path, Send_Data.Caster.GetUnitScale());
                effect.SetBattleSendData(Send_Data);

                var ec = effect.GetEffectComponent();
                var target_trans = ec.GetTargetReachPosition(target);
                var target_pos = target_trans.position;
                target_pos.z = target.transform.position.z;
                effect.transform.position = target_pos;
                effect.StartParticle(ec.Effect_Duration);
            }
        }
    }


    /// <summary>
    /// 지속섣 스킬 이펙트 구현
    /// </summary>
    protected void ExecDurationSkills_V2(List<HeroBase_V2> targets)
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

                var effect = (SkillEffectBase)Factory.CreateEffect(effect_path, target.GetUnitScale());
                effect.SetBattleSendData(Send_Data);

                var ec = effect.GetEffectComponent();
                if (ec != null)
                {
                    var target_trans = ec.GetTargetReachPosition(target);
                    var target_pos = target_trans.position;
                    target_pos.z = target.transform.position.z;
                    effect.transform.position = target_pos;

                    if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE)
                    {
                        float distance = Vector3.Distance(this.transform.position, target_trans.position);
                        float flying_time = 0f;
                        if (ec.Projectile_Duration > 0f)
                        {
                            flying_time = ec.Projectile_Duration;
                        }
                        else
                        {
                            flying_time = distance / ec.Projectile_Velocity;
                        }
                        effect.MoveTarget(target_trans, flying_time);
                    }
                    else if(ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                    {
                        effect.StartParticle(target_trans, ec.Effect_Duration, ec.Is_Loop);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
                //else
                //{
                //    PROJECTILE_TYPE ptype = duration.GetProjectileType();
                //    var target_trans = target.GetBodyPositionByProjectileType(ptype);

                //    var target_pos = target_trans.position;
                //    target_pos.z = target.transform.position.z;
                //    effect.transform.position = target_pos;

                //    float eff_dur = (float)duration.GetEffectDuration();
                //    if (duration.IsThrowingNode())
                //    {
                //        effect.MoveTarget(target_trans, eff_dur);
                //    }
                //    else
                //    {
                //        effect.StartParticle(target_trans, eff_dur, eff_dur == 0);

                //    }
                //}


            }
        }
    }

    public EffectComponent GetEffectComponent()
    {
        CheckEffectComponent();
        return Effect_Comp;
    }

    protected void CheckEffectComponent()
    {
        if (Effect_Comp == null)
        {
            Effect_Comp = GetComponent<EffectComponent>();
        }
    }

    protected void CheckRenderSortingZ()
    {
        if (Sorting_Z == null)
        {
            Sorting_Z = GetComponent<RendererSortingZ>();
        }
    }

    public override void Show(bool show)
    {
        CheckRenderSortingZ();
        Sorting_Z.ShowGameObject(show);
    }

    public override void Spawned()
    {
        base.Spawned();
        CheckEffectComponent();
        CheckRenderSortingZ();
        Send_Data.Reset();
    }

    public override void Despawned()
    {
        base.Despawned();
        CheckRenderSortingZ();
        Sorting_Z.ResetSortingZ();
    }


}
