using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class TestListNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPoolableComponent
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box_Rect;

    [SerializeField, Tooltip("List Text")]
    TMP_Text List_Text;


    Vector2 Press_Scale = new Vector2(0.95f, 0.95f);

    
    public void OnPointerDown(PointerEventData eventData)
    {
        Box_Rect.localScale = Press_Scale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Box_Rect.localScale = Vector2.one;
    }

    public void SetListText(string txt)
    {
        List_Text.text = txt;
    }

    public void Spawned()
    {
        //this.transform.localScale = Vector2.one;
    }
    public void Despawned()
    {
        
    }

}
