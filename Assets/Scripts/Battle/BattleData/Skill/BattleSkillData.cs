
using DocumentFormat.OpenXml.Drawing.Charts;
using System;
using System.Collections.Generic;

public enum UNIT_SKILL_TYPE
{
    NONE = 0,
    PC_SKILL,
    NPC_SKILL
}
public class BattleSkillData : BattleDataBase, ICloneable
{

    protected HeroBase_V2 Caster;
    protected List<HeroBase_V2> Targets = new List<HeroBase_V2>();

    /// <summary>
    /// 효과 비중 수
    /// </summary>
    protected int Max_Effect_Count;
    /// <summary>
    /// 효과 비중 인덱스
    /// </summary>
    protected int Effect_Weight_Index;

    /// <summary>
    /// 누가 사용하는 스킬인지 여부
    /// </summary>
    public UNIT_SKILL_TYPE Unit_Skill_Type { get; protected set; } = UNIT_SKILL_TYPE.NONE;

    /// <summary>
    /// 1회성 스킬 효과 데이터 리스트
    /// </summary>
    protected List<BattleOnetimeSkillData> Onetime_Skill_List = new List<BattleOnetimeSkillData> ();
    /// <summary>
    /// 지속성 스킬 효과 데이터 리스트
    /// </summary>
    protected List<BattleDurationSkillData> Duration_Skill_List = new List<BattleDurationSkillData>();

    public BattleSkillData(UNIT_SKILL_TYPE stype) {  Unit_Skill_Type = stype; }

    public virtual void ExecSkill(BATTLE_SEND_DATA data) 
    {
        //  onetime skill
        int cnt = Onetime_Skill_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Onetime_Skill_List[i].ExecSkill(data);
        }

        //  duration skill
        cnt = Duration_Skill_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Duration_Skill_List[i].ExecSkill(data);
        }
    }


    public virtual void SetSkillID(int skill_id) { }


    /// <summary>
    /// 날아가는 발사체인지, 타겟에서 즉시 발생하는 이펙트인지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsThrowingNode()
    {
        bool is_throwing = false;
        switch (GetProjectileType())
        {
            case PROJECTILE_TYPE.THROW_FOOT:
            case PROJECTILE_TYPE.THROW_BODY:
            case PROJECTILE_TYPE.THROW_HEAD:
                is_throwing = true;
                break;
            case PROJECTILE_TYPE.INSTANT_TARGET_FOOT:
            case PROJECTILE_TYPE.INSTANT_TARGET_BODY:
            case PROJECTILE_TYPE.INSTANT_TARGET_HEAD:
            case PROJECTILE_TYPE.ALL_ROUND:
                is_throwing = false;
                break;
            default:
                UnityEngine.Debug.Assert(false);
                break;
        }
        return is_throwing;
    }

   

    protected void AddOnetimeSkillData(BattleOnetimeSkillData onetime)
    {
        Onetime_Skill_List.Add(onetime);
    }

    protected void AddDurationSkillData(BattleDurationSkillData duration)
    {
        Duration_Skill_List.Add(duration);
    }

    #region Getter
    /// <summary>
    /// 이벤트 이름
    /// </summary>
    /// <param name="evt_name"></param>
    /// <returns></returns>
    public virtual BattleSkillData GetExecuableSkillData(string evt_name)
    {
        return null;
    }


    public int GetEffectWeightIndex() { return Effect_Weight_Index; }

    /// <summary>
    /// 임팩트 비중 정보 반환
    /// </summary>
    /// <param name="weight_index"></param>
    /// <returns></returns>
    public virtual int GetEffectWeightValue(int weight_index) { return 0; }


    public List<BattleOnetimeSkillData> GetOnetimeSkillDataList()
    {
        return Onetime_Skill_List;
    }
    public List<BattleDurationSkillData> GetDurationSkillDataList()
    {
        return Duration_Skill_List;
    }
    public virtual object GetSkillData() { return null; }

    public virtual string GetEffectPrefabPath() { return null; }

    public virtual PROJECTILE_TYPE GetProjectileType() { return PROJECTILE_TYPE.NONE; }

    public virtual double GetEffectDuration() { return 0; }

    public virtual TARGET_TYPE GetTargetType() { return TARGET_TYPE.MY_TEAM; }
    public virtual TARGET_RULE_TYPE GetTargetRuleType() { return TARGET_RULE_TYPE.RANDOM; }


    public virtual int GetTargetOrder() {  return 0; }
    public virtual int GetTargetCount() { return 0; }

    #endregion


    public void SetEffectWeightIndex(int weight_index)
    {
        Effect_Weight_Index = weight_index;
    }
    /// <summary>
    /// 세컨 타겟 룰
    /// </summary>
    /// <returns></returns>
    public virtual SECOND_TARGET_RULE_TYPE GetSecondTargetRuleType() { return SECOND_TARGET_RULE_TYPE.NONE; }
    /// <summary>
    /// 세컨 타겟 수
    /// </summary>
    /// <returns></returns>
    public virtual int GetSecondTargetCount() { return 0; }
    /// <summary>
    /// 세컨 타겟의 반경
    /// </summary>
    /// <returns></returns>
    public virtual double GetSecondTargetRadius() {  return 0; }

    /// <summary>
    /// 스킬 사용 후 데이터 리셋
    /// </summary>
    public virtual void ResetSkill() { }

    public virtual object Clone()
    {
        return null;
    }
}
