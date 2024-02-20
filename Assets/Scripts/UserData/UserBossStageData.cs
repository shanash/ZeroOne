
using FluffyDuck.Util;
using LitJson;
using System;

public class UserBossStageData : UserDataBase
{
    public int Boss_Dungeon_ID { get; protected set; } = 0;

    SecureVar<int> Daily_Challenge_Count = null;
    SecureVar<int> Daily_Win_Count = null;
    SecureVar<int> Star_Point = null;

    Boss_Data Boss;
    Boss_Stage_Data Stage;

    public UserBossStageData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Boss_Dungeon_ID = 0;
    }
    protected override void InitSecureVars()
    {
        if (Daily_Challenge_Count == null)
        {
            Daily_Challenge_Count = new SecureVar<int>();
        }
        if (Daily_Win_Count == null)
        {
            Daily_Win_Count= new SecureVar<int>();
        }
        if (Star_Point == null)
        {
            Star_Point = new SecureVar<int>();
        }
    }

    public void SetBossDungeonID(int boss_dungeon_id)
    {
        Boss_Dungeon_ID = boss_dungeon_id;
        InitMasterData();
        Is_Update_Data = true;
    }
    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        
        Stage = m.Get_BossStageData(Boss_Dungeon_ID);
        Boss = m.Get_BossDataByBossStageGroupID(Stage.boss_stage_group_id);
    }

    public int GetDailyChallengeCount()
    {
        return Daily_Challenge_Count.Get();
    }
    public int GetDailyWinCount()
    {
        return Daily_Win_Count.Get();
    }
    public int GetStarPoint()
    {
        return Star_Point.Get();
    }

    public void SetStarPoint(int pt)
    {
        int star_point = GetStarPoint();
        if (star_point < pt)
        {
            Star_Point.Set(pt);
            Is_Update_Data = true;
        }
    }

    public void AddDailyChallengeCount()
    {
        int cnt = GetDailyChallengeCount();
        cnt += 1;
        Daily_Challenge_Count.Set(cnt);
        Is_Update_Data = true;
    }

    public void AddWinCount()
    {
        int cnt = GetDailyWinCount();
        cnt += 1;
        Daily_Win_Count.Set(cnt);

        Is_Update_Data = true;
    }
    

    /// <summary>
    /// 보스 스테이지 그룹 ID
    /// </summary>
    /// <returns></returns>
    public int GetBossStageGroupID()
    {
        if (Stage != null)
        {
            return Stage.boss_stage_group_id;
        }
        return 0;
    }

    /// <summary>
    /// 별 포인트가 3개 이상이어야만 스킵을 사용할 수 있다.
    /// </summary>
    /// <returns></returns>
    public bool IsEnableSkip()
    {
        return GetStarPoint() >= 3;
    }

    /// <summary>
    /// 일일 횟수 관련 데이터 초기화
    /// </summary>
    public void ResetDailyData()
    {
        Daily_Challenge_Count.Set(0);
        Daily_Win_Count.Set(0);
    }


    public override JsonData Serialized()
    {
        var json = new JsonData();

        json[NODE_BOSS_DUNGEON_ID] = Boss_Dungeon_ID;
        json[NODE_DAILY_CHALLENGE_COUNT] = GetDailyChallengeCount();
        json[NODE_DAILY_WIN_COUNT] = GetDailyWinCount();
        json[NODE_STAR_POINT] = GetStarPoint();

        return json;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }
        InitSecureVars();

        if (json.ContainsKey(NODE_BOSS_DUNGEON_ID))
        {
            Boss_Dungeon_ID = ParseInt(json, NODE_BOSS_DUNGEON_ID);
        }
        if (json.ContainsKey(NODE_DAILY_CHALLENGE_COUNT))
        {
            Daily_Challenge_Count.Set(ParseInt(json, NODE_DAILY_CHALLENGE_COUNT));
        }
        if (json.ContainsKey(NODE_DAILY_WIN_COUNT))
        {
            Daily_Win_Count.Set(ParseInt(json, NODE_DAILY_WIN_COUNT));
        }
        if (json.ContainsKey(NODE_STAR_POINT))
        {
            Star_Point.Set(ParseInt(json, NODE_STAR_POINT));
        }

        InitMasterData();
        
        return true;
    }
    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_BOSS_DUNGEON_ID = "bdid";
    protected const string NODE_DAILY_CHALLENGE_COUNT = "ccnt";
    protected const string NODE_DAILY_WIN_COUNT = "wcnt";
    protected const string NODE_STAR_POINT = "star";


}
