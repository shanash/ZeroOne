using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TouchCanvas : MonoBehaviour
{
    /// <summary>
    /// 터치이펙트에 네이티브 세로사이즈에 따라서 곱해줄 배율
    /// </summary>
    static float Scale_Multiple_Value = 1f;
    public static TouchCanvas Instance { get; private set; }

    /// <summary>
    /// 프리팹 패스
    /// </summary>
    string Touch_Effect_Prefab_Path = "VFX/Prefabs/Touch_Effect_Pink";
    RectTransform Effect_Container;
    List<TouchEffectNode> Used_Touch_Effects = new List<TouchEffectNode>();

    /// <summary>
    /// 씬 로딩 이후 터치 캔버스 컴포넌트를 추가
    /// 어떤 씬에서든 사용 가능.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void AfterSetUp()
    {
        Scale_Multiple_Value = ((float)Screen.height / GameDefine.SCREEN_BASE_HEIGHT);

        var obj = new GameObject("TouchCanvas");
        Instance = obj.AddComponent<TouchCanvas>();
        var canvas = obj.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        var scaler = obj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        scaler.scaleFactor = 1.0f;
        scaler.referencePixelsPerUnit = 100.0f;

        var ray = obj.AddComponent<GraphicRaycaster>();
        ray.ignoreReversedGraphics = true;
        ray.blockingObjects = GraphicRaycaster.BlockingObjects.None;

        RectTransform container_rt = new GameObject(nameof(Effect_Container)).AddComponent<RectTransform>();
        container_rt.SetParent(obj.transform);
        container_rt.localPosition = Vector2.zero;
        container_rt.anchorMin = Vector2.zero;
        container_rt.anchorMax = Vector2.zero;
        container_rt.pivot = Vector2.zero;
        container_rt.offsetMin = Vector2.zero;
        container_rt.offsetMax = Vector2.zero;

        Instance.Effect_Container = container_rt;
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
    }

    void OnEnable()
    {
        InputCanvas.OnInputDown += OnClickDown;
        InputCanvas.OnDrag += OnClickDragging;
    }

    void OnDisable()
    {
        InputCanvas.OnInputDown -= OnClickDown;
        InputCanvas.OnDrag -= OnClickDragging;
    }

    /// <summary>
    /// 이펙트 프리팹 패스 설정
    /// </summary>
    /// <param name="path"></param>
    public void SetTouchEffectPrefabPath(string path)
    {
        Touch_Effect_Prefab_Path = path;
    }

    /// <summary>
    /// 모든 터치 이펙트 제거
    /// </summary>
    public void ClearTouchEffect()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Touch_Effects.Count;
        for (int i = 0; i < cnt; i++)
        {
            var node = Used_Touch_Effects[i];
            node.StopParticle();
            pool.UnusedGameObject(node.gameObject);
        }
        Used_Touch_Effects.Clear();
    }

    /// <summary>
    /// 터치 다운시 호출
    /// </summary>
    /// <param name="position"></param>
    void OnClickDown(Vector2 position, ICollection<ICursorInteractable> components)
    {
        SpawnTouchEffectNode(position);
    }

    /// <summary>
    /// 터치 드래그시 호출(시작,드래그,종료)
    /// </summary>
    /// <param name="phase"></param>
    /// <param name="drag_delta"></param>
    /// <param name="position"></param>
    void OnClickDragging(InputActionPhase phase, Vector2 delta, Vector2 drag_origin, Vector2 position)
    {
        switch (phase)
        {
            case InputActionPhase.Started:      //  드래그 시작
                OnDragBegin(position);
                break;
            case InputActionPhase.Performed:    //  드래깅
                OnDragging(position);
                break;
            case InputActionPhase.Canceled:     //  드래그 종료
                OnDragEnd(position);
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    /// <summary>
    /// 드래그 시작시 호출되는 함수
    /// </summary>
    /// <param name="drag_delta"></param>
    /// <param name="position"></param>
    void OnDragBegin(Vector2 position)
    {
        TouchEffectNode node = null;
        if (Used_Touch_Effects.Exists(x => x.IsAction()))
        {
            node = Used_Touch_Effects.FindLast(x => x.IsAction());
        }
        else
        {
            node = SpawnTouchEffectNode(position);
        }
        node.SetDragging(true);
    }
    /// <summary>
    /// 드래그 중 호출되는 함수
    /// </summary>
    /// <param name="drag_delta"></param>
    /// <param name="position"></param>
    void OnDragging(Vector2 position)
    {
        TouchEffectNode node = null;
        if (Used_Touch_Effects.Exists(x => x.IsDragging()))
        {
            node = Used_Touch_Effects.FindLast(x => x.IsDragging());
        }
        else
        {
            node = SpawnTouchEffectNode(position);
            node.SetDragging(true);
        }
        node.transform.localPosition = position;
    }
    /// <summary>
    /// 드래그 종료시 호출되는 함수
    /// </summary>
    /// <param name="drag_delta"></param>
    /// <param name="position"></param>
    void OnDragEnd(Vector2 position)
    {
        //  혹시 남아있을 수 있는 터치 이펙트를 모두 찾아서 드래그를 종료시켜준다.
        if (Used_Touch_Effects.Exists(x => x.IsDragging()))
        {
            var node_list = Used_Touch_Effects.FindAll(x => x.IsDragging());
            int cnt = node_list.Count;
            for (int i = 0; i < cnt; i++)
            {
                node_list[i].StopParticle();
            }
        }
    }

    void UnusedTouchEffectNode(TouchEffectNode node)
    {
        Used_Touch_Effects.Remove(node);
        GameObjectPoolManager.Instance.UnusedGameObject(node.gameObject);
    }

    /// <summary>
    /// 이펙트 노드 소환
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    TouchEffectNode SpawnTouchEffectNode(Vector2 pos)
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(Touch_Effect_Prefab_Path, Effect_Container);
        var node = obj.GetComponent<TouchEffectNode>();
        node.transform.localPosition = pos;
        node.transform.localScale *= Scale_Multiple_Value;
        Used_Touch_Effects.Add(node);
        node.StartParticle(UnusedTouchEffectNode);
        return node;
    }
}
