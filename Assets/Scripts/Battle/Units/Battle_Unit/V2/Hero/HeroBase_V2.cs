using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.Examples;
using Cysharp.Text;
using Spine;
using System;
using FluffyDuck.Util;
using UnityEngine.Rendering;
using Microsoft.Win32.SafeHandles;
using TMPro;

public enum SD_BODY_TYPE
{
    NONE = 0,
    HEAD,
    BODY,
    FOOT,
}


/// <summary>
/// 플레이어 유닛/NPC 유닛 등 전투에서 사용하는 플레이어의 베이스가 되는 클래스<br/>
/// 몬스터(NPC)는 본 클래스를 상속받아 추가 구현을 해야한다.<br/>
/// 참조가 되는 데이터 타입이 각각 다르기 때문에 상속 필요.<br/>
/// 전투 캐릭의 기본이 되는 여러 가지 기능은 본 클래스에 담아둔다.<br/>
/// </summary>
[RequireComponent(typeof(SkeletonUtility))]
[RequireComponent(typeof(SkeletonRenderTexture))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]

public class HeroBase_V2 : UnitBase_V2
{
    [SerializeField, Tooltip("Skeleton")]
    protected SkeletonAnimation Skeleton;

    [SerializeField, Tooltip("Life Bar Pos")]
    protected Transform Life_Bar_Pos;

    [SerializeField, Tooltip("Body Pos Type")]
    protected List<SD_Body_Pos_Data> Sd_Body_Transforms;

    protected SkeletonRenderTexture Render_Texture;

    protected RendererSortingZ ZOrder;

    protected bool Is_Reposition;

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
    /// </summary>
    public double Attack { get; protected set; }

    /// <summary>
    /// 물리 방어력
    /// </summary>
    public double Defense { get; protected set; }

    /// <summary>
    /// 이동 속도
    /// </summary>
    public double Move_Speed { get; protected set; }

    /// <summary>
    /// 물리 명중률<br/>
    /// 명중 기본값 및 민첩 수치에 따라 결정<br/>
    /// 명중 레벨 테이블의 값을 참조한다.
    /// </summary>
    public double Accuracy_Rate { get; protected set; }

    /// <summary>
    /// 물리 회피율<br/>
    /// 회피 기본값 및 민첩 수치에 따라 결정<br/>
    /// 회피 레벨 테이블의 값을 참조한다.
    /// </summary>
    public double Evasion_Rate { get; protected set; }

    /// <summary>
    /// 내성 수치<br/>
    /// 제어 효과를 받을 시, 공격자의 '통찰' 보다 피격자의 '내성'이 높을 경우 1포인트 마다 2% 확률로 제어 효과를 저항함. 저항은 제어 효과를 받을 수 있는 확률을 줄이는 의미임)<br/>
    /// '통찰' - '내성' = 결과. 결과가 0보다 클 경우, 1포인트마다 2%의 제어 확률 감소
    /// </summary>
    public double Resistance { get; protected set; }
    /// <summary>
    /// 반격 확률<br/>
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
    /// 기본 공격 또는 스킬을 사용하면 급속값을 누적시킨다.<br/>
    /// 누적된 급속 값을 이용하여 턴 관리를 할 수 있다.<br/>
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
    /// 팀 매니저
    /// </summary>
    protected TeamManager_V2 Team_Mng = null;

    /// <summary>
    /// 체력 게이지
    /// </summary>
    protected LifeBarNode Life_Bar = null;


    protected List<HeroBase_V2> Normal_Attack_Target = new List<HeroBase_V2>();

    /// <summary>
    /// 사용자 영웅 데이터
    /// </summary>
    protected UserHeroData User_Hero_Data;

    /// <summary>
    /// 스킬 매니져
    /// </summary>
    protected BattleSkillManager Skill_Mng;

    protected object Duration_Lock = new object();
    /// <summary>
    /// 지속성 효과 데이터를 관리
    /// </summary>
    protected List<BattleDurationSkillData> Used_Battle_Duration_Data_List = new List<BattleDurationSkillData>();
    /// <summary>
    /// 삭제 예약을 위한 지속성 데이터 리스트
    /// </summary>
    protected List<BattleDurationSkillData> Remove_Reserved_Duration_Data_List = new List<BattleDurationSkillData>();

    public int Deck_Order { get; protected set; }

    protected object Effect_Queue_Lock = new object();
    protected float Effect_Queue_Interval;
    protected List<Effect_Queue_Data> Effect_Queue_Data_List = new List<Effect_Queue_Data>();

