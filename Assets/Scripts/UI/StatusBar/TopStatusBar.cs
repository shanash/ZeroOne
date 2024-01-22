using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

enum TOP_STATUS_TYPE
{
    NONE = 0,
    STAMINA,
}

public class TopStatusBar : MonoBehaviour
{
    [SerializeField, Tooltip("Top Status Bar Goods Items")]
    List<TopStatusBarGoodsBase> Goods_Item_List;

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    void UpdateEventCallback(UPDATE_EVENT_TYPE etype)
    {

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
