
public class BattleNpcDurationSkillData : BattleDurationSkillData
{
    protected Npc_Skill_Duration_Data Data;

    protected BattleNpcDurationSkillData() { }

    protected virtual bool Initialize(Npc_Skill_Duration_Data data)
    {
        SetDurationSkillData(data);
        return true;
    }

    bool Initialize(Npc_Skill_Duration_Data data, BATTLE_SEND_DATA send_data, SkillEffectBase skill_effect)
    {
        SetDurationSkillData(data);
        SetBattleSendData(send_data);
        SetSkillEffect(skill_effect);
        return true;
    }

    public override void SetDurationSkillDataID(int skill_duration_id)
    {
        SetDurationSkillData(MasterDataManager.Instance.Get_NpcSkillDurationData(skill_duration_id));
    }

    void SetDurationSkillData(Npc_Skill_Duration_Data data)
    {
        Data = data;
        Duration = Data.time;
        Effect_Count = Data.count;
        Repeat_Interval = Data.repeat_interval;

        Repeat_Onetime_Effect_Data_List.Clear();
        Finish_Onetime_Effect_Data_List.Clear();

        //  반복주기 일회성 효과
        int cnt = Data.repeat_npc_onetime_ids.Length;
        for (int i = 0; i < cnt; i++)
        {
            int onetime_id = Data.repeat_npc_onetime_ids[i];
            if (onetime_id == 0)
            {
                continue;
            }
            var onetime = BattleSkillDataFactory.CreateNpcBattleOnetimeSkillData(onetime_id);
            Repeat_Onetime_Effect_Data_List.Add(onetime);
        }
        //  종료시 적용되는 일회성 효과
        cnt = Data.finish_npc_onetime_ids.Length;
        for (int i = 0; i < cnt; i++)
        {
            int onetime_id = Data.finish_npc_onetime_ids[i];
            if (onetime_id == 0)
            {
                continue;
            }
            var onetime = BattleSkillDataFactory.CreateNpcBattleOnetimeSkillData(onetime_id);
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
    public override PROJECTILE_TYPE GetProjectileType()
    {
        return Data.projectile_type;
    }

    public override double GetEffectDuration()
    {
        return Data.effect_duration;
    }

    public override bool IsUseRepeatInterval()
    {
        return Data.repeat_interval > 0;
    }

    protected override double GetRepeatInterval()
    {
        return Data.repeat_interval;
    }



    public override double GetRate()
    {
        return Data.rate;
    }

    public override DURATION_EFFECT_TYPE GetDurationEffectType()
    {
        return Data.duration_effect_type;
    }

    public override PERSISTENCE_TYPE GetPersistenceType()
    {
        return Data.persistence_type;
    }
    public override bool IsOverlapable()
    {
        return Data.is_overlapable;
    }
    public override double GetMultipleTypeByMultiples()
    {
        return Data.multiple;
    }
    public override double GetMultipleTypeByValues()
    {
        return Data.value;
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
        return FluffyDuck.Util.Factory.Instantiate<BattleNpcDurationSkillData>(Data, Send_Data.Clone(), Skill_Effect);
    }
}
