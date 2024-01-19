


using LitJson;
using System.Collections.Generic;
using System.Linq;

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

        for (int i = 0; i < list.Count; i++)
        {
            var mdata = list[i];
            AddUserMemorialData(mdata.memorial_id, mdata.player_character_id);
        }

        Save();
    }

    public void GetUserMemorialDataList(ref List<UserMemorialData> list)
    {
        list.Clear();
        list.AddRange(User_Memorial_Data_List);
    }
    /// <summary>
    /// 선택 확정되어 있는 메모리얼 리스트 반환
    /// </summary>
    /// <returns></returns>
    public List<UserMemorialData> GetUserMemorialDataListByChoice()
    {
        var list = User_Memorial_Data_List.FindAll(x => x.Is_Choice_Lobby);
        list.Sort((a, b) => a.Lobby_Choice_Number.CompareTo(b.Lobby_Choice_Number));
        return list;
    }

    /// <summary>
    /// 임시 선택된 메모리얼 리스트 반환
    /// </summary>
    /// <returns></returns>
    public List<UserMemorialData> GetUserMemorialDataListbyTempChoice()
    {
        var list = User_Memorial_Data_List.FindAll(x => x.Is_Temp_Choice);
        list.Sort((a, b) => a.Temp_Lobby_Choice_Number.CompareTo(b.Temp_Lobby_Choice_Number));
        return list;
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

    public ERROR_CODE SelectTempMemorialOrder(int memorial_id, int player_character_id)
    {
        var found = FindUserMemorialData(memorial_id, player_character_id);
        if (found != null)
        {
            //  이미 선택된 상태라면, 해제해준다.
            if (found.Is_Temp_Choice)
            {
                //  해제
                found.ResetTempLobbyChoice();

                //  순서를 다시 지정해준다.
                var choice_list = GetUserMemorialDataListbyTempChoice();
                int number = 1;
                for (int i = 0; i < choice_list.Count; i++)
                {
                    var item = choice_list[i];
                    item.SetTempLobbyChoiceNumber(number++);
                }
                return ERROR_CODE.SUCCESS;
            }
            else
            {
                var choice_list = GetUserMemorialDataListbyTempChoice();
                if (choice_list.Count == 0)
                {
                    found.SetTempLobbyChoiceNumber(1);
                    return ERROR_CODE.SUCCESS;
                }
                else
                {
                    //  아직 최대선택수보다 작다면 
                    if (choice_list.Count < GameDefine.MAX_LOBBY_CHARACTER_COUNT)
                    {
                        //  기존에 등록된 번호 중 최대 번호를 찾아, 다음 번호를 찾아준다.
                        int next_number = choice_list.Max(x => x.Temp_Lobby_Choice_Number) + 1;
                        found.SetTempLobbyChoiceNumber(next_number);
                        return ERROR_CODE.SUCCESS;
                    }
                    else
                    {
                        return ERROR_CODE.NOT_WORK;
                    }
                }

            }
        }
        return ERROR_CODE.FAILED;
    }

    /// <summary>
    /// 팝업 진입시 Temp Number에 Choice Number 값을 임시로 세팅해준다.
    /// </summary>
    public void ReadyTempLobbyChoiceNumber()
    {
        int cnt = User_Memorial_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Memorial_Data_List[i];
            item.SetTempLobbyChoiceNumber(item.Lobby_Choice_Number);
        }
    }

    /// <summary>
    /// 임시로 저장된 로비 순서 되돌림
    /// </summary>
    public void RollbackLobbyChoiceOrder()
    {
        User_Memorial_Data_List.ForEach(x => x.ResetTempLobbyChoice());
    }

    /// <summary>
    /// 임시로 저장된 로비 순서 적용하기
    /// </summary>
    public void ConfirmLobbyChoiceOrder()
    {
        int cnt = User_Memorial_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Memorial_Data_List[i];
            item.SetLobbyChoiceNumber(item.Temp_Lobby_Choice_Number);
            item.ResetTempLobbyChoice();
        }

        Save();
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
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_MEMORIAL_DATA_LIST] = arr;
        }
        if(json.Keys.Count > 0)
        {
            return json;
        }
        return null;
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
