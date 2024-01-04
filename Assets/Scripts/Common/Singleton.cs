using System;

public abstract class Singleton<T> : IDisposable where T : Singleton<T>, new()
{
    static T instance = null;
    static protected bool disposed = false;
    static object Sync_Obj = new object();

    public static T Instance
    {
        get
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(instance), "Trying to access disposed singleton");
            }

            lock (Sync_Obj)
            {
                if (instance == null)
                {
                    instance = new T();
                    instance.Initialize();
                }
            }
            return instance;
        }
    }

    protected abstract void Initialize();

    public void Dispose()
    {
        if (!disposed)
        {
            OnDispose();
            disposed = true;
        }
    }

    protected abstract void OnDispose();

    protected Singleton() { }
}
