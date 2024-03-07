using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TopStatusBarGoodsStamina : TopStatusBarGoodsBase
{
    [SerializeField, Tooltip("Remain Time")]
    TMP_Text Remain_Time;

    UserChargeItemData Charge_Item;

    Coroutine Stamina_Coroutine;

    protected override void InitStatusBar()
    {
        var mng = GameData.Instance.GetUserChargeItemDataManager();
        Charge_Item = mng.FindUserChargeItemData(REWARD_TYPE.STAMINA);
        UpdateGoodsItem();
    }

    private void OnEnable()
    {
        if (Stamina_Coroutine != null)
        {
            StopCoroutine(Stamina_Coroutine);
        }
        Stamina_Coroutine = StartCoroutine(StartOnUpdate(1f));
    }
    private void OnDisable()
    {
        if (Stamina_Coroutine != null)
        {
            StopCoroutine(Stamina_Coroutine);
        }
        Stamina_Coroutine = null;
    }
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

        //  remain time
        var remain_time = Charge_Item.GetRemainChargeTime();
        if (remain_time.TotalSeconds > 0)
        {
            Remain_Time.text = ZString.Format("{0:D2}:{1:D2}", remain_time.Minutes, remain_time.Seconds);
        }
        else
        {
            Remain_Time.text = "";
        }
    }

    protected override void ClickCallback()
    {
        CommonUtils.ShowToast("스테미너 구매 준비중", TOAST_BOX_LENGTH.SHORT);
    }

    

}
