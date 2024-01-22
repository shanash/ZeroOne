using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopStatusBarGoodsDia : TopStatusBarGoodsBase
{
    public override void UpdateGoodsItem()
    {
        var mng = GameData.Instance.GetUserGoodsDataManager();
        double cnt = mng.GetGoodsCount(GOODS_TYPE.DIA);
        Goods_Count.text = cnt.ToString("N0");
    }

    protected override void ClickCallback()
    {
        CommonUtils.ShowToast("다이아 구매 준비중", TOAST_BOX_LENGTH.SHORT);
    }
}
