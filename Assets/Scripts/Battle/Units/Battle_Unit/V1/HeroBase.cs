using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Examples;
using Cysharp.Text;
using static Spine.AnimationState;
using Spine;

[RequireComponent(typeof(SkeletonUtility))]
[RequireComponent(typeof(SkeletonRenderTexture))]

public class HeroBase : UnitBase
{
    [SerializeField, Tooltip("Skeleton")]
    protected SkeletonAnimation Skeleton;

    [SerializeField, Tooltip("Focus")]
    protected Transform Focus_Node;

    [SerializeField, Tooltip("Life Bar Pos")]
    protected Transform Life_Bar_Pos;

    protected SkeletonRenderTexture Render_Texture;


    protected List<HeroBase> Attack_Target_List = new List<HeroBase>();

    /// <summary>
    /// 현재 체력
    /// </summary>
    public double Life { get; protected set; }
    /// <summary>
    /// 최대 체력. 
    /// 힘/체력 수치에 따라 결정
    /// </summary>
    public double Max_Life { get; protected set; }
    /// <summary>
    /// 추가 최대 체력. 스킬 및 장비 옵션 등으로 인하여 일시적으로 증가된 최대 체력
    /// </summary>
    public double Added_Max_Life { get; protected set; }
    /// <summary>
    /// 물리 공격력. 
    /// 힘/민첩 수치에 따라 결정
    /// </summary>
    public double Attack { get; protected set; }

    /// <summary>
    /// 물리 방어력
    /// 힘 수치에 따라 결정
    /// </summary>
    public double Defense { get; protected set; }
    /// <summary>
    /// 물리 명중률
    /// 명중 기본값 및 민첩 수치에 따라 결정
    /// 명중 레벨 테이블의 값을 참조한다.
    /// </summary>
    public double Accuracy_Rate { get; protected set; }

    /// <summary>
    /// 물리 회피율
    /// 회피 기본값 및 민첩 수치에 따라 결정
    /// 회피 레벨 테이블의 값을 참조한다.
    /// </summary>
    public double Evasion_Rate { get; protected set; }

    /// <summary>
    /// 급속 정보. 턴 순서에 영향을 준다. 
    /// 스킬을 사용하면 급속값의 배율로 적용.
    /// 급속 값이 높을수록 다음 턴이 빠르다.
    /// 급속 값은 누적이 되어야 다음 턴이 더 빨라질 수 있다.
    /// </summary>
    public double Rapidity { get; protected set; }
    /// <summary>
    /// 통찰 수치
    /// 제어효과를 줄때, 피격자의 '내성'이 공격자의 '통찰' 보다 낮으면 통찰 1포인트 마다 0.5% 확률로 제어 효과의 확률을 증가시킴(100% 이상이면 의미 없음)
    /// 제어 효과의 기본 확률은 100%
    /// </summary>
    public double Insight { get; protected set; }
    /// <summary>
    /// 내성 수치
    /// 제어 효과를 받을 시, 공격자의 '통찰' 보다 피격자의 '내성'이 높을 경우 1포인트 마다 2% 확률로 제어 효과를 저항함. 저항은 제어 효과를 받을 수 있는 확률을 줄이는 의미임)
    /// '통찰' - '내성' = 결과. 결과가 0보다 클 경우, 1포인트마다 2%의 제어 확률 감소
    /// </summary>
    public double Resistance { get; protected set; }
    /// <summary>
    /// 반격 확률
    /// 반격 레벨 테이블 값을 참조한다.
    /// </summary>
    public double Counter_Rate { get; protected set; }

    /// <summary>
    /// 치명타 확률
    /// </summary>
    public double Critical_Rate { get; protected set; }
    /// <summary>
    /// 치명타 파워 - 기본 공격력의 배수
    /// </summary>
    public double Critical_Power { get; protected set; }

    /// <summary>
    /// 흡혈 등급
    /// </summary>
    public double Vampire_Rate { get; protected set; }
    /// <summary>
    /// 보유 에너지
    /// </summary>
    public int Energy_Value { get; protected set; }
    /// <summary>
    /// 협공 확률
    /// </summary>
    public double Cooperative_Attack_Rate { get; protected set; }


