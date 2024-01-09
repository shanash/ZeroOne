using Spine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero_100001 : HeroBase_V2
{


    #region States
    

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

    public override void UnitStateMove()
    {
        MoveLeftTeam();
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

   
}
