using Spine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 라일라
/// </summary>
public class Hero_100002 : HeroBase_V2
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

    /// <summary>
    /// 스파인 애니메이션 시작시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected override void SpineAnimationStart(TrackEntry entry)
    {
        string animation_name = entry.Animation.Name;
        entry.TimeScale = Battle_Speed_Multiple;

        if (animation_name.Equals("<empty>"))
        {
            PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
            return;
        }

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

                //  sfx sound
                PlaySkillVoiceAndSfx(skill_grp);

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
                //  sfx sound
                PlaySkillVoiceAndSfx(skill_grp);
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

    #endregion


}
