using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero_100001 : HeroBase_V2
{


    #region States
    public override void UnitStateMoveInBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.RUN_01);
    }

    public override void UnitStateMoveIn()
    {
        if (Team_Type == TEAM_TYPE.LEFT)
        {
            MoveLeftTeam();
        }
        else
        {
            MoveRightTeam();
        }
        
    }

    public override void UnitStateMoveBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.RUN_01);
    }
    public override void UnitStateMove()
    {
        MoveLeftTeam();
        base.UnitStateMove();
        
    }


    public override void UnitStateWaveRunBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.RUN_01);
    }
    public override void UnitStateWaveRun()
    {
        WaveRunLeftTeam();
        base.UnitStateWaveRun();
    }

    public override void UnitStateAttackReady01Begin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateAttackReady01()
    {
        bool is_delay_finish = Skill_Mng.CalcSkillUseDelay(Time.deltaTime);
        if (is_delay_finish)
        {
            FindApproachTargets();
            if (Normal_Attack_Target.Count == 0)
            {
                ChangeState(UNIT_STATES.MOVE);
                return;
            }
            ChangeState(UNIT_STATES.ATTACK_1);
        }

        base.UnitStateAttackReady01();
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
    protected override void SpineAnimationComplete(TrackEntry entry)
    {
        string animation_name = entry.Animation.Name;
    
        UNIT_STATES state = GetCurrentState();
        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();

            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                GetSkillManager().SetNextSkillPattern();
                FindApproachTargets();
                if (Normal_Attack_Target.Count == 0)
                {
                    ChangeState(UNIT_STATES.MOVE);
                }
                else
                {
                    ChangeState(UNIT_STATES.ATTACK_READY_1);
                }
                return;
            }
        }
        

        base.SpineAnimationComplete(entry);
    }

    

    protected override void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        string animation_name = entry.Animation.Name;
        string evt_name = evt.Data.Name;
        UNIT_STATES state = GetCurrentState();

        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();

            if (evt_name.Equals("debuff"))
            {
                bool a = false;
            }
            
            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                var exec_list = skill.GetExecuableSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        SkillEffectSpawnV2(exec_list[i]);
                    }
                }
            }

        }
    }
    #endregion

}
