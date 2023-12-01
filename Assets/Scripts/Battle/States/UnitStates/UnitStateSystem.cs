using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStateSystem<N, B, U> {

    private N Unit;
    private B Battle_Mng;
    private U UiMng;

    const bool STATE_SYSTEM_DEBUG_MODE = true;

    private Queue<UNIT_STATES> Queue_States = new Queue<UNIT_STATES>();


    private UNIT_STATES _CurrentTransitionID = UNIT_STATES.NONE;
    public UNIT_STATES CurrentTransitionID
    {
        get { return _CurrentTransitionID; }
    }
    private UNIT_STATES _PreviousTransitionID = UNIT_STATES.NONE;
    public UNIT_STATES PreviousTransitionID
    {
        get
        {
            return _PreviousTransitionID;
        }
    }

    private List<UNIT_STATES> States_History = new List<UNIT_STATES>();

    private List<UnitState<N, B, U>> States = new List<UnitState<N, B, U>>();

    void CheckStates()
    {
        if (States == null)
        {
            States = new List<UnitState<N, B, U>>();
        }
    }

    public bool IsContainsID(UNIT_STATES trans)
    {
        var s = States.Find(x => x.TransID == trans);
        return (s != null);
    }

    public void AddTransition(UnitState<N, B, U> state)
    {
        if (state == null)
        {
            Debug.LogError("UnitStateSystem ERROR : UnitState<N, B, U> is not allowed a null pointer");
            return;
        }

        if (state.TransID == UNIT_STATES.NONE)
        {
            Debug.LogError("UnitStateSystem ERROR : UNIT_STATES.NONE is not allowed a real transtion");
            return;
        }

        CheckStates();

        if (IsContainsID(state.TransID))
        {
            Debug.LogErrorFormat("UnitStateSystem ERROR : already has transition {0}. Impossible to assign to another state", state.TransID.ToString());
            return;
        }
        States.Add(state);
    }

    public void RemoveTransition(UNIT_STATES trans)
    {
        if (trans == UNIT_STATES.NONE)
        {
            Debug.LogError("UnitStateSystem ERROR : UNIT_STATES.NONE is not allowed a real transition");
            return;
        }
        if (IsContainsID(trans))
        {
            var s = States.Find(x => x.TransID == trans);
            if (s != null)
            {
                States.Remove(s);
            }
            return;
        }

        Debug.LogErrorFormat("UnitStateSystem ERROR : {0} was not on the state's transition list", trans.ToString());
    }

    public UnitState<N, B, U> GetFindStateByTrans(UNIT_STATES trans)
    {
        var s = States.Find(x => x.TransID == trans);
        return s;
    }

    public void ChangeState(UNIT_STATES trans)
    {
        if (CurrentTransitionID == UNIT_STATES.NONE)
        {
            RealChangeState(trans);
        }
        else
        {
            Queue_States.Enqueue(trans);
        }
    }

    void RealChangeState(UNIT_STATES trans)
    {
        if (trans == UNIT_STATES.NONE)
        {
            Debug.LogError("UnitStateSystem.ChangeState() WARNING : Transition is NONE");
            return;
        }

        if (_CurrentTransitionID == trans)
        {
            //Debug.LogWarningFormat("UnitStateSystem.ChangeState() WARNING : Already Current Transition ID => {0}", trans.ToString());
            return;
        }
        if (STATE_SYSTEM_DEBUG_MODE)
        {
            if (States_History.Count >= 20)
            {
                States_History.RemoveAt(0);
            }
            States_History.Add(trans);
        }

        var newState = GetFindStateByTrans(trans);
        if (newState == null)
        {
            Debug.LogWarningFormat("UnitStateSystem.ChangeState() WARNING : {0} Not found transition id.", trans.ToString());
            return;
        }

        _PreviousTransitionID = _CurrentTransitionID;

        //  현재 상태가 존재한다면 
        var curState = GetFindStateByTrans(_CurrentTransitionID);
        if (curState != null)
        {
            curState.ExitState(Unit, Battle_Mng, UiMng);
        }

        _CurrentTransitionID = newState.TransID;
        if (curState != null)
        {
            curState.FinallyState(Unit, Battle_Mng, UiMng);
        }
        //  새로운 상태가 존재한다면
        if (newState != null)
        {
            newState.EnterState(Unit, Battle_Mng, UiMng);
        }
    }

    public void Lazy_Init_Setting(N unit, B mng, U ui, UNIT_STATES trans)
    {
        this.Unit = unit;
        this.Battle_Mng = mng;
        this.UiMng = ui;
        ChangeState(trans);
    }

    public void UpdateState()
    {
        var curState = GetFindStateByTrans(_CurrentTransitionID);
        if (curState != null)
        {
            curState.UpdateState(Unit, Battle_Mng, UiMng);
        }
        if (Queue_States.Count > 0)
        {
            var trans = Queue_States.Dequeue();
            RealChangeState(trans);
        }
    }




    /// <summary>
    /// 바로 이전상태로 되돌아감
    /// </summary>
    public void RevertState()
    {
        ChangeState(_PreviousTransitionID);
    }

    public List<UNIT_STATES> GetStatesHistory()
    {
        return States_History;
    }
}
