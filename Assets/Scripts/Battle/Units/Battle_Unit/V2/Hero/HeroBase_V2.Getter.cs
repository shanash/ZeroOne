using System;
using UnityEngine;

public partial class HeroBase_V2 : UnitBase_V2
{
    /// <summary>
    /// 자신의 팀 매니저 반환
    /// </summary>
    /// <returns></returns>
    public TeamManager_V2 GetTeamManager()
    {
        return Team_Mng;
    }

    /// <summary>
    /// 일시적으로 영웅에게 체력 실드(보호막)가 생겼을 경우 반환.
    /// 차후 스킬 등 현재 남아있는 보호막 값을 합하여 반환
    /// </summary>
    /// <returns></returns>
    protected double GetLifeShieldPoint()
    {
        return 0;
    }
    /// <summary>
    /// 체력 쉴드를 포함한 최대 체력
    /// 사용할지는 모르겠음
    /// </summary>
    /// <returns></returns>
    protected double GetMaxLifeWithShield()
    {
        double shield = GetLifeShieldPoint();
        double cur_life = Life;
        if (shield + cur_life > Max_Life)
        {
            return shield + cur_life;
        }
        return Max_Life;
    }

    public int GetLevel()
    {
        return Unit_Data.GetLevel();
    }

    /// <summary>
    /// 체력 비율 반환
    /// </summary>
    /// <returns>0~1사이값 반환</returns>
    public float GetLifePercentage()
    {
        double per = Life / Max_Life;
        return (float)per;
    }

    /// <summary>
    /// 접근 사거리 정보 반환
    /// </summary>
    /// <returns></returns>
    public virtual float GetApproachDistance()
    {
        return Unit_Data.GetApproachDistance();
    }
    /// <summary>
    /// 스킬 매니저 반환
    /// </summary>
    /// <returns></returns>
    public BattleSkillManager GetSkillManager()
    {
        return Skill_Mng;
    }



    /// <summary>
    /// 지정 유닛과 자신의 거리 반환
    /// </summary>
    /// <param name="center"></param>
    /// <returns></returns>
    public float GetDistanceFromCenter(UnitBase_V2 center)
    {
        Vector2 center_pos = center.transform.localPosition;
        Vector2 this_pos = transform.localPosition;
        //float distance = Vector2.Distance(center_pos, this_pos);
        float distance = Mathf.Abs(center_pos.x - this_pos.x);
        return distance;
    }

