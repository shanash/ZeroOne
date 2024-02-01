using LitJson;
using System.Collections.Generic;
using System.Linq;

public class UserDeckDataManager : ManagerBase
{
    List<UserDeckData> User_Deck_List = new List<UserDeckData>();
    public UserDeckDataManager(USER_DATA_MANAGER_TYPE utype) : base(utype)
    {
    }


    public override void InitDataManager()
    {
        if (User_Deck_List.Count > 0)
        {
            return;
        }

        //  story mode의 1개 덱만 일단 추가한다. 기본 덱을 선택해준다.
        var default_deck = AddDeck(GAME_TYPE.STORY_MODE);
        default_deck.SetSelect(true);
        Save();
    }
    /// <summary>
    /// 덱 리스트 찾기
    /// </summary>
    /// <param name="game_type"></param>
    /// <param name="list"></param>
    public void FindDeckList(GAME_TYPE game_type, ref List<UserDeckData> list)
    {
        var temp_list = User_Deck_List.FindAll(x => x.Game_Type == game_type);
        temp_list.Sort(delegate (UserDeckData a, UserDeckData b)
        {
            if (a.Deck_Number > b.Deck_Number)
            {
                return 1;
            }
            return -1;
        });
        list.AddRange(temp_list);
    }
    /// <summary>
    /// 현재 선택된 덱 찾기
    /// </summary>
    /// <param name="game_type"></param>
    /// <returns></returns>
    public UserDeckData FindSelectedDeck(GAME_TYPE game_type)
    {
        var deck = User_Deck_List.Find(x => x.Game_Type == game_type && x.Is_Selected);
        if (deck == null)
        {
            deck = User_Deck_List[0];
            deck.SetSelect(true);
        }
        return deck;
    }
    /// <summary>
    /// 덱 선택
    /// </summary>
    /// <param name="game_type"></param>
    /// <param name="deck_num"></param>
    public void SetSelectedDeck(GAME_TYPE game_type, int deck_num)
    {
        List<UserDeckData> deck_list = new List<UserDeckData>();
        FindDeckList(game_type, ref deck_list);
        int cnt = deck_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var deck = deck_list[i];
            deck.SetSelect(deck.Deck_Number == deck_num);
        }
    }
    /// <summary>
    /// 덱 추가
    /// </summary>
    /// <param name="game_type"></param>
    /// <returns></returns>
    public UserDeckData AddDeck(GAME_TYPE game_type)
    {
        List<UserDeckData> deck_list = new List<UserDeckData>();
        FindDeckList(game_type, ref deck_list);

        if (deck_list.Count >= 5)
        {
            return null;
        }
        int max_num = deck_list.Count > 0 ? deck_list.Max(x => x.Deck_Number) : 0;

        int next_num = max_num + 1;
        var new_deck = new UserDeckData();
        new_deck.SetDeckNumber(next_num);
        new_deck.SetGameType(game_type);
        new_deck.SetSelect(false);
        new_deck.ChangeTeamName(string.Format("{0}팀", next_num));
        User_Deck_List.Add(new_deck);
        Is_Update_Data = true;
        return new_deck;
    }
    /// <summary>
    /// 덱 찾기
    /// </summary>
    /// <param name="game_type"></param>
    /// <param name="deck_num"></param>
    /// <returns></returns>
    public UserDeckData FindDeck(GAME_TYPE game_type, int deck_num)
    {
        var deck = User_Deck_List.Find(x => x.Game_Type == game_type && x.Deck_Number == deck_num);
        if (deck == null)
        {
            deck = new UserDeckData();
            deck.SetGameType(game_type);
            deck.SetDeckNumber(deck_num);
            deck.SetSelect(false);

            User_Deck_List.Add(deck);
        }
        return deck;
    }
    /// <summary>
    /// 지정 영웅이 덱에 세팅되어 있는지 여부 반환
    /// </summary>
    /// <param name="hero_data_id"></param>
    /// <param name="hero_data_num"></param>
    /// <returns></returns>
    public bool IsExistInAnyDeck(int hero_data_id, int hero_data_num)
    {
        bool is_exist = User_Deck_List.Exists(x => x.IsExistHeroInDeck(hero_data_id, hero_data_num));
        return is_exist;
    }
    /// <summary>
    /// 지정 영웅이 지정한 게임 타입의 덱 중에 세팅되어 있는지 여부 반환
    /// </summary>
    /// <param name="game_type"></param>
    /// <param name="hero_data_id"></param>
    /// <param name="hero_data_num"></param>
    /// <returns></returns>
    public bool IsExistInDeck(GAME_TYPE game_type, int hero_data_id, int hero_data_num)
    {
        List<UserDeckData> deck_list = new List<UserDeckData>();
        FindDeckList(game_type, ref deck_list);
        bool is_exist = deck_list.Exists(x => x.IsExistHeroInDeck(hero_data_id, hero_data_num));
        return is_exist;
    }


    public override JsonData Serialized()
    {
        var json = new LitJson.JsonData();

        var arr = new LitJson.JsonData();

        int cnt = User_Deck_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var item = User_Deck_List[i];
            var jdata = item.Serialized();
            if (jdata == null)
                continue;

            arr.Add(jdata);
        }
        if (arr.IsArray && arr.Count > 0)
        {
            json[NODE_DECK_DATA_LIST] = arr;
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
        if (json.ContainsKey(NODE_DECK_DATA_LIST))
        {
            var arr = json[NODE_DECK_DATA_LIST];
            if (arr.GetJsonType() == JsonType.Array)
            {
                int cnt = arr.Count;
                for (int i = 0; i < cnt; i++)
                {
                    var jdata = arr[i];

                    int deck_number = 0;
                    int game_type = 0;
                    if (int.TryParse(jdata[NODE_DECK_NUMBER].ToString(), out deck_number) && int.TryParse(jdata[NODE_GAME_TYPE].ToString(), out game_type))
                    {
                        UserDeckData item = FindDeck((GAME_TYPE)game_type, deck_number);
                        if (item != null)
                        {
                            item.Deserialized(jdata);
                        }
                        else
                        {
                            item = AddDeck((GAME_TYPE)game_type);
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
    protected const string NODE_DECK_DATA_LIST = "dlist";

    protected const string NODE_DECK_NUMBER = "dnum";
    protected const string NODE_GAME_TYPE = "gtype";

}
