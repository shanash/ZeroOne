using FluffyDuck.Util;
using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
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

    DateTime Last_Used_Date;

    protected Charge_Value_Data Data;
    protected Max_Bound_Info_Data Max_Bound_Data;
    protected Schedule_Data Schedule;

    public UserChargeItemData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Charge_Item_Type = REWARD_TYPE.NONE;
        Last_Used_Dt = string.Empty;
        Last_Used_Date = DateTime.MinValue;
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
        if (Data.schedule_id != 0)
        {
            Schedule = m.Get_ScheduleData(Data.schedule_id);
        }
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

    public RESPONSE_TYPE AddChargeItem(int cnt)
    {
        RESPONSE_TYPE code = RESPONSE_TYPE.SUCCESS;
        if (cnt > 0)
        {
            int item_count = GetCount();
            item_count += cnt;
            if (item_count > GetMaxBound())
            {
                Last_Used_Date = DateTime.MinValue;
                Last_Used_Dt = string.Empty;
            }
            Count.Set(item_count);

            Is_Update_Data = true;
        }

        return code;
    }

    public RESPONSE_TYPE UseChargeItem(int use_cnt)
    {
        if (!IsUsableChargeItemCount(use_cnt))
        {
            return RESPONSE_TYPE.NOT_ENOUGH_ITEM;
        }

        if (use_cnt > 0)
        {
            CheckDateAndTimeCharge();
            int item_cnt = GetCount();
            item_cnt -= use_cnt;
            Count.Set(item_cnt);
            if (Last_Used_Date == DateTime.MinValue)
            {
                var now = DateTime.Now.ToLocalTime();
                Last_Used_Date = now;
                Last_Used_Dt = Last_Used_Date.ToString(GameDefine.DATE_TIME_FORMAT);
            }
            Is_Update_Data = true;
            return RESPONSE_TYPE.SUCCESS;
        }
        return RESPONSE_TYPE.FAILED;
    }
    /// <summary>
    /// 완전 풀 충전.<br/>
    /// CHARGE_TYPE에 따라 추가 작업 필요. 지금은 기본 타입으로 풀 충전만 해줌
    /// </summary>
    /// <returns></returns>
    public RESPONSE_TYPE FullChargeItem()
    {
        int cnt = GetCount();
        if (cnt > GetMaxBound())
        {
            return RESPONSE_TYPE.NOT_WORK;
        }

        cnt = GetMaxBound();
        Count.Set(cnt);

        Last_Used_Date = DateTime.MinValue;
        Last_Used_Dt = string.Empty;

        Is_Update_Data = true;

        return RESPONSE_TYPE.SUCCESS;
    }

    public bool IsUsableChargeItemCount(int use_cnt)
    {
        if (use_cnt < 0)
        {
            return false;
        }
        return GetCount() >= use_cnt;
    }

    DateTime GetLastUseDateTime()
    {
        return Last_Used_Date;
    }

    

    public override RESPONSE_TYPE CheckDateAndTimeCharge()
    {
        RESPONSE_TYPE code = RESPONSE_TYPE.NOT_WORK;

        if (Data.repeat_type == REPEAT_TYPE.REPEAT_MIN)
        {
            return CheckMinutes();
        }
        else if (Data.repeat_type == REPEAT_TYPE.REPEAT_DAY)
        {
            return CheckDays();
        }
        else if (Data.repeat_type == REPEAT_TYPE.REPEAT_WEEK)
        {
            return CheckWeeks();
        }
        else if (Data.repeat_type == REPEAT_TYPE.REPEAT_MONTH)
        {
            return CheckMonths();
        }
        else if (Data.repeat_type == REPEAT_TYPE.REPEAT_YEAR)
        {
            return CheckYears();
        }
        else
        {
            Debug.Assert(false);
        }

        return code;
    }

    /// <summary>
    /// 다음 쿨타임까지 남은 시간 반환
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public TimeSpan GetRemainChargeTime()
    {
        if (IsFullCharged())
        {
            return TimeSpan.Zero;
        }

        var now = DateTime.Now.ToLocalTime();
        RESPONSE_TYPE result = CheckDateAndTimeCharge();
        if (result == RESPONSE_TYPE.NOT_WORK)
        {
            return TimeSpan.Zero;
        }

        TimeSpan cooltime = TimeSpan.FromSeconds(GetRepeatTime());
        TimeSpan subtract = now - Last_Used_Date;
        return cooltime - subtract;
    }

    /// <summary>
    /// 반복 주기 시간
    /// </summary>
    /// <returns></returns>
    int GetRepeatTime()
    {
        int repeat_time = 0;
        switch (Data.repeat_type)
        {
            case REPEAT_TYPE.REPEAT_MIN:
                repeat_time = Data.repeat_time * 60;
                break;
            case REPEAT_TYPE.REPEAT_DAY:
                repeat_time = Data.repeat_time * 60 * 60 * 24;
                break;
            case REPEAT_TYPE.REPEAT_WEEK:
                repeat_time = Data.repeat_time * 60 * 60 * 24 * 7;
                break;
            case REPEAT_TYPE.REPEAT_MONTH:
                repeat_time = Data.repeat_time * 60 * 60 * 24 * 7 * 28;
                break;
            case REPEAT_TYPE.REPEAT_YEAR:
                repeat_time = Data.repeat_time * 60 * 60 * 24 * 7 * 365;
                break;
            default:
                Debug.Assert(false);
                break;
        }
        return repeat_time;
    }

    /// <summary>
    /// 반복 주기 체크(n분)
    /// </summary>
    RESPONSE_TYPE CheckMinutes()
    {
        RESPONSE_TYPE code = RESPONSE_TYPE.NOT_WORK;
        int repeat_time = GetRepeatTime();
        int now_count = GetCount();
        if (GetMaxBound() != 0 && now_count > GetMaxBound())
        {
            //  not work
            return code;
        }
        var now = DateTime.Now.ToLocalTime();
        var last = GetLastUseDateTime();
        if (last == DateTime.MinValue)
        {
            Last_Used_Date = now;
            Last_Used_Dt = Last_Used_Date.ToString(GameDefine.DATE_TIME_FORMAT);
            Is_Update_Data = true;
            return RESPONSE_TYPE.SUCCESS;
        }

        var total_time = now - last;
        int charge_cnt = (int)total_time.TotalSeconds / repeat_time * Data.charge_count;
        if (charge_cnt > 0)
        {
            int cur_cnt = GetCount();
            cur_cnt += charge_cnt;
            if (cur_cnt > GetMaxBound())
            {
                cur_cnt = GetMaxBound();
                Count.Set(cur_cnt);
                Last_Used_Date = DateTime.MinValue;
                Last_Used_Dt = string.Empty;
            }
            else
            {
                Count.Set(cur_cnt);
                Last_Used_Date = last.AddSeconds(charge_cnt * repeat_time);
                Last_Used_Dt = Last_Used_Date.ToString(GameDefine.DATE_TIME_FORMAT);
            }
            Is_Update_Data = true;
        }
        code = RESPONSE_TYPE.SUCCESS;
        return code;
    }
    /// <summary>
    /// 반복 주기 체크(n일)
    /// </summary>
    RESPONSE_TYPE CheckDays() 
    { 
        if (Schedule == null)
        {
            return RESPONSE_TYPE.NOT_WORK;
        }

        return RESPONSE_TYPE.NOT_WORK; 
    }
    /// <summary>
    /// 반복 주기 체크(n주간)
    /// </summary>
    RESPONSE_TYPE CheckWeeks() { return RESPONSE_TYPE.NOT_WORK; }
    /// <summary>
    /// 반복 주기 체크(n개월)
    /// </summary>
    RESPONSE_TYPE CheckMonths() { return RESPONSE_TYPE.NOT_WORK; }
    /// <summary>
    /// 반복 주기 체크(n년)
    /// </summary>
    RESPONSE_TYPE CheckYears() { return RESPONSE_TYPE.NOT_WORK; }


    public override JsonData Serialized()
    {
        //if (!IsUpdateData())
        //{
        //    return null;
        //}
        var json = new JsonData();

        json[NODE_CHARGE_ITEM_TYPE] = (int)Charge_Item_Type;
        json[NODE_COUNT] = GetCount();
        json[NODE_LAST_USED_DT] = Last_Used_Dt;

        return json;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null) return false;

        InitSecureVars();
        if (json.ContainsKey(NODE_CHARGE_ITEM_TYPE))
        {
            Charge_Item_Type = (REWARD_TYPE)ParseInt(json, NODE_CHARGE_ITEM_TYPE);
        }
        if (json.ContainsKey(NODE_COUNT))
        {
            Count.Set(ParseInt(json, NODE_COUNT));
        }
        if (json.ContainsKey(NODE_LAST_USED_DT))
        {
            Last_Used_Dt = ParseString(json, NODE_LAST_USED_DT);
            if (!string.IsNullOrEmpty(Last_Used_Dt))
            {
                Last_Used_Date = DateTime.Parse(Last_Used_Dt);
            }
        }

        InitMasterData();
        CheckDateAndTimeCharge();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_CHARGE_ITEM_TYPE = "ctype";
    protected const string NODE_COUNT = "cnt";
    protected const string NODE_LAST_USED_DT = "lastdt";
}
