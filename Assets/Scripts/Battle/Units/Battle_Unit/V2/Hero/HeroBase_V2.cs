using Cysharp.Text;
using FluffyDuck.Util;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

/// <summary>
/// 플레이어 유닛/NPC 유닛 등 전투에서 사용하는 플레이어의 베이스가 되는 클래스<br/>
/// 몬스터(NPC)는 본 클래스를 상속받아 추가 구현을 해야한다.<br/>
/// 참조가 되는 데이터 타입이 각각 다르기 때문에 상속 필요.<br/>
/// 전투 캐릭의 기본이 되는 여러 가지 기능은 본 클래스에 담아둔다.<br/>
/// </summary>
[RequireComponent(typeof(SkeletonUtility))]
[RequireComponent(typeof(UnitRenderTexture))]
[RequireComponent(typeof(RendererSortingZ))]
[RequireComponent(typeof(SortingGroup))]
public partial class HeroBase_V2 : UnitBase_V2
{
    [SerializeField, Tooltip("Skeleton")]
    protected SkeletonAnimation Skeleton;

    [SerializeField, Tooltip("Life Bar Pos")]
    protected Transform Life_Bar_Pos;

    [SerializeField, Tooltip("Shadow")]
    protected SpriteRenderer Shadow;

    [SerializeField, Tooltip("Start Projectile Type")]
    protected List<Start_Projectile_Pos_Data> Start_Projectile_Transforms;

    [SerializeField, Tooltip("Reach Pos Type")]
    protected List<Target_Reach_Pos_Data> Reach_Pos_Transforms;

    [SerializeField, Tooltip("Ultimate Skill Playable Director")]
    protected PlayableDirector Ultimate_Skill_Playable_Director;

    protected UnitRenderTexture Render_Texture;

    protected SkeletonUtility Utility;

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
    /// 추가 최대 체력<br/>
    /// 스킬 및 장비 옵션 등으로 인하여 일시적으로 증가된 최대 체력
    /// </summary>
    public double Added_Max_Life { get; protected set; }
    /// <summary>
    /// 물리 공격력. 
    /// </summary>
    public double Physics_Attack => GetPhysicsAttackPoint();
    /// <summary>
    /// 마법 공격력
    /// </summary>
    public double Magic_Attack => GetMagicAttackPoint();

    /// <summary>
    /// 물리 방어력<br/>
    /// 방어율 = 1/(1+방어력/100)
    /// 최종 데미지 = 적 데미지 * 방어율
    /// </summary>
    public double Physics_Defense => GetPhysicsDefensePoint();

    /// <summary>
    /// 마법 방어력<br/>
    /// </summary>
    public double Magic_Defense => GetMagicDefensePoint();

    /// <summary>
    /// 이동 속도
    /// </summary>
    public double Move_Speed => GetMoveSpeed();

    /// <summary>
    /// 물리 명중률<br/>
    /// 명중 기본값 및 민첩 수치에 따라 결정<br/>
    /// 명중 레벨 테이블의 값을 참조한다.<br/>
    /// 명중 확률  = 1 - 회피 확률
    /// 명중 실패시 데미지 감소(빗나감 데미지)
    /// </summary>
    public double Accuracy => GetAccuracy();

    /// <summary>
    /// 물리 회피율<br/>
    /// 회피 기본값 및 민첩 수치에 따라 결정<br/>
    /// 회피 레벨 테이블의 값을 참조한다.<br/>
    /// 회피 확률 = 1 / (1 + 100 / 회피값)
    /// </summary>
    public double Evasion => GetEvasion();

    /// <summary>
    /// 자동 회복<br/>
    /// 한 웨이브를 클리어 했을 때 회복되는 수치<br/>
    /// 자동 회복량 = 최대 체력 * 자동 회복 값(배율)
    /// </summary>
    public double Auto_Recovery_Life => GetAutoRecoveryLife();

    /// <summary>
    /// 물리 치명타 확률<br/>
    /// 크리티컬 발생 확률 = 크리티컬 값 * 0.05 * 0.01 * 레벨 / 적 레벨
    /// </summary>
    public double Physics_Critical_Chance => GetPhysicsCriticalChance();
    /// <summary>
    /// 물리 치명타 파워 증가 - 기본 치명타 배수에 추가 데미지
    /// </summary>
    public double Physics_Critical_Power_Add => GetPhysicsCriticalPowerAdd();
    /// <summary>
    /// 마법 치명타 확률<br/>
    /// </summary>
    public double Magic_Critical_Chance => GetMagicCriticalChance();
    /// <summary>
    /// 마법 치명타 파워
    /// </summary>
    public double Magic_Critical_Power_Add => GetMagicCriticalPowerAdd();

    /// <summary>
    /// 강인함 (상태이상 저항력)<br/>
    /// 상태이상 확률 및 상태이상 지속 시간에 영향<br/>
    /// 상태이상 확률 = 상태이상 확률 - (상태이상 확률 * 강인함 / 1000)
    /// </summary>
    public double Resist_Point => GetResistPoint();

    /// <summary>
    /// 흡혈 
    /// </summary>
    public double Attack_Life_Recovery => GetAttackLifeRecovery();

    /// <summary>
    /// 체력 회복량 증가(힐량 증가)
    /// </summary>
    public double Life_Recovery_Inc => GetLifeRecoveryInc();

    /// <summary>
    /// 보유 에너지
    /// </summary>
    public int Energy_Value { get; protected set; }

    /// <summary>
    /// 무게<br/>
    /// 넉백 및 풀링에서 변수로 사용
    /// </summary>
    public double Weight => GetWeight();

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
    protected LifeBarNode_V2 Life_Bar_V2 = null;

