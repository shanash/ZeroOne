using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopStatusBarGoodsSource : TopStatusBarGoodsBase
{
    public override void UpdateGoodsItem()
    {

    }

    protected override void ClickCallback()
    {
        CommonUtils.ShowToast("ETC 구매 준비중", TOAST_BOX_LENGTH.SHORT);
    }
}
