using Cysharp.Text;
using System.Collections.Generic;

public class BattlePcSkillGroup : BattleSkillGroup
{
    protected Player_Character_Skill_Group Skill_Group;
    protected UserHeroSkillData User_Data;

    public BattlePcSkillGroup(UserHeroSkillData skill) : base(UNIT_SKILL_TYPE.PC_SKILL) 
    {
        User_Data = skill;
    }

    public override void SetSkillGroupID(int skill_group_id)
    {
        var m = MasterDataManager.Instance;
        Skill_Group = m.Get_PlayerCharacterSkillGroupData(skill_group_id);

        List<Player_Character_Skill_Data> skill_list = new List<Player_Character_Skill_Data>();
        m.Get_PlayerCharacterSkillDataListBySkillGroup(skill_group_id, ref skill_list);

        int cnt = skill_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var skill = skill_list[i];
            var battle_skill = FluffyDuck.Util.Factory.Instantiate<BattlePcSkillData>(skill);
            battle_skill.SetSkillLevel(GetSkillLevel());
            Battle_Skill_Data_List.Add(battle_skill);
        }

        //  첫번째 스킬, 최초 1회에 한하여 선쿨타임이 없음
        if (Skill_Order == 0)
        {
            SetFirstSkill(true);
        }
        ResetDelayTime();
    }

    public UserHeroSkillData GetUserHeroSkillData()
    {
        return User_Data;
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
    public override double GetRemainCooltime()
    {
        double remain_cooltime = Delay_Time;
        if (remain_cooltime < 0)
        {
            remain_cooltime = 0;
        }
        return remain_cooltime;
    }
    public override string GetSkillActionName()
    {
        return Skill_Group.action_name;
    }
    public override string GetSkillName()
    {
        if (Skill_Group != null)
        {
            return GameDefine.GetLocalizeString(Skill_Group.name_id);
        }
        return null;
    }

    public override string GetSkillDesc()
    {
        var sb = ZString.CreateStringBuilder();
        for (int i = 0; i < Battle_Skill_Data_List.Count; i++)
        {
            sb.Append(Battle_Skill_Data_List[i].GetSkillDesc().TrimEnd());
        }

        return sb.ToString();
    }

    public override List<string> GetSkillDescList()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < Battle_Skill_Data_List.Count; i++)
        {
            var desc = Battle_Skill_Data_List[i].GetSkillDesc();
            if (!string.IsNullOrEmpty(desc))
            {
                list.Add(desc);
            }
        }
        return list;
    }

    public override SKILL_TYPE GetSkillType()
    {
        if (Skill_Group != null)
            return Skill_Group.skill_type;
        return SKILL_TYPE.NONE;
    }
    public override string[] GetSkillCastEffectPath()
    {
        return Skill_Group.cast_effect_path;
    }

    protected override int GetSpecialSkillTargetSkillID()
    {
        return Skill_Group.target_skill_id;
    }

    public override int GetSkillLevel()
    {
        if (Skill_Group.skill_type != SKILL_TYPE.NORMAL_ATTACK)
        {
            return User_Data.GetLevel();
        }
        return 0;
    }
    public override void SetSkillLevel(int lv)
    {
        if (User_Data != null)
        {
            User_Data.SetLevel(lv);
        }

        for (int i = 0; i < Battle_Skill_Data_List.Count; i++)
        {
            Battle_Skill_Data_List[i].SetSkillLevel(lv);
        }
    }
    /// <summary>
    /// 객체를 복사한 후, 해당 객체의 레벨을 지정<br/>
    /// 변경된 스탯 값을 가져올 수 있다.
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public BattlePcSkillGroup GetCloneSimulateLevelUpData(int lv)
    {
        BattlePcSkillGroup clone = (BattlePcSkillGroup)this.Clone();
        clone.SetSkillLevel(lv);
        //clone.User_Data.SetLevel(lv);
        return clone;
    }

    public override object Clone()
    {
        BattlePcSkillGroup clone = (BattlePcSkillGroup)MemberwiseClone();
        clone.User_Data = (UserHeroSkillData)User_Data.Clone();
        clone.Battle_Skill_Data_List.Clear();
        clone.SetSkillGroupID(User_Data.GetSkillGroupID());
        return clone;
    }

}