    /// <summary>
    /// 기본 공격 또는 스킬을 사용하면 급속값을 누적시킨다.
    /// 누적된 급속 값을 이용하여 턴 관리를 할 수 있다.
    /// 경우에 따라서 2회 연속 턴이 올 수도 있다.
    /// </summary>
    public double Accum_Rapidity_Value { get; protected set; }

    /// <summary>
    /// 행운 포인트 - 전투 시작시 공속 포인트에 영향, 기타 다른 부분에 영향을 준다.
    /// </summary>
    public int Lucky_Point { get; protected set; }

    /// <summary>
    /// 리더/보스 여부
    /// </summary>
    public bool Is_Leader { get; protected set; } = false;
    /// <summary>
    /// 팀 타입
    /// </summary>
    public TEAM_TYPE Team_Type { get; protected set; } = TEAM_TYPE.LEFT;
    /// <summary>
    /// 포지션(위치) 정보 타입
    /// </summary>
    public TEAM_POSITION_TYPE Position_Type { get; protected set; } = TEAM_POSITION_TYPE.NONE;

    /// <summary>
    /// 팀 매니저
    /// </summary>
    protected TeamManager Team_Mng = null;

    /// <summary>
    /// 체력 게이지
    /// </summary>
    protected LifeBarNode Life_Bar = null;

    protected TargetArrowNode Target_Arrow = null;
    /// <summary>
    /// 턴 종료 후 잠시 대기시간
    /// </summary>
    protected float Turn_End_Delay_Delta;
    protected const float TURN_END_DELAY_TIME = 0.5f;

    protected void AddLifeBar()
    {
        if (Life_Bar == null)
        {
            Life_Bar = UI_Mng.AddLifeBarNode(Life_Bar_Pos, Team_Type);
        }
    }

    protected void RemoveLifeBar()
    {
        if (Life_Bar != null)
        {
            UI_Mng.RemoveLifeBarNode(Life_Bar);
        }
        Life_Bar = null;
    }

    protected override void InitStates()
    {
        FSM = new UnitStateSystem<UnitBase, BattleManager, BattleUIManager>();


        SetSkeletonEventListener();
    }

    /// <summary>
    /// 스켈레톤(스파인) 이벤트 리스너 등록
    /// </summary>
    protected void SetSkeletonEventListener()
    {
        if (Skeleton != null)
        {
            Skeleton.AnimationState.Start += SpineAnimationStart;
            Skeleton.AnimationState.Complete += SpineAnimationComplete;
            Skeleton.AnimationState.End += SpineAnimationEnd;
            Skeleton.AnimationState.Event += SpineAnimationEvent;
        }
    }

    /// <summary>
    /// 스파인 애니메이션 시작시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationStart(TrackEntry entry) { }
    /// <summary>
    /// 스파인 애니메이션 동작 완료시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationComplete(TrackEntry entry) { }
    /// <summary>
    /// 스파인 애니메이션 동작 종료시 호출되는 리스너
    /// </summary>
    /// <param name="entry"></param>
    protected virtual void SpineAnimationEnd(TrackEntry entry) { }
    /// <summary>
    /// 스파인 애니메이션 동작 플레이 중 호출되는 이벤트를 받아주는 리스너
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="evt"></param>
    protected virtual void SpineAnimationEvent(TrackEntry entry, Spine.Event evt) { }


    public void SetTeamManager(TeamManager mng)
    {
        Team_Mng = mng;

        Team_Type = Team_Mng.Team_Type;
        
        SpriteRenderer sr = Focus_Node.gameObject.GetComponent<SpriteRenderer>();
        if (Team_Type == TEAM_TYPE.LEFT)
        {
            sr.color = Color.green;
        }
        else
        {
            sr.color = Color.red;
        }
    }
    public void SetTeamPositionType(TEAM_POSITION_TYPE ptype)
    {
        Position_Type = ptype;
    }


