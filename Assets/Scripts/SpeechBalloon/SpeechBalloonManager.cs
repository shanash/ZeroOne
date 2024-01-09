using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBalloonManager : MonoBehaviourSingleton<SpeechBalloonManager>
{
    const int DEFAULT_FONT_SIZE = 30;
    static int s_CURRENT_LAST_ID = 0;
    Dictionary<int, SpeechBalloon> _SpeechBalloons = new Dictionary<int, SpeechBalloon>();

    protected override bool _Is_DontDestroyOnLoad { get { return false; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SetUp()
    {
        _Components_To_Attach = new Type[] { typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster) };
    }

    protected override void OnAwake()
    {
        var canvas = this.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = -1;

        var canvas_scaler = this.GetComponent<CanvasScaler>();
        canvas_scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvas_scaler.referenceResolution = new Vector2(1440, 1080);
        canvas_scaler.matchWidthOrHeight = 1.0f;
    }

    /// <summary>
    /// 효과 없이 즉시 삭제
    /// </summary>
    /// <param name="balloon">말풍선 인스턴스</param>
    void DeleteBalloonImmediatly(SpeechBalloon balloon)
    {
        GameObjectPoolManager.Instance.UnusedGameObject(balloon.gameObject);
    }

    /// <summary>
    /// 3D공간의 base_position의 Canvas 위치로 변환해서 말풍선을 생성합니다.
    /// </summary>
    /// <param name="cb">생성 이후 말풍선ID를 받을 콜백</param>
    /// <param name="text">보여줄 텍스트 내용</param>
    /// <param name="base_position">말풍선을 표시할 기반 3D공간 포지션</param>
    /// <param name="size">사이즈</param>
    /// <param name="size_type">사이즈 타입</param>
    /// <param name="pivot">피벗=꼬리 위치</param>
    /// <param name="font_size">폰트 사이즈</param>
    /// <param name="display_seconds">생존시간(초)</param>
    /// <param name="parent_transform">말풍선이 붙을 부모</param>
    /// <param name="when_delete">말풍선이 삭제될때</param>
    public async void CreateBalloon(
        Action<int> cb,
        string text,
        IActorPositionProvider position_provider,
        Vector2 size = default,
        SpeechBalloon.BalloonSizeType size_type = SpeechBalloon.BalloonSizeType.Flexible,
        SpeechBalloon.Pivot pivot = SpeechBalloon.Pivot.Bottom,
        RectTransform parent_transform = null,
        int font_size = DEFAULT_FONT_SIZE,
        float display_seconds = 0,
        Action when_delete = null)
    {
        var go = await CreateBalloonAsync(text, position_provider, size, size_type, pivot, parent_transform, font_size, display_seconds, when_delete);
        cb?.Invoke(go);
    }

    /// <summary>
    /// 3D공간의 base_position의 Canvas 위치로 변환해서 말풍선을 생성합니다.
    /// </summary>
    /// <param name="text">보여줄 텍스트 내용</param>
    /// <param name="base_position">말풍선을 표시할 기반 3D공간 포지션</param>
    /// <param name="size">사이즈</param>
    /// <param name="size_type">사이즈 타입</param>
    /// <param name="pivot">피벗=꼬리 위치</param>
    /// <param name="font_size">폰트 사이즈</param>
    /// <param name="display_seconds">생존시간(초)</param>
    /// <param name="parent_transform">말풍선이 붙을 부모</param>
    /// <param name="when_delete">말풍선이 삭제될때</param>
    /// <returns>말풍선의 id</returns>
    public async Task<int> CreateBalloonAsync(string text, IActorPositionProvider position_provider, Vector2 size = default(Vector2), SpeechBalloon.BalloonSizeType size_type = SpeechBalloon.BalloonSizeType.Flexible, SpeechBalloon.Pivot pivot = SpeechBalloon.Pivot.Bottom, RectTransform parent_transform = null, int font_size = DEFAULT_FONT_SIZE, float display_seconds = 0, System.Action when_delete = null)
    {
        s_CURRENT_LAST_ID++;

        // 지정된 부모 오브젝트가 없으면 매니저가 직접 가지고 있습니다
        if (parent_transform == null)
        {
            parent_transform = GetComponent<RectTransform>();
        }

        // 오브젝트 생성
        var pool = GameObjectPoolManager.Instance;
        var obj = await pool.GetGameObjectAsync(SpeechBalloon.PREFAB_NAME, parent_transform);
        if (obj == null)
        {
            Debug.Assert(obj != null, "오브젝트가 생성되지 않았습니다");
            return 0;
        }

        // 생성후 초기화
        SpeechBalloon balloon = obj.GetComponent<SpeechBalloon>();
        balloon.Init(s_CURRENT_LAST_ID, text, font_size, pivot, size_type, size, position_provider, parent_transform, display_seconds);

        // 컨테이너에 추가
        _SpeechBalloons.Add(s_CURRENT_LAST_ID, balloon);

        return s_CURRENT_LAST_ID;
    }

    public bool SetTextBalloon(int id, string text)
    {
        if (!_SpeechBalloons.ContainsKey(id)) return false;
        _SpeechBalloons[id].SetText(text);
        return true;
    }

    /// <summary>
    /// 사라지는 효과가 끝나고 삭제
    /// </summary>
    /// <param name="id">말풍선 id</param>
    /// <returns>삭제여부</returns>
    public bool DisappearBalloon(int id)
    {
        if (_SpeechBalloons.TryGetValue(id, out var balloon))
        {
            _SpeechBalloons.Remove(balloon.ID);
            balloon.Disappear(DeleteBalloonImmediatly);
            return true;
        }
        return false;
    }

    public void DeleteAllBalloon()
    {
        foreach (SpeechBalloon balloon in _SpeechBalloons.Values)
        {
            GameObjectPoolManager.Instance.UnusedGameObject(balloon.gameObject);
        }

        _SpeechBalloons.Clear();
    }
}
