
using FluffyDuck.Util;
using System.Collections.Generic;
using Unity.VisualScripting;
using Spine;
using System.Diagnostics;
using System.Runtime.Serialization;

public class UserHeroData : UserDataBase
{

    /// <summary>
    /// 캐릭터 아이디
    /// </summary>
    SecureVar<int> Player_Character_ID = null;

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

    Player_Character_Level_Data Lv_Data = null;
    Player_Character_Love_Level_Data Love_Lv_Data = null;
    Star_Upgrade_Data Star_Data = null;

    /// <summary>
    /// 캐릭터 고유 번호(서버측에서 발급)
    /// </summary>
    public int Player_Character_Num { get; protected set; } = 0;
    public Player_Character_Data Hero_Data { get; private set; } = null;
    public Player_Character_Battle_Data Battle_Data { get; private set; } = null;
    public int Lobby_Choice_Num { get; protected set; } = 0;
    public bool Is_Choice_Lobby { get; protected set; } = false;
    public bool Is_Clone { get; private set; } = false;

    public UserHeroData(LitJson.JsonData json_data)
    {
        Deserialized(json_data);
    }

    public UserHeroData(int player_character_id, int player_character_num)
    {
        SetPlayerCharacterDataID(player_character_id, player_character_num);
    }

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

        if (Love_Level == null)             Love_Level = new SecureVar<int>(1);
        else                                Love_Level.Set(1);