    #region HeroBase States
    public override void UnitStateInitBegin()
    {
        CalcHeroAbility();
        AddLifeBar();
    }

    public override void UnitStateInit()
    {
        ChangeState(UNIT_STATES.READY);
    }
    public override void UnitStateReady()
    {
        ChangeState(UNIT_STATES.SPAWN);
    }

    public override void UnitStateSpawn()
    {
        ChangeState(UNIT_STATES.IDLE);
    }

    public override void UnitStateTurnOnBegin()
    {
        SetFocus(true);
    }

    public override void UnitStateTurnOn()
    {
        ChangeState(UNIT_STATES.ATTACK_READY_1);
    }

    public override void UnitStateAttackEnd()
    {
        ChangeState(UNIT_STATES.TURN_END);
    }

    public override void UnitStateSkillEnd()
    {
        ChangeState(UNIT_STATES.TURN_END);
    }

    public override void UnitStateTurnEndBegin()
    {
        Turn_End_Delay_Delta = TURN_END_DELAY_TIME;
    }
    public override void UnitStateTurnEnd()
    {
        Turn_End_Delay_Delta -= Time.deltaTime;
        if (Turn_End_Delay_Delta < 0f)
        {
            ChangeState(UNIT_STATES.IDLE);
        }
    }

    public override void UnitStateTurnEndExit()
    {
        SetFocus(false);
        Battle_Mng.TurnFinishHero(this);
    }

    public override void UnitStateDeathBegin()
    {
        RemoveLifeBar();
        Team_Mng.TeamMemberDeath(this);
    }

    public override void UnitStateDeath()
    {
        ChangeState(UNIT_STATES.END);
    }

    #endregion

