using System;

public abstract class Singleton<T> : IDisposable where T : Singleton<T>, new()
{
    private static T instance;
    private static bool disposed;

    public static T Instance
    {
        get
        {
            if (disposed)
                throw new ObjectDisposedException(nameof(instance), "Trying to access disposed singleton");

            if (instance == null)
            {
                instance = new T();
                instance.Init();
            }
            return instance;
        }
    }

    protected virtual void Init() { }

    public void Dispose()
    {
        if (!disposed)
        {
            OnDispose();
            disposed = true;
        }
    }

    protected virtual void OnDispose() { }

    protected Singleton() { }
}
