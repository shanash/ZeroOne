
using FluffyDuck.Util;
using System.Runtime.Serialization;

public class UserHeroData : UserDataBase
{
    /// <summary>
    /// 캐릭터 아이디
    /// </summary>
    SecureVar<int> Player_Character_ID = null;
    /// <summary>
    /// 캐릭터 고유 번호(서버측에서 발급)
    /// </summary>
    public int Player_Character_Num { get; protected set; } = 0;
    /// <summary>
    /// 레벨
    /// </summary>
    SecureVar<int> Level = null;
    /// <summary>
    /// 누적 경험치
    /// </summary>
    SecureVar<double> Exp = null;

    /// <summary>
    /// 호감도 레벨
    /// </summary>
    SecureVar<int> Love_Level = null;   
    /// <summary>
    /// 호감도 레벨 누적 경험치
    /// </summary>
    SecureVar<double> Love_Exp = null; 

    /// <summary>
    /// 성급
    /// </summary>
    SecureVar<int> Star_Grade = null;

    public int Lobby_Choice_Num { get; protected set; } = 0;

    public bool Is_Choice_Lobby { get; protected set; } = false;

    public Player_Character_Data Hero_Data { get; private set; } = null;
    public Player_Character_Battle_Data Battle_Data { get; private set; } = null;

    Player_Character_Level_Data Lv_Data;
    Player_Character_Love_Level_Data Love_Lv_Data;
    Star_Upgrade_Data Star_Data;



