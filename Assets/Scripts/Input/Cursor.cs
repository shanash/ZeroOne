using System;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    const string CANNOT_FIND_CURSOR_ICON_MSG = "커서 아이콘을 찾을 수 없습니다!!";

    //TODO: 커서에서 사용할 이미지 경로 및 로딩 부분
    //TODO: 커서 스케일? 속도 조절?
    [SerializeField, Tooltip("커서에서 사용할 RectTransform")]
    RectTransform _Rect_Transform = null;

    [SerializeField, Tooltip("이동 속도"), Range(10, 1000)]
    float _Speed = 500; // 1초에 이동하는 거리

    Image _Icon = null;

    Vector2 _Last_Position;

    public bool Visible { get; set; }

    void Awake()
    {
        _Icon = GetComponent<Image>();
        Debug.Assert(_Icon != null, CANNOT_FIND_CURSOR_ICON_MSG);
    }

    public Vector2 Delta
    {
        get
        {
            return Position - _Last_Position;
        }
    }

    public Vector2 Position
    {
        get
        {
            return _Rect_Transform.anchoredPosition;
        }
        set
        {
            _Last_Position = _Rect_Transform.anchoredPosition;
            _Rect_Transform.anchoredPosition = value;
        }
    }

    public void MoveByFrame(Vector2 direction)
    {
        Position += direction.normalized * _Speed * Time.deltaTime;
    }

    void EnableIcon(bool value)
    {
        if (_Icon == null)
        {
            Debug.Assert(false, CANNOT_FIND_CURSOR_ICON_MSG);
            return;
        }
        _Icon.enabled = value;
    }

    public void Show()
    {
        EnableIcon(true);
    }

    public void Hide()
    {
        EnableIcon(false);
    }
}
