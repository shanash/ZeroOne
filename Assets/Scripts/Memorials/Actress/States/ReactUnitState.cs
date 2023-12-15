using UnityEngine;
public enum REACT_UNIT_STATES
{
    NONE = 0,
    INTRO,
    IDLE,
    REACT,
}



public abstract class ReactUnitState<N, C, U> : StateBase<REACT_UNIT_STATES>
{
    public virtual void EnterState(N unit, C mng, U ui) { }
    public virtual void UpdateState(N unit, C mng, U ui) { }
    public virtual void ExitState(N unit, C mng, U ui) { }
    public virtual void FinallyState(N unit, C mng, U ui) { }
}
