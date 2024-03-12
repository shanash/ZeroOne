using UnityEngine.Timeline;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

/// <summary>
/// 바이올렛
/// </summary>
public class Hero_100003 : HeroBase_V2
{


    #region States

    public override void UnitStateSkillReady01Begin()
    {
        SetPlayableDirector();
    }
    public override void UnitStateSkillReady01()
    {
        ChangeState(UNIT_STATES.SKILL_1);
    }


    #endregion

    #region Etc Funcs
    protected override void SetPlayableDirector()
    {
        base.SetPlayableDirector();
        StartPlayableDirector();
    }

    protected override void UnsetPlayableDirector()
    {
        base.UnsetPlayableDirector();
    }

    public override void UltimateSkillExec()
    {
        var battle_state = Battle_Mng.GetCurrentState();
        if (battle_state != GAME_STATES.PLAYING)
        {
            return;
        }
        //  궁극기를 사용할 수 없는 상태
        var state = GetCurrentState();
        if (state == UNIT_STATES.SKILL_1 || state == UNIT_STATES.SKILL_READY_1 || state == UNIT_STATES.SKILL_END || state == UNIT_STATES.ULTIMATE_PAUSE)
        {
            return;
        }

        //  여러가지 상황상 궁극기를 사용할 수 없는 상황을 체크
        //  체크 완료 후 궁극기를 사용할 수 있는 경우에만 궁극기 사용
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        if (skill_grp == null)
        {
            return;
        }
        //  궁극기가 쿨타임이 남아있을 경우 안됨
        if (!skill_grp.IsPrepareCooltime())
        {
            return;
        }

        for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
        {
            var skill = skill_grp.GetBattleSkillDataList()[i];
            FindTargetsSkillAddTargets(skill);
        }

        var target_skill = skill_grp.GetSpecialSkillTargetSkill();
        //if (target_skill != null)
        //{
        //    FindTargetsSkillAddTargets(target_skill);
        //}
        //  타겟이 잡혀있지 않다면, 일단 스킬 사용 안되도록
        if (target_skill.IsEmptyFindTarget())
        {
            return;
        }

        //  주인공 이외의 모든 캐릭 pause
        Battle_Mng.AllPauseUnitWithoutHero(this);

        //  진행중이던 이펙트 모두 숨기기
        Battle_Mng.GetEffectFactory().OnPauseAndHide();

        //  캐릭터 숨기기는 이벤트를 받아서 하자.

        Skeleton.AnimationState.ClearTracks();
        if (state == UNIT_STATES.ATTACK_READY_1 || state == UNIT_STATES.ATTACK_1 || state == UNIT_STATES.ATTACK_END)
        {
            GetSkillManager().SetNextSkillPattern();
        }

        Battle_Mng.StartUltimateSkill();
        ChangeState(UNIT_STATES.SKILL_READY_1);
    }

    public override void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    {
        //  감추기
        if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_hide.ToString()))
        {
            HideCharacters(evt_val);
        }   //  보이기
        else if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_show.ToString()))
        {
            ShowCharacters(evt_val);
        }
    }
    /// <summary>
    /// 궁극기 시전 중 캐릭터 숨기기 이벤트 적용
    /// </summary>
    /// <param name="evt_val"></param>
    protected void HideCharacters(EventTriggerValue evt_val)
    {
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        List<BattleSkillData> skill_list = new List<BattleSkillData>();
        if (string.IsNullOrEmpty(evt_val.StrValue))
        {
            var target_skill = skill_grp.GetSpecialSkillTargetSkill();
            if (target_skill != null)
            {
                skill_list.Add(target_skill);
            }
        }
        else
        {
            skill_list.AddRange(skill_grp.GetExecutableSkillDatas(evt_val.StrValue));
        }
        if (skill_list.Count == 0)
        {
            return;
        }

        //  제외 타겟 리스트
        List<HeroBase_V2> exclude_targets = new List<HeroBase_V2>();
        switch (evt_val.IntValue)
        {
            case 0:     //  시전자와 타겟을 제외한 모든 캐릭터 숨기기
                exclude_targets.Add(this);
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!exclude_targets.Contains(target))
                        {
                            exclude_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.HideAllUnitWithoutTargets(exclude_targets);
                break;
            case 1:     //  시전자를 제외한 모든 캐릭터 숨기기
                exclude_targets.Add(this);
                Battle_Mng.HideAllUnitWithoutTargets(exclude_targets);
                break;
            case 2:     //  타겟을 제외한 모든 캐릭터 숨기기
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!exclude_targets.Contains(target))
                        {
                            exclude_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.HideAllUnitWithoutTargets(exclude_targets);
                break;
            case 3:     //  모든 캐릭터 숨기기
                Battle_Mng.HideAllUnits();
                break;
        }
    }
    /// <summary>
    /// 궁극기 시전 중 캐릭터 보이기 이벤트 적용
    /// </summary>
    /// <param name="evt_val"></param>
    protected void ShowCharacters(EventTriggerValue evt_val)
    {
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        List<BattleSkillData> skill_list = new List<BattleSkillData>();
        if (string.IsNullOrEmpty(evt_val.StrValue))
        {
            var target_skill = skill_grp.GetSpecialSkillTargetSkill();
            if (target_skill != null)
            {
                skill_list.Add(target_skill);
            }
        }
        else
        {
            skill_list.AddRange(skill_grp.GetExecutableSkillDatas(evt_val.StrValue));
        }
        if (skill_list.Count == 0)
        {
            return;
        }
        //  포함 타겟 리스트
        List<HeroBase_V2> include_targets = new List<HeroBase_V2>();
        switch (evt_val.IntValue)
        {
            case 0:     //  시전자와 타겟만 보이기
                include_targets.Add(this);
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!include_targets.Contains(target))
                        {
                            include_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.ShowUnitTargets(include_targets);
                break;
            case 1:     //  시전자만 보이기
                include_targets.Add(this);
                Battle_Mng.ShowUnitTargets(include_targets);
                break;
            case 2:     //  타겟만 보이기
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!include_targets.Contains(target))
                        {
                            include_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.ShowUnitTargets(include_targets);
                break;
            case 3:     //  모든 캐릭터 보이기
                Battle_Mng.ShowAllUnits();
                break;
        }
    }

    #endregion




}
