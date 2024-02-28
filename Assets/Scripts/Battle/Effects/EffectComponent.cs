using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 시전자의 본체 어느 부위에서 부터 투사체의 위치를 시작하는지 지정
/// </summary>
public enum CASTER_START_POS_TYPE
{
    NONE = 0,           //  없으면 root 좌표(캐스터의 좌표)
    PROJECTILE_POS_01,
    PROJECTILE_POS_02,
    PROJECTILE_POS_03,
    PROJECTILE_POS_04,
    PROJECTILE_POS_05,
    PROJECTILE_POS_06,
    PROJECTILE_POS_07,
    PROJECTILE_POS_08,
    PROJECTILE_POS_09,
    PROJECTILE_POS_10,

    RANDOM = 101,           //  지정된 위치중 임의의 위치 선택
}

/// <summary>
/// 타겟의 어디로 도착하는지
/// </summary>
public enum TARGET_REACH_POS_TYPE
{
    NONE = 0,
    HEAD,
    BODY,
    FOOT,


    TARGET_CENTER,              //  타겟의 중앙(다수의 타겟의 위치중 중앙 위치 찾기)
    LEFT_TEAM_CENTER,         //  아군 팀 중앙
    RIGHT_TEAM_CENTER,      //  상대 팀(적군) 중앙
    WORLD_ZERO,             //  월드 0,0
}


/// <summary>
/// 스킬 이펙트 효과 타입
/// </summary>
public enum SKILL_EFFECT_TYPE
{
    NONE = 0,
    CASTING,                //  캐스팅. 캐스터의 본체 Root 좌표에서 아무 효과없이 이펙트만 보여줌 - 캐스팅 이펙트
    PROJECTILE,             //  투사체. 실제로 포물선 또는 직선으로 날아가는 투사체 (Bullet, 파이어볼 등)
    IMMEDIATE,              //  즉발형. TARGET_REACH_POS_TYPE에 지정된 좌표에서 즉시 이펙트를 구현 (Hit, Impact 등)
}

/// <summary>
/// 투사체의 날아가는 타입
/// </summary>
public enum THROWING_TYPE
{
    NONE = 0,
    LINEAR,                 //  직선형
    PARABOLA,               //  포물선형
    BEZIER,                 //  곡선형
}


public class EffectComponent : MonoBehaviour
{
    [Header("모든 프로퍼티 보기")]
    [SerializeField, Tooltip("모두 보기")]
    protected bool Is_Show_All_Properties;

    [Header("이펙트 타입 선택")]
    [SerializeField, Tooltip("이펙트 타입. \n 캐스팅, 투사체, 즉발형")]
    public SKILL_EFFECT_TYPE Effect_Type = SKILL_EFFECT_TYPE.NONE;

    [SerializeField, Tooltip("이펙트 지속 시간\n 투사체가 아닌 캐스팅/즉발형의 경우 지속시간을 지정해야 한다.")]
    public float Effect_Duration;

    [SerializeField, Tooltip("이펙트 반복 재생 여부")]
    public bool Is_Loop;

    [SerializeField, Tooltip("딜레이 시간 - 스킬 효과를 주기위한 대기 시간")]
    public float Delay_Time;

    [Header("투사체 관련 정의")]
    [SerializeField, Tooltip("투사체 시작 위치\n 시전자의 본체에서 찾는다.")]
    public CASTER_START_POS_TYPE Projectile_Start_Pos_Type = CASTER_START_POS_TYPE.NONE;

    [SerializeField, Tooltip("투사체의 도달 위치(또는 즉발형 이펙트의 발현 위치)\n 타겟의 본체에서 찾는다.")]
    public TARGET_REACH_POS_TYPE Projectile_Reach_Pos_Type = TARGET_REACH_POS_TYPE.NONE;

    [SerializeField, Tooltip("투사 방식의 타입\n 투사 방식에 따라 Curve 또는 Mover를 사용한다.")]
    public THROWING_TYPE Throwing_Type = THROWING_TYPE.NONE;

    [SerializeField, Tooltip("커브 이동 컴포넌트")]
    public BezierMove3D Curve;

    [SerializeField, Tooltip("시작 위치의 커브 변화 영역")]
    public float Start_Curve_Dist;

    [SerializeField, Tooltip("도착 위치의 커브 변화 영역")]
    public float End_Curve_Dist;

    [SerializeField, Tooltip("포물선 컴포넌트")]
    public ParabolaMove3D Parabola;

    [SerializeField, Tooltip("포물선 이동 높이")]
    public float Parabola_Height;

    [SerializeField, Tooltip("직선 이동 컴포넌트")]
    public EasingMover3D Mover;

    [SerializeField, Tooltip("투사체 이동 속도\n 이동속도는 초당 이동 거리를 지정한다.")]
    public float Projectile_Velocity;

    [SerializeField, Tooltip("투사체 이동 시간\n이동 속도 이용하지 않고 시간을 지정.")]
    public float Projectile_Duration;

    [SerializeField, Tooltip("투사체 이동 후 감추기 사용 여부")]
    public bool Use_Hide_Transforms;

    [Header("숨김 오브젝트")]
    [SerializeField, Tooltip("감춰야 할 오브젝트")]
    public List<Transform> Hide_Transforms;

    [SerializeField, Tooltip("감춰야할 오브젝트를 감춰두고, 이펙트 종료까지 대기 시간")]
    public float Hide_After_Delay_Time;

    protected Transform Following_Target;
    
    public void SetFollowingTarget(Transform t)
    {
        Following_Target = t;
    }

    private void Update()
    {
        if (Following_Target == null)
        {
            return;
        }
        var pos = Following_Target.position;
        var this_pos = this.transform.position;
        pos.z = this_pos.z;
        this.transform.position = pos;
    }

    public void ResetFollowingTarget()
    {
        Following_Target = null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="show"></param>
    public void ShowObjects(bool show)
    {
        if (!Use_Hide_Transforms)
        {
            return;
        }
        Hide_Transforms.ForEach(x => x.gameObject.SetActive(show));
    }
    /// <summary>
    /// 타겟의 도착 점 반환
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public Transform GetTargetReachPosition(HeroBase_V2 unit)
    {
        if (unit != null)
        {
            return unit.GetTargetReachPostionByTargetReachPosType(Projectile_Reach_Pos_Type);
        }
        return unit.transform;
    }
    /// <summary>
    /// 본체의 시작점 반환
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public Transform GetProjectileStartTransform(HeroBase_V2 unit)
    {
        if (unit != null)
        {
            return unit.GetStartPosTypeTransform(Projectile_Start_Pos_Type);
        }
        return unit.transform;
    }
    /// <summary>
    /// 타겟의 중앙 위치 찾기
    /// </summary>
    /// <param name="targets"></param>
    /// <returns></returns>
    public Vector3 GetTargetsCenterPosition(List<HeroBase_V2> targets)
    {
        Vector3 sum = Vector3.zero;
        int cnt = targets.Count;
        for (int i = 0; i < cnt; i++)
        {
            sum += targets[i].transform.position;
        }
        Vector3 center = sum / cnt;
        return center;
    }
}
