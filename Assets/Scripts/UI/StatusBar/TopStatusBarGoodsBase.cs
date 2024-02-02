using FluffyDuck.Util;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TopStatusBarGoodsBase : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Status Goods Type")]
    TOP_STATUS_TYPE Status_Type = TOP_STATUS_TYPE.NONE;

    [SerializeField, Tooltip("Goods Count")]
    protected TMP_Text Goods_Count;

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);

    private void Start()
    {
        InitStatusBar();
    }

    protected virtual void InitStatusBar() { }

    /// <summary>
    /// 상태바 아이콘 클릭 이벤트 콜백
    /// </summary>
    protected virtual void ClickCallback() { }
    /// <summary>
    /// 상태바 아이콘 업데이트
    /// </summary>
    public virtual void UpdateGoodsItem() { }

    public void OnPointerDown(PointerEventData eventData)
    {
        Box.localScale = Press_Scale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Box.localScale = Vector2.one;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        ClickCallback();
    }

    public TOP_STATUS_TYPE GetTopStatusType() { return Status_Type; }


    
}
