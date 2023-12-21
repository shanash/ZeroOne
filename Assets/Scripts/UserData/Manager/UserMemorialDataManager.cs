


using LitJson;
using System.Collections.Generic;

public class UserMemorialDataManager : ManagerBase
{
    List<UserMemorialData> User_Memorial_Data_List = new List<UserMemorialData>();

    public UserMemorialDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype) { }

    protected override void Destroy()
    {
        int cnt = User_Memorial_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Memorial_Data_List[i].Dispose();
        }
        User_Memorial_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Memorial_Data_List.Count > 0)
        {
            return;
        }
        DummyDataSetting();
    }
    void DummyDataSetting()
    {
        var m = MasterDataManager.Instance;
        List<Me_Resource_Data> list = new List<Me_Resource_Data>();
        m.Get_MemorialResourceDataList(ref list);

        for (int i = 0;i < list.Count;i++)
        {
            var mdata = list[i];
            AddUserMemorialData(mdata.memorial_id, mdata.player_character_id);
        }

        Save();
    }

    public void GetMemorialDataList(ref List<UserMemorialData> list)
    {
        list.Clear();
        list.AddRange(User_Memorial_Data_List);
    }

    public List<UserMemorialData> FindMemorialDataListByPlayerID(int player_character_id)
    {
        return User_Memorial_Data_List.FindAll(x => x.Player_Character_ID == player_character_id);
    }

    public UserMemorialData FindUserMemorialData(int memorial_id, int player_character_id)
    {
        return User_Memorial_Data_List.Find(x => x.Memorial_ID == memorial_id && x.Player_Character_ID == player_character_id);
    }

    UserMemorialData AddUserMemorialData(int memorial_id, int player_character_id)
    {
        if (memorial_id == 0 || player_character_id == 0)
        {
            return null;
        }
        var memorial = FindUserMemorialData(memorial_id, player_character_id);
        if (memorial == null)
        {
            memorial = new UserMemorialData();
            memorial.SetMemorialDataID(memorial_id, player_character_id);
            User_Memorial_Data_List.Add(memorial);
            Is_Update_Data = true;
        }
        return memorial;
    }

    public override JsonData Serialized()
    {
        var json = new JsonData();

        var arr = new JsonData();
        int cnt = User_Memorial_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Memorial_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        json[NODE_MEMORIAL_DATA_LIST] = arr;

        return json;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null)
            return false;

        if (json.ContainsKey(NODE_MEMORIAL_DATA_LIST))
        {
            var arr = json[NODE_MEMORIAL_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];

                    int memorial_id = 0;
                    int player_character_id = 0;
                    if (int.TryParse(jdata[NODE_MEMORIAL_ID].ToString(), out memorial_id) && int.TryParse(jdata[NODE_PLAYER_CHARACTER_ID].ToString(), out player_character_id))
                    {
                        UserMemorialData item = FindUserMemorialData(memorial_id, player_character_id);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserMemorialData(memorial_id, player_character_id);
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
    protected const string NODE_MEMORIAL_DATA_LIST = "mlist";
    protected const string NODE_MEMORIAL_ID = "mid";
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";

}
