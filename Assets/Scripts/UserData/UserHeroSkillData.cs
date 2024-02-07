using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHeroSkillData : UserDataBase
{

    SecureVar<int> Skill_Group_ID = null;
    SecureVar<int> Player_Character_ID = null;
    SecureVar<int> Player_Character_Num = null;
    SecureVar<int> Level = null;
    SecureVar<double> Exp = null;

    Player_Character_Skill_Group Data;
    Player_Character_Skill_Level_Data Level_Data;

    public UserHeroData Hero_Data { get; private set; }

    public UserHeroSkillData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
    }

    protected override void InitSecureVars()
    {
        if (Player_Character_ID == null)
        {
            Player_Character_ID = new SecureVar<int>();
        }
        if (Player_Character_Num == null)
        {
            Player_Character_Num = new SecureVar<int>();
        }
        if (Skill_Group_ID == null)
        {
            Skill_Group_ID = new SecureVar<int>();
        }
        if (Level == null)
        {
            Level = new SecureVar<int>(1);
        }
        if (Exp == null)
        {
            Exp = new SecureVar<double>();
        }
    }

    public void SetSkillGroupID(int skill_grp_id)
    {
        Skill_Group_ID.Set(skill_grp_id);
        InitMasterData();
        Is_Update_Data = true;
    }

    public void SetUserHero(UserHeroData hero)
    {
        Hero_Data = hero;
        SetUserHeroNumOnly();
        InitMasterData();
        Is_Update_Data = true;
    }

    /// <summary>
    /// 서버가 붙지 않은 상황에서 별도로 세팅해줄 필요가 있어서 보호수준을 공개로 합니다
    /// </summary>
    /// <param name="player_character_id">캐릭터 ID</param>
    /// <param name="player_character_num">유저가 소유하고 있는 캐릭터 번호(인덱스)</param>
    public void SetUserHeroNumOnly(int player_character_id = 0, int player_character_num = 0)
    {
        if (Hero_Data != null)
        {
            player_character_id = Hero_Data.GetPlayerCharacterID();
            player_character_num = Hero_Data.Player_Character_Num;
        }
        Player_Character_ID.Set(player_character_id);
        Player_Character_Num.Set(player_character_num);
    }

    protected override void InitMasterData()
    {
        if (GetSkillGroupID() != 0)
        {
            var m = MasterDataManager.Instance;
            Data = m.Get_PlayerCharacterSkillGroupData(GetSkillGroupID());
            Level_Data = m.Get_PlayerCharacterSkillLevelData(GetLevel());
        }

        var mng = GameData.Instance.GetUserHeroDataManager();
        if (mng != null)
        {
            Hero_Data = mng.FindUserHeroData(GetPlayerCharacterID(), GetPlayerCharacterNum());
        }
    }

    void SetLevel(int lv)
    {
        Level.Set(lv);
        Level_Data = MasterDataManager.Instance.Get_PlayerCharacterSkillLevelData(lv);
    }

    public int GetSkillGroupID() { return Skill_Group_ID.Get(); }  
    public int GetPlayerCharacterID() { return Player_Character_ID.Get(); }
    public int GetPlayerCharacterNum() {  return Player_Character_Num.Get(); }
    public int GetLevel() { return Level.Get(); }
    public double GetExp() { return Exp.Get(); }

    /// <summary>
    /// 스킬의 최대 레벨은 영웅의 현재 레벨
    /// </summary>
    /// <returns></returns>
    public int GetMaxLevel()
    {
        if (Hero_Data != null)
        {
            return Hero_Data.GetLevel();
        }
        return 0;
    }

    public bool IsMaxLevel()
    {
        return GetLevel() >= GetMaxLevel();
    }
    /// <summary>
    /// 다음 레벨업에 필요한 경험치
    /// </summary>
    /// <returns></returns>
    public double GetNextExp()
    {
        if (Level_Data == null)
        {
            Level_Data = MasterDataManager.Instance.Get_PlayerCharacterSkillLevelData(GetLevel());
        }
        return Level_Data.need_exp;
    }
    /// <summary>
    /// 현재 레벨의 경험치
    /// </summary>
    /// <returns></returns>
    public double GetLevelExp()
    {
        if (Level_Data == null)
        {
            Level_Data = MasterDataManager.Instance.Get_PlayerCharacterSkillLevelData(GetLevel());
        }
        return GetExp() - Level_Data.accum_exp;
    }
    /// <summary>
    /// 현재 레벨의 경험치 진행률 
    /// </summary>
    /// <returns></returns>
    public float GetExpPercentage()
    {
        float lv_exp = (float)GetLevelExp();
        float need_exp = (float)GetNextExp();
        return lv_exp / need_exp;
    }

    /// <summary>
    /// 경험치 증가 및 레벨업 체크
    /// </summary>
    /// <param name="xp"></param>
    /// <returns></returns>
    ERROR_CODE AddExp(double xp)
    {
        ERROR_CODE code = ERROR_CODE.FAILED;
        if (xp < 0)
        {
            return code;
        }

        if (!IsMaxLevel())
        {
            double exp = GetExp();
            exp += xp;

            //  level check
            var lv_data = MasterDataManager.Instance.Get_PlayerCharacterSkillLevelDataByAccumExp(exp);
            if (lv_data != null)
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
            Exp.Set(exp);
            Is_Update_Data = true;
        }
        else
        {
            code = ERROR_CODE.ALREADY_COMPLETE;
        }

        return code;
    }

    /// <summary>
    /// 경험치 아이템 사용으로 아이템 증가<br/>
    /// 이 함수는 결정된 아이템 및 재화 사용의 결과를 알려주기만 함.<br/>
    /// 최적의 시뮬레이션은 적용 전에 별도의 로직을 거쳐 알아내야 함
    /// </summary>
    /// <param name="use_list"></param>
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
        //  증가된 경험치가 없거나, 필요 금화량이 0일경우 아무일도 하지 않도록(예외 상황임)
        if (result_simulate.Add_Exp == 0 || result_simulate.Need_Gold == 0)
        {
            result.ResetAndResultCode(ERROR_CODE.NOT_WORK);
            return result;
        }
        
        //  금화가 충분한지 체크
        bool is_usable_gold = goods_mng.IsUsableGoodsCount(GOODS_TYPE.GOLD, result_simulate.Need_Gold);
        if (!is_usable_gold)
        {
            result.ResetAndResultCode(ERROR_CODE.NOT_ENOUGH_GOLD);
            return result;
        }
        else
        {
            result.Used_Gold = result_simulate.Need_Gold;
        }
        bool is_usable_item = true;
        int cnt = use_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var use = use_list[i];
            //  해당 아이템이 충분한지 체크
            if (!item_mng.IsUsableItemCount(use.Item_Type, use.Item_ID, use.Use_Count))
            {
                is_usable_item = false;
                break;
            }
        }
        //  아이템이 충분하지 않음
        if (!is_usable_item)
        {
            result.ResetAndResultCode(ERROR_CODE.NOT_ENOUGH_ITEM);
            return result;
        }
        else
        {
            result.Add_Exp = result_simulate.Add_Exp;
        }

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
    /// 경험치 증가 시뮬레이션.<br/>
    /// 지정 경험치 아이템 사용시 필요 금화와 레벨 상승 결과를 알려준다.<br/>
    /// 최대 레벨을 초과할 경우 초과 경험치 정보도 같이 알려준다.
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
            result.ResetAndResultCode(ERROR_CODE.ALREADY_MAX_LEVEL);
            return result;
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
            //  리스트에 스킬 강화 아이템이 아닌 것이 들어있으면 실패
            if (!IsValidSkillExpItem(use_data.Item_Type))
            {
                result.Code = ERROR_CODE.NOT_ENABLE_WORK;
                return result;
            }
            var item_data = m.Get_ItemData(use_data.Item_Type, use_data.Item_ID);
            if (item_data != null)
            {
                result.Add_Exp += item_data.int_var1 * use_data.Use_Count;
                result.Need_Gold += item_data.int_var2 * use_data.Use_Count;
            }
        }

        int max_level = GetMaxLevel();
        result.Result_Accum_Exp += result.Add_Exp;
        Player_Character_Skill_Level_Data next_lv_data = m.Get_PlayerCharacterSkillLevelDataByAccumExp(result.Result_Accum_Exp);
        //  레벨 데이터가 없으면 failed
        if (next_lv_data == null)
        {
            result.ResetAndResultCode(ERROR_CODE.FAILED);
            return result;
        }
        //  최대 레벨을 초과할 경우, 최대 레벨에 맞춘다
        if (max_level < next_lv_data.level)
        {
            next_lv_data = m.Get_PlayerCharacterSkillLevelData(max_level);
        }

        //  레벨이 증가할 경우 해당 레벨로 적용
        if (result.Result_Lv < next_lv_data.level)
        {
            result.Result_Lv = next_lv_data.level;
            result.Code = ERROR_CODE.LEVEL_UP_SUCCESS;
        }
        else
        {
            result.Code = ERROR_CODE.SUCCESS;
        }
        //  결과 레벨이 최대 레벨일 경우 초과된 경험치 양을 알려준다.
        if (max_level <= next_lv_data.level)
        {
            result.Over_Exp = result.Result_Accum_Exp - next_lv_data.accum_exp;
        }

        return result;
    }

    bool IsValidSkillExpItem(ITEM_TYPE_V2 itype)
    {
        return itype == ITEM_TYPE_V2.EXP_SKILL;
    }


    public override JsonData Serialized()
    {
        if (!IsUpdateData())
        {
            return null;
        }
        var json = new JsonData();
        json[NODE_HERO_SKILL_GROUP_ID] = GetSkillGroupID();
        json[NODE_PLAYER_CHARACTER_ID] = GetPlayerCharacterID();
        json[NODE_PLAYER_CHARACTER_NUM] = GetPlayerCharacterNum();
        json[NODE_LEVEL] = GetLevel();
        json[NODE_EXP] = GetExp();
        return json;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null) return false;

        InitSecureVars();

        if (json.ContainsKey(NODE_HERO_SKILL_GROUP_ID))
        {
            Skill_Group_ID.Set(ParseInt(json, NODE_HERO_SKILL_GROUP_ID));
        }
        if (json.ContainsKey(NODE_PLAYER_CHARACTER_ID))
        {
            Player_Character_ID.Set(ParseInt(json, NODE_PLAYER_CHARACTER_ID));
        }
        if (json.ContainsKey(NODE_PLAYER_CHARACTER_NUM))
        {
            Player_Character_Num.Set(ParseInt(json, NODE_PLAYER_CHARACTER_NUM));
        }
        if (json.ContainsKey(NODE_LEVEL))
        {
            Level.Set(ParseInt(json, NODE_LEVEL));
        }
        if (json.ContainsKey(NODE_EXP))
        {
            Exp.Set(ParseDouble(json, NODE_EXP));
        }

        InitMasterData();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------
    protected const string NODE_HERO_SKILL_GROUP_ID = "grpid";
    protected const string NODE_PLAYER_CHARACTER_ID = "pid";
    protected const string NODE_PLAYER_CHARACTER_NUM = "pnum";
    protected const string NODE_LEVEL = "lv";
    protected const string NODE_EXP = "xp";
}
