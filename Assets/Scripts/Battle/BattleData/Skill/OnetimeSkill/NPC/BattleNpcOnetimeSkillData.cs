
public class BattleNpcOnetimeSkillData : BattleOnetimeSkillData
{
    protected Npc_Skill_Onetime_Data Data;

    public override void SetOnetimeSkillDataID(int skill_onetime_id)
    {
        Data = MasterDataManager.Instance.Get_NpcSkillOnetimeData(skill_onetime_id);
    }

    public override object GetOnetimeSkillData()
    {
        return Data;
    }

    public override string GetEffectPrefab()
    {
        return Data.effect_path;
    }

    public override double GetEffectDuration()
    {
        return Data.effect_duration;
    }

    public override PROJECTILE_TYPE GetProjectileType()
    {
        return Data.projectile_type;
    }



    public override string ToString()
    {
        return Data.ToString();
    }

    public override object Clone()
    {
        var clone = new BattleNpcOnetimeSkillData();
        clone.SetOnetimeSkillDataID(Data.npc_skill_onetime_id);
        return clone;
    }
}
