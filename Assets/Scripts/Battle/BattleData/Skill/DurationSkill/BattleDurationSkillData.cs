
using System;
using System.Collections.Generic;

public class BattleDurationSkillData : BattleDataBase, ICloneable, FluffyDuck.Util.Factory.IProduct
{
    /// <summary>
    /// 지속성 효과 1개와, 이펙트는 1:1로 매칭되어야만 한다.
    /// </summary>
    protected SkillEffectBase Skill_Effect;
    protected BATTLE_SEND_DATA Send_Data;

    /// <summary>
    /// 효과 지속 시간
    /// </summary>
    public double Duration { get; protected set; }
    /// <summary>
    /// 효과 적용 횟수
    /// </summary>
    public int Effect_Count { get; protected set; }
    /// <summary>
    /// 반복 주기 차감 시간
    /// </summary>
    public double Repeat_Interval { get; protected set; }

    /// <summary>
    /// 반복 주기마다 적용되는 일회성 효과
    /// </summary>
    protected List<BattleOnetimeSkillData> Repeat_Onetime_Effect_Data_List = new List<BattleOnetimeSkillData>();
    /// <summary>
    /// 지속성 효과 종료시 적용되는 일회성 효과
    /// </summary>
    protected List<BattleOnetimeSkillData> Finish_Onetime_Effect_Data_List = new List<BattleOnetimeSkillData>();


    protected override void Destroy()
    {
        Skill_Effect?.UnusedEffect();
        Skill_Effect = null;
        Duration = 0;
        Effect_Count = 0;

        int cnt = Repeat_Onetime_Effect_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Repeat_Onetime_Effect_Data_List[i].Dispose();
        }
        Repeat_Onetime_Effect_Data_List.Clear();

        cnt = Finish_Onetime_Effect_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Finish_Onetime_Effect_Data_List[i].Dispose();
        }
    }

    public void SetSkillEffect(SkillEffectBase effect)
    {
        Skill_Effect = effect;
    }
    public void SetBattleSendData(BATTLE_SEND_DATA data)
    {
        Send_Data = data;
        Send_Data.Duration_Effect_Type = Send_Data.Duration.GetDurationEffectType();
    }
    public BATTLE_SEND_DATA GetBattleSendData() { return Send_Data; }

    public virtual void SetDurationSkillDataID(int skill_duration_id) { }

    public virtual object GetDurationSkillData() { return null; }

    public virtual string GetEffectPrefab() { return null; }

    public virtual PROJECTILE_TYPE GetProjectileType() { return PROJECTILE_TYPE.NONE; }

    public virtual DURATION_EFFECT_TYPE GetDurationEffectType() { return DURATION_EFFECT_TYPE.NONE; }

    /// <summary>
    /// 이펙트 지속 시간<br/>
    /// <b>0</b> : loop<br/>
    /// 0보다 클 경우 지정 시간만큼만 이펙트 유지<br/>
    /// </summary>
    /// <returns>이펙트의 지속 시간</returns>
    public virtual double GetEffectDuration() { return 0; }

    /// <summary>
    /// 적용 확률
    /// </summary>
    /// <returns>지속성 효과 적용 확률(10000분율)</returns>
    public virtual double GetRate() { return 0; }

    /// <summary>
    /// 중첩 가능한 스킬 여부 반환
    /// </summary>
    /// <returns></returns>
    public virtual bool IsOverlapable() { return false; }

    /// <summary>
    /// 스탯 타입의 배율 값
    /// </summary>
    /// <returns></returns>
    public virtual double GetMultipleTypeByMultiples() { return 0; }
    /// <summary>
    /// 스탯 타입의 절대 값
    /// </summary>
    /// <returns></returns>
    public virtual double GetMultipleTypeByValues() { return 0; }


    /// <summary>
    /// 지속성 방식 타입<br/>
    /// 1 : TIME (시간 지속)<br/>
    /// 2 : HITTED (피격 횟수 제한)<br/>
    /// 3 : ATTACK (공격 횟수 제한)
    /// </summary>
    /// <returns>지속성 방식 타입 반환</returns>
    public virtual PERSISTENCE_TYPE GetPersistenceType() { return PERSISTENCE_TYPE.NONE; }
    /// <summary>
    /// 종료 이펙트를 사용하는지 여부 반환
    /// </summary>
    /// <returns></returns>
    public bool IsUseFinishEffect() { return Finish_Onetime_Effect_Data_List.Count > 0; }

    /// <summary>
    /// 반복 주기를 사용하는지 여부 
    /// </summary>
    /// <returns></returns>
    public virtual bool IsUseRepeatInterval()
    {
        return false;
    }

    protected virtual double GetRepeatInterval() { return 0; }

    /// <summary>
    /// 반복 효과 일회성 스킬 데이터 반환
    /// </summary>
    /// <returns></returns>
    public List<BattleOnetimeSkillData> GetRepeatOnetimeSkillDataList() { return Repeat_Onetime_Effect_Data_List; }
    /// <summary>
    /// 종료 효과 일회성 스킬 데이터 반환
    /// </summary>
    /// <returns></returns>
    public List<BattleOnetimeSkillData> GetFinishOnetimeSkillDataList() { return Finish_Onetime_Effect_Data_List; }


    public virtual DURATION_CALC_RESULT_TYPE CalcDuration_V2(float dt)
    {
        DURATION_CALC_RESULT_TYPE result = DURATION_CALC_RESULT_TYPE.NONE;
        if (GetPersistenceType() != PERSISTENCE_TYPE.TIME)
        {
            return result;
        }
        Duration -= dt;

        //  반복 주기에 따라 일회성 효과를 적용할 경우(일회성 효과 적용은 호출 본인이 해준다)
        if (IsUseRepeatInterval())
        {
            Repeat_Interval -= dt;
            if (Repeat_Interval <= 0)
            {
                //  반복 주기만큼 다시 시간 초기화
                Repeat_Interval += GetRepeatInterval();
                result = DURATION_CALC_RESULT_TYPE.REPEAT_INTERVAL;
            }
        }
        //  지속성 시간 종료
        if (Duration <= 0)
        {
            //  지속시간 종료의 효과가 있을 경우
            if (IsUseFinishEffect())
            {
                if (result == DURATION_CALC_RESULT_TYPE.NONE)
                {
                    result = DURATION_CALC_RESULT_TYPE.FINISH;
                }
                else
                {
                    result = DURATION_CALC_RESULT_TYPE.REPEAT_AND_FINISH;
                }
            }
            else
            {
                result = DURATION_CALC_RESULT_TYPE.REPEAT_AND_FINISH;
            }
        }
        return result;
    }



    public virtual bool CalcEtcPersistenceCount(PERSISTENCE_TYPE ptype)
    {
        PERSISTENCE_TYPE cur_type = GetPersistenceType();
        if (cur_type == ptype)
        {
            Effect_Count--;
            if (Effect_Count <= 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 날아가는 발사체인지, 타겟에서 즉시 발생하는 이펙트인지 여부 반환
    /// </summary>
    /// <returns>날아가는 발사체인 경우 true 반환</returns>
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
                is_throwing = false;
                break;
            default:
                UnityEngine.Debug.Assert(false);
                break;
        }
        return is_throwing;
    }

    public virtual void ExecSkill(BATTLE_SEND_DATA data)
    {
        Send_Data = data;
        Send_Data.Duration_Effect_Type = Send_Data.Duration.GetDurationEffectType();
    }

    public virtual object Clone()
    {
        return null;
    }
}
