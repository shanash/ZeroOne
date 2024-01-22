using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopStatusBarGoodsGold : TopStatusBarGoodsBase
{
    public override void UpdateGoodsItem()
    {
        var mng = GameData.Instance.GetUserGoodsDataManager();
        double cnt = mng.GetGoodsCount(GOODS_TYPE.GOLD);
        Goods_Count.text = cnt.ToString("N0");
    }

    protected override void ClickCallback()
    {
        CommonUtils.ShowToast("골드 구매 준비중", TOAST_BOX_LENGTH.SHORT);
    }
}