        if (Love_Exp == null)               Love_Exp = new SecureVar<double>();
        else                                Love_Exp.Set(0);
    }

    public void SetPlayerCharacterDataID(int player_character_id, int player_character_num)
    {
        Player_Character_ID.Set(player_character_id);
        Player_Character_Num = player_character_num;
        InitMasterData();
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
        return GetMaxLevel() <= GetLevel();
    }

    public int GetMaxLevel()
    {
        var pinfo = GameData.Instance.GetUserGameInfoDataManager().GetCurrentPlayerInfoData();
        return pinfo.GetLevel();
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

    public USE_EXP_ITEM_RESULT_DATA AddExpUseItem(List<USE_EXP_ITEM_DATA> use_list)
    {
        var goods_mng = GameData.Instance.GetUserGoodsDataManager();
        var item_mng = GameData.Instance.GetUserItemDataManager();
        var m = MasterDataManager.Instance;
        //  ID 높은 순으로(ID 높은것이 경험치 양이 많음)
        use_list.Sort((a, b) => b.Item_ID.CompareTo(a.Item_ID));

        //  result data 초기화
        USE_EXP_ITEM_RESULT_DATA result = new USE_EXP_ITEM_RESULT_DATA();
        result.Code = ERROR_CODE.FAILED;
        result.Result_Lv = GetLevel();
        result.Result_Accum_Exp = GetExp();
        result.Add_Exp = 0;
        result.Used_Gold = 0;
        //  시뮬레이션 결과를 받아온다.
        var result_simulate = GetCalcSimulateExp(use_list);
        //  성공이 아니면 바로 반환
        if (!(result_simulate.Code == ERROR_CODE.SUCCESS || result_simulate.Code == ERROR_CODE.LEVEL_UP_SUCCESS))
        {
            result.ResetAndResultCode(result_simulate.Code);
            return result;
        }
        //  증가된 경험치가 없거나, 필요 금화량이 0인 경우, 아무것도 하지 않도록(이런 경우는 있으면 안되는 상황)
        if (result_simulate.Add_Exp <= 0 || result_simulate.Need_Gold <= 0)
        {
            result.ResetAndResultCode(ERROR_CODE.NOT_WORK);
            return result;
        }

        //  금화가 충분한지 체크
        bool is_useable_gold = goods_mng.IsUsableGoodsCount(GOODS_TYPE.GOLD, result_simulate.Need_Gold);
        if (!is_useable_gold)
        {
            result.ResetAndResultCode(ERROR_CODE.NOT_ENOUGH_GOLD);
            return result;
        }
        result.Used_Gold = result_simulate.Need_Gold;

        //  아이템 체크
        bool is_usable_item = true;
        int cnt = use_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var use = use_list[i];
            if (!item_mng.IsUsableItemCount(use.Item_Type, use.Item_ID, use.Use_Count))
            {
                is_usable_item = false;
                break;
            }
        }
        //  아이템이 충분하지 않음.
        if (!is_usable_item)
        {
            result.ResetAndResultCode(ERROR_CODE.NOT_ENOUGH_ITEM);
            return result;
        }
        result.Add_Exp = result_simulate.Add_Exp;

        //  재화 및 아이템 소모
        goods_mng.UseGoodsCount(GOODS_TYPE.GOLD, result.Used_Gold);
        cnt = use_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var use = use_list[i];
            item_mng.UseItemCount(use.Item_Type, use.Item_ID, use.Use_Count);
        }
        //  경험치 증가 및 레벨업
        result.Code = AddExp(result_simulate.Add_Exp);

        return result;
    }

    /// <summary>
    /// 캐릭터 경험치 증가 시뮬레이션<br/>
    /// 지정 경험치 아이템 사용시 필요 금화와 레벨 상승 결과를 알려준다.<br/>
    /// 최대 레벨을 초과할 경우, 초과 경험치 정보도 같이 알려준다.
    /// </summary>
    /// <param name="use_list"></param>
    /// <returns></returns>
    public EXP_SIMULATE_RESULT_DATA GetCalcSimulateExp(List<USE_EXP_ITEM_DATA> use_list)
    {
        EXP_SIMULATE_RESULT_DATA result = new EXP_SIMULATE_RESULT_DATA();
        //  가능한 결과 코드 : ALREADY_MAX_LEVEL, FAILED, SUCCESS, LEVEL_UP_SUCCESS, 
        //  최초 초기화
        result.Code = ERROR_CODE.FAILED;
        result.Result_Lv = GetLevel();
        result.Result_Accum_Exp = GetExp();

        result.Need_Gold = 0;
        result.Add_Exp = 0;
        result.Over_Exp = 0;

        //  이미 최대레벨일 경우 강화 불가
        if (IsMaxLevel())
        {
            return new EXP_SIMULATE_RESULT_DATA(ERROR_CODE.ALREADY_MAX_LEVEL);
        }

        var m = MasterDataManager.Instance;
        int cnt = use_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var use_data = use_list[i];
            //  0이하인 경우는 거의 없겠지만, 해킹에 의해 0이하인 경우 아이템의 수량이 증가하는 문제가 있을 수 있음.
            if (use_data.Use_Count <= 0)
            {
                continue;
            }
            //  캐릭터 경험치 아이템이 아니면 스킵 
            if (!IsValidExpItem(use_data.Item_Type))
            {
                continue;
            }

            var item_data = m.Get_ItemData(use_data.Item_Type, use_data.Item_ID);
            if (item_data == null)
            {
                continue;
            }
            result.Add_Exp += item_data.int_var1 * use_data.Use_Count;
            result.Need_Gold += item_data.int_var2 * use_data.Use_Count;
        }

        int max_level = GetMaxLevel();
        result.Result_Accum_Exp += result.Add_Exp;
        Player_Character_Level_Data next_lv_data = m.Get_PlayerCharacterLevelDataByAccumExp(result.Result_Accum_Exp);
        //  레벨 데이터가 없으면 failed
        if (next_lv_data == null)
        {
            return new EXP_SIMULATE_RESULT_DATA(ERROR_CODE.FAILED);
        }
        //  최대 레벨을 초과할 경우, 최대 레벨에 맞춘다.
        if (max_level < next_lv_data.level)
        {
            next_lv_data = m.Get_PlayerCharacterLevelData(max_level);
        }
        //  레벨이 증가할 경우, 해당 레벨로 적용
        if (result.Result_Lv < next_lv_data.level)
        {
            result.Result_Lv = next_lv_data.level;
            result.Code = ERROR_CODE.LEVEL_UP_SUCCESS;
        }
        else
        {
            result.Code = ERROR_CODE.SUCCESS;
        }

        //  결과 레벨이 최대 레벨일 경우, 초과된 경험치 양을 알려준다.
        if (max_level <= next_lv_data.level)
        {
            result.Over_Exp = result.Result_Accum_Exp - next_lv_data.accum_exp;
        }

        return result;
    }

    bool IsValidExpItem(ITEM_TYPE_V2 itype)
    {
        return itype == ITEM_TYPE_V2.EXP_POTION_C;
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
    
    void SetStarGrade(int grade)
    {
        Star_Grade.Set(grade);

        var m = MasterDataManager.Instance;
        Battle_Data = m.Get_PlayerCharacterBattleData(Hero_Data.battle_info_id, grade);
        Star_Data = m.Get_StarUpgradeData(grade);
    }

    /// <summary>
    /// 성급 진화 시뮬레이션.
    /// 필요 캐릭터 조각과 필요 골드를 알려준다.
    /// </summary>
    /// <returns></returns>
    public ERROR_CODE CheckAdvanceStarGrade()
    {
        ERROR_CODE result = ERROR_CODE.SUCCESS;

        //  이미 최대 성급 레벨입니다.
        if (IsMaxStarGrade())
        {
            result = ERROR_CODE.ALREADY_MAX_LEVEL;
            return result;
        }

        var goods_mng = GameData.Instance.GetUserGoodsDataManager();
        var item_mng = GameData.Instance.GetUserItemDataManager();

        var need_piece = GetNeedPiece();
        var need_gold = GetNeedGold();

        //  현재 성급에서 상위 성급으로 진화시 필요한 캐릭터 조각 수 체크
        if (need_piece == 0 || !item_mng.IsUsableItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, GetPlayerCharacterID(), need_piece))
        {
            //  캐릭터 조각이 부족합니다.
            result = ERROR_CODE.NOT_ENOUGH_ITEM;
        }

        //  필요 골드 체크
        if (need_gold == 0 || !goods_mng.IsUsableGoodsCount(GOODS_TYPE.GOLD, need_gold))
        {
            //  골드가 부족합니다.
            result = (result != ERROR_CODE.NOT_ENOUGH_ITEM) ? ERROR_CODE.NOT_ENOUGH_GOLD : ERROR_CODE.NOT_ENOUGH_ALL;
        }

        return result;
    }

    /// <summary>
    /// 성급 진화 요청
    /// </summary>
    /// <returns></returns>
    public ERROR_CODE AdvanceStarGrade()
    {
        var data = CheckAdvanceStarGrade();

        if (data != ERROR_CODE.SUCCESS)
        {
            return data;
        }

        //  캐릭터 조각 소모
        ERROR_CODE piece_use_result = GameData.Instance.GetUserItemDataManager().UseItemCount(ITEM_TYPE_V2.PIECE_CHARACTER, GetPlayerCharacterID(), GetNeedPiece());
        if (piece_use_result != ERROR_CODE.SUCCESS)
        {
            return piece_use_result;
        }
        //  필요 골드 소모
        ERROR_CODE gold_use_result = GameData.Instance.GetUserGoodsDataManager().UseGoodsCount(GOODS_TYPE.GOLD, GetNeedGold());
        if (gold_use_result != ERROR_CODE.SUCCESS)
        {
            return gold_use_result;
        }

        //  성급 진화
        SetNextStarGrade();

        return ERROR_CODE.SUCCESS;
    }

    /// <summary>
    /// 성급 진화 전에도 진화에 관한 데이터가 필요하기 때문에
    /// 확인을 위해서 public으로 메소드를 별도로 만듭니다.
    /// 각종 스탯 비교를 위해서가 아니면 절대로 밖에서 호출하지 맙시다.
    /// </summary>
    public ERROR_CODE TryUpNextStarGrade()
    {
        if (!Is_Clone)
        {
            return ERROR_CODE.NOT_ENABLE_WORK;
        }

        if (IsMaxStarGrade())
        {
            return ERROR_CODE.ALREADY_MAX_LEVEL;
        }

        SetNextStarGrade();
        return ERROR_CODE.SUCCESS;
    }

    void SetNextStarGrade()
    {
        int next_star_grade = GetStarGrade() + 1;
        SetStarGrade(next_star_grade);
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
    /// 성급 진화에 필요한 캐릭터 조각
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
    /// 성급 진화에 필요한 골드
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

    public override object Clone()
    {
        UserHeroData clone = (UserHeroData)this.MemberwiseClone();
        clone.Player_Character_ID = new SecureVar<int>(Player_Character_ID);
        clone.Level = new SecureVar<int>(Level);
        clone.Exp = new SecureVar<double>(Exp);
        clone.Love_Level = new SecureVar<int>(Love_Level);
        clone.Love_Exp = new SecureVar<double>(Love_Exp);
        clone.Star_Grade = new SecureVar<int>(Star_Grade);
        clone.Is_Clone = true;

        return clone;
    }

    public override LitJson.JsonData Serialized()
    {
        //if (!IsUpdateData())
        //{
        //    return null;
        //}
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
