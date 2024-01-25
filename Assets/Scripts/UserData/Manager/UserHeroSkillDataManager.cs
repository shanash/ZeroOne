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

    public void AddUserHeroSkillGroups(UserHeroData hero, int[] skill_pattern)
    {
        var m = MasterDataManager.Instance;
        int cnt = skill_pattern.Length;
        for (int i = 0; i < cnt; i++)
        {
            int skill_group_id = skill_pattern[i];
            if (skill_group_id == 0)
            {
                continue;
            }
            var skill_data = m.Get_PlayerCharacterSkillGroupData(skill_group_id);
            if (skill_data == null)
            {
                continue;
            }
            //  기본 공격은 스킬 강화 불가
            if (skill_data.skill_type == SKILL_TYPE.NORMAL_ATTACK)
            {
                continue;
            }
            AddUserHeroSkillData(hero, skill_data.pc_skill_group_id);
        }
    }
    

    public UserHeroSkillData AddUserHeroSkillData(UserHeroData user_hero, int skill_group_id)
    {
        var skill = FindUserHeroSkillData(user_hero.GetPlayerCharacterID(), user_hero.Player_Character_Num, skill_group_id);
        if (skill == null)
        {
            skill = new UserHeroSkillData();
            skill.SetSkillGroupID(user_hero, skill_group_id);
            User_Hero_Skill_Data_List.Add(skill);
            Is_Update_Data = true;
        }

        return skill;
    }

    public UserHeroSkillData AddUserHeroSkillData(int player_character_id, int player_character_num, int skill_group_id)
    {
        var skill = FindUserHeroSkillData(player_character_id, player_character_num, skill_group_id);
        if (skill == null)
        {
            skill = new UserHeroSkillData();
            skill.SetSkillGroupID(player_character_id, player_character_num, skill_group_id);
            User_Hero_Skill_Data_List.Add(skill);
            Is_Update_Data = true;
        }
        return skill;
    }

    public override JsonData Serialized()
    {
        var json = new JsonData();

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
        if (json == null) return false;

        if (json.ContainsKey(NODE_HERO_SKILL_DATA_LIST))
        {
            var arr = json[NODE_HERO_SKILL_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];
                    int player_character_id = 0;
                    int player_character_num = 0;
                    int skill_grp_id = 0;
                    if (int.TryParse(jdata[NODE_PLAYER_CHARACTER_ID].ToString(), out player_character_id) && int.TryParse(jdata[NODE_PLAYER_CHARACTER_NUM].ToString(), out player_character_num) && int.TryParse(jdata[NODE_HERO_SKILL_GROUP_ID].ToString(), out skill_grp_id))
                    {
                        UserHeroSkillData item = FindUserHeroSkillData(player_character_id, player_character_num, skill_grp_id);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddUserHeroSkillData(player_character_id, player_character_num, skill_grp_id);
                            item.Deserialized(jdata);
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
    protected const string NODE_HERO_SKILL_DATA_LIST = "slist";

    protected const string NODE_HERO_SKILL_GROUP_ID = "grpid";
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";

}
