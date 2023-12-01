using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroTest : HeroBase
{
    float Test_Attack_Delta;


    public override void UnitStateAttackReady01()
    {
        //  타겟 찾기
        Attack_Target_List.Clear();
        Team_Mng.FindTarget(this, TARGET_TYPE.ENEMY_TEAM, TARGET_RULE_TYPE.RANDOM, 1, ref Attack_Target_List);
        if (Attack_Target_List.Count > 0)
        {
            SendAttackTargetArrowNodes();
            ChangeState(UNIT_STATES.ATTACK_1);
        }
    }

    public override void UnitStateAttack01Begin()
    {
        Test_Attack_Delta = 2.0f;
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.ATTACK_01);
    }
    public override void UnitStateAttack01()
    {
        Test_Attack_Delta -= Time.deltaTime;
        if (Test_Attack_Delta < 0.0f)
        {
            ChangeState(UNIT_STATES.ATTACK_END);
        }
    }


    public override void UnitStateAttackEndExit()
    {
        SendRemoveAttackTargetArrowNodes();
    }


    #region Spine Event Listener
    protected override void SpineAnimationComplete(TrackEntry entry)
    {
        base.SpineAnimationComplete(entry);

        string ani_name = entry.Animation.Name;
        if (ani_name.Equals("1_attack_1"))
        {
            PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
        }
    }

    protected override void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        base.SpineAnimationEvent(entry, evt);

        Debug.Log(evt.ToString());
    }
    #endregion
}
