
using FluffyDuck.Util;
using LitJson;
/**/
public class UserStoryStageData : UserDataBase
{
    public int Stage_ID { get; protected set; } = 0;

    SecureVar<int> Challenage_Count = null;
    SecureVar<int> Win_Count = null;
    SecureVar<int> Star_Point = null;


    Stage_Data Data;
    World_Data World;
    Zone_Data Zone;

    public UserStoryStageData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Stage_ID = 0;
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
    }

    public void AddChallenageCount()
    {
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

    public bool IsStageCleared()
    {
        return GetWinCount() > 0;
    }

    
    public int GetStageGroupID()
    {
        //if (Zone != null)
        //{
        //    return Zone.stage_group_id;
        //}
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

    public override JsonData Serialized()
    {
        //if (!IsUpdateData())
        //{
        //    return null;
        //}
        var json = new LitJson.JsonData();

        json[NODE_STAGE_ID] = Stage_ID;
        json[NODE_CHALLENGE_COUNT] = GetChallengeCount();
        json[NODE_WIN_COUNT] = GetWinCount();
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
        }

        InitMasterData();

        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_STAGE_ID = "sid";
    protected const string NODE_CHALLENGE_COUNT = "ccnt";
    protected const string NODE_WIN_COUNT = "wcnt";
    protected const string NODE_STAR_POINT = "star";
}