    public UserHeroData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Player_Character_Num = 0;
        Lobby_Choice_Num = 0;
        Is_Choice_Lobby = false;
    }

    protected override void InitSecureVars()
    {
        if (Player_Character_ID == null)    Player_Character_ID = new SecureVar<int>();
        else                                Player_Character_ID.Set(0);

        if (Level == null)                  Level = new SecureVar<int>(1);
        else                                Level.Set(1);

        if (Exp == null)                    Exp = new SecureVar<double>();
        else                                Exp.Set(0);

        if (Star_Grade == null)             Star_Grade = new SecureVar<int>();
        else                                Star_Grade.Set(0);
        if (Love_Level == null)
        {
            Love_Level = new SecureVar<int>(1);
        }
        if (Love_Exp == null)
        {
            Love_Exp = new SecureVar<double>();
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
        
        Lv_Data = m.Get_PlayerCharacterLevelData(GetLevel());
        Love_Lv_Data = m.Get_PlayerCharacterLoveLevelData(GetLoveLevel());
        if (GetStarGrade() == 0)
        {
            Star_Grade.Set(Hero_Data.default_star);
        }
        Battle_Data = m.Get_PlayerCharacterBattleData(Hero_Data.battle_info_id, GetStarGrade());
        Star_Data = m.Get_StarUpgradeData(GetStarGrade());
    }

    public int GetPlayerCharacterID()
    {
        return Player_Character_ID.Get();
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

    #region Level
    public int GetLevel()
    {
        return Level.Get();
    }

    void SetLevel(int lv)
    {
        Level.Set(lv);
        Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLevelData(lv);
    }

    public double GetExp()
    {
        return Exp.Get();
    }

    /// <summary>
    /// 캐릭터 경험치 추가
    /// </summary>
    /// <param name="xp"></param>
    /// <returns></returns>
    public ERROR_CODE AddExp(double xp)
    {
        ERROR_CODE code = ERROR_CODE.FAILED;

        if (xp < 0)
        {
            return code;
        }
        if (!IsMaxLevel())
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
            code = ERROR_CODE.ALREADY_MAX_LEVEL;
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
    /// 다음 레벨까지 남은 경험치
    /// </summary>
    /// <returns></returns>
    public double GetRemainNextExp()
    {
        double next_exp = GetNextExp();
        double cur_exp = GetLevelExp();
        return next_exp - cur_exp;
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

    #endregion

    #region Love Level
    public int GetLoveLevel()
    {
        return Love_Level.Get();
    }
    void SetLoveLevel(int lv)
    {
        Love_Level.Set(lv);
        //  호감도 레벨 데이터 가져오기 [todo]
        Love_Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLoveLevelData(lv);
    }

    public double GetLoveExp()
    {
        return Love_Exp.Get();
    }

    /// <summary>
    /// 호감도 레벨이 최대 인지 체크(임시로 10으로 했음. 차후 변경 해야함)
    /// </summary>
    /// <returns></returns>
    public bool IsMaxLoveLevel()
    {
        return GetLoveLevel() >= 10;
    }

    /// <summary>
    /// 캐릭터 호감도 레벨 경험치 추가
    /// </summary>
    /// <param name="xp"></param>
    /// <returns></returns>
    public ERROR_CODE AddLoveExp(double xp)
    {
        ERROR_CODE code = ERROR_CODE.FAILED;

        if (xp < 0)
        {
            return code;
        }
        if (!IsMaxLoveLevel())
        {
            double _exp = GetLoveExp();
            _exp += xp;

            //  level check
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterLoveLevelDataByAccumExp(_exp);
            if (lv_data == null)
            {
                return code;
            }
            if (GetLoveLevel() < lv_data.level)
            {
                code = ERROR_CODE.LEVEL_UP_SUCCESS;
                SetLoveLevel(lv_data.level);
            }
            else
            {
                code = ERROR_CODE.SUCCESS;
            }

            Love_Exp.Set(_exp);
            Is_Update_Data = true;
        }
        else
        {
            code = ERROR_CODE.ALREADY_MAX_LEVEL;
        }

        return code;
    }

    /// <summary>
    /// 다음 레벨업에 필요한 경험치
    /// </summary>
    /// <returns></returns>
    public double GetNextLoveExp()
    {
        if (Love_Lv_Data == null)
        {
            Love_Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLoveLevelData(GetLevel());
        }
        return Love_Lv_Data.need_exp;
    }
    /// <summary>
    /// 현재 경험치
    /// </summary>
    /// <returns></returns>
    public double GetLoveLevelExp()
    {
        if (Love_Lv_Data == null)
        {
            Love_Lv_Data = MasterDataManager.Instance.Get_PlayerCharacterLoveLevelData(GetLevel());
        }
        return GetLoveExp() - Love_Lv_Data.accum_exp;
    }

    /// <summary>
    /// 호감도 다음 레벨까지 남은 경험치
    /// </summary>
    /// <returns></returns>
    public double GetRemainNextLoveExp()
    {
        double next_exp = GetNextLoveExp();
        double cur_exp = GetLoveLevelExp();
        return next_exp - cur_exp;
    }
    /// <summary>
    /// 호감도 현재 경험치 진행률
    /// </summary>
    /// <returns></returns>
    public float GetLoveExpPercetage()
    {
        float lv_exp = (float)GetLoveLevelExp();
        float need_exp = (float)GetNextLoveExp();
        return lv_exp / need_exp;
    }

    #endregion

    #region Star Grade
    public int GetStarGrade()
    {
        return Star_Grade.Get();
    }
    public void SetStarGrade(int grade)
    {
        Star_Grade.Set(grade);
        //  등급에 맞는 Battle_Data를 다시 찾아야 함. 수정 필요.
        var m = MasterDataManager.Instance;
        Battle_Data = m.Get_PlayerCharacterBattleData(Hero_Data.battle_info_id, grade);
        Star_Data = m.Get_StarUpgradeData(grade);
    }

    /// <summary>
    /// 성급 강화 요청
    /// </summary>
    /// <returns></returns>
    public ERROR_CODE SetStarGradeUp()
    {
        //  이미 최대 성급 레벨입니다.
        if (IsMaxStarGrade())
        {
            return ERROR_CODE.ALREADY_MAX_LEVEL;
        }
        var goods_mng = GameData.Instance.GetUserGoodsDataManager();
        var item_mng = GameData.Instance.GetUserItemDataManager();

        //  현재 성급에서 상위 성급으로 진급시 필요한 캐릭터 조각 수 체크
        int need_piece_count = GetNeedPiece();
        if (need_piece_count == 0 || !item_mng.IsUsableItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, GetPlayerCharacterID(), need_piece_count))
        {
            //  캐릭터 조각이 부족합니다.
            return ERROR_CODE.NOT_ENOUGH_ITEM;
        }

        //  필요 골드 체크
        double need_gold = GetNeedGold();
        if (need_gold == 0 || !goods_mng.IsUsableGoodsCount(GOODS_TYPE.GOLD, need_gold))
        {
            //  골드가 부족합니다.
            return ERROR_CODE.NOT_ENOUGH_GOLD;
        }

        //  캐릭터 조각 소모
        ERROR_CODE piece_use_result = item_mng.UseItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, GetPlayerCharacterID(), need_piece_count);
        if (piece_use_result != ERROR_CODE.SUCCESS)
        {
            return piece_use_result;
        }
        //  필요 골드 소모
        ERROR_CODE gold_use_result = goods_mng.UseGoodsCount(GOODS_TYPE.GOLD, need_gold);
        if (gold_use_result != ERROR_CODE.SUCCESS)
        {
            return gold_use_result;
        }

        //  성급 상승
        int next_star_grade = GetStarGrade() + 1;
        SetStarGrade(next_star_grade);

        return ERROR_CODE.SUCCESS;
    }


    /// <summary>
    /// 최대 성급인지 반환
    /// </summary>
    /// <returns></returns>
    public bool IsMaxStarGrade()
    {
        const int max_star_grade = 5;
        return GetStarGrade() >= max_star_grade;
    }
    /// <summary>
    /// 성급 강화에 필요한 캐릭터 조각
    /// </summary>
    /// <returns></returns>
    public int GetNeedPiece()
    {
        if (Star_Data != null)
        {
            return Star_Data.need_char_piece;
        }
        return 0;
    }
    /// <summary>
    /// 성급 강화에 필요한 골드
    /// </summary>
    /// <returns></returns>
    public double GetNeedGold()
    {
        if (Star_Data != null)
        {
            return Star_Data.need_gold;
        }
        return 0;
    }
    #endregion







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
    
    public override LitJson.JsonData Serialized()
    {
        if (!IsUpdateData())
        {
            return null;
        }
        var json = new LitJson.JsonData();

        json[NODE_PLAYER_CHARACTER_ID] = GetPlayerCharacterID();
        json[NODE_PLAYER_CHARACTER_NUM] = Player_Character_Num;
        json[NODE_LEVEL] = GetLevel();
        json[NODE_EXP] = GetExp();
        json[NODE_LOVE_LEVEL] = GetLoveLevel();
        json[NODE_LOVE_EXP] = GetLoveExp();
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
            if (json.ContainsKey(NODE_LOVE_LEVEL))
            {
                Love_Level.Set(ParseInt(json, NODE_LOVE_LEVEL));
            }
            if (json.ContainsKey(NODE_LOVE_EXP))
            {
                Love_Exp.Set(ParseDouble(json, NODE_LOVE_EXP));
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
    protected const string NODE_LOVE_LEVEL = "lolv";
    protected const string NODE_LOVE_EXP = "loxp";
    protected const string NODE_STAR_GRADE = "star";
    protected const string NODE_LOBBY_CHOICE_NUMBER = "cnum";
    protected const string NODE_IS_CHOICE = "choice";
}
