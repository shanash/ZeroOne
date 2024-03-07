using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

public enum TOP_STATUS_TYPE
{
    NONE = 0,
    STAMINA,
    GOLD,
    DIA,
    SOURCE,
}

public class TopStatusBar : MonoBehaviour
{
    [SerializeField, Tooltip("Top Status Bar Goods Items")]
    List<TopStatusBarGoodsBase> Goods_Item_List;

    private void Start()
    {
        UpdateAllGoods();
    }
    /// <summary>
    /// 모든 재화 업데이트
    /// </summary>
    void UpdateAllGoods()
    {
        Goods_Item_List.ForEach(x => x.UpdateGoodsItem());
    }

    private void OnEnable()
    {
        var evt_dispatcher = UpdateEventDispatcher.Instance;
        evt_dispatcher.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ALL, UpdateEventCallback);
        evt_dispatcher.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_STAMINA, UpdateEventCallback);
        evt_dispatcher.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_GOLD, UpdateEventCallback);
        evt_dispatcher.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_DIA, UpdateEventCallback);
        evt_dispatcher.AddEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE, UpdateEventCallback);
    }
    private void OnDisable()
    {
        if (UpdateEventDispatcher.Instance == null)
        {
            return;
        }
        var evt_dispatcher = UpdateEventDispatcher.Instance;
        evt_dispatcher.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ALL, UpdateEventCallback);
        evt_dispatcher.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_STAMINA, UpdateEventCallback);
        evt_dispatcher.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_GOLD, UpdateEventCallback);
        evt_dispatcher.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_DIA, UpdateEventCallback);
        evt_dispatcher.RemoveEventCallback(UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE, UpdateEventCallback);
    }
    /// <summary>
    /// 업데이트 이벤트 콜백
    /// </summary>
    /// <param name="etype"></param>
    void UpdateEventCallback(UPDATE_EVENT_TYPE etype)
    {
        switch (etype)
        {
            case UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ALL:
                {
                    UpdateAllGoods();
                }
                break;
            case UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_GOLD:
                {
                    var found = FindGoods(TOP_STATUS_TYPE.GOLD);
                    if (found != null)
                    {
                        found.UpdateGoodsItem();
                    }
                }
                break;
            case UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_DIA:
                {
                    var found = FindGoods(TOP_STATUS_TYPE.DIA);
                    if (found != null)
                    {
                        found.UpdateGoodsItem();
                    }
                }
                break;
            case UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_STAMINA:
                {
                    {
                        var found = FindGoods(TOP_STATUS_TYPE.STAMINA);
                        if (found != null)
                        {
                            found.UpdateGoodsItem();
                        }
                    }
                }
                break;
            case UPDATE_EVENT_TYPE.UPDATE_TOP_STATUS_BAR_ESSESNCE:
                {
                    var found = FindGoods(TOP_STATUS_TYPE.SOURCE);
                    if (found != null)
                    {
                        found.UpdateGoodsItem();
                    }
                }
                break;
        }
    }
    TopStatusBarGoodsBase FindGoods(TOP_STATUS_TYPE stype)
    {
        return Goods_Item_List.Find(x => x.GetTopStatusType() == stype); 
    }


    public void OnClickBack()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.FULLPAGE_TYPE);
    }

    public void OnClickGoHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.CloseAll();
    }
}
