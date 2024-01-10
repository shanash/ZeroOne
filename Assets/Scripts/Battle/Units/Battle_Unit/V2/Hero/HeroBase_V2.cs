using Cysharp.Text;
using FluffyDuck.Util;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//public enum SD_BODY_TYPE
//{
//    NONE = 0,
//    HEAD,
//    BODY,
//    FOOT,
//}


/// <summary>
/// 플레이어 유닛/NPC 유닛 등 전투에서 사용하는 플레이어의 베이스가 되는 클래스<br/>
/// 몬스터(NPC)는 본 클래스를 상속받아 추가 구현을 해야한다.<br/>
/// 참조가 되는 데이터 타입이 각각 다르기 때문에 상속 필요.<br/>
/// 전투 캐릭의 기본이 되는 여러 가지 기능은 본 클래스에 담아둔다.<br/>
/// </summary>
[RequireComponent(typeof(SkeletonUtility))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(UnitRenderTexture))]
[RequireComponent(typeof(RendererSortingZ))]
[RequireComponent(typeof(SortingGroup))]


public partial class HeroBase_V2 : UnitBase_V2
{
    [SerializeField, Tooltip("Skeleton")]
    protected SkeletonAnimation Skeleton;

    [SerializeField, Tooltip("Life Bar Pos")]
    protected Transform Life_Bar_Pos;

    [SerializeField, Tooltip("Body Pos Type")]
    protected List<SD_Body_Pos_Data> Sd_Body_Transforms;

    protected UnitRenderTexture Render_Texture;

    protected SkeletonUtility Utility;

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
    /// 추가 최대 체력. 스킬 및 장비 옵션 등으로 인하여 일시적으로 증가된 최대 체력 (사용 안할듯) 
    /// </summary>
    public double Added_Max_Life { get; protected set; }
    /// <summary>
    /// 물리 공격력. 
    /// </summary>
    public double Attack { get; protected set; }

    /// <summary>
    /// 방어력<br/>
    /// 방어율 = 1/(1+방어력/100)
    /// 최종 데미지 = 적 데미지 * 방어율
    /// </summary>
    public double Defense { get; protected set; }

    /// <summary>
    /// 이동 속도
    /// </summary>
    public double Move_Speed { get; protected set; }

    /// <summary>
    /// 물리 명중률<br/>
    /// 명중 기본값 및 민첩 수치에 따라 결정<br/>
    /// 명중 레벨 테이블의 값을 참조한다.<br/>
    /// 명중 확률  = 1 - 회피 확률
    /// 명중 실패시 데미지 감소(빗나감 데미지)
    /// </summary>
    public double Accuracy { get; protected set; }

    /// <summary>
    /// 물리 회피율<br/>
    /// 회피 기본값 및 민첩 수치에 따라 결정<br/>
    /// 회피 레벨 테이블의 값을 참조한다.<br/>
    /// 회피 확률 = 1 / (1 + 100 / 회피값)
    /// </summary>
    public double Evasion { get; protected set; }

    /// <summary>
    /// 자동 회복<br/>
    /// 한 웨이브를 클리어 했을 때 회복되는 수치<br/>
    /// 자동 회복량 = 최대 체력 * 자동 회복 값(배율)
    /// </summary>
    public double Auto_Recovery_Life { get; protected set; }

    /// <summary>
    /// 치명타 확률<br/>
    /// 크리티컬 발생 확률 = 크리티컬 값 * 0.05 * 0.01 * 레벨 / 적 레벨
    /// </summary>
    public double Critical_Rate { get; protected set; }
    /// <summary>
    /// 치명타 파워 - 기본 공격력의 배수
    /// </summary>
    public double Critical_Power { get; protected set; }

    /// <summary>
    /// 흡혈 
    /// </summary>
    public double Vampire_Point { get; protected set; }

    /// <summary>
    /// 보유 에너지
    /// </summary>
    public int Energy_Value { get; protected set; }


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
    /// 영웅 데이터
    /// </summary>
    protected BattleUnitData Unit_Data;

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

    /// <summary>
    /// 스킬 슬롯에서 필요한 이벤트를 받기 위해 델리게이트 등록
    /// </summary>
    /// <param name="evt_type"></param>
    public delegate void Skill_Slot_Event_Delegate(SKILL_SLOT_EVENT_TYPE evt_type);
    public event Skill_Slot_Event_Delegate Slot_Events;


