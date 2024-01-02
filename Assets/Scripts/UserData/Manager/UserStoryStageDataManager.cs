using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStoryStageDataManager : ManagerBase
{
    STAGE_DIFFICULTY_TYPE Current_Difficulty_Type = STAGE_DIFFICULTY_TYPE.NORMAL;

    List<UserStoryStageData> User_Story_Stage_Data = new List<UserStoryStageData>();

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
        if (User_Story_Stage_Data.Count > 0)
        {
            return;
        }

        Current_Difficulty_Type = STAGE_DIFFICULTY_TYPE.NORMAL;
        //  first stage init
        var m = MasterDataManager.Instance;
        List<World_Data> world_list = new List<World_Data>();
        m.Get_WorldDataList(ref world_list);
        if (world_list.Count == 0)
            return;
        var first_world = world_list[0];
        List<Zone_Data> zone_list = new List<Zone_Data>();
        m.Get_ZoneDataList(first_world.world_id, Current_Difficulty_Type, ref zone_list);
        if (zone_list.Count == 0)
        {
            return;
        }
        var first_zone = zone_list[0];

        List<Stage_Data> stage_list = new List<Stage_Data>();
        m.Get_StageDataList(first_zone.zone_id, ref stage_list);

        if (stage_list.Count == 0)
            return;

        var first_stage = stage_list[0];
        AddUserStoryStageData(first_stage.stage_id);

        Save();
    }

    public STAGE_DIFFICULTY_TYPE GetCurrentStageDifficultyType()
    {
        return Current_Difficulty_Type;
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

    public override JsonData Serialized()
    {
        var json = new LitJson.JsonData();
        json[NODE_CURRENT_DIFFICULTY_TYPE] = (int)Current_Difficulty_Type;

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
        json[NODE_STAGE_DATA_LIST] = arr;

        return json;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }

        if (json.ContainsKey(NODE_CURRENT_DIFFICULTY_TYPE))
        {
            Current_Difficulty_Type = (STAGE_DIFFICULTY_TYPE)ParseInt(json, NODE_CURRENT_DIFFICULTY_TYPE);
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
    protected const string NODE_CURRENT_DIFFICULTY_TYPE = "dtype";
    protected const string NODE_STAGE_DATA_LIST = "slist";

    protected const string NODE_STAGE_ID = "sid";

}
