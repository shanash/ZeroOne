

using System;

public class BattleOnetimeSkillData : BattleDataBase, ICloneable, FluffyDuck.Util.Factory.IProduct
{
    public virtual void SetOnetimeSkillDataID(int skill_onetime_id) { }

    public virtual object GetOnetimeSkillData() { return null; }

    public virtual string GetEffectPrefab() { return null; }

    public virtual ONETIME_EFFECT_TYPE GetOnetimeEffectType() { return ONETIME_EFFECT_TYPE.NONE; }

   
    public virtual void ExecSkill(BATTLE_SEND_DATA data)
    {
    }

    public virtual object Clone()
    {
        return null;
    }
}
