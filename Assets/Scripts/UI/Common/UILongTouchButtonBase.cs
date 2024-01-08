using System.Collections;
using System.Collections.Generic;
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


public class UILongTouchButtonBase : Selectable, IPointerClickHandler
{
    [SerializeField, Tooltip("Scaling RectTransform")]
    protected RectTransform Scale_Rect;

    [SerializeField, Tooltip("Press Scale")]
    protected Vector2 Press_Scale;

    [SerializeField, Tooltip("Use Long Touch")]
    protected bool Use_Long_Touch;

    [SerializeField, Tooltip("Touch Callback")]
    protected UnityEvent<TOUCH_RESULT_TYPE> Touch_Callback;

    /// <summary>
    /// 0.5초 이상 누르고 있으면 롱터치로 인식
    /// </summary>
    const float LONG_PRESS_DURATION = 0.7f;
    bool Is_Long_Press;

    Coroutine Long_Touch_Coroutine;

    public void AddTouchCallback(UnityAction<TOUCH_RESULT_TYPE> cb)
    {
        Touch_Callback.AddListener(cb);
    }

    public void RemovTouchCallback(UnityAction<TOUCH_RESULT_TYPE> cb)
    {
        Touch_Callback.RemoveListener(cb);
    }

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

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (!interactable) { return; }
        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Vector2.one;
        }

        if (Is_Long_Press)
        {
            Touch_Callback?.Invoke(TOUCH_RESULT_TYPE.RELEASE);
        }

        if (Long_Touch_Coroutine != null)
        {
            StopCoroutine(Long_Touch_Coroutine);
            Long_Touch_Coroutine = null;
            Is_Long_Press = false;
        }
        
    }

    /// <summary>
    /// 단순 클릭
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable) { return; }
        if (Is_Long_Press) { return; }

        Touch_Callback?.Invoke(TOUCH_RESULT_TYPE.CLICK);

        if (Long_Touch_Coroutine != null)
        {
            StopCoroutine(Long_Touch_Coroutine);
            Long_Touch_Coroutine = null;
        }
    }
    /// <summary>
    /// 롱터치 이벤트 감지
    /// </summary>
    /// <returns></returns>
    IEnumerator StartLongTouch()
    {
        Is_Long_Press = false;
        yield return new WaitForSeconds(LONG_PRESS_DURATION);
        Long_Touch_Coroutine = null;
        Is_Long_Press = true;

        Touch_Callback?.Invoke(TOUCH_RESULT_TYPE.LONG_PRESS);
    }

    protected override void OnDestroy()
    {
        Touch_Callback.RemoveAllListeners();
    }
}
