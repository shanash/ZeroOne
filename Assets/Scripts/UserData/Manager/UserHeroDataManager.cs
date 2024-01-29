using LitJson;
using System.Collections.Generic;

public class UserHeroDataManager : ManagerBase
{

    List<UserHeroData> User_Hero_Data_List = new List<UserHeroData>();

    public UserHeroDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {
        int cnt = User_Hero_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Hero_Data_List[i].Dispose();
        }
        User_Hero_Data_List.Clear();
    }

    public override void InitDataManager()
    {
        if (User_Hero_Data_List.Count > 0)
        {
            return;
        }
        DummyDataSettting();
    }

    void DummyDataSettting()
    {
        var m = MasterDataManager.Instance;
        List<Player_Character_Data> pc_data_list = new List<Player_Character_Data>();
        m.Get_PlayerCharacterDataList(ref pc_data_list);
        int hero_data_num = 1;
        int cnt = pc_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var pdata = pc_data_list[i];
            AddUserHeroData(pdata.player_character_id, hero_data_num++);
        }

        Save();
    }

    public void GetUserHeroDataList(ref List<UserHeroData> list)
    {
        list.Clear();
        list.AddRange(User_Hero_Data_List);
    }

    public IReadOnlyList<UserHeroData> GetUserHeroDataList()
    {
        return User_Hero_Data_List;
    }

    /// <summary>
    /// 사용자 캐릭터 데이터 찾기(덱 정보롤 찾기) - 아직 사용하지 않음
    /// </summary>
    /// <param name="h"></param>
    /// <returns></returns>
    public UserHeroData FindUserHeroData(UserHeroDeckMountData h)
    {
        return User_Hero_Data_List.Find(x => x.GetPlayerCharacterID() == h.Player_Character_ID && x.Player_Character_Num == h.Player_Character_Num); ;
    }
    /// <summary>
    /// 사용자 캐릭터 데이터 찾기(id/num 으로 찾기)
    /// </summary>
    /// <param name="hero_data_id"></param>
    /// <param name="hero_data_num"></param>
    /// <returns></returns>
    public UserHeroData FindUserHeroData(int hero_data_id, int hero_data_num)
    {
        return User_Hero_Data_List.Find(x => x.GetPlayerCharacterID() == hero_data_id && x.Player_Character_Num == hero_data_num);
    }
    /// <summary>
    /// 사용자 캐릭터 데이터 추가
    /// </summary>
    /// <param name="hero_data_id"></param>
    /// <param name="hero_data_num"></param>
    /// <returns></returns>
    UserHeroData AddUserHeroData(int hero_data_id, int hero_data_num)
    {
        if (hero_data_id == 0)
        {
            return null;
        }
        var hero = FindUserHeroData(hero_data_id, hero_data_num);
        if (hero == null)
        {
            hero = new UserHeroData();
            hero.SetPlayerCharacterDataID(hero_data_id, hero_data_num);
            User_Hero_Data_List.Add(hero);
            Is_Update_Data = true;

            //  hero skill data init
            var skill_mng = GameData.Instance.GetUserHeroSkillDataManager();
            skill_mng?.AddUserHeroSkillGroups(hero, hero.GetPlayerCharacterBattleData().skill_pattern);
        }

        return hero;
    }


    public override LitJson.JsonData Serialized()
    {
        var json = new LitJson.JsonData();

        var arr = new LitJson.JsonData();

        int cnt = User_Hero_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Hero_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_HERO_DATA_LIST] = arr;
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
        if (json.ContainsKey(NODE_HERO_DATA_LIST))
        {
            var arr = json[NODE_HERO_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];

                    int player_character_id = 0;
                    int player_character_num = 0;
                    if (int.TryParse(jdata[NODE_PLAYER_CHARACTER_ID].ToString(), out player_character_id) && int.TryParse(jdata[NODE_PLAYER_CHARACTER_NUM].ToString(), out player_character_num))
                    {
                        UserHeroData item = FindUserHeroData(player_character_id, player_character_num);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserHeroData(player_character_id, player_character_num);
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
    protected const string NODE_HERO_DATA_LIST = "hlist";

    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";
}
