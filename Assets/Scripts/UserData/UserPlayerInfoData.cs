
using FluffyDuck.Util;
using LitJson;

public class UserPlayerInfoData : UserDataBase
{
    public string Nickname { get; protected set; } = string.Empty;

    SecureVar<int> Level = null;

    SecureVar<double> Exp = null;

    Player_Level_Data Lv_Data;
    Player_Level_Reward_Data Lv_Stamina_Data;
    public UserPlayerInfoData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        InitMasterData();
        Nickname = string.Empty;
    }

    protected override void Destroy()
    {

    }

    protected override void InitSecureVars()
    {
        if (Level == null)
        {
            Level = new SecureVar<int>(1);
        }
        if (Exp == null)
        {
            Exp = new SecureVar<double>();
        }
    }

    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Lv_Data = m.Get_PlayerLevelData(GetLevel());
        Lv_Stamina_Data = m.Get_PlayerLevelRewardData(GetLevel());
    }

    public int GetLevel()
    {
        return Level.Get();
    }
    void SetLevel(int lv)
    {
        Level.Set(lv);
        var m = MasterDataManager.Instance;
        Lv_Data = m.Get_PlayerLevelData(GetLevel());
        Lv_Stamina_Data = m.Get_PlayerLevelRewardData(GetLevel());
    }

    /// <summary>
    /// 현재 레벨의 스테미너 증가량 
    /// </summary>
    /// <returns></returns>
    public int GetAddMaxStaminaBound()
    {
        return Lv_Stamina_Data.increase_stamina;
    }

    public double GetExp()
    {
        return Exp.Get();
    }

    public void SetNickname(string nick)
    {
        if (string.IsNullOrEmpty(nick))
        {
            return;
        }
        Nickname = nick;
        Is_Update_Data = true;
    }

    public RESPONSE_TYPE AddExp(double xp)
    {
        RESPONSE_TYPE code = RESPONSE_TYPE.FAILED;
        if (xp < 0)
        {
            return code;
        }


        if (!IsMaxLevel())
        {
            double _exp = GetExp();
            _exp += xp;

            //  level check
            var lv_data = MasterDataManager.Instance.Get_PlayerLevelDataByAccumExp(_exp);
            if (lv_data == null)
            {
                return code;
            }
            if (GetLevel() < lv_data.level)
            {
                code = RESPONSE_TYPE.LEVEL_UP_SUCCESS;
                SetLevel(lv_data.level);
            }
            else
            {
                code = RESPONSE_TYPE.SUCCESS;
            }
            Exp.Set(_exp);
            Is_Update_Data = true;
        }
        else
        {
            code = RESPONSE_TYPE.NOT_WORK;
        }

        return code;
    }

    bool IsMaxLevel()
    {
        return MasterDataManager.Instance.Get_PlayerLevelMaxLevel() <= GetLevel();
    }
    /// <summary>
    /// 다음 레벨업에 필요한 경험치
    /// </summary>
    /// <returns></returns>
    public double GetNextExp()
    {
        if (Lv_Data == null)
        {
            Lv_Data = MasterDataManager.Instance.Get_PlayerLevelData(GetLevel());
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
            Lv_Data = MasterDataManager.Instance.Get_PlayerLevelData(GetLevel());
        }
        return GetExp() - Lv_Data.accum_exp;
    }

    /// <summary>
    /// 현재 경험치 진행률
    /// </summary>
    /// <returns></returns>
    public float GetExpPercentage()
    {
        float lv_exp = (float)GetLevelExp();
        float need_exp = (float)GetNextExp();
        return lv_exp / need_exp;
    }

    public override JsonData Serialized()
    {
        var json = new LitJson.JsonData();

        json[NODE_PLAYER_NICKNAME] = Nickname;
        json[NODE_PLAYER_LEVEL] = GetLevel();
        json[NODE_PLAYER_EXP] = GetExp();

        return json;
    }
    public override bool Deserialized(JsonData json)
    {
        if (json == null)
        {
            return false;
        }
        InitSecureVars();
        if (json.ContainsKey(NODE_PLAYER_NICKNAME))
        {
            Nickname = ParseString(json, NODE_PLAYER_NICKNAME);
        }
        if (json.ContainsKey(NODE_PLAYER_LEVEL))
        {
            Level.Set(ParseInt(json, NODE_PLAYER_LEVEL));
        }
        if (json.ContainsKey(NODE_PLAYER_EXP))
        {
            Exp.Set(ParseDouble(json, NODE_PLAYER_EXP));
        }

        InitMasterData();
        return true;
    }

    //-------------------------------------------------------------------------
    // Json Node Name
    //-------------------------------------------------------------------------

    protected const string NODE_PLAYER_NICKNAME = "nick";
    protected const string NODE_PLAYER_LEVEL = "lv";
    protected const string NODE_PLAYER_EXP = "xp";
}
