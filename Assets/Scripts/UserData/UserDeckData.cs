

using LitJson;
using System;
using System.Collections.Generic;

public class UserDeckData : UserDataBase
{
    public int Deck_Number { get; protected set; } = 0;

    public GAME_TYPE Game_Type { get; protected set; } = 0;   //  차후 enum 으로 정의 해야 함

    public bool Is_Selected { get; protected set; } = false;

    public string Team_Name { get; protected set; } = string.Empty;

    List<UserHeroDeckMountData> Deck_Heroes = new List<UserHeroDeckMountData>();

    List<Attribute_Synergy_Data> Team_Synergy_Data_List = new List<Attribute_Synergy_Data>();

    public UserDeckData() : base() { }

    protected override void Reset()
    {
        Deck_Number = 0;
        Game_Type = GAME_TYPE.NONE;
        ClearDeckHeroes();
    }

    protected override void Destroy()
    {
        ClearDeckHeroes();
    }

    /// <summary>
    /// 현재 덱 그룹의 영웅 정보를 모두 삭제
    /// </summary>
    void ClearDeckHeroes()
    {
        int cnt = Deck_Heroes.Count;
        for (int i = 0; i < cnt; i++)
        {
            Deck_Heroes[i].Dispose();
        }
        Deck_Heroes.Clear();
    }

    public void SetDeckNumber(int deck)
    {
        Deck_Number = deck;
    }
    public void SetGameType(GAME_TYPE game_type)
    {
        this.Game_Type = game_type;
    }

    /// <summary>
    /// 팀 이름 설정 (덱에 이름을 설정할 필요가 있을 경우)
    /// </summary>
    /// <param name="team_name"></param>
    public void ChangeTeamName(string team_name)
    {
        if (string.IsNullOrEmpty(team_name))
        {
            return;
        }
        this.Team_Name = team_name;
        Is_Update_Data = true;
    }

    /// <summary>
    /// 마운트되어 있는 영웅 찾기
    /// </summary>
    /// <param name="player_character_id"></param>
    /// <param name="player_character_num"></param>
    /// <returns></returns>
    public UserHeroDeckMountData FindUserHeroDeckMountData(int player_character_id, int player_character_num)
    {
        return Deck_Heroes.Find(x => x.Player_Character_ID == player_character_id && x.Player_Character_Num == player_character_num);
    }

    /// <summary>
    /// 덱 영웅 변경
    /// </summary>
    /// <param name="new_hero"></param>
    /// <returns></returns>
    public ERROR_CODE AddSlotHero(UserHeroDeckMountData new_hero)
    {
        if (!IsExistEmptySlot())
        {
            return ERROR_CODE.NOT_EXIST_EMPTY_SLOT;
        }
        //  현재 리더가 없으면 신규 착용되는 영웅을 리더로 설정한다.
        bool is_leader = false;
        if (!IsExistLeader())
        {
            is_leader = true;
        }
        //  변경할 영웅이 이미 덱에 포함되어 있는 영웅인지 체크
        var found_hero = Deck_Heroes.Find(x => x.Player_Character_ID == new_hero.Player_Character_ID && x.Player_Character_Num == new_hero.Player_Character_Num);
        if (found_hero != null)
        {
            //  이미 포함되어 있던 영웅 정보 제거
            Deck_Heroes.Remove(found_hero);
            //  새로운 영웅 정보로 교체
            Deck_Heroes.Add(new_hero);
            Is_Update_Data = true;
        }
        else // 변경할 영웅이 덱에 포함되어 있지 않을 경우
        {
            //  별도의 슬롯 번호가 없기 때문에 슬롯 번호를 사용하지 않는 덱 구성으로 변경 필요
            Deck_Heroes.Add(new_hero);
            Is_Update_Data = true;
        }

        new_hero.SetLeader(is_leader);
        CalcTeamSynergy();

        return ERROR_CODE.SUCCESS;
    }

    public ERROR_CODE AddSlotHero(int player_character_id, int player_character_num)
    {
        var mount_hero = new UserHeroDeckMountData();
        mount_hero.SetUserHeroData(player_character_id, player_character_num);

        return AddSlotHero(mount_hero);
    }

    /// <summary>
    /// 영웅 덱 정보를 이용하여 덱에서 영웅 제거
    /// </summary>
    /// <param name="hero"></param>
    public void RemoveHero(UserHeroDeckMountData hero)
    {
        var found = Deck_Heroes.Find(x => x.Player_Character_ID == hero.Player_Character_ID && x.Player_Character_Num == hero.Player_Character_Num);
        if (found != null)
        {
            Deck_Heroes.Remove(found);
            if (found.Is_Leader && Deck_Heroes.Count > 0)
            {
                Deck_Heroes[0].SetLeader(true);
            }
            CalcTeamSynergy();
            Is_Update_Data = true;
        }
    }

    /// <summary>
    /// 영웅 정보만을 조회하여 덱에서 제거
    /// </summary>
    /// <param name="hero"></param>
    /// <returns></returns>
    public void RemoveHero(UserHeroData hero)
    {
        var found = Deck_Heroes.Find(x => x.Player_Character_ID == hero.GetPlayerCharacterID() && x.Player_Character_Num == hero.Player_Character_Num);
        if (found != null)
        {
            Deck_Heroes.Remove(found);
            if (found.Is_Leader)
            {
                if (found.Is_Leader && Deck_Heroes.Count > 0)
                {
                    Deck_Heroes[0].SetLeader(true);
                }
            }
            CalcTeamSynergy();
            Is_Update_Data = true;
        }
    }