    public void SetUserHeroData(UserHeroData ud)
    {
        User_Hero_Data = ud;

        var bdata = User_Hero_Data.GetPlayerCharacterBattleData();
        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetPlayerCharacterSkillGroups(bdata.skill_pattern);
    }
    public void SetDeckOrder(int order)
    {
        Deck_Order = order;
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
    /// 체력 바 추가
    /// </summary>
    protected void AddLifeBar()
    {
        if (Life_Bar == null)
        {
            Life_Bar = UI_Mng.AddLifeBarNode(Life_Bar_Pos, Team_Type);
        }
    }
    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    protected void RemoveLifeBar()
    {
        if (Life_Bar != null)
        {
            UI_Mng.RemoveLifeBarNode(Life_Bar);
        }
        Life_Bar = null;
    }
    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    protected void UpdateLifeBar()
    {
        float per = (float)(Life / Max_Life);
        Life_Bar?.SetLifePercent(per);

    }

    protected override void InitStates()
    {
        FSM = new UnitStateSystem<UnitBase_V2, BattleManager_V2, BattleUIManager_V2>();

        FSM.AddTransition(new UnitStateInit_V2());
        FSM.AddTransition(new UnitStateReady_V2());
        FSM.AddTransition(new UnitStateSpawn_V2());
        FSM.AddTransition(new UnitStateIdle_V2());

        FSM.AddTransition(new UnitStateMove_V2());
        FSM.AddTransition(new UnitStateMoveIn_V2());

        FSM.AddTransition(new UnitStateAttackReady01_V2());
        FSM.AddTransition(new UnitStateAttack01_V2());
        FSM.AddTransition(new UnitStateAttackReady02_V2());
        FSM.AddTransition(new UnitStateAttack02_V2());
        FSM.AddTransition(new UnitStateAttackEnd_V2());

        FSM.AddTransition(new UnitStateSkillReady01_V2());
        FSM.AddTransition(new UnitStateSkill01_V2());
        FSM.AddTransition(new UnitStateSkillReady02_V2());
        FSM.AddTransition(new UnitStateSkill02_V2());
        FSM.AddTransition(new UnitStateSkillReady03_V2());
        FSM.AddTransition(new UnitStateSkill03_V2());
        FSM.AddTransition(new UnitStateSkillEnd_V2());

        FSM.AddTransition(new UnitStateStun_V2());
        FSM.AddTransition(new UnitStateSleep_V2());
        FSM.AddTransition(new UnitStateFreeze_V2());
        FSM.AddTransition(new UnitStateBind_V2());

        FSM.AddTransition(new UnitStateWaveRun_V2());
        FSM.AddTransition(new UnitStatePause_V2());

        FSM.AddTransition(new UnitStateWin_V2());
        FSM.AddTransition(new UnitStateLose_V2());
        FSM.AddTransition(new UnitStateDeath_V2());

        FSM.AddTransition(new UnitStateEnd_V2());

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
    protected virtual void SpineAnimationComplete(TrackEntry entry) 
    {
        string animation_name = entry.Animation.Name;

        UNIT_STATES state = GetCurrentState();
        if (state == UNIT_STATES.DEATH)
        {
            if (animation_name.Equals("1_death"))
            {
                ChangeState(UNIT_STATES.END);
            }
        }
    }
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

    public void SetTeamManager(TeamManager_V2 mng)
    {
        Team_Mng = mng;

        Team_Type = Team_Mng.Team_Type;
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
    public override void UnitStateIdle()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateStunBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateStun()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateSleepBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateSleep()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateFreezeBegin()
    {
        //Skeleton.AnimationState.ClearTracks();
        var tracks = FindAllTrakcs();
        int len = tracks.Length;
        for (int i = 0; i < len; i++)
        {
            tracks[i].TimeScale = 0f;
        }
    }
    public override void UnitStateFreeze()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateFreezeExit()
    {
        var tracks = FindAllTrakcs();
        int len = tracks.Length;
        for (int i = 0; i < len; i++)
        {
            tracks[i].TimeScale = 1f;
        }
    }

    public override void UnitStateBindBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.IDLE_01);
    }
    public override void UnitStateBind()
    {
        //  attack check

        CalcDurationSkillTime();
    }

    public override void UnitStateAttack01()
    {
        CalcDurationSkillTime();
    }

    public override void UnitStateMove()
    {
        CalcDurationSkillTime();
    }



    public override void UnitStateDeathBegin()
    {
        RemoveLifeBar();
        ClearDurationSkillDataList();
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.DEATH_01);
    }

    public override void UnitStateWinBegin()
    {
        PlayAnimation(HERO_PLAY_ANIMATION_TYPE.WIN_01);
    }

    public override void UnitStateEndBegin()
    {
        //  team member remove
        Team_Mng.RemoveDeathMember(this);
    }
    #endregion

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

    protected void PlayAnimation(int track, string anim_name, bool loop)
    {
        Skeleton.AnimationState.SetAnimation(track, anim_name, loop);
    }
    
