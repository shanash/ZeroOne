using System;

public abstract class StateBase<S>
    where S : Enum
{
    private S _transID = default;

    public S TransID
    {
        get { return _transID; }
        protected set { _transID = value; }
    }
    protected float Delta_Time;

    // Action 또는 Func 대리자를 멤버로 선언
    public Delegate EnterStateAction;
    public Delegate ExitStateAction;
    public Delegate FinallyStateAction;
    public Delegate UpdateStateAction;
}