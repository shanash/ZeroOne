public class BattlePcDurationSkillData : BattleDurationSkillData
{
    protected Player_Character_Skill_Duration_Data Data;

    protected BattlePcDurationSkillData() { }

    protected virtual bool Initialize(Player_Character_Skill_Duration_Data data)
    {
        SetDurationSkillData(data);
        return true;
    }

    bool Initialize(Player_Character_Skill_Duration_Data data, BATTLE_SEND_DATA send_data)
    {
        SetDurationSkillData(data);
        SetBattleSendData(send_data);
        return true;
    }

    bool Initialize(Player_Character_Skill_Duration_Data data, BATTLE_SEND_DATA send_data, SkillEffectBase skill_effect)
    {
        SetDurationSkillData(data);
        SetBattleSendData(send_data);
        SetSkillEffect(skill_effect);
        return true;
    }

    public override void SetDurationSkillDataID(int skill_duration_id)
    {
        SetDurationSkillData(MasterDataManager.Instance.Get_PlayerCharacterSkillDurationData(skill_duration_id));
    }

    void SetDurationSkillData(Player_Character_Skill_Duration_Data data)
    {
        Data = data;
        Duration = (float)Data.time;
        Effect_Count = Data.count;
        Repeat_Interval = (float)Data.repeat_interval;

        Repeat_Onetime_Effect_Data_List.Clear();
        Finish_Onetime_Effect_Data_List.Clear();

        //  반복주기 일회성 효과
        int cnt = Data.repeat_pc_onetime_ids.Length;
        for (int i = 0; i < cnt; i++)
        {
            int onetime_id = Data.repeat_pc_onetime_ids[i];
            if (onetime_id == 0)
            {
                continue;
            }
            var onetime = BattleSkillDataFactory.CreatePcBattleOnetimeSkillData(onetime_id);
            Repeat_Onetime_Effect_Data_List.Add(onetime);
        }

        //  종료시 적용되는 일회성 효과
        cnt = Data.finish_pc_onetime_ids.Length;
        for (int i = 0; i < cnt; i++)
        {
            int onetime_id = Data.finish_pc_onetime_ids[i];
            if (onetime_id == 0)
            {
                continue;
            }
            var onetime = BattleSkillDataFactory.CreatePcBattleOnetimeSkillData(onetime_id);
            Finish_Onetime_Effect_Data_List.Add(onetime);
        }
    }

    public override object GetDurationSkillData()
    {
        return Data;
    }

    public override string GetEffectPrefab()
    {
        return Data.effect_path;
    }
    public override string GetIconPath()
    {
        return Data.icon_path;
    }
    public override bool IsUseRepeatInterval()
    {
        return Data.repeat_interval > 0;
    }

    protected override double GetRepeatInterval()
    {
        return Data.repeat_interval;
    }

    public override PERSISTENCE_TYPE GetPersistenceType()
    {
        return Data.persistence_type;
    }
    public override DURATION_EFFECT_TYPE GetDurationEffectType()
    {
        return Data.duration_effect_type;
    }

    public override bool IsOverlapable()
    {
        return Data.is_overlapable;
    }
    public override STAT_MULTIPLE_TYPE GetStatMultipleType()
    {
        return Data.multiple_type;
    }
    public override double GetRate()
    {
        return Data.rate + (GetSkillLevel() - 1) * Data.up_rate;
    }
    public double GetTime()
    {
        return Data.time;
    }
    public override double GetMultiple()
    {
        return Data.multiple + (GetSkillLevel() - 1) * Data.up_multiple;
    }
    public override double GetValue()
    {
        return Data.value + (GetSkillLevel() - 1) * Data.up_value;
    }

    public override DURATION_CALC_RESULT_TYPE CalcDuration_V2(float dt)
    {
        var result = base.CalcDuration_V2(dt);
        if (result == DURATION_CALC_RESULT_TYPE.FINISH || result == DURATION_CALC_RESULT_TYPE.REPEAT_AND_FINISH)
        {
            Duration = Data.time;
        }
        return result;
    }

    /// <summary>
    /// 지속성 효과의 지속성 방식 타입 갱신<br/>
    /// 시간 지속성 방식은 <see cref="CalcDurationSkillTime"/>을 사용한다.<br/>
    /// 그 외에 피격 횟수 제한, 공격 횟수 제한의 경우 본 함수 사용<br/>
    /// </summary>
    /// <param name="ptype"></param>
    public override bool CalcEtcPersistenceCount(PERSISTENCE_TYPE ptype)
    {
        bool is_calc = base.CalcEtcPersistenceCount(ptype);
        if (is_calc)
        {
            Effect_Count = Data.count;
        }
        return is_calc;
    }

    public override string ToString()
    {
        return Data.ToString();
    }

    public override object Clone()
    {
        if (Skill_Effect != null)
        {
            return FluffyDuck.Util.Factory.Instantiate<BattlePcDurationSkillData>(Data, Send_Data.Clone(), Skill_Effect);
        }
        else
        {
            return FluffyDuck.Util.Factory.Instantiate<BattlePcDurationSkillData>(Data, Send_Data.Clone());
        }
    }
}
