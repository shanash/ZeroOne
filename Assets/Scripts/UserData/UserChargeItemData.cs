using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChargeItemData : UserDataBase
{
    public REWARD_TYPE Charge_Item_Type { get; private set; }  = REWARD_TYPE.NONE;

    protected SecureVar<int> Count = null;

    public string Last_Usd_Dt { get; private set; } = string.Empty;

    protected Charge_Value_Data Data;

    public UserChargeItemData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Count.Set(0);
    }

    protected override void InitSecureVars()
    {
        if (Count == null)
        {
            Count = new SecureVar<int>();
        }
    }

    protected override void InitMasterData()
    {
        
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
        return 0;
    }



}
