using FluffyDuck.Util;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBalloon : MonoBehaviour, IPoolableComponent
{
    public const string PREFAB_NAME = "Assets/AssetResources/Prefabs/SpeechBalloon/SpeechBalloon";
    const int BALLOON_LINE_THICKNESS = 2;

    public enum BalloonSizeType
    {
        None = 0,
        Flexible,
        Fixed,
        FixedWidth,
        FixedHeight,
    }

    public enum Pivot
    {
        None = 0x00,    // 000000
        Right = 0x01,   // 000001
        Top = 0x02,     // 000010
        Left = 0x04,    // 000100
        Bottom = 0x08,  // 001000
    }

    public bool IsInit
    {
        get
        {
            return ID != 0;
        }
    }

    public int ID { get; private set; } = 0;

    [SerializeField]
    Image _Balloon_Image = null;
    [SerializeField]
    Image _Tail_Image = null;
    [SerializeField]
    TMP_Text _Message_Text = null;
    [SerializeField]
    EasingCanvasGroupAlpha _Easing_Fader = null;
    [SerializeField]
    CanvasGroup _Canvas_Group = null;

    float _Display_Seconds = 0;
    System.Action<int> _Callback_When_Disappear = null;
    RectTransform _RectTransform = null;
    RectTransform _Parent_RectTransform = null;
    IActorPositionProvider _Position_Provider = null;
    ContentSizeFitter _Text_Content_Size_Fitter = null;
    BalloonSizeType _Type = BalloonSizeType.None;
    Vector2 _Size = Vector2.zero;

    /// <summary>
    /// 말풍선 초기화
    /// </summary>
    /// <param name="id">발급한 id</param>
    /// <param name="text">텍스트</param>
    /// <param name="pivot">피봇=꼬리</param>
    /// <param name="size_type">사이즈 타입</param>
    /// <param name="size">사이즈</param>
    /// <param name="base_position">말을 하는 대상의 3D 포지션</param>
    /// <param name="parent_size_delta">말풍선이 붙을 UI의 사이즈</param>
    /// <param name="display_seconds">0일경우는 무한</param>
    /// <param name="callback_when_disappear">사라질때 콜백</param>
    /// <returns></returns>
    public bool Init(int id, string text, int font_size, Pivot pivot, BalloonSizeType size_type, Vector2 size, IActorPositionProvider position_provider, RectTransform parent_recttransfrom, float display_seconds, Action<int> callback_when_disappear = null)
    {
        ID = id;
        _Message_Text.fontSize = font_size;

        // 말풍선 위치 중심점(피봇=꼬리) 입력
        Vector2 v_pivot = new Vector2(0.5f, 0.5f);
        int i_pivot = (int)pivot;

        var degree90 = Mathf.PI / 2.0f;
        var degree = 0.0f;

        while (i_pivot > 0)
        {
            if ((i_pivot & 1) == 1)
            {
                v_pivot += new Vector2(Mathf.Cos(degree), Mathf.Sin(degree)) * 0.5f;
            }

            degree += degree90;
            i_pivot >>= 1;
        }

        _RectTransform.pivot = v_pivot;
        _Parent_RectTransform = parent_recttransfrom;
        _Position_Provider = position_provider;

        SetText(text);
        SetSize(size_type, size);
        CalculateTail(_RectTransform.pivot, pivot);

        _Display_Seconds = display_seconds;

        if (_Display_Seconds > 0)
        {
            _Callback_When_Disappear = callback_when_disappear;
            StartCoroutine(CoAutoDisappearAfterSeconds(_Display_Seconds));
        }

        SetPositionFromWorld(_Position_Provider.GetBalloonWorldPosition());

        _Canvas_Group.alpha = 0.0f;
        _Easing_Fader.SetEasing(EasingFunction.Ease.Linear, 0, 0.5f);
        _Easing_Fader.StartEasing(new EasingCanvasGroupAlpha.EasingAlphaData { Dest_Alpha = 1, Dest_CanvasGroup = _Canvas_Group }, null);

        return true;
    }


    /// <summary>
    /// 내용을 변경합니다
    /// </summary>
    /// <param name="text">변경할 내용</param>
    public void SetText(string text)
    {
        _Message_Text.text = text;
        SetSize();
    }

    /// <summary>
    /// 내용을 변경합니다
    /// </summary>
    /// <param name="text">변경할 내용</param>
    public void SetText(string text, BalloonSizeType type, Vector2 size)
    {
        _Message_Text.text = text;
        SetSize(type, size);
    }

    public void SetSize(BalloonSizeType type = BalloonSizeType.None, Vector2 size = default(Vector2))
    {
        if (type != BalloonSizeType.None && size != default(Vector2) ||
            type == BalloonSizeType.Flexible)
        {
            _Type = type;
            _Size = size;
        }

        Vector2 text_ui_size = Vector2.zero;
        // 사이즈 타입마다의 말풍선 재조정
        // TODO: 잘 생각해보면 개선할 필요가 분명히 있다
        // FixedWidth는 괜찮은데 FixedHeight는 줄바꿈을 하지 않으면 사용하지 못하는 문제가 있다
        switch (_Type)
        {
            case BalloonSizeType.Flexible:
                _Text_Content_Size_Fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                _Text_Content_Size_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                text_ui_size = _Message_Text.GetPreferredValues(_Message_Text.text);
                break;
            case BalloonSizeType.FixedWidth:
                _Text_Content_Size_Fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                _Text_Content_Size_Fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

                _Message_Text.rectTransform.sizeDelta = _Size;
                LayoutRebuilder.ForceRebuildLayoutImmediate(_Message_Text.rectTransform);

                text_ui_size = new Vector2(_Size.x, _Message_Text.rectTransform.sizeDelta.y);
                break;
            case BalloonSizeType.FixedHeight:
                _Text_Content_Size_Fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                _Text_Content_Size_Fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

                _Message_Text.rectTransform.sizeDelta = _Size;
                LayoutRebuilder.ForceRebuildLayoutImmediate(_Message_Text.rectTransform);

                text_ui_size = new Vector2(_Message_Text.rectTransform.sizeDelta.x, _Size.y);
                break;
            case BalloonSizeType.Fixed:
                _Text_Content_Size_Fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                _Text_Content_Size_Fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;

                _Message_Text.rectTransform.sizeDelta = _Size;
                text_ui_size = _Size;
                break;
        }

        _RectTransform.sizeDelta = text_ui_size 
            + new Vector2(_Balloon_Image.sprite.border.w + _Balloon_Image.sprite.border.y, _Balloon_Image.sprite.border.x + _Balloon_Image.sprite.border.z)
            + (_Tail_Image.sprite.rect.size - new Vector2(BALLOON_LINE_THICKNESS, BALLOON_LINE_THICKNESS)) * 2;
    }

    void Update()
    {
        SetPositionFromWorld(_Position_Provider.GetBalloonWorldPosition());
    }

    public void SetPositionFromWorld(Vector3 world_pos)
    {
        // 3D 포지션을 화면 위치로 변환
        Vector2 viewPos = Camera.main.WorldToViewportPoint(world_pos);
        Vector2 WorldObject_ScreenPosition = new Vector2(
         ((viewPos.x * _Parent_RectTransform.sizeDelta.x) - (_Parent_RectTransform.sizeDelta.x * 0.5f)),
         ((viewPos.y * _Parent_RectTransform.sizeDelta.y) - (_Parent_RectTransform.sizeDelta.y * 0.5f)));

        // 말풍선 컴포넌트 위치 및 사이즈 입력
        _RectTransform.anchoredPosition = WorldObject_ScreenPosition;
    }

    /// <summary>
    /// 말풍선 생성할때 입력된 시간만큼 기다리다가 사라집니다
    /// </summary>
    /// <param name="seconds">기다려야 하는 시간(초)</param>
    /// <returns></returns>
    IEnumerator CoAutoDisappearAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SpeechBalloonManager.Instance.DisappearBalloon(this.ID);
    }

    /// <summary>
    /// 말풍선은 사라지는 효과 이후에 사라집니다
    /// </summary>
    public void Disappear(Action<SpeechBalloon> callback)
    {
        _Easing_Fader.StartEasing(new EasingCanvasGroupAlpha.EasingAlphaData { Dest_Alpha = 0, Dest_CanvasGroup = _Canvas_Group }, () =>
        {
            callback(this);
        });
    }

    /// <summary>
    /// 말풍선 사이즈를 재조정합니다
    /// </summary>
    void CalculateBalloon()
    {
        var balloon_rt = _Balloon_Image.GetComponent<RectTransform>();

        // 말풍선 값 입력
        balloon_rt.anchoredPosition = Vector2.zero;
        balloon_rt.anchorMin = Vector2.zero;
        balloon_rt.anchorMax = Vector2.one;
        balloon_rt.pivot = new Vector2(0.5f, 0.5f);
        balloon_rt.sizeDelta = (- _Tail_Image.sprite.rect.size + new Vector2(BALLOON_LINE_THICKNESS, BALLOON_LINE_THICKNESS)) * 2;
    }

    /// <summary>
    /// 말풍선 꼬리 위치 및 사이즈를 재조정합니다
    /// </summary>
    void CalculateTail(Vector2 anchor, Pivot pivot)
    {
        _Tail_Image.SetNativeSize();

        _Tail_Image.rectTransform.pivot = new Vector2(1.0f, 0.5f);
        _Tail_Image.rectTransform.anchorMax = anchor;
        _Tail_Image.rectTransform.anchorMin = anchor;

        // 꼬리 각도 수정
        float angle = 0;
        int iPivot = (int)pivot;

        int cnt = 4;
        for (int i = 0; i < cnt; i++)
        {
            if ((iPivot & 1) == 1)
            {
                if ((iPivot & 2) == 2)
                {
                    angle += 45;
                }
                else if (i == 0 && iPivot == 9)
                {
                    angle = 315;
                }
                break;
            }

            angle += 90;
            iPivot >>= 1;
        }

        _Tail_Image.rectTransform.eulerAngles = new Vector3();
        _Tail_Image.rectTransform.Rotate(new Vector3(0, 0, 1), angle);


        float pX = 0;
        float pY = 0;

        // 꼬리가 모서리에 있을경우 위치를 11씩 보정
        if (Mathf.Abs((anchor.x - 0.5f)).Equals(0.5f) && Mathf.Abs((anchor.y - 0.5f)).Equals(0.5f))
        {
            pX = ((anchor.x - 0.5f) > 0) ? -11 : 11;
            pY = ((anchor.y - 0.5f) > 0) ? -11 : 11;
        }

        _Tail_Image.rectTransform.anchoredPosition = new Vector2(pX, pY);
    }

    /// <summary>
    /// 오브젝트 풀에서 해당 게임 오브젝트를 가져올 때 최초 호출되는 함수. 
    /// Init보다 먼저 호출됨.
    /// Awake / Start 보다는 늦게 호출됨
    /// </summary>
    public virtual void Spawned()
    {
        CalculateBalloon();
        _RectTransform = GetComponent<RectTransform>();
        _Text_Content_Size_Fitter = _Message_Text.GetComponent<ContentSizeFitter>();
    }

    /// <summary>
    /// 오브젝트 풀에 게임오브젝트를 반환할 때 반환되기 직전에 호출되는 함수.
    /// 이 함수 호출 이후 게임오브젝트 비활성화 됨(부모 트랜스폼이 변경되기 직전 호출)
    /// </summary>
    public virtual void Despawned()
    {
        _Callback_When_Disappear?.Invoke(ID);
    }
}
