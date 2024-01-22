using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopStatusBarGoodsStamina : TopStatusBarGoodsBase
{
    public override void UpdateGoodsItem()
    {
        Goods_Count.text = "10/10";
    }

    protected override void ClickCallback()
    {
        CommonUtils.ShowToast("스테미너 구매 준비중", TOAST_BOX_LENGTH.SHORT);
    }
}
