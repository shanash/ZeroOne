using LitJson;
using System.Collections.Generic;
using System.Linq;

public class UserStoryStageDataManager : ManagerBase
{
    int Current_Zone_ID;

    List<UserStoryStageData> User_Story_Stage_Data = new List<UserStoryStageData>();
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

        if (Current_Zone_ID != 0)
        {
            SetCurrentZoneID(Current_Zone_ID);
        }

        if (User_Story_Stage_Data.Count > 0)
        {
            return;
        }

        //  first stage init

        List<World_Data> world_list = new List<World_Data>();
        m.Get_WorldDataList(ref world_list);
        if (world_list.Count == 0)
            return;
        var first_world = world_list[0];
        List<Zone_Data> zone_list = new List<Zone_Data>();
        m.Get_ZoneDataList(first_world.world_id, STAGE_DIFFICULTY_TYPE.NORMAL, ref zone_list);
        if (zone_list.Count == 0)
        {
            return;
        }
        var first_zone = zone_list[0];
        Current_Zone = first_zone;
        Current_Zone_ID = first_zone.zone_id;

        //List<Stage_Data> stage_list = new List<Stage_Data>();
        //m.Get_StageDataList(Current_Zone_ID, ref stage_list);

        var stage_list = m.Get_StageDataListByZoneID(Current_Zone_ID);

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

    public STAGE_DIFFICULTY_TYPE GetCurrentStageDifficultyType()
    {
        if (Current_Zone != null)
        {
            return Current_Zone.zone_difficulty;
        }
        return STAGE_DIFFICULTY_TYPE.NONE;
    }

    public int GetTotalStarCount(int zone_id)
    {
        var stage_list = MasterDataManager.Instance.Get_StageDataListByZoneID(zone_id);
        int sum = stage_list.Count * 3;
        return sum;
    }

    public int GetGainStarPoints(int zone_id)
    {
        var list = FindUserStoryStageDataList(zone_id);
        int sum = User_Story_Stage_Data.Sum(x => x.GetStarPoint());
        return sum;
    }


    public int GetCurrentZoneID()
    {
        return Current_Zone_ID;
    }

    public Zone_Data GetCurrentZoneData()
    {
        return Current_Zone;
    }

    public List<UserStoryStageData> FindUserStoryStageDataList(int zone_id)
    {
        return User_Story_Stage_Data.FindAll(x => x.GetZoneID() == zone_id);
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
    public ERROR_CODE StoryStageWin(int stage_id)
    {
        var stage = FindUserStoryStageData(stage_id);
        if (stage == null)
        {
            return ERROR_CODE.FAILED;
        }
        stage.AddWinCount();

        OpenNextStage(stage_id);

        return ERROR_CODE.SUCCESS;
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
        SetCurrentZoneID(next_stage.zone_id);
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
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserStoryStageData(stage_id);
                            item?.Deserialized(jdata);
                        }
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
    protected const string NODE_CURRENT_ZONE_ID = "zid";
    protected const string NODE_STAGE_DATA_LIST = "slist";

    protected const string NODE_STAGE_ID = "sid";

}
