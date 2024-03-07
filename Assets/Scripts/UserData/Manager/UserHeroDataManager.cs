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
        var pc_data_list = m.Get_PlayerCharacterDataListByFirstOpen();
        int cnt = pc_data_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var pdata = pc_data_list[i];
            var hero = AddUserHeroData(pdata.player_character_id);
            AddUserHeroSkillData(hero);
        }

        Save();
        GameData.Instance.GetUserHeroSkillDataManager().Save();
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

    public UserHeroData FindUserHeroData(int hero_data_id)
    {
        return User_Hero_Data_List.Find(x => x.GetPlayerCharacterID() == hero_data_id);
    }

    public void AddUserHeroSkillData(UserHeroData hero)
    {
        var skill_mng = GameData.Instance.GetUserHeroSkillDataManager();
        if (skill_mng != null)
        {
            var battle_data = hero.GetPlayerCharacterBattleData();
            int cnt = battle_data.skill_pattern.Length;
            //  normal skill
            for (int i = 0; i < cnt; i++)
            {
                skill_mng.AddUserHeroSkillGroups(hero, battle_data.skill_pattern[i]);
            }

            //  special skill
            skill_mng.AddUserHeroSkillGroups(hero, battle_data.special_skill_group_id);
        }
    }

    /// <summary>
    /// 사용자 캐릭터 데이터 추가하기
    /// </summary>
    /// <param name="hero_data_id"></param>
    /// <param name="hero_data_num"></param>
    /// <returns></returns>
    public UserHeroData AddUserHeroData(int hero_data_id)
    {
        var hero = FindUserHeroData(hero_data_id);
        if (hero == null)
        {
            hero = new UserHeroData(hero_data_id, User_Hero_Data_List.Count + 1);
            User_Hero_Data_List.Add(hero);
            Is_Update_Data = true;
        }
        return hero;
    }

    public override JsonData Serialized()
    {
        var json = base.Serialized();

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
        if (!base.Deserialized(json))
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
                            item = AddUserHeroData(player_character_id);
                            item.Deserialized(jdata);
                            AddUserHeroSkillData(item);
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
