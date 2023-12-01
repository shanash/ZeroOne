

using System.Collections.Generic;

public class UserDeckData : UserDataBase
{
    public int Deck_Number { get; protected set; } = 0;

    public GAME_TYPE Game_Type { get; protected set; } = 0;   //  차후 enum 으로 정의 해야 함

    public bool Is_Selected { get; protected set; } = false;

    public string Team_Name { get; protected set; } = string.Empty;

    List<UserHeroDeckMountData> Deck_Heroes = new List<UserHeroDeckMountData>();

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
        var found_hero = Deck_Heroes.Find(x => x.Hero_Data_ID == new_hero.Hero_Data_ID && x.Hero_Data_Num == new_hero.Hero_Data_Num);
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

        return ERROR_CODE.SUCCESS;
    }

    public ERROR_CODE AddSlotHero(UserHeroData new_hero)
    {
        var hero = new UserHeroDeckMountData();
        hero.SetUserHeroData(new_hero);
        
        return AddSlotHero(hero);
    }

    /// <summary>
    /// 영웅 덱 정보를 이용하여 덱에서 영웅 제거
    /// </summary>
    /// <param name="hero"></param>
    public void RemoveHero(UserHeroDeckMountData hero)
    {
        var found = Deck_Heroes.Find(x => x.Hero_Data_ID == hero.Hero_Data_ID && x.Hero_Data_Num == hero.Hero_Data_Num);
        if (found != null)
        {
            Deck_Heroes.Remove(found);
            if (found.Is_Leader && Deck_Heroes.Count > 0)
            {
                Deck_Heroes[0].SetLeader(true);
            }

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
        var found = Deck_Heroes.Find(x => x.Hero_Data_ID == hero.GetPlayerCharacterID() && x.Hero_Data_Num == hero.Player_Character_Num);
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
        return Deck_Heroes.Exists(x => x.Hero_Data_ID == hero_data_id && x.Hero_Data_Num == hero_data_num);
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
        Deck_Heroes.Sort(delegate (UserHeroDeckMountData a, UserHeroDeckMountData b)
        {
            if (a.GetUserHeroData().GetDistance() > b.GetUserHeroData().GetDistance())
            {
                return 1;
            }
            return -1;
        });
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

}
