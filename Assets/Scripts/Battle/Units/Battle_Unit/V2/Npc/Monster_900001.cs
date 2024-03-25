using Spine;
using UnityEngine;

public class Monster_900001 : MonsterBase_V2
{

    #region States
    public override void UnitStateSkillReady01()
    {
        ChangeState(UNIT_STATES.SKILL_1);
    }

    public override void UnitStateSkill01Begin()
    {
        if (IsPreviousStatePause() || IsPreviousStateUltimatePause())
        {
            return;
        }
        string skill_action_name = GetSkillManager().GetSpecialSkillGroup().GetSkillActionName();
        var name_list = skill_action_name.Split('_');
        int track = 0;
        if (name_list.Length > 0)
        {
            track = int.Parse(name_list[0]);
        }
        PlayAnimation(track, skill_action_name, false);
    }

    #endregion




    #region Etc Funcs

    public override void UltimateSkillExec()
    {
        var battle_state = Battle_Mng.GetCurrentState();
        if (battle_state != GAME_STATES.PLAYING)
        {
            return;
        }
        var state = GetCurrentState();
        if (state == UNIT_STATES.SKILL_1 || state == UNIT_STATES.SKILL_READY_1 || state == UNIT_STATES.SKILL_END || state == UNIT_STATES.ULTIMATE_PAUSE)
        {
            return;
        }
        var ultimate_skill = GetSkillManager().GetSpecialSkillGroup();
        var target_skill = ultimate_skill.GetSpecialSkillTargetSkill();
        if (target_skill != null)
        {
            FindTargetsSkillAddTargets(target_skill);
        }

        ChangeState(UNIT_STATES.SKILL_READY_1);
    }

    protected override void AttackAnimationComplete()
    {
        //  궁극기가 있는지 찾는다.
        var ultimate_skill = GetSkillManager().GetSpecialSkillGroup();
        if (ultimate_skill != null)
        {
            if (ultimate_skill.IsPrepareCooltime())
            {
                //  궁극기 사용 요청
                UltimateSkillExec();
            }
            else
            {
                //  궁극기 준비되어 있지 않으면 다음 순서의 일반 스킬 사용
                ChangeState(UNIT_STATES.ATTACK_READY_1);
            }
        }
        else
        {
            //  없으면 다음 순서의 일반 스킬 사용
            ChangeState(UNIT_STATES.ATTACK_READY_1);
        }
    }
    /// <summary>
    /// 스파인 애니메이션 시작시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected override void SpineAnimationStart(TrackEntry entry)
    {
        string animation_name = entry.Animation.Name;
        entry.TimeScale = Battle_Speed_Multiple;

        UNIT_STATES state = GetCurrentState();
        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill_grp = GetSkillManager().GetCurrentSkillGroup();
            //  스킬 시작시 각 스킬의 타겟을 미리 설정해준다.(중간에 변경되는 일이 없도록 하기 위해)
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
                {
                    var skill = skill_grp.GetBattleSkillDataList()[i];
                    FindTargetsSkillAddTargets(skill);
                }
                SpawnSkillCastEffect(skill_grp);
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            var skill_grp = GetSkillManager().GetSpecialSkillGroup();
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
                {
                    var skill = skill_grp.GetBattleSkillDataList()[i];
                    FindTargetsSkillAddTargets(skill);
                }

                SpawnSkillCastEffect(skill_grp);
            }
        }
    }

    /// <summary>
    /// 스파인 애니메이션 동작 완료시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected override void SpineAnimationComplete(TrackEntry entry)
    {
        string animation_name = entry.Animation.Name;

        UNIT_STATES state = GetCurrentState();
        if (state == UNIT_STATES.PAUSE)
        {
            return;
        }

        if (state == UNIT_STATES.DEATH)
        {
            if (animation_name.Equals("00_death"))
            {
                ChangeState(UNIT_STATES.END);
            }
        }
        else if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();

            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                GetSkillManager().SetNextSkillPattern();
                FindApproachTargets();
                if (Approach_Targets.Count == 0)
                {
                    ChangeState(UNIT_STATES.MOVE);
                }
                else
                {
                    AttackAnimationComplete();
                }
                return;
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            if (animation_name.Equals("00_ultimate"))
            {
                UnsetPlayableDirector();
                var skill = GetSkillManager().GetSpecialSkillGroup();
                if (skill != null)
                {
                    skill.ResetSkill();
                }
                Battle_Mng.FinishUltimateSkill(this);
            }
            else if (animation_name.Equals("00_ultimate_enemy"))
            {
                var skill = GetSkillManager().GetSpecialSkillGroup();
                if (skill != null)
                {
                    skill.ResetSkill();
                }
                ChangeState(UNIT_STATES.ATTACK_READY_1);
            }
        }
    }

    /// <summary>
    /// 스파인 애니메이션 동작 플레이 중 호출되는 이벤트를 받아주는 리스너
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="evt"></param>
    protected override void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        string animation_name = entry.Animation.Name;
        string evt_name = evt.Data.Name;
        UNIT_STATES state = GetCurrentState();

        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill_grp = GetSkillManager().GetCurrentSkillGroup();
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                var exec_list = skill_grp.GetExecutableCloneSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        var skill = exec_list[i];
                        if (skill.IsEmptyFindTarget())
                        {
                            FindTargetsSkillAddTargets(skill);
                        }

                        SpawnSkillEffect_V3(exec_list[i]);
                    }
                }
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            var skill_grp = GetSkillManager().GetSpecialSkillGroup();
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                var exec_list = skill_grp.GetExecutableCloneSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        var skill = exec_list[i];
                        if (skill.IsEmptyFindTarget())
                        {
                            FindTargetsSkillAddTargets(skill);
                        }
                        SpawnSkillEffect_V3(skill);
                    }
                }
            }
        }
    }

    #endregion


}
