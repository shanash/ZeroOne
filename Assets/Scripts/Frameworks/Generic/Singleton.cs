using System;
using System.Reflection;
using UnityEngine;

namespace FluffyDuck.Util
{
    public abstract class Singleton<T> where T : Singleton<T>
    {
        static T Saved_Instance = null;
        protected static bool IsQuitting = false;
        static object Sync_Obj = new object();

        public static T Instance
        {
            get
            {
                if (IsQuitting)
                {
                    return null;
                }

                lock (Sync_Obj)
                {
                    if (Saved_Instance == null)
                    {
                        Saved_Instance = (T)Activator.CreateInstance(typeof(T), BindingFlags.Instance | BindingFlags.NonPublic, null, null, null);
                        Saved_Instance.Initialize();
                        Application.quitting += Saved_Instance.OnAppQuit;
                    }
                }
                return Saved_Instance;
            }
        }

        void OnAppQuit()
        {
            Saved_Instance = null;
            IsQuitting = true;
        }

        protected abstract void Initialize();
    }
}
