using System.Collections;
using System.Collections.Generic;
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

    /// <summary>
    /// 접근 사거리 정보 반환
    /// </summary>
    /// <returns></returns>
    protected virtual float GetApproachDistance()
    {
        return Unit_Data.GetApproachDistance();
    }
    /// <summary>
    /// 스킬 매니저 반환
    /// </summary>
    /// <returns></returns>
    protected BattleSkillManager GetSkillManager()
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
        float distance = Vector2.Distance(center_pos, this_pos);
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
    /// body 타입에 따른 좌표 정보 반환
    /// </summary>
    /// <param name="btype"></param>
    /// <returns></returns>
    protected Transform GetBodyTypeTransform(SD_BODY_TYPE btype)
    {
        if (Sd_Body_Transforms.Exists(x => x.Body_Type == btype))
        {
            return Sd_Body_Transforms.Find(x => x.Body_Type == btype).Trans;
        }
        return null;
    }
    /// <summary>
    /// 발사체 타입에 따른 타겟의 Transform 반환
    /// </summary>
    /// <param name="ptype"></param>
    /// <returns></returns>
    public Transform GetBodyPositionByProjectileType(PROJECTILE_TYPE ptype)
    {
        switch (ptype)
        {
            case PROJECTILE_TYPE.THROW_FOOT:
            case PROJECTILE_TYPE.INSTANT_TARGET_FOOT:
                return GetBodyTypeTransform(SD_BODY_TYPE.FOOT);
            case PROJECTILE_TYPE.THROW_BODY:
            case PROJECTILE_TYPE.INSTANT_TARGET_BODY:
                return GetBodyTypeTransform(SD_BODY_TYPE.BODY);
            case PROJECTILE_TYPE.THROW_HEAD:
            case PROJECTILE_TYPE.INSTANT_TARGET_HEAD:
                return GetBodyTypeTransform(SD_BODY_TYPE.HEAD);
        }
        return null;
    }

    public BattleUnitData GetBattleUnitData()
    {
        return Unit_Data;
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
}
