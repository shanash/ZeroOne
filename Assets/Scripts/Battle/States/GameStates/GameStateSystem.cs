using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSystem<B, U> {

    private B BattleMng;
    private U UiMng;

    const bool STATE_SYSTEM_DEBUG_MODE = true;

    private Queue<GAME_STATES> Queue_States = new Queue<GAME_STATES>();

    private GAME_STATES _CurrentTransitionID = GAME_STATES.NONE;
    public GAME_STATES CurrentTransitionID
    {
        get { return _CurrentTransitionID; }
    }
    private GAME_STATES _PreviousTransitionID = GAME_STATES.NONE;
    public GAME_STATES PreviousTransitionID
    {
        get { return _PreviousTransitionID; }
    }

    private List<GameState<B, U>> States = new List<GameState<B, U>>();

    private List<GAME_STATES> States_History = new List<GAME_STATES>();

    void CheckStates()
    {
        if (States == null)
        {
            States = new List<GameState<B, U>>();
        }
    }

    public bool IsContainsID(GAME_STATES trans)
    {
        var s = States.Find(x => x.TransID == trans);
        return (s != null);
    }

    public void AddTransition(GameState<B, U> state)
    {
        if (state == null)
        {
            Debug.LogError("GameStateSystem ERROR : GameState<B, U> is not allowed a null pointer");
            return;
        }

        if (state.TransID == GAME_STATES.NONE)
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

    public void RemoveTransition(GAME_STATES trans)
    {
        if (trans == GAME_STATES.NONE)
        {
            Debug.LogError("GameStateSystem ERROR : GAME_STATES.NONE is not allowed a real transition");
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

        Debug.LogErrorFormat("GameStateSystem ERROR : {0} was not on the state's transition list", trans.ToString());
    }

    public GameState<B, U> GetFindStateByTrans(GAME_STATES trans)
    {
        var s = States.Find(x => x.TransID == trans);
        return s;
    }

    public void ChangeState(GAME_STATES trans)
    {
        if (CurrentTransitionID == GAME_STATES.NONE)
        {
            RealChangeState(trans);
        }
        else
        {
            Queue_States.Enqueue(trans);
        }
        
    }

    void RealChangeState(GAME_STATES trans)
    {
        if (trans == GAME_STATES.NONE)
        {
            Debug.LogError("GameStateSystem.ChangeState() WARNING : Transition is NONE");
            return;
        }

        if (_CurrentTransitionID == trans)
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
            curState.ExitState(BattleMng, UiMng);
        }


        _CurrentTransitionID = newState.TransID;
        //  Finish 
        if (curState != null)
        {
            curState.FinallyState(BattleMng, UiMng);
        }


        //  새로운 상태가 존재한다면
        if (newState != null)
        {
            newState.EnterState(BattleMng, UiMng);
        }
    }

    public void Lazy_Init_Setting(B mng, U ui, GAME_STATES trans)
    {
        BattleMng = mng;
        UiMng = ui;
        ChangeState(trans);
    }

    public void UpdateState()
    {
        var curState = GetFindStateByTrans(_CurrentTransitionID);
        if (curState != null)
        {
            curState.UpdateState(BattleMng, UiMng);
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

    public List<GAME_STATES> GetStatesHistory()
    {
        return States_History;
    }
}
