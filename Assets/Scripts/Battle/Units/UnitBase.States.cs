using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UnitBase : MonoBehaviour, IUpdateComponent
{

    protected UnitStateSystem<UnitBase, BattleManager, BattleUIManager> FSM = null;

    protected BattleManager Battle_Mng;
    protected BattleUIManager UI_Mng;

    private void Awake()
    {
        InitStates();
    }
    protected virtual void InitStates()
    {
        FSM = new UnitStateSystem<UnitBase, BattleManager, BattleUIManager> ();

        
    }

    public virtual void Lazy_Init(BattleManager mng, BattleUIManager ui, UNIT_STATES trans)
    {
        Battle_Mng = mng;
        UI_Mng = ui;
        if (FSM == null)
        {
            InitStates ();
        }
        FSM.Lazy_Init_Setting(this, mng, ui, trans);
    }

    public void ChangeState(UNIT_STATES trans)
    {
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

    public virtual void UnitStateMoveBegin() { }
    public virtual void UnitStateMove() { }
    public virtual void UnitStateMoveExit() { }

    public virtual void UnitStateSpawnBegin() { }
    public virtual void UnitStateSpawn() { }
    public virtual void UnitStateSpawnExit() { }


    public virtual void UnitStateIdleBegin() { }
    public virtual void UnitStateIdle() { }
    public virtual void UnitStateIdleExit() { }

    public virtual void UnitStateTurnOnBegin() { }
    public virtual void UnitStateTurnOn() { }
    public virtual void UnitStateTurnOnExit() { }

    public virtual void UnitStatePlayingBegin() { }
    public virtual void UnitStatePlaying() { }
    public virtual void UnitStatePlayingExit() { }


    public virtual void UnitStateAttackReady01Begin() { }
    public virtual void UnitStateAttackReady01() { }
    public virtual void UnitStateAttackReady01Exit() { }

    public virtual void UnitStateAttack01Begin() { }
    public virtual void UnitStateAttack01() { }
    public virtual void UnitStateAttack01Exit() { }

    public virtual void UnitStateAttackReady02Begin() { }
    public virtual void UnitStateAttackReady02() { }
    public virtual void UnitStateAttackReady02Exit() { }

    public virtual void UnitStateAttack02Begin() { }
    public virtual void UnitStateAttack02() { }
    public virtual void UnitStateAttack02Exit() { }

    public virtual void UnitStateAttackEndBegin() { }
    public virtual void UnitStateAttackEnd() { }
    public virtual void UnitStateAttackEndExit() { }


    public virtual void UnitStateSkillReady01Begin() { }
    public virtual void UnitStateSkillReady01() { }
    public virtual void UnitStateSkillReady01Exit() { }

    public virtual void UnitStateSkill01Begin() { }
    public virtual void UnitStateSkill01() { }
    public virtual void UnitStateSkill01Exit() { }

    public virtual void UnitStateSkillReady02Begin() { }
    public virtual void UnitStateSkillReady02() { }
    public virtual void UnitStateSkillReady02Exit() { }

    public virtual void UnitStateSkill02Begin() { }
    public virtual void UnitStateSkill02() { }
    public virtual void UnitStateSkill02Exit() { }

    public virtual void UnitStateSkillReady03Begin() { }
    public virtual void UnitStateSkillReady03() { }
    public virtual void UnitStateSkillReady03Exit() { }

    public virtual void UnitStateSkill03Begin() { }
    public virtual void UnitStateSkill03() { }
    public virtual void UnitStateSkill03Exit() { }


    public virtual void UnitStateSkillEndBegin() { }
    public virtual void UnitStateSkillEnd() { }
    public virtual void UnitStateSkillEndExit() { }

    public virtual void UnitStateTurnEndBegin() { }
    public virtual void UnitStateTurnEnd() { }
    public virtual void UnitStateTurnEndExit() { }

    public virtual void UnitStateWaveRunBegin() { }
    public virtual void UnitStateWaveRun() { }
    public virtual void UnitStateWaveRunExit() { }



    public virtual void UnitStatePauseBegin() 
    {
        OnPause();
    }
    public virtual void UnitStatePause() { }
    public virtual void UnitStatePauseExit() 
    {
        OnResume();
    }

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
