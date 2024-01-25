using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHeroSkillData : UserDataBase
{
    SecureVar<int> Skill_ID = null;
    SecureVar<int> Player_Character_ID = null;
    SecureVar<int> Player_Character_Num = null;
    SecureVar<int> Level = null;
    SecureVar<double> Exp = null;

    UserHeroData Hero_Data;
    Player_Character_Skill_Data Data;

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
        if (Skill_ID == null)
        {
            Skill_ID = new SecureVar<int>(0);
        }
        if (Level == null)
        {
            Level = new SecureVar<int>(1);
        }
        if (Exp == null)
        {
            Exp = new SecureVar<double>(0);
        }
    }

    public void SetSkillID(int player_character_id, int player_character_num, int skill_id)
    {
        this.Skill_ID.Set(skill_id);
        Player_Character_ID.Set(player_character_id);
        Player_Character_Num.Set(player_character_num);
        InitMasterData();
    }
    protected override void InitMasterData()
    {
        Data = MasterDataManager.Instance.Get_PlayerCharacterSkillData(GetSkillID());

        var mng = GameData.Instance.GetUserHeroDataManager();
        Hero_Data = mng.FindUserHeroData(GetPlayerCharacterID(), GetPlayerCharacterNum());
        Is_Update_Data = true;
    }

    public int GetSkillID() { return Skill_ID.Get(); }  
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

    

    public override JsonData Serialized()
    {
        return base.Serialized();
    }
    public override bool Deserialized(JsonData json)
    {
        return base.Deserialized(json);
    }
}
