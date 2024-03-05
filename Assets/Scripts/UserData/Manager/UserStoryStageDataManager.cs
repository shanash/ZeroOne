using LitJson;
using System.Collections.Generic;
using System.Linq;

public class UserStoryStageDataManager : ManagerBase
{
    int Current_World_ID;
    int Current_Zone_ID;

    List<UserStoryStageData> User_Story_Stage_Data = new List<UserStoryStageData>();
    World_Data Current_World;
    Zone_Data Current_Zone;

    public UserStoryStageDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {
        int cnt = User_Story_Stage_Data.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Story_Stage_Data[i].Dispose();
        }
        User_Story_Stage_Data.Clear();
    }

    public override void InitDataManager()
    {
        var m = MasterDataManager.Instance;

        if (Current_World_ID != 0)
        {
            SetCurrentWorldID(Current_World_ID);
        }
        if (Current_Zone_ID != 0)
        {
            SetCurrentZoneID(Current_Zone_ID);
        }

        if (User_Story_Stage_Data.Count > 0)
        {
            return;
        }

        //  first stage init
        var world_list = m.Get_WorldDataList();
        if (world_list.Count == 0)
            return;
        Current_World = world_list[0];
        Current_World_ID = Current_World.world_id;

        var zone_list = m.Get_ZoneDataList(Current_World.zone_group_id, STAGE_DIFFICULTY_TYPE.NORMAL);
        
        if (zone_list.Count == 0)
        {
            return;
        }
        var first_zone = zone_list[0];
        Current_Zone = first_zone;
        Current_Zone_ID = Current_Zone.zone_id;

        var stage_list = m.Get_StageDataListByStageGroupID(Current_Zone.stage_group_id);

        if (stage_list.Count == 0)
            return;

        var first_stage = stage_list[0];
        AddUserStoryStageData(first_stage.stage_id);

        Save();
    }

    public void SetCurrentZoneID(int zone_id)
    {
        Current_Zone_ID = zone_id;
        Current_Zone = MasterDataManager.Instance.Get_ZoneData(Current_Zone_ID);
        Is_Update_Data = true;
    }


    public void SetCurrentWorldID(int world_id)
    {
        Current_World_ID = world_id;
        Current_World = MasterDataManager.Instance.Get_WorldData(Current_World_ID);
        Is_Update_Data = true;
    }
    /// <summary>
    /// 이전 존이 존재하는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsOpenPrevZone()
    {
        var m = MasterDataManager.Instance;
        var zone_list = m.Get_ZoneDataListByDifficulty(GetCurrentStageDifficultyType());
        if (zone_list.Count > 0)
        {
            var find_zone = zone_list.FindLast(x => x.zone_id < Current_Zone.zone_id);
            if (find_zone != null)
            {
                return User_Story_Stage_Data.Exists(x => x.GetStageGroupID() == find_zone.stage_group_id);
            }
        }
        return false;
    }
    /// <summary>
    /// 다음 존이 오픈되어 있는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsOpenNextZone()
    {
        var m = MasterDataManager.Instance;
        var zone_list = m.Get_ZoneDataListByDifficulty(GetCurrentStageDifficultyType());
        if (zone_list.Count > 0)
        {
            var find_zone = zone_list.Find(x => x.zone_id > Current_Zone.zone_id);
            if (find_zone != null)
            {
                return User_Story_Stage_Data.Exists(x => x.GetStageGroupID() == find_zone.stage_group_id);
            }
        }
        return false;
    }

    /// <summary>
    /// 이전 존으로 이동
    /// </summary>
    /// <returns></returns>
    public bool MovePrevZone()
    {
        if (!IsOpenPrevZone())
        {
            return false;
        }
        var m = MasterDataManager.Instance;
        var zone_list = m.Get_ZoneDataListByDifficulty(GetCurrentStageDifficultyType());
        if (zone_list.Count > 0)
        {
            var find_zone = zone_list.FindLast(x => x.zone_id < Current_Zone.zone_id);
            SetCurrentZoneID(find_zone.zone_id);
            return true;
        }
        return false;
    }

    public bool MoveNextZone()
    {
        if (!IsOpenNextZone())
        {
            return false;
        }
        var m = MasterDataManager.Instance;
        var zone_list = m.Get_ZoneDataListByDifficulty(GetCurrentStageDifficultyType());
        if (zone_list.Count > 0)
        {
            var find_zone = zone_list.Find(x => x.zone_id > Current_Zone.zone_id);
            SetCurrentZoneID(find_zone.zone_id);
            return true;
        }
        return false;
    }
    

    public STAGE_DIFFICULTY_TYPE GetCurrentStageDifficultyType()
    {
        if (Current_Zone != null)
        {
            return Current_Zone.zone_difficulty;
        }
        return STAGE_DIFFICULTY_TYPE.NONE;
    }

    public int GetTotalStarCount(int stage_group_id)
    {
        var stage_list = MasterDataManager.Instance.Get_StageDataListByStageGroupID(stage_group_id);
        int sum = stage_list.Count * 3;
        return sum;
    }

    public int GetGainStarPoints(int stage_group_id)
    {
        var list = FindUserStoryStageDataList(stage_group_id);
        int sum = User_Story_Stage_Data.Sum(x => x.GetStarPoint());
        return sum;
    }


    public int GetCurrentZoneID()
    {
        return Current_Zone_ID;
    }

    public int GetCurrentWorldID()
    {
        return Current_World_ID;
    }

    public Zone_Data GetCurrentZoneData()
    {
        return Current_Zone;
    }
    public World_Data GetCurrentWorldData()
    {
        return Current_World;
    }

    public List<UserStoryStageData> FindUserStoryStageDataList(int stage_group_id)
    {
        return User_Story_Stage_Data.FindAll(x => x.GetStageGroupID() == stage_group_id);
    }

    public UserStoryStageData FindUserStoryStageData(int stage_id)
    {
        return User_Story_Stage_Data.Find(x => x.Stage_ID == stage_id);
    }

    UserStoryStageData AddUserStoryStageData(int stage_id)
    {
        if (stage_id == 0)
        {
            return null;
        }
        var stage = FindUserStoryStageData(stage_id);
        if (stage == null)
        {
            stage = new UserStoryStageData();
            stage.SetStageID(stage_id);
            User_Story_Stage_Data.Add(stage);
            Is_Update_Data = true;
        }
        return stage;
    }

    public void SetStageStarPoint(int stage_id, int star_pt)
    {
        var stage = FindUserStoryStageData(stage_id);
        if (stage != null)
        {
            stage.SetStarPoint(star_pt);
        }
    }

    /// <summary>
    /// 스테이지 클리어 요청.<br/>
    /// 스테이지 클리어시 다음 스테이지를 오픈해주는 역할 필요.<br/>
    /// 마지막 스테이지인 경우(다음 스테이지가 없을 경우) 게임데이터 로딩(초기화)시 마지막 스테이지 체크 후 다음 스테이지 오픈 여부 판단 필요<br/>
    /// </summary>
    /// <param name="stage_id"></param>
    public RESPONSE_TYPE StoryStageWin(int stage_id)
    {
        var stage = FindUserStoryStageData(stage_id);
        if (stage == null)
        {
            return RESPONSE_TYPE.FAILED;
        }
        stage.AddWinCount();

        OpenNextStage(stage_id);

        return RESPONSE_TYPE.SUCCESS;
    }

    /// <summary>
    /// 다음 스테이지 오픈
    /// </summary>
    /// <param name="stage_id"></param>
    void OpenNextStage(int stage_id)
    {
        var m = MasterDataManager.Instance;
        var next_stage = m.Get_NextStageData(stage_id);
        if (next_stage == null)
        {
            return;
        }
        AddUserStoryStageData(next_stage.stage_id);
        var zone = m.Get_ZoneDataByStageGroupID(next_stage.stage_group_id);
        SetCurrentZoneID(zone.zone_id);
        var world = m.Get_WorldDataByZoneGroupID(zone.zone_group_id);
        SetCurrentWorldID(world.world_id);
    }

    /// <summary>
    /// 오픈되어 있는 마지막 스테이지 정보 반환
    /// </summary>
    /// <returns></returns>
    public UserStoryStageData GetLastOpenStage()
    {
        User_Story_Stage_Data.Sort((a, b) => a.Stage_ID.CompareTo(b.Stage_ID));
        return User_Story_Stage_Data.Last();
    }


    public override JsonData Serialized()
    {
        var json = new LitJson.JsonData();
        json[NODE_CURRENT_WORLD_ID] = Current_World_ID;
        json[NODE_CURRENT_ZONE_ID] = Current_Zone_ID;

        var arr = new JsonData();

        int cnt = User_Story_Stage_Data.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Story_Stage_Data[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_STAGE_DATA_LIST] = arr;
        }

        if (json.Keys.Count > 0)
        {
            return json;
        }
        return null;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }
        if (json.ContainsKey(NODE_CURRENT_WORLD_ID))
        {
            int world_id = ParseInt(json, NODE_CURRENT_WORLD_ID);
            SetCurrentWorldID(world_id);
        }
        if (json.ContainsKey(NODE_CURRENT_ZONE_ID))
        {
            int zone_id = ParseInt(json, NODE_CURRENT_ZONE_ID);
            SetCurrentZoneID(zone_id);
        }


        if (json.ContainsKey(NODE_STAGE_DATA_LIST))
        {
            var arr = json[NODE_STAGE_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];

                    int stage_id = 0;
                    if (int.TryParse(jdata[NODE_STAGE_ID].ToString(), out stage_id))
                    {
                        UserStoryStageData item = FindUserStoryStageData(stage_id);
                        
                        if (item == null)
                        {
                            item = AddUserStoryStageData(stage_id);
                        }
                        item?.Deserialized(jdata);
                    }
                }
            }
        }

        InitUpdateData();

        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_CURRENT_WORLD_ID = "wid";
    protected const string NODE_CURRENT_ZONE_ID = "zid";
    protected const string NODE_STAGE_DATA_LIST = "slist";

    protected const string NODE_STAGE_ID = "sid";

}
