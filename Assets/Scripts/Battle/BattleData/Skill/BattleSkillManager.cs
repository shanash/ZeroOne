using System.Collections.Generic;
using System.Linq;

public class BattleSkillManager : BattleDataBase
{
    /// <summary>
    /// 스킬 사용 순서 인덱스
    /// </summary>
    int Skill_Pattern_Order;
    List<BattleSkillGroup> _Skill_Groups;

    public IReadOnlyList<BattleSkillGroup> Skill_Groups => _Skill_Groups.AsReadOnly();

    protected override void Reset()
    {
        base.Reset();
        Skill_Pattern_Order = 0;
        _Skill_Groups = new List<BattleSkillGroup>();
    }

    /// <summary>
    /// 플레이어 캐릭터의 스킬 패턴에 따른 스킬 그룹 추가
    /// </summary>
    /// <param name="skill_patterns"></param>
    public void SetPlayerCharacterSkillGroups(int[] skill_patterns)
    {
        int len = skill_patterns.Length;
        for (int i = 0; i < len; i++)
        {
            int skill_group_id = skill_patterns[i];
            if (skill_group_id == 0)
                continue;
            AddPlayerCharacterBattleSkillGroup(skill_group_id, i);
        }
    }

    public void SetPlayerCharacterSpecialSkillGroup(int special_skill_id)
    {
        if (special_skill_id == 0)
        {
            return;
        }
        AddPlayerCharacterBattleSkillGroup(special_skill_id, 10);
    }

    void AddPlayerCharacterBattleSkillGroup(int skill_group_id, int order)
    {
        var grp = new BattlePcSkillGroup();
        grp.SetSkillOrder(order);
        grp.SetSkillGroupID(skill_group_id);
        _Skill_Groups.Add(grp);
    }

    /// <summary>
    /// npc 의 스킬 패턴에 따른 스킬 그룹 추가
    /// </summary>
    /// <param name="skill_patterns"></param>
    public void SetNpcSkillGroups(int[] skill_patterns)
    {
        int len = skill_patterns.Length;
        for (int i = 0; i < len; i++)
        {
            int skill_group_id = skill_patterns[i];
            if (skill_group_id == 0)
            {
                continue;
            }
            AddNpcBattleSkillGroup(skill_group_id, i);
        }
    }
    public void SetNpcSpecialSkillGroup(int special_skill_id)
    {
        if (special_skill_id == 0)
        {
            return;
        }
        AddNpcBattleSkillGroup(special_skill_id, 10);
    }
    void AddNpcBattleSkillGroup(int skill_group_id, int order)
    {
        var grp = new BattleNpcSkillGroup();
        grp.SetSkillOrder(order);
        grp.SetSkillGroupID(skill_group_id) ;
        _Skill_Groups.Add(grp);
    }

    /// <summary>
    /// 현재 진행중인 스킬 그룹 반환<br/>
    /// 해당 스킬 사용후에는 다음 스킬로 넘어가야 함
    /// </summary>
    /// <returns></returns>
    public BattleSkillGroup GetCurrentSkillGroup()
    {
        var skill_list = _Skill_Groups.FindAll(x => x.GetSkillType() != SKILL_TYPE.SPECIAL_SKILL);
        if (skill_list.Count > 0)
        {
            return skill_list.Find(x => x.Skill_Order == Skill_Pattern_Order);
        }
        return null;
    }
    /// <summary>
    /// 궁극기 스킬 데이터 반환
    /// </summary>
    /// <returns></returns>
    public BattleSkillGroup GetSpecialSkillGroup()
    {
        return FindSkillType(SKILL_TYPE.SPECIAL_SKILL);
    }

    /// <summary>
    /// 다음 스킬 패턴으로 이동
    /// </summary>
    public void SetNextSkillPattern()
    {
        //  현재 스킬 공격 대기시간 초기화
        var cur_skill = GetCurrentSkillGroup();
        cur_skill?.ResetSkill();

        //  다음 스킬로 이동
        Skill_Pattern_Order += 1;
        var skill_list = _Skill_Groups.FindAll(x => x.GetSkillType() != SKILL_TYPE.SPECIAL_SKILL);
        int max_order = skill_list.Max(x => x.Skill_Order);
        if (Skill_Pattern_Order > max_order)
        {
            Skill_Pattern_Order = 0;
        }
    }

    /// <summary>
    /// 현재 사용가능한 스킬의 선 쿨타임 진행
    /// </summary>
    /// <param name="delta_time"></param>
    /// <returns></returns>
    public bool CalcSkillUseDelay(float delta_time)
    {
        return GetCurrentSkillGroup().CalcDelayTime(delta_time);
    }

    public bool CalcSpecialSkillCooltime(float delta_time)
    {
        return GetSpecialSkillGroup().CalcDelayTime(delta_time);
    }

    /// <summary>
    /// 스킬 타입의 스킬 그룹 데이터 반환
    /// </summary>
    /// <param name="stype"></param>
    /// <returns></returns>
    public BattleSkillGroup FindSkillType(SKILL_TYPE stype)
    {
        return _Skill_Groups.Find(x => x.GetSkillType() == stype);
    }
}
