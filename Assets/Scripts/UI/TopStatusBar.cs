using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopStatusBar : MonoBehaviour
{
    [SerializeField, Tooltip("Top Status Bar Goods Items")]
    List<TopStatusBarGoodsItem> Goods_Item_List;

    public void OnClickBack()
    {
        PopupManager.Instance.RemoveLastPopupType(POPUP_TYPE.UI_TYPE);
    }

    public void OnClickGoHome()
    {
        PopupManager.Instance.CloseAll();
    }
}
