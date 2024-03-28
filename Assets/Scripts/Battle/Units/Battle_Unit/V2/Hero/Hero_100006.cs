using UnityEngine.Timeline;
using UnityEngine;
using Spine;
/// <summary>
/// 에일린
/// </summary>
public class Hero_100006 : HeroBase_V2
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
                if (evt_name.Equals("apply_3"))
                {
                    bool a = false;
                }
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


    //public override void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    //{
    //    //  감추기
    //    if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_hide.ToString()))
    //    {
    //        HideCharacters(evt_val);
    //    }   //  보이기
    //    else if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_show.ToString()))
    //    {
    //        ShowCharacters(evt_val);
    //    }
    //}

    #endregion





}
