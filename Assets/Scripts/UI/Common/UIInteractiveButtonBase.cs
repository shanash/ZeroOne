using System.Collections;
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

    bool Is_Long_Press = true;
    Coroutine Long_Touch_Coroutine = null;

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

        if (Long_Touch_Coroutine != null)
        {
            StopCoroutine(Long_Touch_Coroutine);
            Long_Touch_Coroutine = null;
        }

        Long_Touch_Coroutine = StartCoroutine(StartLongTouch());
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

        if (Long_Touch_Coroutine != null)
        {
            StopCoroutine(Long_Touch_Coroutine);
            Long_Touch_Coroutine = null;
            OnTouchEvent(TOUCH_RESULT_TYPE.CLICK);
        }
        else
        {
            if (Is_Long_Press)
            {
                OnTouchEvent(TOUCH_RESULT_TYPE.RELEASE);
            }
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

        if (Long_Touch_Coroutine == null)
        {
            if (Is_Long_Press)
            {
                //OnTouchEvent(TOUCH_RESULT_TYPE.RELEASE);
            }
        }
    }

    /// <summary>
    /// 롱터치 이벤트 감지
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLongTouch()
    {
        Is_Long_Press = false;
        yield return new WaitForSeconds(Long_Press_Duration);
        Long_Touch_Coroutine = null;
        Is_Long_Press = true;

        OnTouchEvent(TOUCH_RESULT_TYPE.LONG_PRESS);
    }

    protected override void OnDestroy()
    {
        Touch_Callback_Base.RemoveAllListeners();
    }
}
