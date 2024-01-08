using UnityEngine;

namespace FluffyDuck.Util
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        static T Saved_Instance = null;
        static protected bool IsQuitting = false;
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
                        T[] objs = FindObjectsOfType<T>();

                        if (objs.Length > 1)
                        {
                            Debug.LogError($"There is more than one {typeof(T).Name} in the scene.");
                            return null;
                        }

                        if (objs.Length > 0)
                        {
                            Saved_Instance = objs[0];
                        }
                        else
                        {
                            GameObject go = new GameObject(typeof(T).ToString(), typeof(T));
                            Saved_Instance = go.GetComponent<T>();
                            Saved_Instance._Initialize();
                            Application.quitting += Saved_Instance.OnAppQuit;
                        }
                    }
                }
                return Saved_Instance;
            }
        }

        protected abstract bool Is_DontDestroyOnLoad { get; }
        protected abstract void Initialize();

        void OnAppQuit()
        {
            Saved_Instance = null;
            IsQuitting = true;
        }

        void _Initialize()
        {
            Initialize();

            if (Is_DontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
    }
}
