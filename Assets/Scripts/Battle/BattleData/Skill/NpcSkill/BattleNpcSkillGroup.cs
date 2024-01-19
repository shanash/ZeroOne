using DocumentFormat.OpenXml.Drawing.Charts;
using System.Collections.Generic;
using UnityEngine;

public class BattleNpcSkillGroup : BattleSkillGroup
{
    protected Npc_Skill_Group Skill_Group;

    public BattleNpcSkillGroup() : base(UNIT_SKILL_TYPE.NPC_SKILL) { }

    public override void SetSkillGroupID(int skill_group_id)
    {
        var m = MasterDataManager.Instance;

        Skill_Group = m.Get_NpcSkillGroup(skill_group_id);

        Debug.Assert(Skill_Group != null);

        List<Npc_Skill_Data> skill_list = new List<Npc_Skill_Data>();
        m.Get_NpcSkillDataListBySkillGroup(skill_group_id, ref skill_list);

        int cnt = skill_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var skill = skill_list[i];
            var battle_skill = FluffyDuck.Util.Factory.Instantiate<BattleNpcSkillData>(skill);
            Battle_Skill_Data_List.Add(battle_skill);
        }

        //  첫번째 스킬, 최초 1회에 한하여 선쿨타임이 없음
        if (Skill_Order == 0)
        {
            SetFirstSkill(true);
        }
        ResetDelayTime();
    }

    public override void ResetDelayTime()
    {
        if (Is_First_Skill)
        {
            SetDelayTime(0);
        }
        else
        {
            SetDelayTime(Skill_Group.skill_use_delay);
        }
    }
    public override double GetCooltime()
    {
        return Skill_Group.skill_use_delay;
    }

    public override string GetSkillActionName()
    {
        if (Skill_Group != null)
        {
            return Skill_Group.action_name;
        }
        return null;
    }

    public override SKILL_TYPE GetSkillType()
    {
        if (Skill_Group != null)
            return Skill_Group.skill_type;
        return SKILL_TYPE.NONE;
    }

    public override string GetSkillCastEffectPath()
    {
        return Skill_Group.cast_effect_path;
    }

    protected override int GetSpecialSkillTargetSkillID()
    {
        return Skill_Group.target_skill_id;
    }
}
