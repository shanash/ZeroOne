using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

public partial class UnitBase_V2 : MonoBehaviour, IUpdateComponent
{
    protected UnitStateSystem<UnitBase_V2, BattleManager_V2, BattleUIManager_V2> FSM = null;

    protected BattleManager_V2 Battle_Mng;
    protected BattleUIManager_V2 UI_Mng;

    private void Awake()
    {
        OnAwake();
        InitStates();
    }
    protected virtual void OnAwake() { }
    protected virtual void InitStates()
    {
        FSM = new UnitStateSystem<UnitBase_V2, BattleManager_V2, BattleUIManager_V2>();

    }

    public virtual void Lazy_Init(BattleManager_V2 mng, BattleUIManager_V2 ui, UNIT_STATES trans)
    {
        Battle_Mng = mng;
        UI_Mng = ui;
        if (FSM == null)
        {
            InitStates();
        }
        FSM.Lazy_Init_Setting(this, mng, ui, trans);
    }

    public void ChangeState(UNIT_STATES trans)
    {
        if (IsPause() || IsUltimatePause())
        {
            return;
        }
        FSM?.ChangeState(trans);
    }

    public void RevertState()
    {
        FSM?.RevertState();
    }
    public UNIT_STATES GetCurrentState()
    {
        if (FSM != null)
        {
            return FSM.CurrentTransitionID;
        }
        return UNIT_STATES.NONE;
    }
    public UNIT_STATES GetPreviousState()
    {
        if (FSM != null)
        {
            return FSM.PreviousTransitionID;
        }
        return UNIT_STATES.NONE;
    }

    public List<UNIT_STATES> GetStatesHistory()
    {
        if (FSM != null)
        {
            return FSM.GetStatesHistory();
        }
        return null;
    }

    public bool IsReservedState(UNIT_STATES trans)
    {
        if (FSM != null)
        {
            return FSM.IsReservedState(trans);
        }
        return false;
    }

    public bool IsPause()
    {
        return GetCurrentState() == UNIT_STATES.PAUSE;
    }

    public bool IsUltimatePause()
    {
        return GetCurrentState() == UNIT_STATES.ULTIMATE_PAUSE;
    }

    public bool IsPreviousStatePause()
    {
        return GetPreviousState() == UNIT_STATES.PAUSE;
    }

    public virtual void OnUpdate(float dt)
    {
        FSM?.UpdateState();
    }

    #region Unit States Func

    public virtual void UnitStateInitBegin() { }
    public virtual void UnitStateInit() { }
    public virtual void UnitStateInitExit() { }

    public virtual void UnitStateReadyBegin() { }
    public virtual void UnitStateReady() { }
    public virtual void UnitStateReadyExit() { }

    public virtual void UnitStateMoveInBegin() { }
    public virtual void UnitStateMoveIn() { }
    public virtual void UnitStateMoveInExit() { }

    public virtual void UnitStateMoveBegin() { }
    public virtual void UnitStateMove() { }
    public virtual void UnitStateMoveExit() { }

    public virtual void UnitStateSpawnBegin() { }
    public virtual void UnitStateSpawn() { }
    public virtual void UnitStateSpawnExit() { }


    public virtual void UnitStateIdleBegin() { }
    public virtual void UnitStateIdle() { }
    public virtual void UnitStateIdleExit() { }

    public virtual void UnitStatePlayingBegin() { }
    public virtual void UnitStatePlaying() { }
    public virtual void UnitStatePlayingExit() { }


    public virtual void UnitStateAttackReady01Begin() { }
    public virtual void UnitStateAttackReady01() { }
    public virtual void UnitStateAttackReady01Exit() { }

    public virtual void UnitStateAttack01Begin() { }
    public virtual void UnitStateAttack01() { }
    public virtual void UnitStateAttack01Exit() { }


    public virtual void UnitStateAttackEndBegin() { }
    public virtual void UnitStateAttackEnd() { }
    public virtual void UnitStateAttackEndExit() { }


    public virtual void UnitStateSkillReady01Begin() { }
    public virtual void UnitStateSkillReady01() { }
    public virtual void UnitStateSkillReady01Exit() { }

    public virtual void UnitStateSkill01Begin() { }
    public virtual void UnitStateSkill01() { }
    public virtual void UnitStateSkill01Exit() { }

    
    public virtual void UnitStateSkillEndBegin() { }
    public virtual void UnitStateSkillEnd() { }
    public virtual void UnitStateSkillEndExit() { }

    public virtual void UnitStateWaveRunBegin() { }
    public virtual void UnitStateWaveRun() { }
    public virtual void UnitStateWaveRunExit() { }


    public virtual void UnitStateStunBegin() { }
    public virtual void UnitStateStun() { }
    public virtual void UnitStateStunExit() { }

    public virtual void UnitStateSleepBegin() { }
    public virtual void UnitStateSleep() { }
    public virtual void UnitStateSleepExit() { }

    public virtual void UnitStateFreezeBegin() { }
    public virtual void UnitStateFreeze() { }
    public virtual void UnitStateFreezeExit() { }

    public virtual void UnitStateBindBegin() { }
    public virtual void UnitStateBind() { }
    public virtual void UnitStateBindExit() { }

    public virtual void UnitStateUltimatePauseBegin() 
    { 
        OnPause();
    }
    public virtual void UnitStateUltimatePause() { }
    public virtual void UnitStateUltimatePauseExit() 
    {
        OnResume();
    }

    public virtual void UnitStatePauseBegin()
    {
        OnPause();
    }
    public virtual void UnitStatePause() { }
    public virtual void UnitStatePauseExit()
    {
        OnResume();
    }

    public virtual void UnitStateTimeOutBegin() { }
    public virtual void UnitStateTimeOut() { }
    public virtual void UnitStateTimeOutExit() { }

    public virtual void UnitStateDeathBegin() { }
    public virtual void UnitStateDeath() { }
    public virtual void UnitStateDeathExit() { }

    public virtual void UnitStateWinBegin() { }
    public virtual void UnitStateWin() { }
    public virtual void UnitStateWinExit() { }

    public virtual void UnitStateLoseBegin() { }
    public virtual void UnitStateLose() { }
    public virtual void UnitStateLoseExit() { }


    public virtual void UnitStateEndBegin() { }
    public virtual void UnitStateEnd() { }
    public virtual void UnitStateEndExit() { }

    #endregion


}
