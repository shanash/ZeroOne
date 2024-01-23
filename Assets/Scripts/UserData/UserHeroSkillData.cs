using FluffyDuck.Util;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserHeroSkillData : UserDataBase
{
    SecureVar<int> Skill_ID = null;
    SecureVar<int> Level = null;
    SecureVar<double> Exp = null;

    public UserHeroSkillData() : base() { }

    protected override void Reset()
    {
        InitSecureVars();
    }

    protected override void InitSecureVars()
    {
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

    public void SetSkillID(int skill_id)
    {
        this.Skill_ID.Set(skill_id);
        InitMasterData();
    }
    protected override void InitMasterData()
    {
        
    }

    public int GetSkillID() { return Skill_ID.Get(); }  
    public int GetLevel() { return Level.Get(); }
    public double GetExp() { return Exp.Get(); }


    public override JsonData Serialized()
    {
        return base.Serialized();
    }
    public override bool Deserialized(JsonData json)
    {
        return base.Deserialized(json);
    }
}
