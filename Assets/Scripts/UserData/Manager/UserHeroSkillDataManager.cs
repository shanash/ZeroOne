using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHeroSkillDataManager : ManagerBase
{
    List<UserHeroSkillData> User_Hero_Skill_Data_List = new List<UserHeroSkillData>();
    public UserHeroSkillDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }

    protected override void Destroy()
    {
        int cnt = User_Hero_Skill_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            User_Hero_Skill_Data_List[i].Dispose();
        }
        User_Hero_Skill_Data_List.Clear();
    }

    public IReadOnlyList<UserHeroSkillData> GetUserHeroSkillDataList(int player_character_id, int player_character_num)
    {
        return User_Hero_Skill_Data_List.FindAll(x => x.GetPlayerCharacterID() == player_character_id && x.GetPlayerCharacterNum() == player_character_num);
    }

    public UserHeroSkillData FindUserHeroSkillData(int player_character_id, int player_character_num, int skill_group_id)
    {
        return User_Hero_Skill_Data_List.Find(x => x.GetPlayerCharacterID() == player_character_id && x.GetPlayerCharacterNum() == player_character_num && x.GetSkillGroupID() == skill_group_id);
    }

    public void AddUserHeroSkillGroups(UserHeroData hero, int skill_group_id)
    {
        var m = MasterDataManager.Instance;
        if (skill_group_id == 0)
        {
            return;
        }
        var skill_data = m.Get_PlayerCharacterSkillGroupData(skill_group_id);
        if (skill_data == null)
        {
            return;
        }
        //  기본 공격은 스킬 강화 불가
        if (skill_data.skill_type == SKILL_TYPE.NORMAL_ATTACK)
        {
            return;
        }
        AddUserHeroSkillData(hero, skill_data.pc_skill_group_id);
    }

    public UserHeroSkillData AddUserHeroSkillData(int player_character_id, int player_character_num, int skill_group_id)
    {
        UserHeroSkillData result = null;
        UserHeroData user_hero_data = null;

        // 유저 캐릭터 데이터를 찾아서
        var m = GameData.Instance.GetUserHeroDataManager();
        if (m != null)
        {
            user_hero_data = m.FindUserHeroData(player_character_id, player_character_num);
        }

        // 넣어서 만들어줍니다
        result = AddUserHeroSkillData(user_hero_data, skill_group_id);

        // 유저 캐릭터 데이터를 찾지 못했으면 pid와 pnum만 세팅해줍니다
        if (user_hero_data == null)
        {
            result.SetUserHeroNumOnly(player_character_id, player_character_num);
        }

        return result;
    }

    public UserHeroSkillData AddUserHeroSkillData(UserHeroData user_hero, int skill_group_id)
    {
        UserHeroSkillData result = null;

        // 스킬데이터를 찾습니다.
        if (user_hero != null)
        {
            result = FindUserHeroSkillData(user_hero.GetPlayerCharacterID(), user_hero.Player_Character_Num, skill_group_id);
        }

        // 못찾았으면 새로 만들어줍니다
        if (result == null)
        {
            if (skill_group_id == 100402 || skill_group_id == 100403 || skill_group_id == 100404)
            {
                Debug.Log($"Add {skill_group_id}");
            }
            result = new UserHeroSkillData(null, skill_group_id);
            User_Hero_Skill_Data_List.Add(result);
            Is_Update_Data = true;
        }

        // 제공된 유저캐릭터가 기존값이랑 다르면 넣어줍니다
        if (user_hero != null && result.Hero_Data != user_hero)
        {
            result.SetUserHero(user_hero);
            Is_Update_Data = true;
        }

        return result;
    }

    public override JsonData Serialized()
    {
        var json = base.Serialized();

        var arr = new JsonData();
        int cnt = User_Hero_Skill_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Hero_Skill_Data_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_HERO_SKILL_DATA_LIST] = arr;
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

        if (json.ContainsKey(NODE_HERO_SKILL_DATA_LIST))
        {
            var arr = json[NODE_HERO_SKILL_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];
                    if (int.TryParse(jdata[NODE_PLAYER_CHARACTER_ID].ToString(), out int player_character_id)
                        && int.TryParse(jdata[NODE_PLAYER_CHARACTER_NUM].ToString(), out int player_character_num)
                        && int.TryParse(jdata[NODE_HERO_SKILL_GROUP_ID].ToString(), out int skill_grp_id))
                    {
                        UserHeroSkillData item = AddUserHeroSkillData(player_character_id, player_character_num, skill_grp_id);
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
    protected const string NODE_HERO_SKILL_DATA_LIST = "slist";

    protected const string NODE_HERO_SKILL_GROUP_ID = "grpid";
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";

}
