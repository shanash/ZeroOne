using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChargeItemData : UserDataBase
{
    public REWARD_TYPE Charge_Item_Type { get; private set; }  = REWARD_TYPE.NONE;

    protected SecureVar<int> Count = null;

    public string Last_Used_Dt { get; private set; } = string.Empty;

    protected Charge_Value_Data Data;

    public UserChargeItemData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Count.Set(0);
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
        Data = MasterDataManager.Instance.Get_ChargeValueData(Charge_Item_Type);
    }

    public int GetCount()
    {
        return Count.Get();
    }

    public ERROR_CODE AddChargeItem(int cnt)
    {
        ERROR_CODE code = ERROR_CODE.SUCCESS;

        return code;
    }

    public ERROR_CODE UseChargeItem(int use_cnt)
    {
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

    public override ERROR_CODE CheckDateChange()
    {
        ERROR_CODE code = ERROR_CODE.NOT_WORK;

        int now_count = GetCount();
        


        return code;
    }

    public virtual int GetMaxBound()
    {
        return 10;
    }


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
