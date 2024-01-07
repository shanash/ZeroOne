using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// MonoBehaviour 스크립트를 상속받는 싱글톤. 당연히 자동생성된 오브젝트에 붙어서 생성된다.
/// 상속받아서 Awake 선언하지 않기!
/// </summary>
public abstract class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get
        {
            // 종료시에 재생성되는 경우를 막기 위한
            if (App_Is_Closing)
                return null;

            // 인스턴스가 없으면 생성하고 있으면 꺼내옵니다
            lock (Sync_Obj)
            {
                if (Saved_Instance == null)
                {
                    T[] objs = FindObjectsOfType<T>();
                    if (objs.Length > 0)
                    {
                        Saved_Instance = objs[0];
                    }

                    if (objs.Length > 1)
                    {
                        Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                    }

                    if (Saved_Instance == null)
                    {
                        if (Handle.IsValid())
                        {
                            return null;
                        }
                        if (!string.IsNullOrEmpty(_Addressables_Key))
                        {
                            Handle = Addressables.LoadAssetAsync<GameObject>(_Addressables_Key);
                            Handle.Completed += (h) =>
                            {
                                if (h.Status == AsyncOperationStatus.Succeeded)
                                {
                                    GameObject loaded = h.Result as GameObject;
                                    GameObject go = Instantiate(loaded);
                                    if (_Components_To_Attach != null)
                                    {
                                        foreach (var type in _Components_To_Attach)
                                        {
                                            go.AddComponent(type);
                                        }
                                    }
                                    go.name = typeof(T).ToString();
                                    Saved_Instance = go.GetComponentInChildren<T>();
                                }
                            };
                            return Saved_Instance;
                        }
                        string go_Name = typeof(T).ToString();
                        GameObject go = _Components_To_Attach != null ? new GameObject(go_Name, _Components_To_Attach) : new GameObject(go_Name);
                        Saved_Instance = go.AddComponent<T>();
                    }
                }
                return Saved_Instance;
            }
        }
    }

    #region Static Members
    private static T Saved_Instance = null;
    private static object Sync_Obj = new object();
    protected static bool App_Is_Closing = false;
    private static AsyncOperationHandle Handle = default(AsyncOperationHandle);
    // 불러올 프리팹 키
    // static 생성자에서 입력해 놓으면 해당 Prefab에서 생성합니다
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // static void SetUp()
    // {
    //     _Addressables_Key = "Assets/AssetResources/Prefabs/UI/InputCanvas";
    // }
    protected static string _Addressables_Key = string.Empty;
    protected static System.Type[] _Components_To_Attach = null;
    #endregion

    protected abstract bool _Is_DontDestroyOnLoad { get; }


    protected virtual void OnAwake() { }

    #region Monobehaviour Callback
    void Awake()
    {
        // 기존에 존재하는 객체가 있다면 바로 삭제
        if (Saved_Instance != null && Saved_Instance != this as T)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        if (_Is_DontDestroyOnLoad)
        {
            InitSingleton();
        }

        if (Saved_Instance == null)
        {
            Saved_Instance = this as T;
        }

        OnAwake();
    }
    #endregion

    #region Private Method
    /// <summary>
    /// 초기화
    /// </summary>
    protected void InitSingleton()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// 종료시에 어딘가에서 싱글톤을 호출하여 재생성 되는 것을 막는 변수를 true로 올립니다
    /// </summary>
    protected virtual void OnApplicationQuit()
    {
        App_Is_Closing = true;
    }

    private void OnDestroy()
    {
        if (Saved_Instance == this)
        {
            Saved_Instance = null;
        }
    }

    #endregion
}
