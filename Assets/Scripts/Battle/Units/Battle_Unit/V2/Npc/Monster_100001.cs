using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_100001 : MonsterBase_V2
{


    #region States

  

    public override void UnitStateMoveIn()
    {
        MoveRightTeam();
    }
    

    public override void UnitStateMove()
    {
        MoveRightTeam();
        base.UnitStateMove();

    }

    
    public override void UnitStateAttack01Begin()
    {

        string skill_action_name = Skill_Mng.GetCurrentSkillGroup().GetSkillActionName();
        var name_list = skill_action_name.Split('_');
        int track = 0;
        if (name_list.Length > 0)
        {
            track = int.Parse(name_list[0]);
        }
        PlayAnimation(track, skill_action_name, false);
    }



    #endregion




    #region Spine Func Callback
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
