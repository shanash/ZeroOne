using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;

namespace FluffyDuck.Util
{
    public abstract class Singleton<T> : IDisposable where T : Singleton<T>
    {
        static T instance = null;
        static protected bool Disposed = false;
        static object Sync_Obj = new object();

        public static T Instance
        {
            get
            {
                if (Disposed)
                {
                    throw new ObjectDisposedException(nameof(instance), "Trying to access disposed singleton");
                }

                lock (Sync_Obj)
                {
                    if (instance == null)
                    {
                        instance = (T)Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, null, null);
                        instance.Initialize();
                    }
                }
                return instance;
            }
        }

        protected abstract void Initialize();

        public void Dispose()
        {
            if (!Disposed)
            {
                OnDispose();
                Disposed = true;
            }
        }

        protected abstract void OnDispose();
    }
}
