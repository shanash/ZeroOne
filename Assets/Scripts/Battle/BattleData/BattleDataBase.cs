
using System;

public abstract class BattleDataBase : IDisposable, ICloneable
{
    protected bool disposed = false;

    protected BattleDataBase()
    {
        Reset();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //  관리되는 자원 해제
                Destroy();
            }
            disposed = true;
        }
    }

    protected virtual void Reset() { }
    protected virtual void Destroy() { }
    public virtual object Clone() { return null; }
}
