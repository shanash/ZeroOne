using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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


public abstract class UILongTouchButtonBase : Selectable, IPointerClickHandler
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
    protected float Long_Press_Duration = 0.7f;

    bool Is_Long_Press;
    Coroutine Long_Touch_Coroutine;

    protected abstract UnityEventBase Touch_Callback_Base { get; }
    protected abstract Dictionary<TOUCH_RESULT_TYPE, object[]> Parameters_OnPointer { get;  }

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

        if (Is_Long_Press && Parameters_OnPointer.ContainsKey(TOUCH_RESULT_TYPE.RELEASE))
        {
            object obj = Touch_Callback_Base.GetPersistentTarget(0);
            string func_name = Touch_Callback_Base.GetPersistentMethodName(0);
            Type t = obj.GetType();

            MethodInfo m = t.GetMethod(func_name);
            var c_obj = Convert.ChangeType(obj, t);
            m.Invoke(c_obj, Parameters_OnPointer[TOUCH_RESULT_TYPE.RELEASE]);
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

        if (Parameters_OnPointer.ContainsKey(TOUCH_RESULT_TYPE.CLICK))
        {
            object obj = Touch_Callback_Base.GetPersistentTarget(0);
            string func_name = Touch_Callback_Base.GetPersistentMethodName(0);
            Type t = obj.GetType();

            MethodInfo m = t.GetMethod(func_name);
            var c_obj = Convert.ChangeType(obj, t);
            m.Invoke(c_obj, Parameters_OnPointer[TOUCH_RESULT_TYPE.CLICK]);
        }

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
        if (!Parameters_OnPointer.ContainsKey(TOUCH_RESULT_TYPE.LONG_PRESS))
        {
            yield break;
        }

        Is_Long_Press = false;
        yield return new WaitForSeconds(Long_Press_Duration);
        Long_Touch_Coroutine = null;
        Is_Long_Press = true;

        object obj = Touch_Callback_Base.GetPersistentTarget(0);
        string func_name = Touch_Callback_Base.GetPersistentMethodName(0);
        Type t = obj.GetType();

        MethodInfo m = t.GetMethod(func_name);
        var c_obj = Convert.ChangeType(obj, t);
        m.Invoke(c_obj, Parameters_OnPointer[TOUCH_RESULT_TYPE.LONG_PRESS]);

    }

    protected override void OnDestroy()
    {
        Touch_Callback_Base.RemoveAllListeners();
    }
}
