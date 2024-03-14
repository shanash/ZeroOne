using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum TOUCH_RESULT_TYPE
{
    NONE = 0,
    CLICK,              //  클릭
    LONG_PRESS,         //  롱 터치
    RELEASE,            //  릴리즈
}

public abstract class UIInteractiveButtonBase : Selectable, IPointerClickHandler
{
    [SerializeField, Tooltip("Scaling RectTransform")]
    protected RectTransform Scale_Rect;

    [SerializeField, Tooltip("Press Scale")]
    protected Vector2 Press_Scale;

    [SerializeField, Tooltip("Use Long Touch")]
    protected bool Use_Long_Touch;

    /// <summary>
    /// 일정시간 이상 누르고 있으면 롱터치로 인식
    /// </summary>
    [SerializeField, Tooltip("Long Press Dutaion")]
    protected float Long_Press_Duration = 0.2f;

    bool Is_Long_Press = false;
    CancellationTokenSource Cancel_Token = null; // 취소 토큰

    protected abstract UnityEventBase Touch_Callback_Base { get; }

    protected abstract void OnTouchEvent(TOUCH_RESULT_TYPE type);

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (!interactable) { return; }
        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Press_Scale;
        }

        if (Use_Long_Touch)
        {
            CancelToken();

            Cancel_Token = new CancellationTokenSource();
            StartLongTouch().Forget();
        }
    }

    /// <summary>
    /// 단순 클릭
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) { return; }

        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Vector2.one;
        }

        if (Is_Long_Press)
        {
            if (Use_Long_Touch)
            {
                OnTouchEvent(TOUCH_RESULT_TYPE.RELEASE);
                Is_Long_Press = false;
                Cancel_Token = null;
            }
        }
        else
        {
            CancelToken();
            OnTouchEvent(TOUCH_RESULT_TYPE.CLICK);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (!interactable) { return; }

        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Vector2.one;
        }

        if (Is_Long_Press)
        {
            if (Use_Long_Touch)
            {
                OnTouchEvent(TOUCH_RESULT_TYPE.RELEASE);
                Is_Long_Press = false;
                Cancel_Token = null;
            }
        }
    }

    /// <summary>
    /// 롱터치 이벤트 감지
    /// </summary>
    /// <returns></returns>
    async UniTaskVoid StartLongTouch()
    {
        Is_Long_Press = false;
        await UniTask.Delay((int)(Long_Press_Duration * 1000), cancellationToken: Cancel_Token.Token);
        Is_Long_Press = true;

        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Vector2.one;
        }

        OnTouchEvent(TOUCH_RESULT_TYPE.LONG_PRESS);
        Cancel_Token = null;
    }

    void CancelToken()
    {
        if (Cancel_Token != null)
        {
            Cancel_Token.Cancel();
            Cancel_Token = null;
        }
    }

    protected override void OnDestroy()
    {
        Touch_Callback_Base.RemoveAllListeners();
    }
}
