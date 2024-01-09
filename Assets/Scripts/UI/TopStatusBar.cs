using FluffyDuck.UI;
using System.Collections.Generic;
using UnityEngine;

public class TopStatusBar : MonoBehaviour
{
    [SerializeField, Tooltip("Top Status Bar Goods Items")]
    List<TopStatusBarGoodsItem> Goods_Item_List;

    public void OnClickBack()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.UI_TYPE);
    }

    public void OnClickGoHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.CloseAll();
    }
}
