using System;
using System.Collections.Generic;
using UnityEngine;

public class StateSystemBase<S>
    where S : Enum
{
    protected object[] Components;

    const bool STATE_SYSTEM_DEBUG_MODE = true;

    private Queue<S> Queue_States = new Queue<S>();

    private S _CurrentTransitionID = default;
    public S CurrentTransitionID
    {
        get { return _CurrentTransitionID; }
    }
    private S _PreviousTransitionID = default;
    public S PreviousTransitionID
    {
        get { return _PreviousTransitionID; }
    }

    private List<StateBase<S>> States = new List<StateBase<S>>();

    private List<S> States_History = new List<S>();

    void CheckStates()
    {
        if (States == null)
        {
            States = new List<StateBase<S>>();
        }
    }


    public bool IsContainsID(S trans)
    {
        var s = States.Find(x => x.TransID.Equals(trans));
        return (s != null);
    }

    public void AddTransition(StateBase<S> state)
    {
        if (state == null)
        {
            Debug.LogError("GameStateSystem ERROR : GameState<B, U> is not allowed a null pointer");
            return;
        }

        if (state.TransID.Equals(default(S)))
        {
            Debug.LogError("GameStateSystem ERROR : GAME_STATES.NONE is not allowed a real transtion");
            return;
        }

        CheckStates();

        if (IsContainsID(state.TransID))
        {
            Debug.LogErrorFormat("GameStateSystem ERROR : already has transition {0}. Impossible to assign to another state", state.TransID.ToString());
            return;
        }
        States.Add(state);
    }

    public void RemoveTransition(S trans)
    {
        if (trans.Equals(default(S)))
        {
            Debug.LogError("GameStateSystem ERROR : GAME_STATES.NONE is not allowed a real transition");
            return;
        }
        if (IsContainsID(trans))
        {
            var s = States.Find(x => x.TransID.Equals(default(S)));
            if (s != null)
            {
                States.Remove(s);
            }
            return;
        }

        Debug.LogErrorFormat("GameStateSystem ERROR : {0} was not on the state's transition list", trans.ToString());
    }

    public StateBase<S> GetFindStateByTrans(S trans)
    {
        var s = States.Find(x => x.TransID.Equals(trans));
        return s;
    }

    public void ChangeState(S trans)
    {
        //Debug.LogWarning($"ChangeState : {trans}");
        if (CurrentTransitionID.Equals(default(S)))
        {
            RealChangeState(trans);
        }
        else
        {
            Queue_States.Enqueue(trans);
        }

    }

    

    void RealChangeState(S trans)
    {
        if (trans.Equals(default(S)))
        {
            Debug.LogError("GameStateSystem.ChangeState() WARNING : Transition is NONE");
            return;
        }

        if (_CurrentTransitionID.Equals(trans))
        {
            Debug.LogWarningFormat("GameStateSystem.ChangeState() WARNING : Already Current Transition ID => {0}", trans.ToString());
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
            Debug.LogWarningFormat("GameStateSystem.ChangeState() WARNING : {0} Not found transition id.", trans.ToString());
            return;
        }

        _PreviousTransitionID = _CurrentTransitionID;

        //  현재 상태가 존재한다면 
        var curState = GetFindStateByTrans(_CurrentTransitionID);
        if (curState != null)
        {
            curState.ExitStateAction?.Invoke(Components);
        }


        _CurrentTransitionID = newState.TransID;
        //  Finish 
        if (curState != null)
        {
            curState.FinallyStateAction?.Invoke(Components);
        }


        //  새로운 상태가 존재한다면
        if (newState != null)
        {
            newState.EnterStateAction?.Invoke(Components);
        }
    }

    public void Lazy_Init_Setting(S trans, params object[] components)
    {
        Components = components;
        ChangeState(trans);
    }

    public void UpdateState()
    {
        var curState = GetFindStateByTrans(_CurrentTransitionID);
        if (curState != null)
        {
            curState.UpdateStateAction?.Invoke(Components);
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

    public List<S> GetStatesHistory()
    {
        return States_History;
    }
    /// <summary>
    /// 해당 상태가 예약되어 있는지 여부 체크
    /// </summary>
    /// <param name="trans"></param>
    /// <returns></returns>
    public bool IsReservedState(S trans)
    {
        return Queue_States.Contains(trans);
    }

}