    /// <summary>
    /// 게임 타입
    /// </summary>
    protected GAME_TYPE Game_Type = GAME_TYPE.NONE;



    public virtual void SetBattleUnitDataID(params int[] unit_ids)
    {
        if (unit_ids.Length < 2)
        {
            Debug.Assert(false);
        }
        int pc_id = unit_ids[0];
        int pc_num = unit_ids[1];

        Unit_Data = new BattlePcData();
        Unit_Data.SetUnitID(pc_id, pc_num);


        Skill_Mng = new BattleSkillManager();
        Skill_Mng.SetPlayerCharacterSkillGroups(Unit_Data.GetSkillPattern());
    }


    public void SetDeckOrder(int order)
    {
        Deck_Order = order;
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
        Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.LIFE_UPDATE);
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
    protected virtual void SpineAnimationStart(TrackEntry entry) 
    {
        string animation_name = entry.Animation.Name;

        UNIT_STATES state = GetCurrentState();
        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();
            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                SkillCastEffectSpawn(skill);
            }
        }

    }
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
        else if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();

            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                GetSkillManager().SetNextSkillPattern();
                FindApproachTargets();
                if (Normal_Attack_Target.Count == 0)
                {
                    ChangeState(UNIT_STATES.MOVE);
                }
                else
                {
                    ChangeState(UNIT_STATES.ATTACK_READY_1);
                }
                return;
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
    protected virtual void SpineAnimationEvent(TrackEntry entry, Spine.Event evt)
    {
        string animation_name = entry.Animation.Name;
        string evt_name = evt.Data.Name;
        UNIT_STATES state = GetCurrentState();

        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();

            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                var exec_list = skill.GetExecuableSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        SkillEffectSpawnV2(exec_list[i]);
                    }
                }
            }

        }
    }

    public void SetTeamManager(TeamManager_V2 mng)
    {
        Team_Mng = mng;

        Team_Type = Team_Mng.Team_Type;
    }

    /// <summary>
    /// 스켈레톤 알파값 수정
    /// </summary>
    /// <param name="alpha"></param>
    public void SetAlpha(float alpha)
    {
        if (Render_Texture == null)
        {
            Render_Texture = GetComponent<UnitRenderTexture>();
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
        scale.x = Mathf.Abs(scale.x) * flip;
        this.transform.localScale = scale;
    }

    public bool IsAlive()
    {
        return Life > 0;
    }


    protected void PlayAnimation(int track, string anim_name, bool loop)
    {
        Skeleton.AnimationState.SetAnimation(track, anim_name, loop);
    }
    protected void AddAnimation(int track, string anim_name, bool loop)
    {
        Skeleton.AnimationState.AddAnimation(track, anim_name, loop, 0);
    }

    /// <summary>
    /// 동작 플레이 타입
    /// </summary>
    /// <param name="ani_type"></param>
    protected virtual void PlayAnimation(HERO_PLAY_ANIMATION_TYPE ani_type)
    {
        switch (ani_type)
        {
            case HERO_PLAY_ANIMATION_TYPE.NONE:
                break;
            case HERO_PLAY_ANIMATION_TYPE.PREPARE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_01:
                PlayAnimation(1, "1_idle", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                PlayAnimation(1, "1_run", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.STUN:
                PlayAnimation(1, "1_stun", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                PlayAnimation(1, "1_death", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
                PlayAnimation(1, "1_win", true);
                break;
        }
    }



    /// <summary>
    /// 가장 가까운 적을 찾는다.
    /// 공격하기 위함이 아닌, 단지 사거리를 유지하기 위해서
    /// </summary>
    protected virtual void FindApproachTargets()
    {
        float distance = GetApproachDistance();
        Team_Mng.FindTargetInRangeAtApproach(this, TARGET_TYPE.ENEMY_TEAM, distance, ref Normal_Attack_Target);
    }
    /// <summary>
    /// 각 스킬에 지정되어 있는 타겟 찾기
    /// </summary>
    /// <param name="skill"></param>
    protected virtual void FindTargets(BattleSkillData skill)
    {
        Team_Mng.FindTargetInRange(this, skill.GetTargetType(), skill.GetTargetRuleType(), 0, skill.GetTargetOrder(), skill.GetTargetCount(), skill.GetTargetRange(), ref Normal_Attack_Target);
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

    protected virtual void SkillCastEffectSpawn(BattleSkillGroup skill_grp)
    {
        string effect_path = skill_grp.GetSkillCastEffectPath();
        float duration = skill_grp.GetSkillCastEffectDuration();
        if (string.IsNullOrEmpty(effect_path))
        {
            return;
        }

        var factory = Battle_Mng.GetEffectFactory();
        var eff = factory.CreateEffect(effect_path);
        eff.transform.position = this.transform.position;
        eff.StartParticle(duration);
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
            dmg.Damage = GetAttackPoint();
            dmg.Effect_Weight_Index = effect_weight_index;

            string skill_effect_prefab = skill.GetTriggerEffectPrefabPath();
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
                dmg.Damage = GetAttackPoint();
                dmg.Effect_Weight_Index = effect_weight_index;

                string skill_effect_prefab = skill.GetTriggerEffectPrefabPath();
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
                        var body_type = skill.GetSDBodyType();

                        Transform this_trans = this.transform;
                        if (body_type == SD_BODY_TYPE.NONE)
                        {
                            this_trans = GetBodyTypeTransform(body_type);
                        }
                        
                        if (onetime.IsThrowingNode())
                        {
                            //target_pos.z = this.transform.position.z;
                            target_pos.z = this_trans.position.z;
                            effect.transform.position = GetBodyPositionByProjectileType(PROJECTILE_TYPE.THROW_BODY).position;

                            float distance = Vector3.Distance(effect.transform.position, target_pos);
                            float throwing_duration = distance / (float)skill.GetProjectileSpeed();

                            effect.MoveTarget(target_trans, throwing_duration);
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

                    var target_pos = target_trans.position;
                    if (skill.IsThrowingNode())
                    {
                        var effect = (SkillEffectBase)factory.CreateEffect(skill_effect_prefab);
                        effect.SetBattleSendData(dmg);
                        target_pos.z = this.transform.position.z;
                        effect.transform.position = GetBodyPositionByProjectileType(PROJECTILE_TYPE.THROW_BODY).position;

                        var body_type = skill.GetSDBodyType();
                        Transform this_trans = this.transform;
                        if (body_type == SD_BODY_TYPE.NONE)
                        {
                            this_trans = GetBodyTypeTransform(body_type);
                        }

                        //float distance = Vector3.Distance(transform.position, target_pos);
                        float distance = Vector3.Distance(this_trans.position, target_pos);
                        float throwing_duration = distance / (float)skill.GetProjectileSpeed();

                        //effect.MoveTarget(target_trans, (float)skill.GetEffectDuration());
                        effect.MoveTarget(target_trans, throwing_duration);
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

        Game_Type = BlackBoard.Instance.GetBlackBoardData<GAME_TYPE>(BLACK_BOARD_KEY.GAME_TYPE, GAME_TYPE.STORY_MODE);

        if (Render_Texture == null)
        {
            Render_Texture = GetComponent<UnitRenderTexture>();
        }
        if (Render_Texture.quad != null)
        {

        }

        if (Utility == null)
        {
            Utility = GetComponent<SkeletonUtility>();
        }
    }
    public override void Despawned()
    {
        base.Despawned();
        Game_Type = GAME_TYPE.NONE;
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
    /// 피격자가 최종 데미지를 계산 후 데미지 적용이 되면, 해당 데미지를 캐스터(공격자)에게 전달해준다.<br/>
    /// 공격자는 필요에 따라 최종 데미지를 이용하여 회복 등을 사용할 수 있다.<br/>
    /// 체력 회복량 = 준 데미지 * HP 흡수 포인트 / (HP 흡수 포인트 + 상대 레벨 + 100)
    /// </summary>
    /// <param name="damage"></param>
    public void SendLastDamage(BATTLE_SEND_DATA data)
    {
        if (data.Targets.Count == 0)
        {
            return;
        }
        //  todo
        double last_damage = data.Damage;
        var target = data.Targets[0];
        double vampire_hp = last_damage * Vampire_Point / (Vampire_Point + target.GetLevel() + 100);
        if (vampire_hp > 0)
        {
            AddLifeRecovery(vampire_hp);
        }

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

        double last_damage = dmg.Damage;

        //  회피 여기서 계산할까? 회피하면 데미지가 줄어든다고 했는데, 그 로직은?
        if (IsEvation(dmg.Caster.Accuracy))
        {
            last_damage = dmg.Damage * 0.9;  //  임시로 회피시 데미지의 90%만 들어가도록 하자.
        }


        //  피해감소 체크
        double damage_reduce = GetDurationSkillTypesMultiples(DURATION_EFFECT_TYPE.DAMAGE_REDUCE);
        if (damage_reduce > 0)
        {
            last_damage -= last_damage * damage_reduce;
            dmg.Duration_Effect_Type = DURATION_EFFECT_TYPE.DAMAGE_REDUCE;
            CalcDurationCountUse(PERSISTENCE_TYPE.HITTED);
        }

        //  방어율 = 상수 + 방어력 / 100
        //  최종 데미지 계산 (최종 데미지 = 적 데미지 * 방어율)
        last_damage = GetCalcDamage(last_damage);

        //  치명타 확률
        double cri_chance = GetCriticalChanceRate(dmg.Caster.GetLevel(), GetLevel());
        int r = UnityEngine.Random.Range(0, 100000);
        if (r < cri_chance)
        {
            dmg.Is_Critical = true;
            last_damage *= GetCriticalPowerPoint();
        }

        if (last_damage <= 0)
        {
            return;
        }
        dmg.Damage = last_damage;

        //  최종 데미지 계산 후, 캐스터(공격자)에게 전달. 결과를 사용할 일이 있기 때문에
        dmg.Caster.SendLastDamage(dmg);

        Render_Texture.SetHitColorV2(Color.white, 0.05f);
        if (Game_Type != GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE && Game_Type != GAME_TYPE.EDITOR_SKILL_EDIT_MODE)
        {
            Life -= last_damage;
        }

        if (Life <= 0)
        {
            Life = 0;
            AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Damage_Normal_Effect_Text", Life_Bar_Pos, dmg, 1f);
            UpdateLifeBar();
            ChangeState(UNIT_STATES.DEATH);
            return;
        }

        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Damage_Normal_Effect_Text", Life_Bar_Pos, dmg, 1f);
        UpdateLifeBar();

        Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.HITTED);
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
        if (recovery_hp <= 0)
        {
            return;
        }

        Life += recovery_hp;
        if (Life > Max_Life)
        {
            Life = Max_Life;
        }

        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/UI/Heal_Normal_Effect_Text", Life_Bar_Pos, recovery_hp, 1f);
        UpdateLifeBar();
    }

    /// <summary>
    /// 웨이브 종료시 체력 회복
    /// </summary>
    public void WaveEndRecoveryLife()
    {
        double recovery_hp = Max_Life * Auto_Recovery_Life;
        AddLifeRecovery(recovery_hp);
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

        var caster = duration_skill.GetBattleSendData().Caster;

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
                Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
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
                                        BATTLE_SEND_DATA send_data = duration.GetBattleSendData().Clone();
                                        //  todo execSkill
                                        BattleOnetimeSkillData onetime = repeat_onetime_list[o];
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
                                        BATTLE_SEND_DATA send_data = duration.GetBattleSendData().Clone();
                                        BattleOnetimeSkillData onetime = finish_onetime_list[f];
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
                                        BATTLE_SEND_DATA send_data = duration.GetBattleSendData().Clone();
                                        //  todo execSkill
                                        BattleOnetimeSkillData onetime = repeat_onetime_list[o];
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
                                        BATTLE_SEND_DATA send_data = duration.GetBattleSendData().Clone();
                                        BattleOnetimeSkillData onetime = finish_onetime_list[f];
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

                Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
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
                Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
            }
        }

    }
    /// <summary>
    /// Left Team 이동<br/>
    /// 적을 탐색하면서 이동한다.
    /// </summary>
    protected void MoveLeftTeam()
    {
        FindApproachTargets();
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
        FindApproachTargets();
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
    /// <summary>
    /// Left Team 화면 전환때까지 반대쪽으로 달리기(웨이브 이동)
    /// </summary>
    protected void WaveRunLeftTeam()
    {
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
    /// 궁극기 시전 요청
    /// </summary>
    public void SpecialSkillExec()
    {
        //  여러가지 상황상 궁극기를 사용할 수 없는 상황을 체크
        //  체크 완료 후 궁극기를 사용할 수 있는 경우에만 궁극기 사용
        //ChangeState(UNIT_STATES.SKILL_1);
        Debug.Log("SpecialSkillExec");
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
