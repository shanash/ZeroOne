using System.Collections.Generic;
using System.Linq;

public class BattleSkillManager : BattleDataBase
{
    /// <summary>
    /// 스킬 사용 순서 인덱스
    /// </summary>
    int Skill_Pattern_Order;


    List<BattleSkillGroup> Skill_Groups = new List<BattleSkillGroup>();


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
            var battle_group = new BattlePcSkillGroup();
            battle_group.SetSkillOrder(i);
            battle_group.SetSkillGroupID(skill_group_id);
            Skill_Groups.Add(battle_group);
        }
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
            var battle_group = new BattleNpcSkillGroup();
            battle_group.SetSkillOrder(i);
            battle_group.SetSkillGroupID(skill_group_id);
            Skill_Groups.Add(battle_group);
        }
    }

    /// <summary>
    /// 현재 진행중인 스킬 그룹 반환
    /// 해당 스킬 사용후에는 다음 스킬로 넘어가야 함
    /// </summary>
    /// <returns></returns>
    public BattleSkillGroup GetCurrentSkillGroup()
    {
        if (Skill_Groups.Count > 0)
        {
            return Skill_Groups.Find(x => x.Skill_Order == Skill_Pattern_Order);
        }
        return null;
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
        int max_order = Skill_Groups.Max(x => x.Skill_Order);
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

    /// <summary>
    /// 스킬 타입의 스킬 그룹 데이터 반환
    /// </summary>
    /// <param name="stype"></param>
    /// <returns></returns>
    public BattleSkillGroup FindSkillType(SKILL_TYPE stype)
    {
        return Skill_Groups.Find(x => x.GetSkillType() == stype);
    }

}
