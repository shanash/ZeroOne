using UnityEngine;
using UnityEngine.SceneManagement;

namespace FluffyDuck.Util
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        static T Saved_Instance = null;
        static protected bool IsQuitting = false;
        static readonly object Sync_Obj = new object();

        public static T I => Instance;
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
                        }
                    }
                }
                return Saved_Instance;
            }
        }

        /// <summary>
        /// 인스턴스를 날리고 새로운 인스턴스를 만든다
        /// </summary>
        public virtual void ResetInstance()
        {
            Application.quitting -= OnAppQuit;

            if (ResetInstanceOnChangeScene)
            {
                SceneManager.activeSceneChanged -= OnActiveSceneChanged;
            }

            // 혹시라도 유니티에서 비관리 되는 리소스가 있다면 상속받아서 해제해줍시다
            if (Saved_Instance != null)
            {
                DestroyImmediate(Saved_Instance.gameObject);
                Saved_Instance = null;
            }

            _ = Instance;
        }

        protected virtual bool ResetInstanceOnChangeScene => false;
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

            if (ResetInstanceOnChangeScene)
            {
                SceneManager.activeSceneChanged += OnActiveSceneChanged;
            }

            Application.quitting += OnAppQuit;
        }

        void OnActiveSceneChanged(Scene oldScene, Scene newScene)
        {
            ResetInstance();
        }
    }
}
