
public class BattleNpcSkillData : BattleSkillData, FluffyDuck.Util.Factory.IProduct
{
    Npc_Skill_Data Data;

    protected BattleNpcSkillData() : base(UNIT_SKILL_TYPE.NPC_SKILL) { }

    protected virtual bool Initialize(Npc_Skill_Data data)
    {
        SetSkillData(data);
        return true;
    }

    public override void SetSkillID(int skill_id)
    {
        var m = MasterDataManager.Instance;
        SetSkillData(m.Get_NpcSkillData(skill_id));
    }

    void SetSkillData(Npc_Skill_Data data)
    {
        Data = data;
        //  npc onetime skill
        int len = Data.onetime_effect_ids.Length;
        for (int i = 0; i < len; i++)
        {
            int onetime_skill_id = Data.onetime_effect_ids[i];
            if (onetime_skill_id == 0)
            {
                continue;
            }
            var battle_onetime = BattleSkillDataFactory.CreateNpcBattleOnetimeSkillData(onetime_skill_id);
            AddOnetimeSkillData(battle_onetime);
        }

        //  npc duratio skill
        len = Data.duration_effect_ids.Length;
        for (int i = 0; i < len; i++)
        {
            int duration_skill_id = Data.duration_effect_ids[i];
            if (duration_skill_id == 0)
            {
                continue;
            }
            var battle_duration = BattleSkillDataFactory.CreateNpcBattleDurationSkillData(duration_skill_id);
            battle_duration.SetDurationSkillDataID(duration_skill_id);
            AddDurationSkillData(battle_duration);
        }

        //  npc second target onetime skill
        if (Data.second_target_onetime_effect_ids != null)
        {
            len = Data.second_target_onetime_effect_ids.Length;
            for (int i = 0; i < len; i++)
            {
                int onetime_skill_id = Data.second_target_onetime_effect_ids[i];
                if (onetime_skill_id == 0)
                {
                    continue;
                }
                var battle_onetime = BattleSkillDataFactory.CreatePcBattleOnetimeSkillData(onetime_skill_id);
                if (battle_onetime != null)
                {
                    AddSecondTargetOnetimeSkillData(battle_onetime);
                }
            }
        }

        //  npc second target duratio skill
        if (Data.second_target_duration_effect_ids != null)
        {
            len = Data.second_target_duration_effect_ids.Length;
            for (int i = 0; i < len; i++)
            {
                int duration_skill_id = Data.second_target_duration_effect_ids[i];
                if (duration_skill_id == 0)
                {
                    continue;
                }
                var battle_duration = BattleSkillDataFactory.CreatePcBattleDurationSkillData(duration_skill_id);
                if (battle_duration != null)
                {
                    AddSecondTargetDurationSkillData(battle_duration);
                }
            }
        }

        //  스킬 효과 비중 횟수
        Max_Effect_Count = Data.effect_weight.Length;
        Effect_Weight_Index = 0;
    }


    #region Getter
    public override BattleSkillData GetExecuableSkillData(string evt_name)
    {
        if (Data.event_name.Equals(evt_name))
        {
            if (Effect_Weight_Index < Max_Effect_Count)
            {
                var clone = (BattleSkillData)Clone();
                clone.SetEffectWeightIndex(Effect_Weight_Index);
                Effect_Weight_Index++;
                return clone;
            }
        }
        return null;
    }

    public override object GetSkillData()
    {
        return Data;
    }

    public override string GetTriggerEffectPrefabPath()
    {
        return Data.trigger_effect_path;
    }

    //public override PROJECTILE_TYPE GetProjectileType()
    //{
    //    return Data.projectile_type;
    //}

    //public override double GetProjectileSpeed()
    //{
    //    return Data.projectile_speed;
    //}

    public override EFFECT_COUNT_TYPE GetEffectCountType()
    {
        return Data.effect_count_type;
    }
    public override int GetEffectWeightValue(int weight_index)
    {
        int weight = 100;
        if (weight_index < Data.effect_weight.Length)
        {
            weight = Data.effect_weight[weight_index];
        }
        return weight;
    }

    public override TARGET_TYPE GetTargetType()
    {
        return Data.target_type;
    }
    public override TARGET_RULE_TYPE GetTargetRuleType()
    {
        return Data.target_rule_type;
    }
    public override int GetTargetOrder()
    {
        return Data.target_order;
    }
    public override int GetTargetCount()
    {
        return Data.target_count;
    }
    public override float GetTargetRange()
    {
        return (float)Data.target_range;
    }

    public override SECOND_TARGET_RULE_TYPE GetSecondTargetRuleType()
    {
        if (Data != null)
        {
            return Data.second_target_rule;
        }
        return SECOND_TARGET_RULE_TYPE.NONE;
    }
    public override int GetMaxSecondTargetCount()
    {
        if (Data != null)
        {
            return Data.max_second_target_count;
        }
        return 0;
    }
    public override double GetSecondTargetRange()
    {
        if (Data != null)
        {
            return Data.second_target_range;
        }
        return 0;
    }

    #endregion
    public override string ToString()
    {
        return Data.ToString();
    }


    public override void ResetSkill()
    {
        Effect_Weight_Index = 0;
    }

    public override object Clone()
    {
        return FluffyDuck.Util.Factory.Instantiate<BattleNpcSkillData>(Data);
    }
}
