
using FluffyDuck.Util;
using LitJson;
using System;
/**/
public class UserStoryStageData : UserDataBase
{
    public int Stage_ID { get; protected set; } = 0;

    SecureVar<int> Challenage_Count = null;
    SecureVar<int> Win_Count = null;
    SecureVar<int> Star_Point = null;
    /// <summary>
    /// 일일 입장 횟수
    /// </summary>
    SecureVar<int> Daily_Entrance_Count = null;

    /// <summary>
    /// 마지막 사용 시점
    /// </summary>
    string Last_Used_Dt = string.Empty;
    DateTime Last_Used_Date;


    Stage_Data Data;
    World_Data World;
    Zone_Data Zone;
    Schedule_Data Schedule;

    public UserStoryStageData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Stage_ID = 0;
        Last_Used_Dt = string.Empty;
        Last_Used_Date = DateTime.MinValue;
    }

    protected override void InitSecureVars()
    {
        if (Challenage_Count == null)
        {
            Challenage_Count = new SecureVar<int>();
        }
        if (Win_Count == null)
        {
            Win_Count = new SecureVar<int>();
        }
        if (Star_Point == null)
        {
            Star_Point = new SecureVar<int>();
        }
        if (Daily_Entrance_Count == null)
        {
            Daily_Entrance_Count = new SecureVar<int>();
        }
    }

    public void SetStageID(int stage_id)
    {
        Stage_ID = stage_id;
        InitMasterData();
        Is_Update_Data = true;

    }
    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_StageData(Stage_ID);
        Zone = m.Get_ZoneDataByStageGroupID(Data.stage_group_id);
        World = m.Get_WorldDataByZoneGroupID(Zone.zone_group_id);

        if (Data.schedule_id != 0)
        {
            Schedule = m.Get_ScheduleData(Data.schedule_id);
        }
        if (!string.IsNullOrEmpty(Last_Used_Dt))
        {
            Last_Used_Date = DateTime.Parse(Last_Used_Dt);
        }
        else
        {
            if (IsUseDailyEntranceLimit())
            {
                Daily_Entrance_Count.Set(GetMaxDailyEntranceCount());
            }
        }
        
    }

    public void AddChallenageCount()
    {
        if (!IsOpenSchedule())
        {
            return;
        }
        int cnt = GetChallengeCount();
        cnt += 1;
        Challenage_Count.Set(cnt);
        
        Is_Update_Data = true;
    }

    public void AddWinCount()
    {
        int cnt = GetWinCount();
        cnt += 1;
        Win_Count.Set(cnt);

        if (IsUseDailyEntranceLimit())
        {
            if (IsEnableDailyEntrance())
            {
                int daily_enter_cnt = GetDailyEntranceCount();
                daily_enter_cnt -= 1;
                Daily_Entrance_Count.Set(daily_enter_cnt);
                //  마지막 사용 시간 
                var now = DateTime.Now.ToLocalTime();
                Last_Used_Date = now;
                Last_Used_Dt = Last_Used_Date.ToString(GameDefine.DATE_TIME_FORMAT);
            }
        }

        Is_Update_Data = true;
    }
    public void SetStarPoint(int pt)
    {
        int star_point = GetStarPoint();
        if (pt > 3)
        {
            pt = 3;
        }
        if (star_point < pt)
        {
            Star_Point.Set(pt);
            Is_Update_Data = true;
        }
    }

    public int GetChallengeCount()
    {
        return Challenage_Count.Get();
    }
    public int GetWinCount()
    {
        return Win_Count.Get();
    }

    public int GetStarPoint()
    {
        return Star_Point.Get();
    }

    public int GetDailyEntranceCount()
    {
        return Daily_Entrance_Count.Get();
    }

    public bool IsStageCleared()
    {
        return GetWinCount() > 0;
    }
    /// <summary>
    /// 일일 횟수 제한 사용하는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsUseDailyEntranceLimit()
    {
        return GetMaxDailyEntranceCount() > 0;
    }
    /// <summary>
    /// 일일 입장 제한 횟수
    /// </summary>
    /// <returns></returns>
    public int GetMaxDailyEntranceCount()
    {
        if (Data != null)
        {
            return Data.entrance_limit_count;
        }
        return 0;
    }

    /// <summary>
    /// 일일 입장 제한 횟수가 있는 경우, 입장이 가능한 상태인지 여부 체크
    /// </summary>
    /// <returns></returns>
    public bool IsEnableDailyEntrance()
    {
        if (!IsOpenSchedule())
        {
            return false;
        }
        return GetDailyEntranceCount() > 0;
    }

    /// <summary>
    /// 해당 스케쥴 시간이 오픈된 시간인지 여부 체크<br/>
    /// 스케쥴 ID가 없을 경우 항상 오픈
    /// </summary>
    /// <returns></returns>
    bool IsOpenSchedule()
    {
        if (Schedule != null)
        {
            var begin_dt = DateTime.Parse(Schedule.date_start);
            var end_dt = DateTime.Parse(Schedule.date_end);
            var now = DateTime.Now.ToLocalTime();
            if (begin_dt > now || now > end_dt)
            {
                return false;
            }
        }
        return true;
    }

    public int GetStageGroupID()
    {
        if (Data != null)
        {
            return Data.stage_group_id;
        }
        return 0;
    }

    public STAGE_DIFFICULTY_TYPE GetDifficultyType()
    {
        if (Zone != null)
        {
            return Zone.zone_difficulty;
        }
        return STAGE_DIFFICULTY_TYPE.NONE;
    }

    public int GetFirstRewardGroupID()
    {
        if (Data != null)
        {
            return Data.first_reward_group_id;
        }
        return 0;
    }

    public int GetRepeatRewardGroupID()
    {
        if (Data != null)
        {
            return Data.repeat_reward_group_id;
        }
        return 0;
    }

    public int GetStarRewardGroupID()
    {
        if (Data != null)
        {
            return Data.star_reward_group_id;
        }
        return 0;
    }

    public Stage_Data GetStageData()
    {
        return Data;
    }
    public Zone_Data GetZoneData()
    {
        return Zone;
    }

    public World_Data GetWorldData()
    {
        return World;
    }
    DateTime GetLastUseDateTime()
    {
        return Last_Used_Date;
    }
    /// <summary>
    /// 해당 요일이 갱신 지정일로 정해져 있는지 여부 반환
    /// </summary>
    /// <param name="day_of_week"></param>
    /// <returns></returns>
    bool IsExistDayOfWeeks(int day_of_week)
    {
        bool is_exist = false;
        for (int i = 0; i < Schedule.day_of_weeks.Length; i++)
        {
            if (Schedule.day_of_weeks[i] == day_of_week)
            {
                is_exist = true;
                break;
            }
        }
        return is_exist;
    }

    public override RESPONSE_TYPE CheckDateAndTimeCharge()
    {
        RESPONSE_TYPE code = RESPONSE_TYPE.NOT_WORK;
        if (Schedule == null)
        {
            //  not work
            return code;
        }

        if (!IsOpenSchedule())
        {
            return code;
        }

        int now_count = GetDailyEntranceCount();
        if (GetMaxDailyEntranceCount() != 0 && now_count > GetMaxDailyEntranceCount())
        {
            //  not work
            return code;
        }

        var now = DateTime.Now.ToLocalTime();
        var last_dt = GetLastUseDateTime();
        if (last_dt == DateTime.MinValue)
        {
            Last_Used_Date = now;
            Last_Used_Dt = Last_Used_Date.ToString(GameDefine.DATE_TIME_FORMAT);
            Is_Update_Data = true;
            return RESPONSE_TYPE.SUCCESS;
        }

        if (last_dt.Date < now.Date)
        {
            var open_time = TimeSpan.Parse(Schedule.time_open);
            //  마지막 접속 시간을 기준으로 익일 새벽 시간을 찾는다.
            var refresh_dt = last_dt.Date.Add(open_time);
            if (refresh_dt <= last_dt)
            {
                refresh_dt = refresh_dt.AddDays(1);
            }

            //  현재 접속 시간과 갱신 시간을 체크해서, 현재 시간이 갱신 시간을 넘었을 경우 갱신 해줌
            if (now >= refresh_dt)
            {
                //  갱신일이 아니라면 갱신하지 않음
                if (!IsExistDayOfWeeks((int)refresh_dt.DayOfWeek))
                {
                    return RESPONSE_TYPE.NOT_WORK;
                }

                code = FullChargeEntranceCount();
                Is_Update_Data = true;
            }
        }

        return code;
    }
    /// <summary>
    /// 입장 횟수 풀 충전
    /// </summary>
    public RESPONSE_TYPE FullChargeEntranceCount()
    {
        int daily_enter_cnt = GetDailyEntranceCount();
        if (daily_enter_cnt >= GetMaxDailyEntranceCount())
        {
            return RESPONSE_TYPE.NOT_WORK;
        }

        Daily_Entrance_Count.Set(GetMaxDailyEntranceCount());
        Last_Used_Date = DateTime.MinValue;
        Last_Used_Dt = string.Empty;

        return RESPONSE_TYPE.SUCCESS;
    }
    public override JsonData Serialized()
    {
        
        var json = new LitJson.JsonData();

        json[NODE_STAGE_ID] = Stage_ID;
        json[NODE_CHALLENGE_COUNT] = GetChallengeCount();
        json[NODE_WIN_COUNT] = GetWinCount();
        json[NODE_STAR_POINT] = GetStarPoint();

        json[NODE_DAILY_ENTRANCE_COUNT] = GetDailyEntranceCount();
        if (!string.IsNullOrEmpty(Last_Used_Dt))
        {
            json[NODE_LAST_USED_DT] = Last_Used_Dt;
        }

        return json;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }
        InitSecureVars();

        {
            if (json.ContainsKey(NODE_STAGE_ID))
            {
                Stage_ID = ParseInt(json, NODE_STAGE_ID);
            }
            if (json.ContainsKey(NODE_CHALLENGE_COUNT))
            {
                Challenage_Count.Set(ParseInt(json, NODE_CHALLENGE_COUNT));
            }
            if (json.ContainsKey(NODE_WIN_COUNT))
            {
                Win_Count.Set(ParseInt(json, NODE_WIN_COUNT));
            }
            if (json.ContainsKey(NODE_STAR_POINT))
            {
                Star_Point.Set(ParseInt(json, NODE_STAR_POINT));
            }
            if (json.ContainsKey(NODE_LAST_USED_DT))
            {
                Last_Used_Dt = ParseString(json, NODE_LAST_USED_DT);
            }
            if (json.ContainsKey(NODE_DAILY_ENTRANCE_COUNT))
            {
                Daily_Entrance_Count.Set(ParseInt(json, NODE_DAILY_ENTRANCE_COUNT));
            }
        }

        InitMasterData();
        CheckDateAndTimeCharge();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_STAGE_ID = "sid";
    protected const string NODE_CHALLENGE_COUNT = "ccnt";
    protected const string NODE_WIN_COUNT = "wcnt";
    protected const string NODE_STAR_POINT = "star";

    protected const string NODE_DAILY_ENTRANCE_COUNT = "dcnt";
    protected const string NODE_LAST_USED_DT = "lastdt";
}
