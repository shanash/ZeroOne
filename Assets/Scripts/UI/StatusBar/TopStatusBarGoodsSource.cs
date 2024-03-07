using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using TMPro;
using UnityEngine;

public class TopStatusBarGoodsSource : TopStatusBarGoodsBase
{
    UserChargeItemData Charge_Item;

    Coroutine Essense_Coroutine;

    protected override void InitStatusBar()
    {
        var mng = GameData.Instance.GetUserChargeItemDataManager();
        Charge_Item = mng.FindUserChargeItemData(REWARD_TYPE.SEND_ESSENCE);
        UpdateGoodsItem();
        StartCoroutine(StartOnUpdate(60f));
    }

    private void OnEnable()
    {
        if (Essense_Coroutine != null)
        {
            StopCoroutine(Essense_Coroutine);
        }
        Essense_Coroutine = StartCoroutine(StartOnUpdate(1f));
    }
    private void OnDisable()
    {
        if (Essense_Coroutine != null)
        {
            StopCoroutine(Essense_Coroutine);
        }
        Essense_Coroutine = null;
    }
    /// <summary>
    /// 하루 한번만 갱신되기 때문에, 1분에 한번씩만 돌면됨
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator StartOnUpdate(float delay)
    {
        var wait = new WaitForSeconds(delay);
        while (true)
        {
            UpdateGoodsItem();
            yield return wait;
        }
    }

    public override void UpdateGoodsItem()
    {
        if (Charge_Item == null)
        {
            return;
        }
        int max_bound = Charge_Item.GetMaxBound();
        int cnt = Charge_Item.GetCount();

        //  stamina count
        Goods_Count.text = ZString.Format("{0}/{1}", cnt, max_bound);
    }

    protected override void ClickCallback()
    {
        CommonUtils.ShowToast("근원전달 구매 준비중", TOAST_BOX_LENGTH.SHORT);
    }
}
