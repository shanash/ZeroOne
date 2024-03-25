
using System;
using System.Collections.Generic;

public enum UNIT_SKILL_TYPE
{
    NONE = 0,
    PC_SKILL,
    NPC_SKILL
}
public abstract class BattleSkillData : BattleDataBase
{
    protected HeroBase_V2 Caster;
    protected List<HeroBase_V2> Find_Targets = new List<HeroBase_V2>();
    /// <summary>
    /// 효과 비중 수
    /// </summary>
    protected int Max_Effect_Count;
    /// <summary>
    /// 효과 비중 인덱스
    /// </summary>
    protected int Effect_Weight_Index;
    /// <summary>
    /// 1회성 스킬 효과 데이터 리스트
    /// </summary>
    protected List<BattleOnetimeSkillData> Onetime_Skill_List = new List<BattleOnetimeSkillData>();
    /// <summary>
    /// 세컨 타겟용 1회성 스킬 효과 데이터 리스트
    /// </summary>
    protected List<BattleOnetimeSkillData> Second_Target_Onetime_Skill_List = new List<BattleOnetimeSkillData>();
    /// <summary>
    /// 지속성 스킬 효과 데이터 리스트
    /// </summary>
    protected List<BattleDurationSkillData> Duration_Skill_List = new List<BattleDurationSkillData>();
    /// <summary>
    /// 세컨 타겟용 지속성 스킬 효과 데이터 리스트
    /// </summary>
    protected List<BattleDurationSkillData> Second_Target_Duration_Skill_List = new List<BattleDurationSkillData>();

    /// <summary>
    /// 누가 사용하는 스킬인지 여부
    /// </summary>
    public UNIT_SKILL_TYPE Unit_Skill_Type { get; protected set; } = UNIT_SKILL_TYPE.NONE;

    protected int Skill_Level;

    protected BattleSkillData(UNIT_SKILL_TYPE stype) : base() { Unit_Skill_Type = stype; }

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

    public void SetSkillLevel(int lv) 
    { 
        Skill_Level = lv;
        int cnt = Onetime_Skill_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Onetime_Skill_List[i].SetSkillLevel(lv);
        }
        cnt = Duration_Skill_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Duration_Skill_List[i].SetSkillLevel(lv);
        }
    }

    public int[] GetSkillLevel()
    {
        List<int> result = new List<int>();

        int cnt = Onetime_Skill_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            result.Add(Onetime_Skill_List[i].GetSkillLevel());
        }
        cnt = Duration_Skill_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            result.Add(Duration_Skill_List[i].GetSkillLevel());
        }

        return result.ToArray();
    }

    protected void AddOnetimeSkillData(BattleOnetimeSkillData onetime)
    {
        Onetime_Skill_List.Add(onetime);
    }

    protected void AddSecondTargetOnetimeSkillData(BattleOnetimeSkillData onetime)
    {
        Second_Target_Onetime_Skill_List.Add(onetime);
    }

    protected void AddDurationSkillData(BattleDurationSkillData duration)
    {
        Duration_Skill_List.Add(duration);
    }

    protected void AddSecondTargetDurationSkillData(BattleDurationSkillData duration)
    {
        Second_Target_Duration_Skill_List.Add(duration);
    }

    #region Getter


    /// <summary>
    /// 이벤트 이름으로 스킬 찾기(복사함)
    /// </summary>
    /// <param name="evt_name"></param>
    /// <returns></returns>
    public virtual BattleSkillData GetExecutableCloneSkillData(string evt_name)
    {
        return null;
    }
    /// <summary>
    /// 이벤트 이름으로 스킬 찾기(복사 안함)
    /// </summary>
    /// <param name="evt_name"></param>
    /// <returns></returns>
    public virtual BattleSkillData GetExecutableSkillData(string evt_name) { return null; }

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

    public List<BattleOnetimeSkillData> GetSecondTargetOnetimeSkillDataList()
    {
        return Second_Target_Onetime_Skill_List;
    }

    public List<BattleDurationSkillData> GetDurationSkillDataList()
    {
        return Duration_Skill_List;
    }
    public List<BattleDurationSkillData> GetSecondTargetDurationSkillDataList()
    {
        return Second_Target_Duration_Skill_List;
    }

    public virtual object GetSkillData() { return null; }
    /// <summary>
    /// 트리거용 이펙트 프리팹 패스 반환
    /// </summary>
    /// <returns></returns>
    public virtual string GetTriggerEffectPrefabPath() { return null; }

    /// <summary>
    /// 스킬 아이디 반환
    /// </summary>
    /// <returns></returns>
    public virtual int GetSkillID() {  return 0; }


    /// <summary>
    /// 이펙트 카운트 타입<br/>
    /// 단일 이펙트인지<br/>
    /// 각 타겟별 개별적으로 구현되는 이펙트인지 여부
    /// </summary>
    /// <returns></returns>
    public virtual EFFECT_COUNT_TYPE GetEffectCountType() { return EFFECT_COUNT_TYPE.NONE; }

    public virtual TARGET_TYPE GetTargetType() { return TARGET_TYPE.MY_TEAM; }
    
    public virtual TARGET_RULE_TYPE GetTargetRuleType() { return TARGET_RULE_TYPE.RANDOM; }

    public virtual int GetTargetOrder() { return 0; }

    public virtual int GetTargetCount() { return 0; }

    public virtual float GetTargetRange() { return 0; }

    /// <summary>
    /// 세컨 타겟 룰
    /// </summary>
    /// <returns></returns>
    public virtual SECOND_TARGET_RULE_TYPE GetSecondTargetRuleType() { return SECOND_TARGET_RULE_TYPE.NONE; }

    /// <summary>
    /// 세컨 타겟 수
    /// </summary>
    /// <returns></returns>
    public virtual int GetMaxSecondTargetCount() { return 0; }
    
    /// <summary>
    /// 세컨 타겟의 반경
    /// </summary>
    /// <returns></returns>
    public virtual double GetSecondTargetRange() { return 0; }

    #endregion

    public void SetEffectWeightIndex(int weight_index)
    {
        Effect_Weight_Index = weight_index;
    }
    /// <summary>
    /// 이펙트 비중 횟수
    /// </summary>
    /// <returns></returns>
    public int GetMaxEffectWeightCount()
    {
        return Max_Effect_Count;
    }

    /// <summary>
    /// 타겟 추가
    /// </summary>
    /// <param name="t"></param>
    public void AddFindTarget(HeroBase_V2 t)
    {
        if (!Find_Targets.Contains(t))
        {
            Find_Targets.Add(t);
        }
    }
    /// <summary>
    /// 타겟 리스트 추가
    /// </summary>
    /// <param name="targets"></param>
    public void AddFindTargets(List<HeroBase_V2> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            AddFindTarget(targets[i]);
        }
    }

    public List<HeroBase_V2> GetFindTargets()
    {
        return Find_Targets;
    }
    /// <summary>
    /// 타겟이 비어 있는지 여부 판단
    /// </summary>
    /// <returns></returns>
    public bool IsEmptyFindTarget()
    {
        return Find_Targets.Count == 0;
    }

    /// <summary>
    /// 스킬 사용 후 데이터 리셋
    /// </summary>
    public virtual void ResetSkill() 
    {
        Find_Targets.Clear();
    }
    /// <summary>
    /// 스킬 설명
    /// </summary>
    /// <returns></returns>
    public virtual string GetSkillDesc() { return string.Empty; }
}
