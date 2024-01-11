using Spine;

public class Monster_100001 : MonsterBase_V2
{



    #region Etc Funcs
    protected override void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        switch (ani_type)
        {
            case HERO_PLAY_ANIMATION_TYPE.NONE:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                PlayAnimation(0, "00_idle", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                PlayAnimation(0, "00_run", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                PlayAnimation(0, "00_damage", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.STUN:
                PlayAnimation(0, "00_stun", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                PlayAnimation(0, "00_death", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                break;
        }
    }
    #endregion

    #region Spine Func Callback

    //protected override void SpineAnimationStart(TrackEntry entry)
    //{
    //    string animation_name = entry.Animation.Name;

    //    UNIT_STATES state = GetCurrentState();
    //    if (state == UNIT_STATES.ATTACK_1)
    //    {
    //        var skill = GetSkillManager().GetCurrentSkillGroup();
    //        if (animation_name.Equals(skill.GetSkillActionName()))
    //        {
    //            SpawnSkillCastEffect(skill);
    //        }
    //    }
    //}
    //protected override void SpineAnimationComplete(TrackEntry entry)
    //{
    //    string animation_name = entry.Animation.Name;
    //    UNIT_STATES state = GetCurrentState();
    //    if (state == UNIT_STATES.ATTACK_1)
    //    {
    //        var skill = GetSkillManager().GetCurrentSkillGroup();

    //        if (animation_name.Equals(skill.GetSkillActionName()))
    //        {
    //            GetSkillManager().SetNextSkillPattern();
    //            FindApproachTargets();
    //            if (Normal_Attack_Target.Count == 0)
    //            {
    //                ChangeState(UNIT_STATES.MOVE);
    //            }
    //            else
    //            {
    //                ChangeState(UNIT_STATES.ATTACK_READY_1);
    //            }
    //            return;
    //        }
    //    }


    //    base.SpineAnimationComplete(entry);
    //}

    //protected override void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    //{
    //    string animation_name = entry.Animation.Name;
    //    string evt_name = evt.Data.Name;
    //    UNIT_STATES state = GetCurrentState();

    //    if (state == UNIT_STATES.ATTACK_1)
    //    {
    //        var skill = GetSkillManager().GetCurrentSkillGroup();

    //        if (animation_name.Equals(skill.GetSkillActionName()))
    //        {
    //            var exec_list = skill.GetExecuableSkillDatas(evt_name);
    //            if (exec_list.Count > 0)
    //            {
    //                int cnt = exec_list.Count;
    //                for (int i = 0; i < cnt; i++)
    //                {
    //                    SkillEffectSpawnV2(exec_list[i]);
    //                }
    //            }
    //        }

    //    }
    //}

    #endregion

}
