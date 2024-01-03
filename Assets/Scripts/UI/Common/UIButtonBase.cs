using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonBase : Selectable, IPointerClickHandler
{
    [SerializeField, Tooltip("Scaling RectTransform")]
    protected RectTransform Scale_Rect;

    [SerializeField, Tooltip("Press Scale")]
    protected Vector2 Press_Scale;

    [SerializeField, Tooltip("OnClick")]
    protected UnityEvent OnClick = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!interactable)
        {
            return;
        }
        OnClick?.Invoke();
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


}