    /// <summary>
    /// 동작 플레이 타입
    /// </summary>
    /// <param name="ani_type"></param>
    protected void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
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
                PlayAnimation(1, "1_idle", true);
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
                PlayAnimation(1, "1_run", true);
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
                PlayAnimation(1, "1_attack_1", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.ATTACK_02:
                PlayAnimation(1, "1_attack_2", false);
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
                PlayAnimation(1, "1_death", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                PlayAnimation(1, "1_win", true);
                break;
        }
    }


    protected virtual float GetDistance()
    {
        return User_Hero_Data.GetDistance();
    }


    protected virtual void FindTargets() 
    {
        float distance = GetDistance();
        var first_skill = GetSkillManager().GetCurrentSkillGroup().GetFirstSkillData();
        Team_Mng.FindTargetInRange(this, first_skill.GetTargetType(), first_skill.GetTargetRuleType(), distance, first_skill.GetTargetOrder(), first_skill.GetTargetCount(), ref Normal_Attack_Target);
    }
    protected virtual void FindTargets(BattleSkillData skill) 
    {
        float distance = GetDistance();
        Team_Mng.FindTargetInRange(this, skill.GetTargetType(), skill.GetTargetRuleType(), distance, skill.GetTargetOrder(), skill.GetTargetCount(), ref Normal_Attack_Target);
    }
    

    /// <summary>
    /// 현재 스켈레톤 애니메이션의 모든 트랙을 반환
    /// </summary>
    /// <returns></returns>
    protected TrackEntry[] FindAllTrakcs()
    {
        if (Skeleton != null)
        {
            return Skeleton.AnimationState.Tracks.Items;
        }
        return null;
    }

    /// <summary>
    /// 영웅의 능력치를 계산한다. 
    /// 기본 능력 + 레벨에 따른 능력 등 개별 개산
    /// </summary>
    protected void CalcHeroAbility()
    {
        CalcMaxLife();

        CalcAttackPoint();
        CalcDefensePoint();
        CalcMoveSpeed();

    }
    /// <summary>
    /// 최대 체력 계산
    /// </summary>
    protected virtual void CalcMaxLife()
    {
        var bdata = User_Hero_Data.GetPlayerCharacterBattleData();
        Max_Life = bdata.hp;

        Life = Max_Life;
    }
    /// <summary>
    /// 공격력 계산
    /// </summary>
    protected virtual void CalcAttackPoint()
    {
        var bdata = User_Hero_Data.GetPlayerCharacterBattleData();
        Attack = bdata.attack;
    }
    /// <summary>
    /// 방어력 계산
    /// </summary>
    protected virtual void CalcDefensePoint()
    {
        var bdata = User_Hero_Data.GetPlayerCharacterBattleData();
        Defense = bdata.defend;
    }
    /// <summary>
    /// 이동속도 계산
    /// </summary>
    protected virtual void CalcMoveSpeed()
    {
        var bdata = User_Hero_Data.GetPlayerCharacterBattleData();
        Move_Speed = bdata.move_speed;
    }

