using System;

public class BattleOnetimeSkillData : BattleDataBase, FluffyDuck.Util.Factory.IProduct
{
    protected int Skill_Level;

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

    public void SetSkillLevel(int lv) { Skill_Level = lv; }

    public int GetSkillLevel() { return Skill_Level; }

    public virtual STAT_MULTIPLE_TYPE GetStatMultipleType() {  return STAT_MULTIPLE_TYPE.NONE; }

    /// <summary>
    /// 절대값 - 레벨에 따른 증가값 적용해야 함
    /// </summary>
    /// <returns></returns>
    public virtual double GetValue() {  return 0; }
    /// <summary>
    /// 멀티플 값 - 레벨에 따른 증가값 적용해야 함
    /// </summary>
    /// <returns></returns>
    public virtual double GetMultiple() { return 0; }
}
