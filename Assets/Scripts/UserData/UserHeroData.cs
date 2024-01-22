using DocumentFormat.OpenXml.Drawing.Charts;
using FluffyDuck.Util;

public class UserHeroData : UserDataBase
{
    public SecureVar<int> Player_Character_ID { get; protected set; } = null;
    public int Player_Character_Num { get; protected set; } = 0;
    public SecureVar<int> Level { get; protected set; } = null;
    public SecureVar<double> Exp { get; protected set; } = null;

    public SecureVar<int> Star_Grade { get; protected set; } = null;

    public int Lobby_Choice_Num { get; protected set; } = 0;

    public bool Is_Choice_Lobby { get; protected set; } = false;

    public Player_Character_Data Hero_Data { get; private set; }
    public Player_Character_Battle_Data Battle_Data { get; private set; }

    Player_Character_Level_Data Lv_Data;

    public UserHeroData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Player_Character_Num = 0;
        Player_Character_ID.Set(0);
        Level.Set(1);
        Exp.Set(0);
        Star_Grade.Set(0);
        Lobby_Choice_Num = 0;
        Is_Choice_Lobby = false;
    }
    protected override void InitSecureVars()
    {
        if (Player_Character_ID == null)
        {
            Player_Character_ID = new SecureVar<int>();
        }
        if (Level == null)
        {
            Level = new SecureVar<int>();
        }
        if (Exp == null)
        {
            Exp = new SecureVar<double>();
        }
        if (Star_Grade == null)
        {
            Star_Grade = new SecureVar<int>();
        }
    }
    public void SetPlayerCharacterDataID(int player_character_id, int player_character_num)
    {
        Player_Character_ID.Set(player_character_id);
        Player_Character_Num = player_character_num;
        InitMasterData();
        Is_Update_Data = true;
    }
    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Hero_Data = m.Get_PlayerCharacterData(GetPlayerCharacterID());
        Battle_Data = m.Get_PlayerCharacterBattleData(Hero_Data.battle_info_id);
        if (GetStarGrade() == 0)
        {
            Star_Grade.Set(Hero_Data.default_star);
        }
    }

    public Player_Character_Data GetPlayerCharacterData()
    {
        return Hero_Data;
    }

    public Player_Character_Battle_Data GetPlayerCharacterBattleData()
    {
        return Battle_Data;
    }
    /// <summary>
    /// 접근 거리
    /// </summary>
    /// <returns></returns>
    public float GetApproachDistance()
    {
        return (float)Battle_Data.approach;
    }

    public bool IsEquals(UserHeroData ud)
    {
        return IsEquals(ud.GetPlayerCharacterID(), ud.Player_Character_Num);
    }
    public bool IsEquals(int hero_data_id, int hero_data_num)
    {
        return GetPlayerCharacterID() == hero_data_id && Player_Character_Num == hero_data_num;
    }

    public int GetPlayerCharacterID()
    {
        return Player_Character_ID.Get();
    }
    public int GetLevel()
    {
        return Level.Get();
    }
    void SetLevel(int lv)
    {
        Level.Set(lv);
        Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(GetLevel());
    }
    public double GetExp()
    {
        return Exp.Get();
    }

    public int GetStarGrade()
    {
        return Star_Grade.Get();
    }

    public POSITION_TYPE GetPositionType()
    {
        return Battle_Data.position_type;
    }

    public void SetLobbyChoiceNumber(int num)
    {
        Lobby_Choice_Num = num;
        Is_Choice_Lobby = Lobby_Choice_Num > 0;
    }
    public void ReleaseLobbyChoice()
    {
        SetLobbyChoiceNumber(0);
    }

    public ERROR_CODE AddExp(double xp)
    {
        ERROR_CODE code = ERROR_CODE.FAILED;

        if (xp < 0)
        {
            return code;
        }
        if (IsMaxLevel())
        {
            double _exp = GetExp();
            _exp += xp;

            //  level check
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLevelDataByAccumExp(_exp);
            if (lv_data == null)
            {
                return code;
            }
            if (GetLevel() < lv_data.level)
            {
                code = ERROR_CODE.LEVEL_UP_SUCCESS;
                SetLevel(lv_data.level);
            }
            else
            {
                code = ERROR_CODE.SUCCESS;
            }
            Exp.Set(_exp);
            Is_Update_Data = true;
        }
        else
        {
            code = ERROR_CODE.NOT_WORK;
        }

        return code;
    }

    bool IsMaxLevel()
    {
        var pinfo = GameData.Instance.GetUserGameInfoDataManager().GetCurrentPlayerInfoData();

        return pinfo.GetLevel() <= GetLevel();
    }
    /// <summary>
    /// 다음 레벨업에 필요한 경험치
    /// </summary>
    /// <returns></returns>
    public double GetNextExp()
    {
        if (Lv_Data == null)
        {
            Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(GetLevel());
        }
        return Lv_Data.need_exp;
    }
    /// <summary>
    /// 현재 경험치
    /// </summary>
    /// <returns></returns>
    public double GetLevelExp()
    {
        if (Lv_Data == null)
        {
            Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(GetLevel());
        }
        return GetExp() - Lv_Data.accum_exp;
    }
    /// <summary>
    /// 현재 경험치 진행률
    /// </summary>
    /// <returns></returns>
    public float GetExpPercetage()
    {
        float lv_exp = (float)GetLevelExp();
        float need_exp = (float)GetNextExp();
        return lv_exp / need_exp;
    }
    public override LitJson.JsonData Serialized()
    {
        var json = new LitJson.JsonData();

        json[NODE_PLAYER_CHARACTER_ID] = GetPlayerCharacterID();
        json[NODE_PLAYER_CHARACTER_NUM] = Player_Character_Num;
        json[NODE_LEVEL] = GetLevel();
        json[NODE_EXP] = GetExp();
        json[NODE_STAR_GRADE] = GetStarGrade();
        json[NODE_LOBBY_CHOICE_NUMBER] = Lobby_Choice_Num;
        json[NODE_IS_CHOICE] = Is_Choice_Lobby;

        return json;
    }

    public override bool Deserialized(LitJson.JsonData json)
    {
        if (json == null)
        {
            return false;
        }

        InitSecureVars();

        {
            if (json.ContainsKey(NODE_PLAYER_CHARACTER_ID))
            {
                Player_Character_ID.Set(ParseInt(json, NODE_PLAYER_CHARACTER_ID));
            }
            if (json.ContainsKey(NODE_PLAYER_CHARACTER_NUM))
            {
                Player_Character_Num = ParseInt(json, NODE_PLAYER_CHARACTER_NUM);
            }
            if (json.ContainsKey(NODE_LEVEL))
            {
                Level.Set(ParseInt(json, NODE_LEVEL));
            }
            if (json.ContainsKey(NODE_EXP))
            {
                Exp.Set(ParseDouble(json, NODE_EXP));
            }
            if (json.ContainsKey(NODE_STAR_GRADE))
            {
                Star_Grade.Set(ParseInt(json, NODE_STAR_GRADE));
            }
            if (json.ContainsKey(NODE_LOBBY_CHOICE_NUMBER))
            {
                Lobby_Choice_Num = ParseInt(json, NODE_LOBBY_CHOICE_NUMBER);
            }
            if (json.ContainsKey(NODE_IS_CHOICE))
            {
                Is_Choice_Lobby = ParseBool(json, NODE_IS_CHOICE);
            }
        }

        InitMasterData();


        return true;
    }


    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";
    protected const string NODE_LEVEL = "lv";
    protected const string NODE_EXP = "xp";
    protected const string NODE_STAR_GRADE = "star";
    protected const string NODE_LOBBY_CHOICE_NUMBER = "cnum";
    protected const string NODE_IS_CHOICE = "choice";

}