    /// <summary>
    /// 지속성 스킬 중 지정 타입의 스탯 타입 값 반환(배율 값)
    /// </summary>
    /// <param name="dtype"></param>
    /// <returns></returns>
    protected double GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE dtype)
    {
        double sum = 0;
        int cnt = Used_Battle_Duration_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var dur = Used_Battle_Duration_Data_List[i];
            if (dur.GetDurationEffectType() == dtype)
            {
                sum += dur.GetMultipleTypeByMultiples();
            }
        }
        return sum;
    }
    /// <summary>
    /// 지속성 스킬 중 지정 타입의 스탯 타입 값 반환(절대 값)
    /// </summary>
    /// <param name="dtype"></param>
    /// <returns></returns>
    protected double GetDurationSkillTypesValues(DURATION_EFFECT_TYPE dtype)
    {
        double sum = 0;
        int cnt = Used_Battle_Duration_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var dur = Used_Battle_Duration_Data_List[i];
            if (dur.GetDurationEffectType() == dtype)
            {
                sum += dur.GetMultipleTypeByValues();
            }
        }
        return sum;
    }
    
    /// <summary>
    /// 도착 pos type에 따른 좌표 정보 반환
    /// </summary>
    /// <param name="ptype"></param>
    /// <returns></returns>
    protected Transform GetReachPosTypeTransform(TARGET_REACH_POS_TYPE ptype)
    {
        if (ptype >= TARGET_REACH_POS_TYPE.LEFT_TEAM_CENTER)
        {
            var factory = Battle_Mng.GetEffectFactory();
            if (ptype == TARGET_REACH_POS_TYPE.LEFT_TEAM_CENTER)
            {
                return factory.GetLeftCenter();
            }
            else if (ptype == TARGET_REACH_POS_TYPE.RIGHT_TEAM_CENTER)
            {
                return factory.GetRightCenter();
            }
            else if (ptype == TARGET_REACH_POS_TYPE.WORLD_ZERO)
            {
                return factory.GetWorldZero();
            }
            return null;
        }
        else
        {
            if (Reach_Pos_Transforms.Exists(x => x.Pos_Type == ptype))
            {
                return Reach_Pos_Transforms.Find(x => x.Pos_Type == ptype).Trans;
            }

            return null;
        }
        
    }
    /// <summary>
    /// 투사체의 시작 위치 찾기
    /// </summary>
    /// <param name="ptype"></param>
    /// <returns></returns>
    public Transform GetStartPosTypeTransform(CASTER_START_POS_TYPE ptype)
    {
        if (ptype == CASTER_START_POS_TYPE.RANDOM)
        {
            if (Start_Projectile_Transforms.Count > 0)
            {
                int r = UnityEngine.Random.Range(0, Start_Projectile_Transforms.Count);
                return Start_Projectile_Transforms[r].Trans;
            }
        }
        else
        {
            if (Start_Projectile_Transforms.Exists(x => x.Pos_Type == ptype))
            {
                return Start_Projectile_Transforms.Find(x => x.Pos_Type == ptype).Trans;
            }
        }
        return null;
    }

  
    /// <summary>
    /// 이펙트 발현 타겟 위치<br/>
    /// 투사체 : 이펙트의 도착 위치<br/>
    /// 캐스팅, 즉발형 : 이펙트의 시작 위치
    /// </summary>
    /// <param name="ptype"></param>
    /// <returns></returns>
    public Transform GetTargetReachPostionByTargetReachPosType(TARGET_REACH_POS_TYPE ptype)
    {
        return GetReachPosTypeTransform(ptype);
    }

    public BattleUnitData GetBattleUnitData()
    {
        return Unit_Data;
    }

    public POSITION_TYPE GetPositionType()
    {
        if (Unit_Data != null)
        {
            return Unit_Data.GetPositionType();
        }
        return POSITION_TYPE.NONE;
    }



    /// <summary>
    /// 스켈레톤 반환
    /// </summary>
    /// <returns></returns>
    public Spine.Skeleton GetUnitSpineSkeleton()
    {
        if (Skeleton != null)
        {
            return Skeleton.skeleton;
        }
        return null;
    }

    /// <summary>
    /// 체력 게이지 위치 트랜스폼
    /// </summary>
    /// <returns></returns>
    public Transform GetHPPositionTransform()
    {
        return Life_Bar_Pos;
    }


    public UnitRenderTexture GetSkeletonRenderTexture()
    {
        return Render_Texture;
    }


    /// <summary>
    /// 방어율<br/>
    /// 방어율 = 1 / (1 + 방어력 / 100)<br/>
    /// 최종 데미지 = 적 데미지 * 방어율
    /// </summary>
    /// <returns></returns>
    protected double GetDefenseRate()
    {
        double def_pt = GetDefensePoint();
        double defense_rate = 1 / (1 + (def_pt / 100));
        return defense_rate;
    }
    /// <summary>
    /// 최종 데미지<br/>
    /// 최종 데미지 = 적 데미지 * 방어율
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    protected double GetCalcDamage(double damage)
    {
        double defense_rate = GetDefenseRate();
        double last_damage = damage * defense_rate;
        return last_damage;
    }

    /// <summary>
    /// 실시간 버프 등이 적용된 공격력 가져오기
    /// </summary>
    /// <returns></returns>    
    public double GetAttackPoint()
    {
        double attk_up_rate = GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE.PHYSICS_ATTACK_UP);
        double attk_up_value = GetDurationSkillTypesValues(DURATION_EFFECT_TYPE.PHYSICS_ATTACK_UP);

        double attk_down_rate = GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE.PHYSICS_ATTACK_DOWN);
        double attk_down_value = GetDurationSkillTypesValues(DURATION_EFFECT_TYPE.PHYSICS_ATTACK_DOWN);

        double pt = Physics_Attack + (attk_up_value - attk_down_value);
        if (attk_up_rate > 0 || attk_down_rate > 0)
        {
            pt += pt * (attk_up_rate - attk_down_rate);
        }

        return pt;
    }
    /// <summary>
    /// 실시간 버프 등이 적용된 방어력 가져오기
    /// </summary>
    /// <returns></returns>
    public double GetDefensePoint()
    {
        double def_up_rate = GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE.PHYSICS_DEFEND_UP);
        double def_up_value = GetDurationSkillTypesValues(DURATION_EFFECT_TYPE.PHYSICS_DEFEND_UP);

        double def_down_rate = GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE.PHYSICS_DEFEND_DOWN);
        double def_down_value = GetDurationSkillTypesValues(DURATION_EFFECT_TYPE.PHYSICS_DEFEND_DOWN);

        double pt = Physics_Defense + (def_up_value - def_down_value);
        if (def_up_rate > 0 || def_down_rate > 0)
        {
            pt += pt * (def_up_rate - def_down_rate);
        }
        return pt;
    }

    /// <summary>
    /// 실시간 버프 등이 적용된 크리티컬 찬스 포인트
    /// </summary>
    /// <returns></returns>
    public double GetCriticalChancePoint()
    {
        //  버프/디버프 관련 데이터를 가져와서 적용할 필요가 있음. [todo]
        double pt = Physics_Critical_Chance;
        return pt;
    }

    /// <summary>
    /// 실시간 버프 등이 적용된 크리티컬 파워
    /// </summary>
    /// <returns></returns>
    public double GetCriticalPowerPoint()
    {
        //  버프/디버프 관련 데이터를 가져와서 적용할 필요가 있음. [todo]
        double pt = Physics_Critical_Power_Add;

        return pt;
    }

    /// <summary>
    /// 회피율<br/>
    /// 회피 확률 = 1 / (1 + 100 / (회피 값 - 적의 명중 값))<br/>
    /// </summary>
    /// <param name="caster_accuracy">캐스터의 명중 수치</param>
    /// <returns>백만분율로 변환</returns>
    protected double GetEvationRate(double caster_accuracy)
    {
        //  버프/디버프 관련 데이터를 가져와서 적용할 필요가 있음. [todo]

        double evation_rate = 1 / (1 + 100 / (Evasion - caster_accuracy));

        return evation_rate * 1000000;
    }

    /// <summary>
    /// 물리/마법 공격력 중 낮은 포인트 반환
    /// </summary>
    /// <returns></returns>
    public double GetLowAttackPoint()
    {
        return Physics_Attack < Magic_Attack ? Physics_Attack : Magic_Attack;
    }
    /// <summary>
    /// 물리/마법 공격력 중 높은 포인트 반환
    /// </summary>
    /// <returns></returns>
    public double GetHighAttackPoint()
    {
        return Physics_Attack > Magic_Attack ? Physics_Attack : Magic_Attack;
    }
    /// <summary>
    /// 물리/마법 방어력 중 낮은 포인트 반환
    /// </summary>
    /// <returns></returns>
    public double GetLowDefensePoint()
    {
        return Physics_Defense < Magic_Defense ? Physics_Defense : Magic_Defense;
    }
    /// <summary>
    /// 물리/마법 방어력 중 높은 포인트 반환
    /// </summary>
    /// <returns></returns>
    public double GetHighDefensePoint()
    {
        return Physics_Defense > Magic_Defense ? Physics_Defense : Magic_Defense;
    }

    /// <summary>
    /// 회피 여부 판단
    /// </summary>
    /// <param name="caster_accuracy">캐스터의 명중수치</param>
    /// <returns></returns>
    public bool IsEvation(double caster_accuracy)
    {
        //  명중이 회피보다 높은 경우, 명중(회피실패)
        if (caster_accuracy >= Evasion)
        {
            return false;
        }

        double evation_rate = GetEvationRate(caster_accuracy);
        int rand = UnityEngine.Random.Range(0, 1000000);
        return rand < evation_rate;
    }

    /// <summary>
    /// 상태이상 회피 여부 판단<br/>
    /// 상태이상 명중 = 1 - (상대 레벨 - 내 레벨) / 100<br/>
    /// 시전자의 레벨이 피격자의 레벨보다 100이상 많으면 100% 명중. 수치는 변경될 수 있음
    /// </summary>
    /// <param name="caster_lv">시전자 레벨</param>
    /// <param name="caster_accuracy">시전자 명중값</param>
    /// <returns></returns>
    protected bool IsTransEvation(int caster_lv, double caster_accuracy)
    {
        if (caster_lv >= GetLevel() + 100)
        {
            return false;
        }
        //  아직 계산식이 정확하게 정의 되지 않음 [todo]
        return true;
    }

    /// <summary>
    /// 계산된 크리티컬 확률 계산<br/>
    /// 치명타 발생 확률 = 공격자의 치명타 값 * 0.05 * 공격자의 레벨 / 피격 대상의 레벨 * 0.01 * 10000 <br/>
    /// 백만분율로 변환. (1/1000000)
    /// </summary>
    /// <param name="caster_lv">공격자의 레벨</param>
    /// <param name="target_lv">피격자의 레벨</param>
    /// <returns>백만분율로 변환</returns>
    protected double GetCriticalChanceRate(int caster_lv, int target_lv)
    {

        double chance = (GetCriticalChancePoint() * 0.05 * ((double)caster_lv / (double)target_lv) * 0.01) * 1000000;
        return chance;
    }



    #region Get Stats

    protected virtual double GetPhysicsAttackPoint()
    {
        return Unit_Data != null ? Unit_Data.GetPhysicsAttackPoint() : 0;
    }
    protected virtual double GetMagicAttackPoint()
    {
        return Unit_Data != null ? Unit_Data.GetMagicAttackPoint() : 0;
    }

    protected virtual double GetPhysicsDefensePoint()
    {
        return Unit_Data != null ? Unit_Data.GetPhysicsDefensePoint() : 0;
    }
    protected virtual double GetMagicDefensePoint()
    {
        return Unit_Data != null ? Unit_Data.GetMagicDefensePoint() : 0;
    }

    protected virtual double GetAccuracy()
    {
        return Unit_Data != null ? Unit_Data.GetAccuracyPoint() : 0;
    }
    protected virtual double GetEvasion()
    {
        return Unit_Data != null ? Unit_Data.GetEvasionPoint() : 0;
    }

    protected virtual double GetAutoRecoveryLife()
    {
        return Unit_Data != null ? Unit_Data.GetAutoRecoveryLife() : 0;
    }

    protected virtual double GetPhysicsCriticalChance()
    {
        return Unit_Data != null ? Unit_Data.GetPhysicsCriticalChance() : 0;
    }
    protected virtual double GetMagicCriticalChance()
    {
        return Unit_Data != null ? Unit_Data.GetMagicCriticalChance() : 0;
    }
    protected virtual double GetPhysicsCriticalPowerAdd()
    {
        return Unit_Data != null ? Unit_Data.GetPhysicsCriticalPowerAdd() : 0;
    }
    protected virtual double GetMagicCriticalPowerAdd()
    {
        return Unit_Data != null ? Unit_Data.GetMagicCriticalPowerAdd() : 0;
    }

    protected virtual double GetResistPoint()
    {
        return Unit_Data != null ? Unit_Data.GetResistPoint() : 0;
    }

    protected virtual double GetVampirePoint()
    {
        return Unit_Data != null ? Unit_Data.GetVampirePoint() : 0;
    }

    protected virtual double GetLifeRecoveryInc()
    {
        return Unit_Data != null ? Unit_Data.GetLifeRecoveryInc() : 0;
    }

    protected virtual double GetWeight()
    {
        return Unit_Data != null ? Unit_Data.GetWeight() : 0;
    }

    protected virtual double GetMoveSpeed()
    {
        return Unit_Data != null ? Unit_Data.GetMoveSpeed() : 0;
    }
    #endregion


}