    /// <summary>
    /// 공격용 타겟 리스트
    /// </summary>
    protected List<HeroBase_V2> Attack_Targets = new List<HeroBase_V2>();
    /// <summary>
    /// 접근용 타겟 리스트
    /// </summary>
    protected List<HeroBase_V2> Approach_Targets = new List<HeroBase_V2>();

    /// <summary>
    /// 영웅 데이터
    /// </summary>
    protected BattleUnitData Unit_Data;

    //protected object Duration_Lock = new object();
    ///// <summary>
    ///// 지속성 효과 데이터를 관리
    ///// </summary>
    //protected List<BattleDurationSkillData> Used_Battle_Duration_Data_List = new List<BattleDurationSkillData>();
    ///// <summary>
    ///// 삭제 예약을 위한 지속성 데이터 리스트
    ///// </summary>
    //protected List<BattleDurationSkillData> Remove_Reserved_Duration_Data_List = new List<BattleDurationSkillData>();

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
    /// 게임 배속
    /// </summary>
    protected float Battle_Speed_Multiple = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];

    /// <summary>
    /// 게임 타입
    /// </summary>
    protected GAME_TYPE Game_Type = GAME_TYPE.NONE;

    /// <summary>
    /// 전투 시작 시 최초 시작 위치
    /// </summary>
    protected Vector3 Team_Field_Position = Vector3.zero;


    public virtual void SetBattleUnitData(BattleUnitData unit_dt)
    {
        Unit_Data = unit_dt;
        Unit_Data.SetHeroBase(this);
        //Skill_Mng = new BattleSkillManager();
        //Skill_Mng.SetPlayerCharacterSkillGroups(Unit_Data.GetSkillPattern());
        //Skill_Mng.SetPlayerCharacterSpecialSkillGroup(Unit_Data.GetSpecialSkillID());

    }

    public void SetTeamFieldPosition(Vector3 pos) 
    { 
        Team_Field_Position = pos;
        this.transform.localPosition = Team_Field_Position;
    }

    public void ResetTeamFieldPosition()
    {
        SetTeamFieldPosition(Team_Field_Position);
    }

    public void SetBattleSpeed(float speed)
    {
        Battle_Speed_Multiple = speed;
        //Unit_Data.SetBattleSpeed(speed);
        var tracks = FindAllTrakcs();
        if (tracks != null && tracks.Length > 0)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                var t = tracks[i];
                t.TimeScale = Battle_Speed_Multiple;
            }
        }

        if (Ultimate_Skill_Playable_Director != null)
        {
            if (Ultimate_Skill_Playable_Director.playableGraph.IsValid())
            {
                Ultimate_Skill_Playable_Director.playableGraph.GetRootPlayable(0).SetSpeed(Battle_Speed_Multiple);
            }
            
        }
    }


    public void SetDeckOrder(int order)
    {
        Deck_Order = order;
    }

    protected virtual void StartPlayableDirector()
    {
        if (Ultimate_Skill_Playable_Director == null)
        {
            return;
        }

        Battle_Mng.GetVirtualCineManager().ResetVirtualCameraEtcVars();

        Ultimate_Skill_Playable_Director.Play();
        if (Ultimate_Skill_Playable_Director.playableGraph.IsValid())
        {
            Ultimate_Skill_Playable_Director.playableGraph.GetRootPlayable(0).SetSpeed(Battle_Speed_Multiple);
        }
        //  궁극기 캐스팅 이펙트
        var skill = GetSkillManager().GetSpecialSkillGroup();
        SpawnSkillCastEffect(skill);
    }

    protected virtual void SetPlayableDirector()
    {
        if (Ultimate_Skill_Playable_Director == null)
        {
            return;
        }

        var unit_back_bg = Battle_Mng.GetBattleField().GetUnitBackFaceBG();
        var virtual_cam = Battle_Mng.GetVirtualCineManager();
        var brain_cam = virtual_cam.GetBrainCam();
        var stage_cam = virtual_cam.GetStageCamera();

        var character_cam = virtual_cam.GetCharacterCamera();
        var free_cam = virtual_cam.GetFreeCamera();
        var land_cam = virtual_cam.GetLandscapeCamera();

        var ta = (TimelineAsset)Ultimate_Skill_Playable_Director.playableAsset;
        var tracks = ta.GetOutputTracks();

        foreach (var track in tracks)
        {
            if (track is AnimationTrack)
            {
                if (track.name.Equals("imation_TrackAnimation_Track"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, unit_back_bg.GetComponent<Animator>());
                }
                else if (track.name.Equals("CharacterCameraAnimationTrack"))
                {
                    character_cam.Follow = this.transform;
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, character_cam.GetComponent<Animator>());
                }
                else if (track.name.Equals("LandscapeCameraAnimationTrack"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, land_cam.GetComponent<Animator>());
                }
            }
            else if (track is CinemachineTrack)
            {
                if (track.name.Equals("Cinemachine_Track"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, brain_cam);
                    var clips = track.GetClips();
                    foreach (var clip in clips)
                    {
                        CinemachineShot shot = clip.asset as CinemachineShot;
                        if (shot != null)
                        {
                            if (shot.DisplayName.Equals("CharacterCamera"))
                            {
                                Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, character_cam);
                            }
                            else if (shot.DisplayName.Equals("FreeCamera"))
                            {
                                Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, free_cam);
                            }
                            else if (shot.DisplayName.Equals("StageCamera"))
                            {
                                Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, stage_cam);
                            }
                            else if (shot.DisplayName.Equals("LandscapeCamera"))
                            {
                                Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, land_cam);
                            }
                        }
                    }
                }
            }
            else if (track is ShakeCameraTrack)
            {
                if (track.name.Equals("ShakeCameraTrack"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, virtual_cam);
                }
            }
        }
    }

    protected virtual void UnsetPlayableDirector()
    {
        var virtual_mng = Battle_Mng.GetVirtualCineManager();

        var character_cam = virtual_mng.GetCharacterCamera();
        if (character_cam != null)
        {
            character_cam.Follow = null;
        }

        var active_group_cam = virtual_mng.GetActiveTargetGroupCamera();
        if (active_group_cam != null)
        {
            active_group_cam.Follow = null;
        }


        var active_target_group = virtual_mng.GetActiveTargetGroup();
        if (active_target_group != null)
        {
            int cnt = Attack_Targets.Count;
            for (int i = 0; i < cnt; i++)
            {
                active_target_group.RemoveMember(Attack_Targets[i].transform);
            }
        }
    }


    /// <summary>
    /// 체력 바 추가
    /// </summary>
    protected void AddLifeBar()
    {
        if (Life_Bar_V2 == null)
        {
            var obj = GameObjectPoolManager.Instance.GetGameObject("Assets/AssetResources/Prefabs/Units/Life_Bar_Node_V2", GetHPPositionTransform());
            obj.transform.localPosition = Vector3.zero;
            Life_Bar_V2 = obj.GetComponent<LifeBarNode_V2>();
        }
    }
    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    protected void RemoveLifeBar()
    {
        if (Life_Bar_V2 != null)
        {
            GameObjectPoolManager.Instance.UnusedGameObject(Life_Bar_V2.gameObject);
        }
        Life_Bar_V2 = null;
    }
    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    protected void UpdateLifeBar()
    {
        float per = (float)(Life / Max_Life);
        Life_Bar_V2?.SetLifePercent(per);
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
        entry.TimeScale = Battle_Speed_Multiple;

        UNIT_STATES state = GetCurrentState();
        if (state == UNIT_STATES.ATTACK_1)
        {
            var skill = GetSkillManager().GetCurrentSkillGroup();
            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                SpawnSkillCastEffect(skill);
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            //var skill = GetSkillManager().GetSpecialSkillGroup();
            //if (animation_name.Equals(skill.GetSkillActionName()))
            //{
            //    SpawnSkillCastEffect(skill);
            //}
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
        if (state == UNIT_STATES.PAUSE)
        {
            return;
        }

        if (state == UNIT_STATES.DEATH)
        {
            if (animation_name.Equals("1_death") || animation_name.Equals("00_death"))
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
                if (Approach_Targets.Count == 0)
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
        else if (state == UNIT_STATES.SKILL_1)
        {
            if (animation_name.Equals("00_ultimate"))
            {
                Battle_Mng.ShowAllUnits();

                UnsetPlayableDirector();
                var skill = GetSkillManager().GetSpecialSkillGroup();
                if (skill != null)
                {
                    skill.ResetSkill();
                }
                Battle_Mng.FinishUltimateSkill(this);
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
                        SpawnSkillEffect_V3(exec_list[i]);
                    }
                }
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            var skill = GetSkillManager().GetSpecialSkillGroup();
            if (animation_name.Equals(skill.GetSkillActionName()))
            {
                var exec_list = skill.GetExecuableSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        SpawnSkillEffect_V3(exec_list[i]);
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

    
    public void SetAlphaAnimation(float alpha, float duration, bool render_enable)
    {
        if (Render_Texture == null)
        {
            Render_Texture = GetComponent<UnitRenderTexture>();
        }
        if (Render_Texture.enabled == render_enable)
        {
            return;
        }
        Render_Texture.SetAlphaAnimation(alpha, duration, render_enable);
        ShadowAlphaAnimation(alpha, duration);
        if (alpha == 0f)
        {
            Life_Bar_V2.HideLifeBar();
        }
    }
    public void UnitRenderTextureEnable(bool enable)
    {
        Render_Texture.enabled = enable;
    }

    protected void ShadowAlphaAnimation(float alpha, float duration)
    {
        if (Shadow == null)
        {
            return;
        }
        StartCoroutine(StartShadowAlphaAnimation(alpha, duration));
    }
    IEnumerator StartShadowAlphaAnimation(float alpha, float duration)
    {
        float delta = 0f;
        var color = Shadow.color;
        while (delta < duration)
        {
            delta += Time.deltaTime;
            color.a = Mathf.Lerp(Shadow.color.a, alpha, delta / duration);
            Shadow.color = color;
            yield return null;
        }

        color.a = alpha;
        Shadow.color = color;
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
        Team_Mng.FindTargetInRangeAtApproach(this, TARGET_TYPE.ENEMY_TEAM, distance, ref Approach_Targets);
    }
    /// <summary>
    /// 각 스킬에 지정되어 있는 타겟 찾기
    /// </summary>
    /// <param name="skill"></param>
    protected virtual void FindTargets(BattleSkillData skill)
    {
        Team_Mng.FindTargetInRange(this, skill.GetTargetType(), skill.GetTargetRuleType(), 0, skill.GetTargetOrder(), skill.GetTargetCount(), skill.GetTargetRange(), ref Attack_Targets);
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

    public float GetUnitScale()
    {
        return Unit_Data.GetUnitScale();
    }

    protected virtual void SpawnSkillCastEffect(BattleSkillGroup skill_grp)
    {
        string[] effect_paths = skill_grp.GetSkillCastEffectPath();
        if (effect_paths == null)
        {
            return;
        }
        
        var factory = Battle_Mng.GetEffectFactory();
        int cnt = effect_paths.Length;
        for (int i = 0; i < cnt; i++)
        {
            string e_path = effect_paths[i];
            if (string.IsNullOrEmpty(e_path))
            {
                continue;
            }
            var eff = (SkillEffectBase)factory.CreateEffect(e_path, GetUnitScale());
            var ec = eff.GetEffectComponent();
            if (ec != null)
            {
                var tran = GetReachPosTypeTransform(eff.GetEffectComponent().Projectile_Reach_Pos_Type);
                eff.transform.position = tran.position;
            }
            else
            {
                eff.transform.position = this.transform.position;
            }

            eff.StartParticle(ec.Effect_Duration);

            var animator = eff.GetComponent<Animator>();
            if (animator != null)
            {
                animator.speed = Battle_Speed_Multiple;
            }
        }

        
    }

    /// <summary>
    /// 스킬 구현 V3<br/>
    /// 스킬 프리팹에 EffectComponent 정의를 사용하여<br/>
    /// 이펙트에서 사용할 정의는 해당 컴포넌트를 참조하여 사용한다.<br/>
    /// 스킬에 이펙트 프리팹 정보가 있을 경우 해당 이펙트를 트리거로 사용하여 효과를 줄 수 있다<br/>
    /// 스킬에 이펙트 프리팹 정보가 없을 경우, 즉시 발동하는 스킬로 해당 스킬에서 바로 적용할 수 있도록 한다.
    /// </summary>
    /// <param name="skill"></param>
    protected virtual void SpawnSkillEffect_V3(BattleSkillData skill)
    {
        var factory = Battle_Mng.GetEffectFactory();
        FindTargets(skill);
        int target_cnt = Attack_Targets.Count;
        int effect_weight_index = skill.GetEffectWeightIndex();

        string skill_trigger_effect_prefab = skill.GetTriggerEffectPrefabPath();
        if (string.IsNullOrEmpty(skill_trigger_effect_prefab))
        {
            //  트리거 이펙트가 없으면, 일회성/지속성 스킬로 트리거를 발생시킨다.(트리거 이펙트가 없으면, EFFECT_COUNT_TYPE.EACH_TARGET_EFFECT 형식으로 각각의 이펙트를 구현해준다)
            for (int i = 0; i < target_cnt; i++)
            {
                var target = Attack_Targets[i];
                BATTLE_SEND_DATA dmg = new BATTLE_SEND_DATA();
                dmg.Caster = this;
                dmg.AddTarget(target);
                dmg.Skill = skill;
                dmg.Physics_Attack_Point = GetPhysicsAttackPoint();
                dmg.Magic_Attack_Point = GetMagicAttackPoint();
                dmg.Effect_Weight_Index = effect_weight_index;

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

                    var effect = (SkillEffectBase)factory.CreateEffect(effect_path, GetUnitScale());
                    effect.SetBattleSendData(dmg);

                    var ec = effect.GetEffectComponent();
                    Vector3 target_pos = Vector3.zero;
                    //  적들의 중앙 위치 찾기
                    if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
                    {
                        target_pos = ec.GetTargetsCenterPosition(Attack_Targets);
                    }
                    else
                    {
                        Transform target_trans = ec.GetTargetReachPosition(Attack_Targets.First());
                        target_pos = target_trans.position;
                    }

                    //  즉발형 이펙트
                    if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                    {
                        effect.transform.position = target_pos;
                        effect.StartParticle(ec.Effect_Duration);
                    }
                    else if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE) // 투사체 이펙트
                    {
                        Transform start_trans = ec.GetProjectileStartTransform(this);
                        var start_pos = start_trans.position;
                        start_pos.z = start_trans.position.z;

                        effect.transform.position = start_pos;

                        float distance = Vector3.Distance(start_pos, target_pos);
                        float flying_time = 0f;
                        if (ec.Projectile_Duration > 0f)
                        {
                            flying_time = ec.Projectile_Duration;
                        }
                        else
                        {
                            flying_time = distance / ec.Projectile_Velocity;
                        }

                        effect.MoveTarget(target_pos, flying_time);
                    }
                    else
                    {
                        Debug.Assert(false);
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

                    var effect = (SkillEffectBase)factory.CreateEffect(effect_path, target.GetUnitScale());
                    effect.SetBattleSendData(dmg);

                    var ec = effect.GetEffectComponent();
                    Vector3 target_pos = Vector3.zero;
                    //  적들의 중앙 위치 찾기
                    if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
                    {
                        target_pos = ec.GetTargetsCenterPosition(Attack_Targets);
                    }
                    else
                    {
                        Transform target_trans = ec.GetTargetReachPosition(Attack_Targets.First());
                        target_pos = target_trans.position;
                    }

                    if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                    {
                        effect.transform.position = target_pos;
                        effect.StartParticle(target.transform, ec.Effect_Duration, ec.Is_Loop);
                    }
                    else if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE)
                    {
                        //  nothing - 아직 투사체로 날아가서 발생하는 지속성 효과는 없음
                    }
                    else
                    {
                        Debug.Assert(false);
                    }

                }
            }
        }
        else
        {
            //  단일 이펙트
            if (skill.GetEffectCountType() == EFFECT_COUNT_TYPE.SINGLE_EFFECT)
            {
                BATTLE_SEND_DATA dmg = new BATTLE_SEND_DATA();
                dmg.Caster = this;
                dmg.AddTargets(Attack_Targets);
                dmg.Skill = skill;
                dmg.Physics_Attack_Point = GetPhysicsAttackPoint();
                dmg.Magic_Attack_Point = GetMagicAttackPoint();
                dmg.Effect_Weight_Index = effect_weight_index;

                //  첫번재 이펙트만 보여줘야 하므로
                if (effect_weight_index == 0)
                {
                    //  onetime skill
                    var onetime_list = skill.GetOnetimeSkillDataList();
                    if (onetime_list.Count > 0)
                    {
                        dmg.Onetime = onetime_list[0];
                    }

                    //  트리거 이펙트가 있으면, 본 이펙트를 출현함으로써, 일회성/지속성 스킬의 트리거로 사용할 수 있다.
                    var trigger_effect = (SkillEffectBase)factory.CreateEffect(skill_trigger_effect_prefab, GetUnitScale());
                    trigger_effect.SetBattleSendData(dmg);

                    var ec = trigger_effect.GetEffectComponent();
                    
                    Vector3 target_pos = Vector3.zero;
                    //  적들의 중앙 위치 찾기
                    if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
                    {
                        target_pos = ec.GetTargetsCenterPosition(Attack_Targets);
                    }
                    else
                    {
                        Transform target_trans = ec.GetTargetReachPosition(Attack_Targets.First());
                        target_pos = target_trans.position;
                    }
                    //  즉발형인지
                    if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                    {
                        trigger_effect.transform.position = target_pos;
                        trigger_effect.StartParticle(ec.Effect_Duration);
                    }
                    else if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE) //  투사체인지
                    {
                        //  nothing (현재까지 투사체 샘플이 없음)

                        Transform start_trans = ec.GetProjectileStartTransform(this);
                        var start_pos = start_trans.position;
                        start_pos.z = start_trans.position.z;

                        trigger_effect.transform.position = start_pos;

                        float distance = Vector3.Distance(start_pos, target_pos);
                        float flying_time = 0f;
                        if (ec.Projectile_Duration > 0f)
                        {
                            flying_time = ec.Projectile_Duration;
                        }
                        else
                        {
                            flying_time = distance / ec.Projectile_Velocity;
                        }

                        trigger_effect.MoveTarget(target_pos, flying_time);
                    }
                    else
                    {
                        Debug.Assert(false);
                    }
                }
                else
                {
                    //  일회성 / 지속성 효과만 적용
                    //  트리거 이펙트가 없으면, 일회성/지속성 스킬로 트리거를 발생시킨다.(트리거 이펙트가 없으면, EFFECT_COUNT_TYPE.EACH_TARGET_EFFECT 형식으로 각각의 이펙트를 구현해준다)
                    for (int i = 0; i < target_cnt; i++)
                    {
                        var target = Attack_Targets[i];

                        //  onetime skill
                        var onetime_list = skill.GetOnetimeSkillDataList();
                        int one_cnt = onetime_list.Count;
                        for (int o = 0; o < one_cnt; o++)
                        {
                            var onetime = onetime_list[o];
                            string effect_path = onetime.GetEffectPrefab();
                            if (string.IsNullOrEmpty(effect_path))
                            {
                                dmg.Onetime = onetime;
                                onetime.ExecSkill(dmg);
                                continue;
                            }
                            dmg.Onetime = onetime;

                            var effect = (SkillEffectBase)factory.CreateEffect(effect_path, GetUnitScale());
                            effect.SetBattleSendData(dmg);

                            var ec = effect.GetEffectComponent();
                            Vector3 target_pos = Vector3.zero;
                            //  적들의 중앙 위치 찾기
                            if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
                            {
                                target_pos = ec.GetTargetsCenterPosition(Attack_Targets);
                            }
                            else
                            {
                                Transform target_trans = ec.GetTargetReachPosition(Attack_Targets.First());
                                target_pos = target_trans.position;
                            }


                            //  즉발형 이펙트
                            if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                            {
                                effect.transform.position = target_pos;
                                effect.StartParticle(ec.Effect_Duration);
                            }
                            else if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE) // 투사체 이펙트
                            {
                                Transform start_trans = ec.GetProjectileStartTransform(this);
                                var start_pos = start_trans.position;
                                start_pos.z = start_trans.position.z;

                                effect.transform.position = start_pos;

                                float distance = Vector3.Distance(start_pos, target_pos);
                                float flying_time = 0f;
                                if (ec.Projectile_Duration > 0f)
                                {
                                    flying_time = ec.Projectile_Duration;
                                }
                                else
                                {
                                    flying_time = distance / ec.Projectile_Velocity;
                                }

                                //effect.MoveTarget(target_trans, flying_time);
                                effect.MoveTarget(target_pos, flying_time);
                            }
                            else
                            {
                                Debug.Assert(false);
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
                                dmg.Duration = duration;
                                duration.ExecSkill(dmg);
                                continue;
                            }
                            dmg.Duration = duration;

                            var effect = (SkillEffectBase)factory.CreateEffect(effect_path, target.GetUnitScale());
                            effect.SetBattleSendData(dmg);

                            var ec = effect.GetEffectComponent();
                            Vector3 target_pos = Vector3.zero;
                            //  적들의 중앙 위치 찾기
                            if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
                            {
                                target_pos = ec.GetTargetsCenterPosition(Attack_Targets);
                            }
                            else
                            {
                                Transform target_trans = ec.GetTargetReachPosition(Attack_Targets.First());
                                target_pos = target_trans.position;
                            }

                            if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                            {
                                effect.transform.position = target_pos;
                                effect.StartParticle(target.transform, ec.Effect_Duration, ec.Is_Loop);
                            }
                            else if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE)
                            {
                                //  nothing - 아직 투사체로 날아가서 발생하는 지속성 효과는 없음
                            }
                            else
                            {
                                Debug.Assert(false);
                            }

                        }
                    }

                }
            }
            else  // 각 타겟별 이펙트
            {
                for (int i = 0; i < target_cnt; i++)
                {
                    var target = Attack_Targets[i];

                    BATTLE_SEND_DATA dmg = new BATTLE_SEND_DATA();
                    dmg.Caster = this;
                    dmg.AddTarget(target);
                    dmg.Skill = skill;
                    dmg.Physics_Attack_Point = GetPhysicsAttackPoint();
                    dmg.Magic_Attack_Point = GetMagicAttackPoint();
                    dmg.Effect_Weight_Index = effect_weight_index;

                    //  트리거 이펙트가 있으면, 본 이펙트를 출현함으로써 일회성/지속성 스킬의 트리거로 사용할 수 있다.
                    var trigger_effect = (SkillEffectBase)factory.CreateEffect(skill_trigger_effect_prefab, GetUnitScale());
                    trigger_effect.SetBattleSendData(dmg);

                    var ec = trigger_effect.GetEffectComponent();

                    Vector3 target_pos = Vector3.zero;
                    //  적들의 중앙 위치 찾기
                    if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
                    {
                        target_pos = ec.GetTargetsCenterPosition(Attack_Targets);
                    }
                    else
                    {
                        Transform target_trans = ec.GetTargetReachPosition(Attack_Targets.First());
                        target_pos = target_trans.position;
                    }

                    if (ec.Effect_Type == SKILL_EFFECT_TYPE.IMMEDIATE)
                    {
                        trigger_effect.transform.position = target_pos;
                        trigger_effect.StartParticle(ec.Effect_Duration);
                    }
                    else if (ec.Effect_Type == SKILL_EFFECT_TYPE.PROJECTILE)
                    {
                        Transform start_trans = ec.GetProjectileStartTransform(this);
                        var start_pos = start_trans.position;
                        start_pos.z = start_trans.position.z;

                        trigger_effect.transform.position = start_pos;

                        float distance = Vector3.Distance(start_pos, target_pos);
                        float flying_time = 0f;
                        if (ec.Projectile_Duration > 0f)
                        {
                            flying_time = ec.Projectile_Duration;
                        }
                        else
                        {
                            flying_time = distance / ec.Projectile_Velocity;
                        }

                        //trigger_effect.MoveTarget(target_trans, flying_time);
                        trigger_effect.MoveTarget(target_pos, flying_time);
                    }
                    else
                    {
                        Debug.Assert(false);
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
                var track = all_tracks[i];
                if (track == null)
                    continue;
                track.TimeScale = Battle_Speed_Multiple;
            }
        }
    }
    /// <summary>
    /// 지속성 스킬 효과 데이터를 모두 제거<br/>
    /// 데이터를 제거하면 자동으로 이펙트도 사라지도록 구현되어 있음
    /// </summary>
    protected void ClearDurationSkillDataList()
    {
        //lock (Duration_Lock)
        //{
        //    int cnt = Used_Battle_Duration_Data_List.Count;
        //    for (int i = 0; i < cnt; i++)
        //    {
        //        var duration = Used_Battle_Duration_Data_List[i];
        //        duration.Dispose();
        //    }
        //    Used_Battle_Duration_Data_List.Clear();
        //}
        GetSkillManager().ClearDurationSkillDataList();

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

        if (Shadow != null && Shadow.color.a != 1f)
        {
            var color = Shadow.color;
            color.a = 1f;
            Shadow.color = color;
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
        double last_damage = data.Physics_Attack_Point;
        var target = data.Targets[0];
        double vampire_hp = last_damage * Attack_Life_Recovery / (Attack_Life_Recovery + target.GetLevel() + 100);
        vampire_hp = Math.Truncate(vampire_hp);
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

        ONETIME_EFFECT_TYPE etype = dmg.Onetime.GetOnetimeEffectType();

        //  물리 마법 결정 데미지
        double damage = etype == ONETIME_EFFECT_TYPE.MAGIC_DAMAGE ? dmg.Magic_Attack_Point : dmg.Physics_Attack_Point;

        double last_damage = damage;

        //  회피 여기서 계산할까? 회피하면 미스 판정.(데미지 0)
        if (IsEvation(dmg.Caster.Accuracy))
        {
            //last_damage = damage * 0.9;  //  임시로 회피시 데미지의 90%만 들어가도록 하자.
            //  miss 소환 필요
            return;
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
        last_damage = GetCalcDamage(last_damage, etype == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE);

        //  치명타 확률
        double cri_chance = GetCriticalChanceRate(dmg.Caster.GetLevel(), GetLevel(), etype == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE);
        int r = UnityEngine.Random.Range(0, 100000);
        if (r < cri_chance)
        {
            if (etype == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE)
            {
                dmg.Is_Physics_Critical = true;
                last_damage *= 1.5 * Physics_Critical_Power_Add;
            }
            else
            {
                dmg.Is_Magic_Critical = true;
                last_damage *= 1.5 * Magic_Critical_Power_Add;
            }
        }

        if (last_damage <= 0)
        {
            return;
        }
        //  속성 계산
        double attribute_dmg_inc = GetSuperiorityAttributeDamageInc(dmg.Caster.GetAttributeType());
        if (attribute_dmg_inc > 0)
        {
            last_damage += last_damage * attribute_dmg_inc;
        }

        //  소수점 이하 버리기
        last_damage = Math.Truncate(last_damage);
        if (last_damage <= 0)
        {
            return;
        }
        if (etype == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE)
        {
            dmg.Physics_Attack_Point = last_damage;
        }
        else
        {
            dmg.Magic_Attack_Point = last_damage;
        }


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
            AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/DamageText_Effect_V2", GetReachPosTypeTransform(TARGET_REACH_POS_TYPE.BODY), dmg, 1f);
            UpdateLifeBar();
            ChangeState(UNIT_STATES.DEATH);
            return;
        }

        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/DamageText_Effect_V2", GetReachPosTypeTransform(TARGET_REACH_POS_TYPE.BODY), dmg, 1f);
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

        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/HealText_Effect", Life_Bar_Pos, recovery_hp, 1f);
        UpdateLifeBar();
    }

    /// <summary>
    /// 속성 체크.<br/>
    /// 공격자가 피격자보다 우세한 속성일 경우, 최종 데미지 추가 비율 반환
    /// </summary>
    /// <param name="caster_atype"></param>
    /// <returns></returns>
    double GetSuperiorityAttributeDamageInc(ATTRIBUTE_TYPE caster_atype)
    {
        double inc_pt = 0;
        var superiority = MasterDataManager.Instance.Get_AttributeSuperiorityData(caster_atype);
        if (superiority != null)
        {
            if (superiority.defender_attribute_type == GetAttributeType())
            {
                inc_pt = superiority.final_damage_per;
            }
        }
        return inc_pt;
    }

    /// <summary>
    /// 웨이브 종료시 체력 회복
    /// </summary>
    public void WaveEndRecoveryLife()
    {
        double recovery_hp = Max_Life * Auto_Recovery_Life;
        recovery_hp = Math.Truncate(recovery_hp);
        if (recovery_hp > 0)
        {
            AddLifeRecovery(recovery_hp);
        }
    }



    public void AddSpawnEffectText(string path, Transform target, object data, float duration)
    {
        lock (Effect_Queue_Lock)
        {
            var d = new Effect_Queue_Data();
            d.effect_path = path;
            d.Target_Position = target;
            d.Data = data;
            d.Duration = duration + ((Battle_Speed_Multiple - 1) * 1f);
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

                    Effect_Queue_Interval = 0.09f / Battle_Speed_Multiple;
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
        var effect = Battle_Mng.GetEffectFactory().CreateEffect(edata.effect_path);
        effect.transform.position = edata.Target_Position.position;
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
        GetSkillManager().AddDurationSkillEffect(duration_skill);

        //int rate = UnityEngine.Random.Range(0, 10000);
        //if (rate < duration_skill.GetRate())
        //{
        //    lock (Duration_Lock)
        //    {
        //        var d_type = duration_skill.GetDurationEffectType();
        //        if (duration_skill.IsOverlapable())
        //        {
        //            Used_Battle_Duration_Data_List.Add(duration_skill);
                    
        //            AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/TransText_Effect", GetReachPosTypeTransform(TARGET_REACH_POS_TYPE.BODY), d_type, 1f);
        //        }
        //        else
        //        {
        //            //  같은 스킬 효과가 있을 경우 교체(나중에 로직에 따라 변경하자)
        //            var found = Used_Battle_Duration_Data_List.Find(x => x.GetDurationEffectType() == duration_skill.GetDurationEffectType());
        //            if (found != null)
        //            {
        //                Used_Battle_Duration_Data_List.Remove(found);
        //                found.Dispose();
        //                found = null;
        //            }

        //            if (d_type == DURATION_EFFECT_TYPE.FREEZE)
        //            {
        //                ChangeState(UNIT_STATES.FREEZE);
        //            }
        //            else if (d_type == DURATION_EFFECT_TYPE.STUN)
        //            {
        //                ChangeState(UNIT_STATES.STUN);
        //            }
        //            else if (d_type == DURATION_EFFECT_TYPE.BIND)
        //            {
        //                ChangeState(UNIT_STATES.BIND);
        //            }

        //            Used_Battle_Duration_Data_List.Add(duration_skill);
                    
        //            AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/TransText_Effect", GetReachPosTypeTransform(TARGET_REACH_POS_TYPE.BODY), d_type, 1f);

        //        }
        //        Slot_Events?.Invoke(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
        //    }

        //}
        //else
        //{
        //    duration_skill.Dispose();
        //    duration_skill = null;
        //}
    }

    public void SendSlotEvent(SKILL_SLOT_EVENT_TYPE etype)
    {
        Slot_Events?.Invoke(etype);
    }

    /// <summary>
    /// 지속성 효과에서 발생하는 일회성 효과 이펙트 구현
    /// </summary>
    public void SpawnOnetimeEffectFromDurationSkill(BattleOnetimeSkillData onetime, BATTLE_SEND_DATA send_data)
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
            send_data.Onetime = onetime;

            var effect = (SkillEffectBase)factory.CreateEffect(effect_path, send_data.Caster.GetUnitScale());
            effect.SetBattleSendData(send_data);

            var ec = effect.GetEffectComponent();

            var target_trans = ec.GetTargetReachPosition(this);
            var target_pos = target_trans.position;
            effect.transform.position = target_pos;
            effect.StartParticle(ec.Effect_Duration);
        }
    }

    /// <summary>
    /// 지속성 효과의 시간을 계산 해주기 위한 함수 V2
    /// </summary>
    protected void CalcDurationSkillTime()
    {
        float dt = Time.deltaTime * Battle_Speed_Multiple;
        GetSkillManager().CalcDurationSkillTime(dt);
    }

    /// <summary>
    /// 지속성 효과의 지속성 방식 타입 갱신<br/>
    /// 시간 지속성 방식은 <see cref="CalcDurationSkillTime"/>을 사용한다.<br/>
    /// 그 외에 피격 횟수 제한, 공격 횟수 제한의 경우 본 함수 사용<br/>
    /// </summary>
    /// <param name="ptype"></param>
    protected void CalcDurationCountUse(PERSISTENCE_TYPE ptype)
    {
        GetSkillManager().CalcDurationCountUse(ptype);
    }

    /// <summary>
    /// 최초 필드 등장시 Left Team 이동<br/>
    /// Center Point를 기준으로 각자의 사거리까지 도착하면 Idle 상태로 변경
    /// </summary>
    protected void MoveInLeftTeam()
    {
        FindApproachTargets();
        if (Approach_Targets.Count > 0)
        {
            ChangeState(UNIT_STATES.IDLE);
            return;
        }
        float move = (float)Move_Speed * Time.deltaTime * Battle_Speed_Multiple;
        var pos = this.transform.position;
        pos.x += move;
        this.transform.position = pos;
    }
    /// <summary>
    /// NPC 최초 필드 등장시 Right Team 이동<br/>
    /// Center Point를 기준으로 각자의 사거리까지 도착하면 Idle 상태로 변경
    /// </summary>
    protected void MoveInRightTeam()
    {
        FindApproachTargets();
        if (Approach_Targets.Count > 0)
        {
            ChangeState(UNIT_STATES.IDLE);
            return;
        }
        float move = (float)Move_Speed * Time.deltaTime * Battle_Speed_Multiple;
        var pos = this.transform.position;
        pos.x -= move;
        this.transform.position = pos;
    }
    /// <summary>
    /// Left Team 이동<br/>
    /// 적을 탐색하면서 이동한다.
    /// </summary>
    protected void MoveLeftTeam()
    {
        FindApproachTargets();
        if (Approach_Targets.Count > 0)
        {
            ChangeState(UNIT_STATES.ATTACK_READY_1);
            return;
        }

        float move = (float)Move_Speed * Time.deltaTime * Battle_Speed_Multiple;
        var pos = this.transform.position;
        pos.x += move;
        this.transform.position = pos;
    }

    /// <summary>
    /// Right Team 이동<br/>
    /// 적을 탐색하면서 이동한다.
    /// </summary>
    protected void MoveRightTeam()
    {
        FindApproachTargets();
        if (Approach_Targets.Count > 0)
        {
            ChangeState(UNIT_STATES.ATTACK_READY_1);
            return;
        }

        float move = (float)Move_Speed * Time.deltaTime * Battle_Speed_Multiple;
        var pos = this.transform.position;
        pos.x -= move;
        this.transform.position = pos;
    }
    /// <summary>
    /// Left Team 화면 전환때까지 반대쪽으로 달리기(웨이브 이동)
    /// </summary>
    protected void WaveRunLeftTeam()
    {
        float move = (float)Move_Speed * Time.deltaTime * Battle_Speed_Multiple;
        var pos = this.transform.localPosition;
        pos.x += move;
        //if (Is_Reposition)
        //{
        //    float zmove = (float)Move_Speed * Time.deltaTime * Battle_Speed_Multiple;
        //    pos.z += zmove;
        //}
        this.transform.localPosition = pos;
    }
    /// <summary>
    /// 궁극기 시전 요청
    /// </summary>
    public virtual void SpecialSkillExec()
    {
        var battle_state = Battle_Mng.GetCurrentState();
        if (battle_state != GAME_STATES.PLAYING)
        {
            return;
        }
        var state = GetCurrentState();
        if (state == UNIT_STATES.SKILL_1 || state == UNIT_STATES.SKILL_READY_1 || state == UNIT_STATES.SKILL_END || state == UNIT_STATES.ULTIMATE_PAUSE)
        {
            return;
        }

        //  여러가지 상황상 궁극기를 사용할 수 없는 상황을 체크
        //  체크 완료 후 궁극기를 사용할 수 있는 경우에만 궁극기 사용
        var skill = GetSkillManager().GetSpecialSkillGroup();
        if (skill == null)
        {
            return;
        }
        if (!skill.IsPrepareCooltime())
        {
            return;
        }
        var target_skill = skill.GetSpecialSkillTargetSkill();
        if (target_skill != null)
        {
            FindTargets(target_skill);
        }
        //  주인공 이외의 모든 캐릭 pause
        Battle_Mng.AllPauseUnitWithoutHero(this);

        //  진행중이던 이펙트 모두 숨기기
        Battle_Mng.GetEffectFactory().OnPauseAndHide();

        //  주인공 및 타겟을 제외한 다른 유닛은 모두 숨기기
        List<HeroBase_V2> targets = new List<HeroBase_V2>();
        targets.Add(this);
        targets.AddRange(Attack_Targets);
        Battle_Mng.HideAllUnitWithoutTargets(targets);

        Skeleton.AnimationState.ClearTracks();
        if (state == UNIT_STATES.ATTACK_READY_1 || state == UNIT_STATES.ATTACK_1 || state == UNIT_STATES.ATTACK_END)
        {
            GetSkillManager().SetNextSkillPattern();
        }

        Battle_Mng.StartUltimateSkill();
        ChangeState(UNIT_STATES.SKILL_READY_1);
    }


    public override string ToString()
    {
        var state = GetCurrentState();
        var sb = ZString.CreateStringBuilder();
        sb.AppendLine($"{nameof(Team_Type)} : <color=#ffffff>[{Team_Type}]</color>");
        sb.AppendLine($"{nameof(state)} => {state}");
        sb.AppendLine($"{nameof(Life)} => {Life}");
        sb.AppendLine($"{nameof(Max_Life)} => {Max_Life}");
        

        return sb.ToString();
    }

    [ContextMenu("ToStringHeroBase")]
    public void ToStringHeroBase()
    {
        Debug.Log(ToString());

    }
}
