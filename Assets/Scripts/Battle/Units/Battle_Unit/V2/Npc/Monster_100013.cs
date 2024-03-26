
using Spine;
/// <summary>
/// 엘프 연금술사 원거리 딜러 (전기/마력/요력/베리타리움) 모두 사용<br/>
/// 사용 NPC_ID<br/>
/// 100013 ~ 100016 까지 공동 사용<br/>
/// 같은 SD의 다른 이미지 사용이라 SD 이름만 다르고 모두 같음.
/// </summary>
public class Monster_100013 : MonsterBase_V2
{
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

                        SpawnSkillEffect_V3(skill_grp, skill);
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
                        SpawnSkillEffect_V3(skill_grp, skill);
                    }
                }
            }
        }
    }


}
