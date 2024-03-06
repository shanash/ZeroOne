public class BattleNpcOnetimeSkillData : BattleOnetimeSkillData
{
    protected Npc_Skill_Onetime_Data Data;

    protected BattleNpcOnetimeSkillData() { }

    protected virtual bool Initialize(Npc_Skill_Onetime_Data data)
    {
        Data = data;
        return true;
    }

    public override void SetOnetimeSkillDataID(int skill_onetime_id)
    {
        Data = MasterDataManager.Instance.Get_NpcSkillOnetimeData(skill_onetime_id);
    }

    public override object GetOnetimeSkillData()
    {
        return Data;
    }
    public override ONETIME_EFFECT_TYPE GetOnetimeEffectType()
    {
        return Data.onetime_effect_type;
    }
    public override ATTRIBUTE_TYPE GetAttributeType()
    {
        return Data.attribute_type;
    }
    public override STAT_MULTIPLE_TYPE GetStatMultipleType()
    {
        return Data.multiple_type;
    }
    public override string GetEffectPrefab()
    {
        return Data.effect_path;
    }

    public override double GetValue()
    {
        return Data.value + (GetSkillLevel() - 1) * Data.up_value;
    }
    public override double GetMultiple()
    {
        return Data.multiple + (GetSkillLevel() - 1) * Data.up_multiple;
    }


    public override string ToString()
    {
        return Data.ToString();
    }

    public override object Clone()
    {
        return FluffyDuck.Util.Factory.Instantiate<BattleNpcOnetimeSkillData>(Data);
    }
}
