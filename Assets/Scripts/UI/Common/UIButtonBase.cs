using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonBase : Selectable, IPointerClickHandler
{
    [SerializeField, Tooltip("Scaling RectTransform")]
    protected RectTransform Scale_Rect = null;

    [SerializeField, Tooltip("Press Scale")]
    protected Vector2 Press_Scale = default;

    [SerializeField, Tooltip("OnClick")]
    protected UIButtonEvent OnClick = new UIButtonEvent();


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
        {
            return;
        }
        OnClick?.Invoke(this);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (!interactable)
        {
            return;
        }
        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Press_Scale;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        if (!interactable)
        {
            return;
        }
        if (Scale_Rect != null)
        {
            Scale_Rect.localScale = Vector2.one;
        }
    }

    protected override void OnDestroy()
    {
        OnClick.RemoveAllListeners();
    }

    [Serializable]
    public class UIButtonEvent : UnityEvent<UIButtonBase> { }
}
