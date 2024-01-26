using Cysharp.Text;
using System.Collections.Generic;

public abstract class BattleSkillGroup : BattleDataBase
{
    protected List<BattleSkillData> Battle_Skill_Data_List;
    protected double Delay_Time;
    protected bool Is_First_Skill;

    public int Skill_Order { get; protected set; }
    public UNIT_SKILL_TYPE Unit_Skill_Type { get; protected set; }

    protected BattleSkillGroup(UNIT_SKILL_TYPE stype) : base() { Unit_Skill_Type = stype; }

    protected override void Reset()
    {
        base.Reset();

        Battle_Skill_Data_List = new List<BattleSkillData>();
        Delay_Time = 0;
        Is_First_Skill = false;

        Skill_Order = 0;
        Unit_Skill_Type = UNIT_SKILL_TYPE.NONE;
    }

    /// <summary>
    /// 스킬 그룹 아이디 세팅
    /// </summary>
    /// <param name="skill_group_id"></param>
    public abstract void SetSkillGroupID(int skill_group_id);

    /// <summary>
    /// 스킬 리스트 반환
    /// </summary>
    /// <returns></returns>
    public List<BattleSkillData> GetBattleSkillDataList() { return Battle_Skill_Data_List; }

    /// <summary>
    /// 스킬 오더 설정
    /// </summary>
    /// <param name="order"></param>
    public void SetSkillOrder(int order) { this.Skill_Order = order; }

    /// <summary>
    /// 첫번째 스킬 가져오기
    /// </summary>
    /// <returns></returns>
    public virtual BattleSkillData GetFirstSkillData()
    {
        if (Battle_Skill_Data_List.Count > 0)
        {
            return Battle_Skill_Data_List[0];
        }
        return null;
    }

    protected virtual int GetSpecialSkillTargetSkillID()
    {
        return 0;
    }

    public BattleSkillData GetSpecialSkillTargetSkill()
    {
        return Battle_Skill_Data_List.Find(x => x.GetSkillID() == GetSpecialSkillTargetSkillID());
    }

    /// <summary>
    /// 본 스킬의 선 쿨타임 시간을 지정한다(공격 대기 시간)
    /// </summary>
    /// <param name="delay"></param>
    protected void SetDelayTime(double delay)
    {
        Delay_Time = delay;
    }

    public virtual double GetCooltime() {  return 0; }
    public virtual double GetRemainCooltime() { return 0; }

    /// <summary>
    /// 스킬 액션 이름 반환
    /// </summary>
    /// <returns></returns>
    public abstract string GetSkillActionName();

    /// <summary>
    /// 첫번째 스킬의 경우 선쿨타임이 없음
    /// </summary>
    /// <param name="is_first"></param>
    public void SetFirstSkill(bool is_first)
    {
        Is_First_Skill = is_first;
        if (Is_First_Skill)
        {
            Delay_Time = 0;
        }
    }

    /// <summary>
    /// 대기 시간 계산
    /// 대기시간이 모두 소진되면 true,
    /// 아직 대기시간이 남아있으면 false
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public bool CalcDelayTime(float dt)
    {
        Delay_Time -= dt;
        if (Delay_Time < 0)
        {
            Is_First_Skill = false;
            return true;
        }
        return false;
    }

    public bool IsPrepareCooltime()
    {
        return Delay_Time < 0;
    }

    /// <summary>
    /// 공격 대기시간 초기화
    /// </summary>
    public abstract void ResetDelayTime();

    /// <summary>
    /// 스킬 그룹 내의 모든 스킬 실행.
    /// 지정된 타겟에서 적용
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="target"></param>
    public void ExecSkillGroup(HeroBase_V2 caster, HeroBase_V2 target)
    {
    }

    /// <summary>
    /// 스킬 그룹 내의 모든 스킬을 실행
    /// 모든 타겟에 적용
    /// </summary>
    /// <param name="caster"></param>
    /// <param name="targets"></param>
    public void ExecSkillGroup(HeroBase_V2 caster, List<HeroBase_V2> targets)
    {
    }

    /// <summary>
    /// 스킬 사용가능한 데이터 반환
    /// 스킬 효과를 불러올 때 사용가능한 스킬 정보를 가져온다.
    /// 스킬 효과의 비중 횟수에 따라 다름
    /// </summary>
    /// <param name="evt_name"></param>
    /// <returns></returns>
    public List<BattleSkillData> GetExecuableSkillDatas(string evt_name)
    {
        List<BattleSkillData> execuable_list = new List<BattleSkillData>();
        int cnt = Battle_Skill_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var skill = Battle_Skill_Data_List[i].GetExecuableSkillData(evt_name);
            if (skill != null)
            {
                execuable_list.Add(skill);
            }
        }
        return execuable_list;
    }

    /// <summary>
    /// 스킬 사용 후 초기화
    /// </summary>
    public void ResetSkill()
    {
        int cnt = Battle_Skill_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Battle_Skill_Data_List[i].ResetSkill();
        }

        ResetDelayTime();
    }

    public abstract SKILL_TYPE GetSkillType();

    public abstract string[] GetSkillCastEffectPath();

    public virtual string GetSkillIconPath() { return "Assets/AssetResources/Textures/Icons/Icon_Skill_Dummy"; }

    public override string ToString()
    {
        var sb = ZString.CreateStringBuilder();

        sb.AppendLine($"[{nameof(Skill_Order)}] <color=yellow>[{Skill_Order}]</color>");
        sb.AppendLine($"[{nameof(Unit_Skill_Type)}] <color=yellow>[{Unit_Skill_Type}]</color>");

        int cnt = Battle_Skill_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            sb.AppendLine(Battle_Skill_Data_List[i].ToString());
        }
        return sb.ToString();
    }
}