    /// <summary>
    /// 스킬 구현 V2<br/>
    /// 스킬의 이펙트 정의 여부에 따라 달라진다.<br/>
    /// 스킬에 이펙트 프리팹 정보가 있을 경우 해당 이펙트를 트리거로 사용하여 효과를 줄 수 있다<br/>
    /// 스킬에 이펙트 프리팹 정보가 없을 경우, 즉시 발동하는 스킬로, 해당 스킬에서 바로 적용될 수 있도록 한다.
    /// </summary>
    /// <param name="skill"></param>
    protected virtual void SkillEffectSpawnV2(BattleSkillData skill)
    {
        var factory = Battle_Mng.GetEffectFactory();
        FindTargets(skill);
        int target_cnt = Normal_Attack_Target.Count;
        int effect_weight_index = skill.GetEffectWeightIndex();

        if (skill.GetProjectileType() == PROJECTILE_TYPE.ALL_ROUND)
        {
            Transform center = Team_Type == TEAM_TYPE.LEFT ? factory.GetRightCenter() : factory.GetLeftCenter();
            BATTLE_SEND_DATA dmg = new BATTLE_SEND_DATA();
            dmg.Caster = this;
            dmg.AddTargets(Normal_Attack_Target);
            dmg.Skill = skill;
            dmg.Damage = Attack;
            dmg.Effect_Weight_Index = effect_weight_index;

            string skill_effect_prefab = skill.GetEffectPrefabPath();
            if (string.IsNullOrEmpty(skill_effect_prefab))
            {

            }
            else
            {
                var target_pos = center.position;
                if (skill.IsThrowingNode())
                {
                }
                else
                {
                    var effect = (SkillEffectBase)factory.CreateEffect(skill_effect_prefab);
                    effect.SetBattleSendData(dmg);
                    target_pos.z = center.position.z;
                    effect.transform.position = target_pos;
                    effect.StartParticle(1f);

                }
            }
        }
        else
        {
            for (int i = 0; i < target_cnt; i++)
            {
                var target = Normal_Attack_Target[i];

                BATTLE_SEND_DATA dmg = new BATTLE_SEND_DATA();
                dmg.Caster = this;
                dmg.AddTarget(target);
                dmg.Skill = skill;
                dmg.Damage = Attack;
                dmg.Effect_Weight_Index = effect_weight_index;

                string skill_effect_prefab = skill.GetEffectPrefabPath();
                //  이펙트가 정의되어 있지 않다면, 즉시 시전 및 트리거 방식으로 적용
                if (string.IsNullOrEmpty(skill_effect_prefab))
                {
                    //  onetime skill
                    var onetime_list = skill.GetOnetimeSkillDataList();
                    int one_cnt = onetime_list.Count;
                    for (int o = 0; o < one_cnt; o++)
                    {
                        var onetime = onetime_list[o];
                        string effect_path = onetime.GetEffectPrefab();
                        if (string.IsNullOrEmpty(effect_path))
                        {
                            continue;
                        }
                        dmg.Onetime = onetime;

                        var effect = (SkillEffectBase)factory.CreateEffect(effect_path);
                        effect.SetBattleSendData(dmg);
                        PROJECTILE_TYPE ptype = onetime.GetProjectileType();
                        var target_trans = target.GetBodyPositionByProjectileType(ptype);
                        var target_pos = target_trans.position;
                        if (onetime.IsThrowingNode())
                        {
                            target_pos.z = this.transform.position.z;
                            effect.transform.position = GetBodyPositionByProjectileType(PROJECTILE_TYPE.THROW_BODY).position;
                            effect.MoveTarget(target_trans, (float)onetime.GetEffectDuration());
                        }
                        else
                        {
                            target_pos.z = target.transform.position.z;
                            effect.transform.position = target_pos;
                            effect.StartParticle((float)onetime.GetEffectDuration());
                        }
                    }

                    //  duration skill
                    var duration_list = skill.GetDurationSkillDataList();
                    int dur_cnt = duration_list.Count;
                    for (int d = 0; d < dur_cnt; d++)
                    {
                        var duration = duration_list[d];
                        if (duration.GetDurationEffectType() == DURATION_EFFECT_TYPE.POISON)
                        {
                            bool a = false;
                        }
                        string effect_path = duration.GetEffectPrefab();
                        if (string.IsNullOrEmpty(effect_path))
                        {
                            continue;
                        }
                        dmg.Duration = duration;
                        PROJECTILE_TYPE ptype = duration.GetProjectileType();
                        if (duration.IsThrowingNode())
                        {
                            //  지속성 효과를 투사체로 사용하기 위해서는 자체 이펙트를 이용해서 사용하는 방법은 아직 없음.
                            //  상위 스킬의 이펙트 효과 트리거로 사용하고 있는 중.
                        }
                        else
                        {
                            var target_trans = target.GetBodyPositionByProjectileType(ptype);
                            var effect = (SkillEffectBase)factory.CreateEffect(effect_path);

                            effect.SetBattleSendData(dmg);
                            var target_pos = target_trans.position;
                            target_pos.z = target.transform.position.z;
                            effect.transform.position = target_pos;

                            effect.MoveTarget(target_trans, (float)duration.GetEffectDuration());

                        }

                    }
                }
                else // 이펙트가 정의되어 있다면, 해당 이펙트의 사용으로 스킬 시전 트리거를 발생시킨다.
                {
                    PROJECTILE_TYPE ptype = skill.GetProjectileType();
                    var target_trans = target.GetBodyPositionByProjectileType(ptype);
                    if (target_trans == null)
                    {
                        bool a = false;
                    }
                    var target_pos = target_trans.position;
                    if (skill.IsThrowingNode())
                    {
                        var effect = (SkillEffectBase)factory.CreateEffect(skill_effect_prefab);
                        effect.SetBattleSendData(dmg);
                        target_pos.z = this.transform.position.z;
                        effect.transform.position = GetBodyPositionByProjectileType(PROJECTILE_TYPE.THROW_BODY).position;
                        effect.MoveTarget(target_trans, (float)skill.GetEffectDuration());
                    }
                    else
                    {
                        //  아직은 투사체가 아닌 방식의 이펙트 트리거가 없음. 아마도 장판같은 이펙트를 사용할 수 있을 것으로 예상
                    }
                }
            }
        }

        
    }


