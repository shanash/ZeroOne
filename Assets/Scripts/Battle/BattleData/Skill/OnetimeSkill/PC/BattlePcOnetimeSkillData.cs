
public class BattlePcOnetimeSkillData : BattleOnetimeSkillData
{
    protected Player_Character_Skill_Onetime_Data Data;

    protected BattlePcOnetimeSkillData() { }

    protected virtual bool Initialize(Player_Character_Skill_Onetime_Data data)
    {
        Data = data;
        return true;
    }

    public override void SetOnetimeSkillDataID(int skill_onetime_id)
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_PlayerCharacterSkillOnetimeData(skill_onetime_id);
    }

    public override object GetOnetimeSkillData()
    {
        return Data;
    }
    public override ONETIME_EFFECT_TYPE GetOnetimeEffectType()
    {
        return Data.onetime_effect_type;
    }
    public override string GetEffectPrefab()
    {
        return Data.effect_path;
    }

    public override string ToString()
    {
        return Data.ToString();
    }

    public override object Clone()
    {
        return FluffyDuck.Util.Factory.Instantiate<BattlePcOnetimeSkillData>(Data);
    }
}
