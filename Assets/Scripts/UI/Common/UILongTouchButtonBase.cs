using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILongTouchButtonBase : Selectable, IPointerClickHandler
{
    [SerializeField, Tooltip("Scaling RectTransform")]
    protected RectTransform Scale_Rect;

    [SerializeField, Tooltip("Press Scale")]
    protected Vector2 Press_Scale;

    [SerializeField, Tooltip("Use Long Touch")]
    protected bool Use_Long_Touch;

    [SerializeField, Tooltip("Touch Callback")]
    protected UnityEvent<TOUCH_STATES_TYPE> Touch_Callback;

    public void AddTouchCallback(UnityAction<TOUCH_STATES_TYPE> cb)
    {
        Touch_Callback.AddListener(cb);
    }

    public void RemovTouchCallback(UnityAction<TOUCH_STATES_TYPE> cb)
    {
        Touch_Callback.RemoveListener(cb);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    protected override void OnDestroy()
    {
        Touch_Callback.RemoveAllListeners();
    }
}