    /// <summary>
    /// 현재 재생중인 애니메이션을 일시 정지.<br/>
    /// 이동 등의 상태는 Pause 상태에서는 변경되지 않도록 로직을 구성할 필요가 있음.
    /// </summary>
    protected override void OnPause()
    {
        var all_tracks = FindAllTrakcs();
        if (all_tracks != null)
        {
            int len = all_tracks.Length;
            for (int i = 0; i < len; i++)
            {
                all_tracks[i].TimeScale = 0f;
            }
        }
    }
    /// <summary>
    /// 현재 일시정지 중인 애니메이션을 재생<br/>
    /// 이동 등의 상태는 상태가 Pause에서 풀리면 자동으로 갱신되도록 로직을 구성해야 함
    /// </summary>
    protected override void OnResume()
    {
        var all_tracks = FindAllTrakcs();
        if (all_tracks != null)
        {
            int len = all_tracks.Length;
            for (int i = 0; i < len; i++)
            {
                all_tracks[i].TimeScale = 1f;
            }
        }
    }
    /// <summary>
    /// 지속성 스킬 효과 데이터를 모두 제거<br/>
    /// 데이터를 제거하면 자동으로 이펙트도 사라지도록 구현되어 있음
    /// </summary>
    protected void ClearDurationSkillDataList()
    {
        lock (Duration_Lock)
        {
            int cnt = Used_Battle_Duration_Data_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                var duration = Used_Battle_Duration_Data_List[i];
                duration.Dispose();
            }
            Used_Battle_Duration_Data_List.Clear();
        }
        
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Render_Texture == null)
        {
            Render_Texture = GetComponent<SkeletonRenderTexture>();
        }
        if (ZOrder == null)
        {
            ZOrder = Render_Texture.quad.AddComponent<RendererSortingZ>();
            ZOrder.SetZorderIndex(ZORDER_INDEX.HERO);

            Render_Texture.quad.AddComponent<SortingGroup>();
        }
        
    }
    public override void Despawned()
    {
        base.Despawned();

        Team_Mng = null;
    }

    /// <summary>
    /// 지정한 유닛을 기준으로 지정거리내에 들어오는지 여부 반환
    /// </summary>
    /// <param name="center"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    public bool InRange(UnitBase_V2 center, float distance)
    {
        return GetDistanceFromCenter(center) <= distance;
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
    /// 적에게서 데미지를 받는다.
    /// </summary>
    /// <param name="dmg"></param>
    public void AddDamage(BATTLE_SEND_DATA dmg)
    {
        if (!IsAlive())
        {
            return;
        }

        double damage = dmg.Damage;
        

        //  피해감소 체크
        double damage_reduce = GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE.DAMAGE_REDUCE);
        if (damage_reduce > 0)
        {
            damage -= damage * damage_reduce;
            dmg.Duration_Effect_Type = DURATION_EFFECT_TYPE.DAMAGE_REDUCE;
            CalcDurationCountUse(PERSISTENCE_TYPE.HITTED);
        }

        damage = Math.Truncate(dmg.Damage);
        if (damage <= 0)
        {
            return;
        }

        Life -= damage;
        if (Life <= 0)
        {
            Life = 0;
            //SpawnDamageText(dmg);
            AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Damage_Normal_Effect_Text", Life_Bar_Pos, dmg, 1f);
            UpdateLifeBar();
            ChangeState(UNIT_STATES.DEATH);
            return;
        }

        //SpawnDamageText(dmg);
        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Damage_Normal_Effect_Text", Life_Bar_Pos, dmg, 1f);
        UpdateLifeBar();
    }
    /// <summary>
    /// 체력 회복
    /// </summary>
    /// <param name="recovery_hp"></param>
    public void AddLifeRecovery(double recovery_hp)
    {
        if (!IsAlive())
        {
            return;
        }
        if (recovery_hp < 0)
        {
            return;
        }
        recovery_hp = Math.Truncate(recovery_hp);
        Life += recovery_hp;
        if (Life > Max_Life)
        {
            Life = Max_Life;
        }
        //SpawnHealText(recovery_hp);
        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Heal_Normal_Effect_Text", Life_Bar_Pos, recovery_hp, 1f);
        UpdateLifeBar();
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

    protected void AddSpawnEffectText(string path, Transform target, object data, float duration)
    {
        lock (Effect_Queue_Lock)
        {
            var d = new Effect_Queue_Data();
            d.effect_path = path;
            d.Target_Position = target;
            d.Data = data;
            d.Duration = duration;
            Effect_Queue_Data_List.Add(d);
        }
    }

    public override void OnUpdate(float dt)
    {
        base.OnUpdate(dt);
        
        lock (Effect_Queue_Lock)
        {
            if (Effect_Queue_Data_List.Count > 0)
            {
                Effect_Queue_Interval -= dt;
                if (Effect_Queue_Interval < 0f)
                {
                    var edata = Effect_Queue_Data_List[0];
                    Effect_Queue_Data_List.RemoveAt(0);

                    SpawnQueueEffect(edata);

                    Effect_Queue_Interval = 0.1f;
                }
                
            }
        }
    }

    /// <summary>
    /// 이펙트 소환
    /// </summary>
    /// <param name="edata"></param>
    void SpawnQueueEffect(Effect_Queue_Data edata)
    {
        var effect = Battle_Mng.GetEffectFactory().CreateEffect(edata.effect_path, UI_Mng.GetDamageContainer());
        effect.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, edata.Target_Position.position);
        effect.SetData(edata.Data);
        effect.StartParticle(edata.Duration);
    }

    //protected void SpawnHealText(double recovery_hp)
    //{
    //    var heal_effect = (Heal_Normal_Effect_Text)Battle_Mng.GetEffectFactory().CreateEffect("Assets/AssetResources/Prefabs/Effects/UI/Heal_Normal_Effect_Text", UI_Mng.GetDamageContainer());
    //    heal_effect.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, Life_Bar_Pos.position);
    //    heal_effect.SetData(Math.Truncate(recovery_hp));
    //    heal_effect.StartParticle(1f);
    //}

    //protected void SpawnDamageText(BATTLE_SEND_DATA dmg)
    //{
    //    var dmg_effect = (Damage_Normal_Effect_Text)Battle_Mng.GetEffectFactory().CreateEffect("Assets/AssetResources/Prefabs/Effects/UI/Damage_Normal_Effect_Text", UI_Mng.GetDamageContainer());
    //    dmg_effect.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, Life_Bar_Pos.position);
    //    dmg_effect.SetData(dmg);
    //    dmg_effect.StartParticle(1f);
    //}

    //protected void SpawnTransText(DURATION_EFFECT_TYPE etype)
    //{
    //    var trans_effect = (Trans_Effect_Text)Battle_Mng.GetEffectFactory().CreateEffect("Assets/AssetResources/Prefabs/Effects/UI/Trans_Effect_Text", UI_Mng.GetDamageContainer());
    //    trans_effect.transform.position = RectTransformUtility.WorldToScreenPoint(Camera.main, GetBodyPositionByProjectileType(PROJECTILE_TYPE.INSTANT_TARGET_BODY).position);
    //    trans_effect.SetData(etype);
    //    trans_effect.StartParticle(1f);
    //}

    /// <summary>
    /// 지속성 효과 추가(확률에 따라 적용됨)
    /// </summary>
    /// <param name="duration_skill"></param>
    public void AddDurationSkillEffect(BattleDurationSkillData duration_skill)
    {
        if (!IsAlive())
        {
            duration_skill.Dispose();
            duration_skill = null;
            return;
        }

        int rate = UnityEngine.Random.Range(0, 10000);
        if (rate < duration_skill.GetRate())
        {
            lock (Duration_Lock)
            {
                var d_type = duration_skill.GetDurationEffectType();
                if (duration_skill.IsOverlapable())
                {
                    Used_Battle_Duration_Data_List.Add(duration_skill);

                    AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Trans_Effect_Text", GetBodyPositionByProjectileType(PROJECTILE_TYPE.INSTANT_TARGET_BODY), d_type, 1f);
                }
                else
                {
                    //  같은 스킬 효과가 있을 경우 교체(나중에 로직에 따라 변경하자)
                    var found = Used_Battle_Duration_Data_List.Find(x => x.GetDurationEffectType() == duration_skill.GetDurationEffectType());
                    if (found != null)
                    {
                        Used_Battle_Duration_Data_List.Remove(found);
                        found.Dispose();
                        found = null;
                    }

                    if (d_type == DURATION_EFFECT_TYPE.FREEZE)
                    {
                        ChangeState(UNIT_STATES.FREEZE);
                    }
                    else if (d_type == DURATION_EFFECT_TYPE.STUN)
                    {
                        ChangeState(UNIT_STATES.STUN);
                    }
                    else if (d_type == DURATION_EFFECT_TYPE.BIND)
                    {
                        ChangeState(UNIT_STATES.BIND);
                    }

                    Used_Battle_Duration_Data_List.Add(duration_skill);
                    AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Trans_Effect_Text", GetBodyPositionByProjectileType(PROJECTILE_TYPE.INSTANT_TARGET_BODY), d_type, 1f);

                }
            }
            
        }
        else
        {
            duration_skill.Dispose();
            duration_skill = null;
        }
    }

    /// <summary>
    /// 지속성 효과에서 발생하는 일회성 효과 이펙트 구현
    /// </summary>
    void SpawnOnetimeEffectFromDurationSkill(BattleOnetimeSkillData onetime, BATTLE_SEND_DATA send_data)
    {
        var factory = Battle_Mng.GetEffectFactory();

        string effect_path = onetime.GetEffectPrefab();
        if (string.IsNullOrEmpty(effect_path))
        {
            //  todo
            onetime.ExecSkill(send_data);
        }
        else
        {
            var effect = (SkillEffectBase)factory.CreateEffect(effect_path);
            effect.SetBattleSendData(send_data);

            PROJECTILE_TYPE ptype = onetime.GetProjectileType();
            var target_trans = GetBodyPositionByProjectileType(ptype);
            var target_pos = target_trans.position;
            if (onetime.IsThrowingNode())
            {
                //  todo
            }
            else
            {
                target_pos.z = transform.position.z;
                effect.transform.position = target_pos;
                effect.StartParticle((float)onetime.GetEffectDuration());
            }
        }
    }

    /// <summary>
    /// 지속성 효과의 시간을 계산 해주기 위한 함수 V2
    /// </summary>
    protected void CalcDurationSkillTime()
    {
        lock (Duration_Lock)
        {
            if (Used_Battle_Duration_Data_List.Count == 0)
            {
                return;
            }

            //  상태변경용 내부 함수
            System.Action<UNIT_STATES, DURATION_EFFECT_TYPE> trans_change_state = (state, dtype) =>
            {
                if (dtype == DURATION_EFFECT_TYPE.FREEZE)
                {
                    if (state == UNIT_STATES.FREEZE)
                    {
                        ChangeState(UNIT_STATES.ATTACK_READY_1);
                    }
                }
                else if (dtype == DURATION_EFFECT_TYPE.STUN)
                {
                    if (state == UNIT_STATES.STUN)
                    {
                        ChangeState(UNIT_STATES.ATTACK_READY_1);
                    }
                }
                else if (dtype == DURATION_EFFECT_TYPE.BIND)
                {
                    if (state == UNIT_STATES.BIND)
                    {
                        ChangeState(UNIT_STATES.ATTACK_READY_1);
                    }
                }
            };

            var state = GetCurrentState();
            float dt = Time.deltaTime;
            Remove_Reserved_Duration_Data_List.Clear();
            for (int i = 0; i < Used_Battle_Duration_Data_List.Count; i++)
            {
                BattleDurationSkillData duration = Used_Battle_Duration_Data_List[i];
                DURATION_CALC_RESULT_TYPE result = duration.CalcDuration_V2(dt);
                if (result != DURATION_CALC_RESULT_TYPE.NONE)
                {
                    var d_type = duration.GetDurationEffectType();
                    switch (result)
                    {
                        case DURATION_CALC_RESULT_TYPE.REPEAT_INTERVAL:
                            {
                                //  반복 효과 적용
                                if (duration.IsUseRepeatInterval())
                                {
                                    var repeat_onetime_list = duration.GetRepeatOnetimeSkillDataList();
                                    int repeat_onetime_cnt = repeat_onetime_list.Count;
                                    for (int o = 0; o < repeat_onetime_cnt; o++)
                                    {
                                        var send_data = duration.GetBattleSendData().Clone();
                                        //  todo execSkill
                                        var onetime = repeat_onetime_list[o];
                                        send_data.Onetime = onetime;
                                        //  일회성 효과 이펙트 
                                        SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }

                            }
                            break;
                        case DURATION_CALC_RESULT_TYPE.FINISH:
                            {
                                //  종료 효과 적용 체크
                                if (duration.IsUseFinishEffect())
                                {
                                    var finish_onetime_list = duration.GetFinishOnetimeSkillDataList();
                                    int finish_onetime_cnt = finish_onetime_list.Count;
                                    for (int f = 0; f < finish_onetime_cnt; f++)
                                    {
                                        var send_data = duration.GetBattleSendData().Clone();
                                        var onetime = finish_onetime_list[f];
                                        send_data.Onetime = onetime;
                                        //  일회성 효과 이펙트
                                        SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }
                                Remove_Reserved_Duration_Data_List.Add(duration);
                                //  상태변경
                                trans_change_state(state, d_type);


                            }
                            break;
                        case DURATION_CALC_RESULT_TYPE.REPEAT_AND_FINISH:
                            {
                                //  반복 효과 적용
                                if (duration.IsUseRepeatInterval())
                                {
                                    var repeat_onetime_list = duration.GetRepeatOnetimeSkillDataList();
                                    int repeat_onetime_cnt = repeat_onetime_list.Count;
                                    for (int o = 0; o < repeat_onetime_cnt; o++)
                                    {
                                        var send_data = duration.GetBattleSendData().Clone();
                                        //  todo execSkill
                                        var onetime = repeat_onetime_list[o];
                                        send_data.Onetime = onetime;
                                        //  일회성 효과 이펙트 
                                        SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }
                                //  종료 효과 적용
                                if (duration.IsUseFinishEffect())
                                {
                                    var finish_onetime_list = duration.GetFinishOnetimeSkillDataList();
                                    int finish_onetime_cnt = finish_onetime_list.Count;
                                    for (int f = 0; f < finish_onetime_cnt; f++)
                                    {
                                        var send_data = duration.GetBattleSendData().Clone();
                                        var onetime = finish_onetime_list[f];
                                        send_data.Onetime = onetime;
                                        //  일회성 효과 이펙트
                                        SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }
                                Remove_Reserved_Duration_Data_List.Add(duration);
                                //  상태변경
                                trans_change_state(state, d_type);
                            }
                            break;
                    }
                }
            }
            //  종료된 스킬 제거
            if (Remove_Reserved_Duration_Data_List.Count > 0)
            {
                int reserved_cnt = Remove_Reserved_Duration_Data_List.Count;
                for (int i = 0; i < reserved_cnt; i++)
                {
                    BattleDurationSkillData duration = Remove_Reserved_Duration_Data_List[i];
                    Used_Battle_Duration_Data_List.Remove(duration);
                    duration.Dispose();
                    duration = null;
                }
            }
        }


    }

   

    /// <summary>
    /// 지속성 효과의 지속성 방식 타입 갱신<br/>
    /// 시간 지속성 방식은 <see cref="CalcDurationSkillTime"/>을 사용한다.<br/>
    /// 그 외에 피격 횟수 제한, 공격 횟수 제한의 경우 본 함수 사용<br/>
    /// </summary>
    /// <param name="ptype"></param>
    protected void CalcDurationCountUse(PERSISTENCE_TYPE ptype)
    {
        if (ptype == PERSISTENCE_TYPE.TIME)
        {
            return;
        }
        
        lock (Duration_Lock)
        {
            int cnt = Used_Battle_Duration_Data_List.Count;
            if (cnt == 0)
            {
                return;
            }
            Remove_Reserved_Duration_Data_List.Clear();
            for (int i = 0; i < cnt; i++)
            {
                BattleDurationSkillData duration = Used_Battle_Duration_Data_List[i];
                if (duration.CalcEtcPersistenceCount(ptype))
                {
                    //  지속 횟수 종료
                    Remove_Reserved_Duration_Data_List.Add(duration);
                }
            }
            //  종료된 지속성 효과 제거
            if (Remove_Reserved_Duration_Data_List.Count > 0)
            {
                int reserved_cnt = Remove_Reserved_Duration_Data_List.Count;
                for (int i = 0; i < reserved_cnt; i++)
                {
                    BattleDurationSkillData duration = Remove_Reserved_Duration_Data_List[i];
                    Used_Battle_Duration_Data_List.Remove(duration);
                    duration.Dispose();
                    duration = null;
                }
            }
        }
        
    }
    /// <summary>
    /// Left Team 이동<br/>
    /// 적을 탐색하면서 이동한다.
    /// </summary>
    protected void MoveLeftTeam()
    {
        FindTargets();
        if (Normal_Attack_Target.Count > 0)
        {
            ChangeState(UNIT_STATES.ATTACK_READY_1);
            return;
        }

        float move = (float)Move_Speed * Time.deltaTime;
        var pos = this.transform.localPosition;
        pos.x += move;
        if (Is_Reposition)
        {
            float zmove = (float)Move_Speed * Time.deltaTime;
            pos.z += zmove;
        }
        this.transform.localPosition = pos;
    }
    /// <summary>
    /// Right Team 이동<br/>
    /// 적을 탐색하면서 이동한다.
    /// </summary>
    protected void MoveRightTeam()
    {
        FindTargets();
        if (Normal_Attack_Target.Count > 0)
        {
            ChangeState(UNIT_STATES.ATTACK_READY_1);
            return;
        }

        float move = (float)Move_Speed * Time.deltaTime;
        var pos = this.transform.localPosition;
        pos.x -= move;
        if (Is_Reposition)
        {
            float zmove = (float)Move_Speed * Time.deltaTime;
            pos.z += zmove;
        }
        this.transform.localPosition = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        UnitTriggerEnter(other);
    }
    private void OnTriggerExit(Collider other)
    {
        UnitTriggerExit(other);
    }

    protected virtual void UnitTriggerEnter(Collider other) 
    {
        if (other.gameObject.CompareTag(GameDefine.TAG_HERO))
        {
            var monster = other.gameObject.GetComponent<HeroBase_V2>();
            if (monster != null)
            {
                if (monster.Deck_Order < Deck_Order)
                {
                    //  change reposition
                    Is_Reposition = true;
                }
            }
        }
    }
    protected virtual void UnitTriggerExit(Collider other) 
    {
        if (other.gameObject.CompareTag(GameDefine.TAG_HERO))
        {
            var monster = other.gameObject.GetComponent<HeroBase_V2>();
            if (monster != null)
            {
                if (monster.Deck_Order < Deck_Order)
                {
                    //  change reposition
                    Is_Reposition = false;
                }
            }
        }
    }


    public override string ToString()
    {
        var sb = ZString.CreateStringBuilder();
        sb.AppendFormat("Team Type : <color=#ffffff>[{0}]</color>", Team_Type);
        return sb.ToString();
    }

    [ContextMenu("ToStringHeroBase")]
    public void ToStringHeroBase()
    {
        Debug.Log(ToString());
    }
}
