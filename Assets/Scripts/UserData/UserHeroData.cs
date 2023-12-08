using FluffyDuck.Util;

public class UserHeroData : UserDataBase
{
    public SecureVar<int> Player_Character_ID { get; protected set; } = null;
    public int Player_Character_Num { get; protected set; } = 0;
    public SecureVar<int> Level { get; protected set; } = null;
    public SecureVar<int> Exp { get; protected set; } = null;

    Player_Character_Data Hero_Data;
    Player_Character_Battle_Data Battle_Data;

    public UserHeroData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
        Player_Character_Num = 0;
        Player_Character_ID.Set(0);
        Level.Set(1);
        Exp.Set(0);
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
            Exp = new SecureVar<int>();
        }
    }
    public void SetHeroDataID(int hero_data_id, int hero_data_num)
    {
        Player_Character_ID.Set(hero_data_id);
        Player_Character_Num = hero_data_num;
        InitMasterData();
        Is_Update_Data = true;
    }
    protected override void InitMasterData()
    {
        var m = MasterDataManager.Instance;
        Hero_Data = m.Get_PlayerCharacterData(GetPlayerCharacterID());
        Battle_Data = m.Get_PlayerCharacterBattleData(Hero_Data.battle_info_id);
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
    public int GetExp()
    {
        return Exp.Get();
    }
    public POSITION_TYPE GetPositionType()
    {
        return Battle_Data.position_type;
    }

}