    /// <summary>
    /// 해당 영웅이 현재 덱에 구성되어 있는지 여부 체크
    /// </summary>
    /// <param name="hero_data_id"></param>
    /// <param name="hero_data_num"></param>
    /// <returns></returns>
    public bool IsExistHeroInDeck(int hero_data_id, int hero_data_num)
    {
        return Deck_Heroes.Exists(x => x.Player_Character_ID == hero_data_id && x.Player_Character_Num == hero_data_num);
    }
    public bool IsExistHeroInDeck(UserHeroData hero)
    {
        if (hero == null)
        {
            return false;
        }
        return IsExistHeroInDeck(hero.GetPlayerCharacterID(), hero.Player_Character_Num);
    }

    /// <summary>
    /// 해당 덱에 리더가 존재하고 있는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsExistLeader()
    {
        return Deck_Heroes.Exists(x => x.Is_Leader);
    }

    public List<UserHeroDeckMountData> GetDeckHeroes()
    {
        //  사거리에 따라 정렬(사거리가 가까운 영웅부터)
        Deck_Heroes.Sort((a, b) => a.GetUserHeroData().GetApproachDistance().CompareTo(b.GetUserHeroData().GetApproachDistance()));
        return Deck_Heroes;
    }

    /// <summary>
    /// 빈 슬롯이 있는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsExistEmptySlot()
    {
        return Deck_Heroes.Count < 5;
    }

    /// <summary>
    /// 현재 덱에 영웅이 세팅되어 있는지 여부
    /// </summary>
    /// <returns></returns>
    public bool IsExistHeroInDeck()
    {
        return Deck_Heroes.Count > 0;
    }

    public void SetSelect(bool select)
    {
        if (Is_Selected != select)
        {
            Is_Update_Data = true;
        }
        Is_Selected = select;
    }

    public List<Attribute_Synergy_Data> GetTeamSynergyList()
    {
        return Team_Synergy_Data_List;
    }

    /// <summary>
    /// 덱에 캐릭터 추가/삭제시마다 팀 시너지 업데이트
    /// </summary>
    void CalcTeamSynergy()
    {
        var m = MasterDataManager.Instance;
        Team_Synergy_Data_List.Clear();
        foreach (ATTRIBUTE_TYPE attr in Enum.GetValues(typeof(ATTRIBUTE_TYPE)))
        {
            if (attr == ATTRIBUTE_TYPE.NONE)
            {
                continue;
            }
            //  같은 속성을 가진 캐릭터 리스트 반환
            var same_attr_list = Deck_Heroes.FindAll(x => x.GetUserHeroData().GetAttributeType() == attr);
            //  같은 속성을 가진 캐릭터가 2개 이상일 경우, 시너지 효과 있음
            if (same_attr_list.Count > 1)
            {
                var synergy = m.Get_AttributeSynergyData(attr, same_attr_list.Count);
                if (synergy != null)
                {
                    //  시너지 등록
                    Team_Synergy_Data_List.Add(synergy);
                }
            }
        }
    }

    public override JsonData Serialized()
    {
        //if (!IsUpdateData())
        //{
        //    return null;
        //}
        var json = new JsonData();

        var heroes_arr = new JsonData();

        json[NODE_DECK_NUMBER] = Deck_Number;
        json[NODE_GAME_TYPE] = (int)Game_Type;
        json[NODE_IS_SELECTED] = Is_Selected;
        json[NODE_TEAM_NAME] = Team_Name;

        int cnt = Deck_Heroes.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = Deck_Heroes[i];
            var jdata = item.Serialized();
            if (jdata == null)
            {
                continue;
            }
            heroes_arr.Add(jdata);
        }
        if (heroes_arr.IsArray && heroes_arr.Count > 0)
        {
            json[NODE_DECK_HEROES] = heroes_arr;
        }

        return json;
    }

    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }

        if (json.ContainsKey(NODE_DECK_NUMBER))
        {
            Deck_Number = ParseInt(json, NODE_DECK_NUMBER);
        }
        if (json.ContainsKey(NODE_GAME_TYPE))
        {
            Game_Type = (GAME_TYPE)ParseInt(json, NODE_GAME_TYPE);
        }
        if (json.ContainsKey(NODE_IS_SELECTED))
        {
            Is_Selected = ParseBool(json, NODE_IS_SELECTED);
        }
        if (json.ContainsKey(NODE_TEAM_NAME))
        {
            Team_Name = ParseString(json, NODE_TEAM_NAME);
        }
        if (json.ContainsKey(NODE_DECK_HEROES))
        {
            var arr = json[NODE_DECK_HEROES];
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
                        UserHeroDeckMountData item = FindUserHeroDeckMountData(player_character_id, player_character_num);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            var code = AddSlotHero(player_character_id, player_character_num);
                            if (code == ERROR_CODE.SUCCESS)
                            {
                                item = FindUserHeroDeckMountData(player_character_id, player_character_num);
                                item?.Deserialized(jdata);
                            }
                        }
                    }
                }
            }
        }

        InitUpdateData();
        CalcTeamSynergy();

        return true;
    }


    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_DECK_NUMBER = "dnum";
    protected const string NODE_GAME_TYPE = "gtype";
    protected const string NODE_IS_SELECTED = "sel";
    protected const string NODE_TEAM_NAME = "tname";
    protected const string NODE_DECK_HEROES = "heroes";


    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";

}
