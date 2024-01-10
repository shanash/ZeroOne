using UnityEngine;

public partial class HeroBase_V2 : UnitBase_V2
{

    protected override void InitStates()
    {
        FSM = new UnitStateSystem<UnitBase_V2, BattleManager_V2, BattleUIManager_V2>();

        FSM.AddTransition(new UnitStateInit_V2());
        FSM.AddTransition(new UnitStateReady_V2());
        FSM.AddTransition(new UnitStateSpawn_V2());
        FSM.AddTransition(new UnitStateIdle_V2());

        FSM.AddTransition(new UnitStateMove_V2());
        FSM.AddTransition(new UnitStateMoveIn_V2());

        FSM.AddTransition(new UnitStateAttackReady01_V2());
        FSM.AddTransition(new UnitStateAttack01_V2());
        //FSM.AddTransition(new UnitStateAttackReady02_V2());
        //FSM.AddTransition(new UnitStateAttack02_V2());
        FSM.AddTransition(new UnitStateAttackEnd_V2());

        FSM.AddTransition(new UnitStateSkillReady01_V2());
        FSM.AddTransition(new UnitStateSkill01_V2());
        //FSM.AddTransition(new UnitStateSkillReady02_V2());
        //FSM.AddTransition(new UnitStateSkill02_V2());
        //FSM.AddTransition(new UnitStateSkillReady03_V2());
        //FSM.AddTransition(new UnitStateSkill03_V2());
        FSM.AddTransition(new UnitStateSkillEnd_V2());

        FSM.AddTransition(new UnitStateStun_V2());
        FSM.AddTransition(new UnitStateSleep_V2());
        FSM.AddTransition(new UnitStateFreeze_V2());
        FSM.AddTransition(new UnitStateBind_V2());

        FSM.AddTransition(new UnitStateWaveRun_V2());
        FSM.AddTransition(new UnitStatePause_V2());

        FSM.AddTransition(new UnitStateWin_V2());
        FSM.AddTransition(new UnitStateLose_V2());
        FSM.AddTransition(new UnitStateDeath_V2());

        FSM.AddTransition(new UnitStateEnd_V2());

        SetSkeletonEventListener();

    }

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
        if (Team_Type == TEAM_TYPE.LEFT)
        {
            MoveLeftTeam();
        }
        else
        {
            MoveRightTeam();
        }
        CalcDurationSkillTime();
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


    public override void UnitStateDeathBegin()
    {
        RemoveLifeBar();
        ClearDurationSkillDataList();
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.DEATH_01);
        Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.DEATH);
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
