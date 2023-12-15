public abstract class StateBase<S>
    where S : System.Enum
{
    private S _transID = default;
    public S TransID
    {
        get { return _transID; }
        protected set { _transID = value; }
    }
    protected float Delta_Time;
}
