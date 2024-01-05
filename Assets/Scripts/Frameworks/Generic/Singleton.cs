using System;
using System.Reflection;
using UnityEngine;

namespace FluffyDuck.Util
{
    public abstract class Singleton<T> : IDisposable where T : Singleton<T>
    {
        static T Saved_Instance = null;
        static protected bool Disposed = false;
        static object Sync_Obj = new object();

        public static T Instance
        {
            get
            {
                if (Disposed)
                {
                    return null;
                }

                lock (Sync_Obj)
                {
                    if (Saved_Instance == null)
                    {
                        Saved_Instance = (T)Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, null, null);
                        Saved_Instance.Initialize();
                        Application.quitting += () => Saved_Instance.Dispose();
                    }
                }
                return Saved_Instance;
            }
        }

        protected abstract void Initialize();

        public void Dispose()
        {
            if (!Disposed)
            {
                Saved_Instance = null;
                OnDispose();

                Disposed = true;
            }
        }

        protected abstract void OnDispose();
    }
}
