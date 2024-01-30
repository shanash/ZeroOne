using FluffyDuck.Util;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class UserChargeItemData : UserDataBase
{
    /// <summary>
    /// 챠지 아이템 타입
    /// </summary>
    public REWARD_TYPE Charge_Item_Type { get; private set; }  = REWARD_TYPE.NONE;
    /// <summary>
    /// 충전 갯수(보유수)
    /// </summary>
    protected SecureVar<int> Count = null;
    /// <summary>
    /// 마지막 사용 시점(이 시점을 기준으로 충전)
    /// </summary>
    public string Last_Used_Dt { get; private set; } = string.Empty;

    protected Charge_Value_Data Data;
    protected Max_Bound_Info_Data Max_Bound_Data;

    public UserChargeItemData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Charge_Item_Type = REWARD_TYPE.NONE;
        Last_Used_Dt = string.Empty;
    }

    protected override void InitSecureVars()
    {
        if (Count == null)
        {
            Count = new SecureVar<int>();
        }
    }

    public void SetRewardType(REWARD_TYPE rtype)
    {
        Charge_Item_Type = rtype;
        InitMasterData();
        Is_Update_Data = true;
    }

    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_ChargeValueData(Charge_Item_Type);
        Max_Bound_Data = m.Get_MaxBoundInfoData(Charge_Item_Type);

        if (string.IsNullOrEmpty(Last_Used_Dt))
        {
            Count.Set((int)Max_Bound_Data.base_max);
        }
    }

    public int GetCount()
    {
        return Count.Get();
    }
    /// <summary>
    /// 최대 수
    /// </summary>
    /// <returns></returns>
    public int GetMaxBound()
    {
        return (int)Max_Bound_Data.base_max;
    }
    /// <summary>
    /// 완충 상태인지 여부 체크
    /// </summary>
    /// <returns></returns>
    public bool IsFullCharged()
    {
        return GetCount() >= GetMaxBound();
    }

    public ERROR_CODE AddChargeItem(int cnt)
    {
        ERROR_CODE code = ERROR_CODE.SUCCESS;
        if (cnt > 0)
        {
            int item_count = GetCount();
            item_count += cnt;
            Count.Set(item_count);

            Is_Update_Data = true;
        }

        return code;
    }

    public ERROR_CODE UseChargeItem(int use_cnt)
    {
        if (!IsUsableChargeItemCount(use_cnt))
        {
            return ERROR_CODE.NOT_ENOUGH_ITEM;
        }

        if (use_cnt > 0)
        {

        }
        return ERROR_CODE.FAILED;
    }

    public ERROR_CODE FullChargeItem()
    {
        return ERROR_CODE.FAILED;
    }

    public bool IsUsableChargeItemCount(int use_cnt)
    {
        if (use_cnt < 0)
        {
            return false;
        }
        return GetCount() >= use_cnt;
    }

    //public override ERROR_CODE CheckDateChange()
    //{
    //    ERROR_CODE code = ERROR_CODE.NOT_WORK;

    //    int now_count = GetCount();
    //    if (GetMaxBound() != 0 && now_count > GetMaxBound())
    //    {
    //        //  not work
    //        return code;
    //    }
    //    var now = DateTime.Now.ToLocalTime();
    //    if (!string.IsNullOrEmpty(Last_Used_Dt))
    //    {
    //        var last_dt = DateTime.Parse(Last_Used_Dt);
    //        if (last_dt.Date < now.Date)
    //        {

    //        }
    //    }
    //    return code;
    //}



    public override JsonData Serialized()
    {
        return base.Serialized();
    }
    public override bool Deserialized(JsonData json)
    {
        return base.Deserialized(json);
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_CHARGE_ITEM_TYPE = "ctype";
    protected const string NODE_COUNT = "cnt";
    protected const string NODE_LAST_USED_DT = "lastdt";
}
