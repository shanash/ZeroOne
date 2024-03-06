using System;

public class BattleOnetimeSkillData : BattleDataBase, FluffyDuck.Util.Factory.IProduct
{
    public virtual void SetOnetimeSkillDataID(int skill_onetime_id) { }

    public virtual object GetOnetimeSkillData() { return null; }

    public virtual string GetEffectPrefab() { return null; }
    /// <summary>
    /// 일회성 효과 스킬의 속성 타입 반환
    /// </summary>
    /// <returns></returns>
    public virtual ATTRIBUTE_TYPE GetAttributeType() { return ATTRIBUTE_TYPE.NONE; }

    public virtual ONETIME_EFFECT_TYPE GetOnetimeEffectType() { return ONETIME_EFFECT_TYPE.NONE; }

   
    public virtual void ExecSkill(BATTLE_SEND_DATA data)
    {
    }
}
