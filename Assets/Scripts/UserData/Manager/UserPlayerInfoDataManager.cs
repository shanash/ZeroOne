using LitJson;
using System.Collections.Generic;

public class UserPlayerInfoDataManager : ManagerBase
{
    List<UserPlayerInfoData> User_Player_Info_Data_List = new List<UserPlayerInfoData>();
    public UserPlayerInfoDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
        
    }

    protected override void Destroy()
    {
        int cnt = User_Player_Info_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Player_Info_Data_List[i].Dispose();
        }
        User_Player_Info_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Player_Info_Data_List.Count > 0)
        {
            return;
        }
        AddUserPlayerInfo();
        Save();
    }

    public UserPlayerInfoData GetCurrentPlayerInfoData()
    {
        if (User_Player_Info_Data_List.Count > 0)
        {
            return User_Player_Info_Data_List[0];
        }
        var player_info = AddUserPlayerInfo();
        return player_info;
    }

    UserPlayerInfoData AddUserPlayerInfo()
    {
        var info = new UserPlayerInfoData();
        User_Player_Info_Data_List.Add(info);
        return info;
    }

    /// <summary>
    /// 플레이어 경험치 추가
    /// </summary>
    /// <param name="xp"></param>
    /// <returns></returns>
    public RESPONSE_TYPE AddPlayerExp(double xp)
    {
        var info = GetCurrentPlayerInfoData();
        if (info != null)
        {
            return info.AddExp(xp);
        }
        return RESPONSE_TYPE.NOT_WORK;
    }


    public override JsonData Serialized()
    {
        var json = new JsonData();

        var arr = new JsonData();
        int cnt = User_Player_Info_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Player_Info_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_PLAYER_INFO_DATA_LIST] = arr;
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

        if (json.ContainsKey(NODE_PLAYER_INFO_DATA_LIST))
        {
            var arr = json[NODE_PLAYER_INFO_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];
                    UserPlayerInfoData item = GetCurrentPlayerInfoData();
                    if (item != null)
                    {
                        item.Deserialized(jdata);
                    }
                    else
                    {
                        item = AddUserPlayerInfo();
                        item.Deserialized(jdata);
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
    protected const string NODE_PLAYER_INFO_DATA_LIST = "plist";
}