    void SetFocus(bool focus)
    {
        Focus_Node.gameObject.SetActive(focus);
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
    /// <summary>
    /// 스켈레톤 알파값 수정
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlpha(float alpha)
    {
        if (Render_Texture == null)
        {
            Render_Texture = GetComponent<SkeletonRenderTexture>();
        }
        Render_Texture.color.a = alpha;
    }

    /// <summary>
    /// 스파인 플립 설정
    /// </summary>
    /// <param name="is_flip"></param>
    public void SetFlipX(bool is_flip)
    {
        float flip = is_flip ? -1f : 1f;
        var scale = this.transform.localScale;
        scale.x *= Mathf.Abs(scale.x) * flip;
        this.transform.localScale = scale;
    }

    public bool IsAlive()
    {
        return Life > 0;
    }

    /// <summary>
    /// 전투 시작시 행동력 초기화
    /// 전투 시작시 랜덤한 비율만큼 속도를 추가해서 각 영웅별로 약간의 오차를 가져간다.
    /// </summary>
    /// <param name="add_rate"></param>
    public void BattleBeginRapidityPoint(double add_rate)
    {
        if (add_rate < 0)
        {
            add_rate *= -1;
        }
        double rate = add_rate / 10000;
        double base_rapidity_point = Rapidity;
        double add_rate_value = base_rapidity_point * rate;
        Accum_Rapidity_Value = base_rapidity_point + add_rate_value;
    }
    
    /// <summary>
    /// 속도 행동력 증가
    /// 스킬 등의 효과로 인해 행동력을 증가시킬 수 있는 함수
    /// </summary>
    /// <param name="point"></param>
    public void AddRapidityPoint(double point)
    {
        if (point < 0)
        {
            return;
        }
        //  상태이상일 경우 특정 상태이상은 행동력의 변화를 줄 수 없음. (추후 필요시 관련 로직 추가)

        Accum_Rapidity_Value += point;
    }
    /// <summary>
    /// 속도 행동력 감소
    /// 스킬 등의 효과로 인하여 행동력을 감소 시켜 턴을 느리게 만드는 효과
    /// </summary>
    /// <param name="point"></param>
    public void DecRapidityPoint(double point)
    {
        if (point < 0)
        {
            return;
        }
        Accum_Rapidity_Value -= point;
    }
    
    /// <summary>
    /// 다른 영웅의 턴 종료시 자신의 행동력 증가
    /// </summary>
    public void CalcIncRapidityPoint()
    {
        Accum_Rapidity_Value += Rapidity;
    }

    /// <summary>
    /// 자신의 턴이 종료되면 행동력을 초기화한다.
    /// </summary>
    public void TurnEndRapidityPointReset()
    {
        Accum_Rapidity_Value = Rapidity;
    }


    public virtual void TurnOnHero()
    {
        ChangeState(UNIT_STATES.TURN_ON);
    }

    /// <summary>
    /// 아군 팀 매니저 반환
    /// </summary>
    /// <returns></returns>
    public TeamManager GetTeamManager()
    {
        return Team_Mng;
    }

    /// <summary>
    /// 타겟이 된 영웅들에게 화살표 표시를 알려줌
    /// </summary>
    protected void SendAttackTargetArrowNodes()
    {
        int cnt = Attack_Target_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Attack_Target_List[i].AddTargetArrowNode();
        }
    }
    /// <summary>
    /// 타겟에 대한 행동이 종료된 후 타겟 화살표 표시 해제를 알려줌
    /// </summary>
    protected void SendRemoveAttackTargetArrowNodes()
    {
        int cnt = Attack_Target_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Attack_Target_List[i].RemoveTargetArrowNode();
        }
    }

    protected void AddTargetArrowNode()
    {
        Target_Arrow = UI_Mng.AddTargetArrowNode(GetHPPositionTransform());
    }
    protected void RemoveTargetArrowNode()
    {
        UI_Mng.RemoveTargetArrowNode(Target_Arrow);
        Target_Arrow = null;
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
    /// 동작 플레이 타입
    /// </summary>
    /// <param name="ani_type"></param>
    public void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        switch (ani_type)
        {
            case HERO_PLAY_ANIMATION_TYPE.NONE:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                PlayIdle01();
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.READY_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.READY_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.JUMP_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.JUMP_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.WALK_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_01:
                PlayAttack01();
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.SKILL_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.SKILL_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.SKILL_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                break;
            default:
                break;
        }
    }

    protected virtual void PlayAttack01()
    {
        //var entry = Skeleton.AnimationState.GetCurrent(0);
        //if (entry != null)
        //{

        //}
        Skeleton.AnimationState.SetAnimation(0, "1_attack_1", false);
    }
    protected virtual void PlayIdle01()
    {
        Skeleton.AnimationState.SetAnimation(0, "1_idle", true);
    }


    /// <summary>
    /// 영웅의 능력치를 계산한다. 
    /// 기본 능력 + 레벨에 따른 능력 등 개별 개산
    /// </summary>
    protected virtual void CalcHeroAbility()
    {
        CalcMaxLife();

        CalcAttackPoint();

        CalcRapidity();
    }
    /// <summary>
    /// 최대 체력 계산
    /// </summary>
    void CalcMaxLife()
    {
        Max_Life = Random.Range(10, 20);
        Life = Max_Life;
    }
    /// <summary>
    /// 공격력 계산
    /// </summary>
    void CalcAttackPoint()
    {
        Attack = Random.Range(1, 5);
    }
    /// <summary>
    /// 행동력 계산
    /// </summary>
    void CalcRapidity()
    {
        Rapidity = Random.Range(80, 115);
    }


    
    public override void Spawned()
    {
        base.Spawned();
        //SetFocus(false);
    }
    public override void Despawned()
    {
        base.Despawned();

        Team_Mng = null;
    }

    public override string ToString()
    {
        var sb = ZString.CreateStringBuilder();
        sb.AppendFormat("Team Type : <color=#ffffff>[{0}]</color>, Position : <color=#ffffff>[{1}]</color>, Accum_Rapidity : <color=#ffffff>[{2}]</color>", Team_Type, Position_Type, Accum_Rapidity_Value);
        return sb.ToString();
    }

    [ContextMenu("ToStringHeroBase")]
    public void ToStringHeroBase()
    {
        Debug.Log(ToString());
    }
}
