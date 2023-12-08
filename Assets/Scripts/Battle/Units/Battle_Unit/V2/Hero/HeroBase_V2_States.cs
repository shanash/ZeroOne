using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class HeroBase_V2 : UnitBase_V2
{
    public override void UnitStateInitBegin()
    {
        CalcHeroAbility();
        AddLifeBar();
    }

    public override void UnitStateInit()
    {
        ChangeState(UNIT_STATES.READY);

    }

    public override void UnitStateReady()
    {
        ChangeState(UNIT_STATES.SPAWN);
    }
    public override void UnitStateIdle()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateStunBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateStun()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateSleepBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateSleep()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateFreezeBegin()
    {
        //Skeleton.AnimationState.ClearTracks();
        var tracks = FindAllTrakcs();
        int len = tracks.Length;
        for (int i = 0; i < len; i++)
        {
            tracks[i].TimeScale = 0f;
        }
    }
    public override void UnitStateFreeze()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateFreezeExit()
    {
        var tracks = FindAllTrakcs();
        int len = tracks.Length;
        for (int i = 0; i < len; i++)
        {
            tracks[i].TimeScale = 1f;
        }
    }

    public override void UnitStateBindBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateBind()
    {
        //  attack check

        CalcDurationSkillTime();
    }

    public override void UnitStateAttack01()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateMove()
    {
        CalcDurationSkillTime();
    }



    public override void UnitStateDeathBegin()
    {
        RemoveLifeBar();
        ClearDurationSkillDataList();
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.DEATH_01);
    }

    public override void UnitStateWinBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.WIN_01);
    }

    public override void UnitStateEndBegin()
    {
        //  team member remove
        Team_Mng.RemoveDeathMember(this);
    }
}
