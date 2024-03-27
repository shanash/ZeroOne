using Cysharp.Text;
using FluffyDuck.UI;
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
[RequireComponent (typeof(EventTriggerObject))]
public partial class HeroBase_V2 : UnitBase_V2, IEventTrigger
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
    /// 체력 게이지(슬라이더)
    /// </summary>
    protected LifeBarNode Life_Bar = null;

    /// <summary>
    /// 공격용 타겟 리스트
    /// </summary>
    protected List<HeroBase_V2> Find_Targets = new List<HeroBase_V2>();
    /// <summary>
    /// 접근용 타겟 리스트
    /// </summary>
    protected List<HeroBase_V2> Approach_Targets = new List<HeroBase_V2>();

    /// <summary>
    /// 영웅 데이터
    /// </summary>
    protected BattleUnitData Unit_Data;

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

    /// <summary>
    /// 피격 효과 코루틴
    /// </summary>
    protected Coroutine Hitted_Corouine;

    protected Vector3 Before_Hitted_Position = Vector3.zero;

    /// <summary>
    /// 각 스킬별 토탈 데미지를 계산하기 위해 데미지 정보 및 스킬 정보 등을 저장하는 함수
    /// </summary>
    protected List<Total_Damage_Data> Total_Damage_Data = new List<Total_Damage_Data>();

    /// <summary>
    /// Weight Index 순서대로 표현해보자.
    /// </summary>
    List<Vector3> Damage_Text_Position_List = new List<Vector3>();

    int Damage_Text_Position_Order_Index;

    /// <summary>
    /// 타임라인 이벤트 트리거 ID 리스트(ToString()으로 비교 사용)
    /// </summary>
    protected enum TRIGGER_EVENT_IDS
    {
        chr_hide,
        chr_show,
        change_chr_cam,         //  캐릭터 캠 - Follow 변경
        change_active_cam,      //  액티브 그룹 캠 - Follow Group Targets 변경
        change_target_cam,      //  타겟 그룹 캡 - Follow Group Targets 변경
        change_color,           //  타겟의 색 변경
        rollback_color,         //  타겟의 색 롤백
    }


    public virtual void SetBattleUnitData(BattleUnitData unit_dt)
    {
        Unit_Data = unit_dt;
        Unit_Data.SetHeroBase(this);
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
        var tracks = FindAllTrakcs();
        if (tracks != null && tracks.Length > 0)
        {
            for (int i = 0; i < tracks.Length; i++)
            {
                var t = tracks[i];
                if (t != null)
                {
                    t.TimeScale = Battle_Speed_Multiple;
                }
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

        //LandscapeCamera - 스테이지캠과 초기값 같음. 확대 축소 등 필요시컨트롤해서 사용(컴포넌트 혹은 애니메이션 트랙 활용)
        //CharacterCamera - 시전자 팔로우 캠
        //ActiveGroupCamera - 시전자 + 타겟(단일, 다수)그룹의 중점 포커싱.
        //TargetGroupCamera - 타겟(단일, 다수)그룹의 중점 포커싱.
        //FreeCamera - 스테이지캠 초기값과 같음. 임의로 자유롭게 연출용으로 사용할 예정.

        var unit_back_bg = Battle_Mng.GetBattleField().GetUnitBackFaceBG();
        var virtual_cam = Battle_Mng.GetVirtualCineManager();
        var brain_cam = virtual_cam.GetBrainCam();

        var stage_cam = virtual_cam.GetStageCamera();
        var character_cam = virtual_cam.GetCharacterCamera();
        var free_cam = virtual_cam.GetFreeCamera();
        var land_cam = virtual_cam.GetLandscapeCamera();
        var active_group_cam = virtual_cam.GetActiveTargetGroupCamera();
        var active_group = virtual_cam.GetActiveTargetGroup();

        var target_group_cam = virtual_cam.GetTargetGroupCamera();
        var target_group = virtual_cam.GetTargetGroup();


        var ta = (TimelineAsset)Ultimate_Skill_Playable_Director.playableAsset;
        var tracks = ta.GetOutputTracks();

        //  skill
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        var target_skill = skill_grp.GetSpecialSkillTargetSkill();

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
                else if (track.name.Equals("ActiveGroupCameraAnimationTrack"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, active_group_cam.GetComponent<Animator>());
                }
                else if (track.name.Equals("TargetGroupCameraAnimationTrack"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, target_group_cam.GetComponent<Animator>());
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
                            else if (shot.DisplayName.Equals("TargetGroupCamera"))
                            {
                                //  TargetGroupCamera - 타겟(단일, 다수)그룹의 중점 포커싱.
                                for (int i = 0; i < target_skill.GetFindTargets().Count; i++)
                                {
                                    target_group.AddMember(target_skill.GetFindTargets()[i].transform, 1, 1);
                                }
                                target_group_cam.Follow = target_group.transform;
                                Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, target_group_cam);
                            }
                            else if (shot.DisplayName.Equals("ActiveGroupCamera"))
                            {
                                //  ActiveGroupCamera - 시전자 + 타겟(단일, 다수)그룹의 중점 포커싱.
                                active_group.AddMember(this.transform, 2, 1);
                                for (int i = 0; i < target_skill.GetFindTargets().Count; i++)
                                {
                                    if (!target_skill.GetFindTargets().Contains(this))
                                    {
                                        active_group.AddMember(target_skill.GetFindTargets()[i].transform, 1, 1);
                                    }
                                }
                                active_group_cam.Follow = active_group.transform;
                                Ultimate_Skill_Playable_Director.SetReferenceValue(shot.VirtualCamera.exposedName, active_group_cam);
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
            else if (track is EventTriggerTrack)
            {
                if (track.name.Equals("EventTriggerTrack"))
                {
                    Ultimate_Skill_Playable_Director.SetGenericBinding(track, this);
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

        var target_group_camp = virtual_mng.GetTargetGroupCamera();
        if (target_group_camp != null)
        {
            target_group_camp.Follow = null;
        }

        var active_target_group = virtual_mng.GetActiveTargetGroup();
        if (active_target_group != null)
        {
            var targets = active_target_group.m_Targets;
            for (int i = 0; i < targets.Length; i++)
            {
                var t = targets[i];
                active_target_group.RemoveMember(t.target);
            }
        }

        var target_group = virtual_mng.GetTargetGroup();
        if (target_group != null)
        {
            var targets = target_group.m_Targets;
            for (int i = 0; i < targets.Length; i++)
            {
                var t = targets[i];
                target_group.RemoveMember(t.target);
            }
        }
    }


    /// <summary>
    /// 체력 바 추가
    /// </summary>
    protected void AddLifeBar()
    {
        if (Life_Bar == null)
        {
            Life_Bar = UI_Mng.AddLifeBar(this);
        }
    }
    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    protected void RemoveLifeBar()
    {
        UI_Mng.UnusedLifeBarNode(Life_Bar);
        Life_Bar = null;
    }
    /// <summary>
    /// 체력바 업데이트
    /// </summary>
    protected void UpdateLifeBar()
    {
        float per = (float)(Life / Max_Life);
        var state = GetCurrentState();
        if (state == UNIT_STATES.ULTIMATE_PAUSE || state == UNIT_STATES.SKILL_1)
        {
            Life_Bar?.SetLifeSliderPercent(per, false);
        }
        else
        {
            Life_Bar?.SetLifeSliderPercent(per, true);
        }
        
        SendSlotEvent(SKILL_SLOT_EVENT_TYPE.LIFE_UPDATE);
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
            Skeleton.AnimationState.Interrupt += SpineAnimationInterrupt;
            Skeleton.AnimationState.Dispose += SpineAnimationDispose;
        }
    }

    protected virtual void SpineAnimationInterrupt(TrackEntry entry) { }
    protected virtual void SpineAnimationDispose(TrackEntry entry) { }

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
            var skill_grp = GetSkillManager().GetCurrentSkillGroup();
            //  스킬 시작시 각 스킬의 타겟을 미리 설정해준다.(중간에 변경되는 일이 없도록 하기 위해)
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
                {
                    var skill = skill_grp.GetBattleSkillDataList()[i];
                    FindTargetsSkillAddTargets(skill);
                }
                SpawnSkillCastEffect(skill_grp);
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            var skill_grp = GetSkillManager().GetSpecialSkillGroup();
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
                {
                    var skill = skill_grp.GetBattleSkillDataList()[i];
                    FindTargetsSkillAddTargets(skill);
                }

                SpawnSkillCastEffect(skill_grp);
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
        if (state == UNIT_STATES.PAUSE)
        {
            return;
        }

        if (state == UNIT_STATES.DEATH)
        {
            if (animation_name.Equals("00_death"))
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
                    AttackAnimationComplete();
                }
                return;
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            if (animation_name.Equals("00_ultimate"))
            {
                UnsetPlayableDirector();
                var skill = GetSkillManager().GetSpecialSkillGroup();
                if (skill != null)
                {
                    skill.ResetSkill();
                }
                Battle_Mng.FinishUltimateSkill(this);
            }
            else if (animation_name.Equals("00_ultimate_enemy"))
            {
                var skill = GetSkillManager().GetSpecialSkillGroup();
                if (skill != null)
                {
                    skill.ResetSkill();
                }
                ChangeState(UNIT_STATES.ATTACK_READY_1);
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
            var skill_grp = GetSkillManager().GetCurrentSkillGroup();
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                var exec_list = skill_grp.GetExecutableCloneSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        var skill = exec_list[i];
                        if (skill.IsEmptyFindTarget())
                        {
                            FindTargetsSkillAddTargets(skill);
                        }
                        
                        SpawnSkillEffect_V3(skill_grp, skill);
                    }
                }
            }
        }
        else if (state == UNIT_STATES.SKILL_1)
        {
            var skill_grp = GetSkillManager().GetSpecialSkillGroup();
            if (animation_name.Equals(skill_grp.GetSkillActionName()))
            {
                var exec_list = skill_grp.GetExecutableCloneSkillDatas(evt_name);
                if (exec_list.Count > 0)
                {
                    int cnt = exec_list.Count;
                    for (int i = 0; i < cnt; i++)
                    {
                        var skill = exec_list[i];
                        if (skill.IsEmptyFindTarget())
                        {
                            FindTargetsSkillAddTargets(skill);
                        }
                        SpawnSkillEffect_V3(skill_grp, skill);
                    }
                }
            }
        }
    }


    protected virtual void AttackAnimationComplete()
    {
        ChangeState(UNIT_STATES.ATTACK_READY_1);
    }
    public void SetTeamManager(TeamManager_V2 mng)
    {
        Team_Mng = mng;

        Team_Type = Team_Mng.Team_Type;
    }
    public void HideLifeBar()
    {
        if (Life_Bar != null)
        {
            Life_Bar.HideLifeBar();
        }
    }
    /// <summary>
    /// 유닛 알파값 변경 애니메이션
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="duration"></param>
    /// <param name="finish_render_enable">애니메이션 종료 후 렌더러 상태 적용</param>
    public void SetAlphaAnimation(float alpha, float duration, bool finish_render_enable)
    {
        if (Render_Texture == null)
        {
            Render_Texture = GetComponent<UnitRenderTexture>();
        }

        float dur = duration / Battle_Speed_Multiple;
        Render_Texture.SetAlphaAnimation(alpha, dur, finish_render_enable);
        ShadowAlphaAnimation(alpha, dur);
        if (alpha == 0f)
        {
            Life_Bar?.HideLifeBar();
        }
    }
    /// <summary>
    /// 유닛의 렌더 텍스쳐 사용 여부<br/>
    /// 알파 값을 사용하기 위해서는 렌더 텍스쳐의 활성화가 필요함
    /// </summary>
    /// <param name="enable"></param>
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
                PlayAnimation(0, "00_idle", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.IDLE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.RUN_01:
                PlayAnimation(0, "00_run", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_01:
                PlayAnimation(0, "00_damage", false);
                AddAnimation(0, "00_idle", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_02:
                break;
            case HERO_PLAY_ANIMATION_TYPE.DAMAGE_03:
                break;
            case HERO_PLAY_ANIMATION_TYPE.STUN:
                PlayAnimation(0, "00_stun", true);
                break;
            case HERO_PLAY_ANIMATION_TYPE.DEATH_01:
                PlayAnimation(0, "00_death", false);
                break;
            case HERO_PLAY_ANIMATION_TYPE.WIN_01:
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
    /// 각 스킬에 지정되어 있는 타겟을 찾기<br/>
    /// 로컬 데이터로 찾아서, 전체 타겟에 영향을 주지 않기 위해
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    protected virtual void FindTargetsSkillAddTargets(BattleSkillData skill)
    {
        Find_Targets.Clear();
        Team_Mng.FindTargetInRange(this, skill.GetTargetType(), skill.GetTargetRuleType(), 0, skill.GetTargetOrder(), skill.GetTargetCount(), skill.GetTargetRange(), ref Find_Targets);
        skill.AddFindTargets(Find_Targets);
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
    /// <summary>
    /// 캐스팅 이펙트 소환(아무런 효과가 없이 이펙트만 표현)
    /// </summary>
    /// <param name="skill_grp"></param>
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
    /// 1회성 효과 이펙트 호출
    /// </summary>
    /// <param name="effect_path"></param>
    /// <param name="send_data"></param>
    /// <param name="target"></param>
    /// <param name="targets"></param>
    void SpawnOnetimeEffect(string effect_path, BATTLE_SEND_DATA send_data, HeroBase_V2 target, List<HeroBase_V2> targets)
    {
        if (string.IsNullOrEmpty(effect_path))
        {
            return;
        }
        var factory = Battle_Mng.GetEffectFactory();
        var effect = (SkillEffectBase)factory.CreateEffect(effect_path, GetUnitScale());
        effect.SetBattleSendData(send_data);

        var ec = effect.GetEffectComponent();
        Vector3 target_pos = Vector3.zero;
        //  적들의 중앙 위치 찾기
        if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
        {
            target_pos = ec.GetTargetsCenterPosition(targets);
        }
        else
        {
            Transform target_trans = ec.GetTargetReachPosition(target);
            if (target != null)
            {
                target_trans = ec.GetTargetReachPosition(target);
            }
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
    /// <summary>
    /// 지속성 효과 이펙트 소환
    /// </summary>
    /// <param name="effect_path"></param>
    /// <param name="send_data"></param>
    /// <param name="target"></param>
    /// <param name="targets"></param>
    void SpawnDurationEffect(string effect_path, BATTLE_SEND_DATA send_data, HeroBase_V2 target, List<HeroBase_V2> targets)
    {
        if (string.IsNullOrEmpty(effect_path))
        {
            return;
        }
        var factory = Battle_Mng.GetEffectFactory();
        var effect = (SkillEffectBase)factory.CreateEffect(effect_path, target.GetUnitScale());
        effect.SetBattleSendData(send_data);

        var ec = effect.GetEffectComponent();
        Vector3 target_pos = Vector3.zero;
        
        //  적들의 중앙 위치 찾기
        if (ec.Projectile_Reach_Pos_Type == TARGET_REACH_POS_TYPE.TARGET_CENTER)
        {
            target_pos = ec.GetTargetsCenterPosition(send_data.Targets);
        }
        else
        {
            Transform target_trans = ec.GetTargetReachPosition(target);
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

    /// <summary>
    /// 스킬 구현 V3<br/>
    /// 스킬 프리팹에 EffectComponent 정의를 사용하여<br/>
    /// 이펙트에서 사용할 정의는 해당 컴포넌트를 참조하여 사용한다.<br/>
    /// 스킬에 이펙트 프리팹 정보가 있을 경우 해당 이펙트를 트리거로 사용하여 효과를 줄 수 있다<br/>
    /// 스킬에 이펙트 프리팹 정보가 없을 경우, 즉시 발동하는 스킬로 해당 스킬에서 바로 적용할 수 있도록 한다.
    /// </summary>
    /// <param name="skill"></param>
    protected virtual void SpawnSkillEffect_V3(BattleSkillGroup group, BattleSkillData skill)
    {
        if (skill.IsEmptyFindTarget())
        {
            return;
        }

        int target_cnt = skill.GetFindTargets().Count;
        int effect_weight_index = skill.GetEffectWeightIndex();

        string skill_trigger_effect_prefab = skill.GetTriggerEffectPrefabPath();
        //  트리거 이펙트가 없을 경우
        if (string.IsNullOrEmpty(skill_trigger_effect_prefab))
        {
            //  트리거 이펙트가 없으면, 일회성/지속성 스킬로 트리거를 발생시킨다.(트리거 이펙트가 없으면, EFFECT_COUNT_TYPE.EACH_TARGET_EFFECT 형식으로 각각의 이펙트를 구현해준다)
            for (int i = 0; i < target_cnt; i++)
            {
                var target = skill.GetFindTargets()[i];
                BATTLE_SEND_DATA send_data = new BATTLE_SEND_DATA();
                send_data.Caster = this;
                send_data.AddTarget(target);
                send_data.Skill_Group = group;
                send_data.Skill = skill;
                send_data.Effect_Weight_Index = effect_weight_index;

                //  onetime skill
                var onetime_list = skill.GetOnetimeSkillDataList();
                int one_cnt = onetime_list.Count;
                for (int o = 0; o < one_cnt; o++)
                {
                    var onetime = onetime_list[o];
                    string effect_path = onetime.GetEffectPrefab();
                    if (string.IsNullOrEmpty(effect_path))
                    {
                        send_data.Onetime = onetime;
                        onetime.ExecSkill(send_data);
                        continue;
                    }
                    send_data.Onetime = onetime;

                    SpawnOnetimeEffect(effect_path, send_data, target, skill.GetFindTargets());
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
                        send_data.Duration = duration;
                        duration.ExecSkill(send_data);
                        continue;
                    }
                    send_data.Duration = duration;
                    SpawnDurationEffect(effect_path, send_data, target, skill.GetFindTargets());
                }
            }
        }
        else //     트리거 이펙트가 있을 경우
        {
            //  단일 이펙트 (대표 이펙트 - 1개의 초기 이펙트만 표현하고, 이후에는 이벤트에 따른 데미지 또는 추가 이펙트 정도만 표현해준다)
            if (skill.GetEffectCountType() == EFFECT_COUNT_TYPE.SINGLE_EFFECT)
            {
                BATTLE_SEND_DATA send_data = new BATTLE_SEND_DATA();
                send_data.Caster = this;
                send_data.AddTargets(skill.GetFindTargets());
                send_data.Skill_Group = group;
                send_data.Skill = skill;
                send_data.Effect_Weight_Index = effect_weight_index;

                //  첫번재 이펙트만 보여줘야 하므로
                if (effect_weight_index == 0)
                {
                    //  onetime skill
                    var onetime_list = skill.GetOnetimeSkillDataList();
                    if (onetime_list.Count > 0)
                    {
                        send_data.Onetime = onetime_list[0];
                    }

                    //  트리거 이펙트가 있으면, 본 이펙트를 출현함으로써, 일회성/지속성 스킬의 트리거로 사용할 수 있다.
                    SpawnOnetimeEffect(skill_trigger_effect_prefab, send_data, skill.GetFindTargets().First(),  skill.GetFindTargets());
                }
                else
                {
                    //  일회성 / 지속성 효과만 적용
                    //  트리거 이펙트가 없으면, 일회성/지속성 스킬로 트리거를 발생시킨다.(트리거 이펙트가 없으면, EFFECT_COUNT_TYPE.EACH_TARGET_EFFECT 형식으로 각각의 이펙트를 구현해준다)

                    //  onetime skill
                    var onetime_list = skill.GetOnetimeSkillDataList();
                    for (int o = 0; o < onetime_list.Count; o++)
                    {
                        var onetime = onetime_list[o];
                        string effect_path = onetime.GetEffectPrefab();
                        if (string.IsNullOrEmpty(effect_path))
                        {
                            send_data.Onetime = onetime;
                            onetime.ExecSkill(send_data);
                            continue;
                        }
                        send_data.Onetime = onetime;
                        SpawnOnetimeEffect(effect_path, send_data, send_data.GetFirstTarget(), send_data.Targets);
                    }
                    //  duration skill
                    var duration_list = skill.GetDurationSkillDataList();
                    for (int d = 0; d < duration_list.Count; d++)
                    {
                        var duration = duration_list[d];
                        string effect_path = duration.GetEffectPrefab();
                        if (string.IsNullOrEmpty(effect_path))
                        {
                            send_data.Duration = duration;
                            duration.ExecSkill(send_data);
                            continue;
                        }
                        send_data.Duration = duration;
                        SpawnDurationEffect(effect_path, send_data, send_data.GetFirstTarget(), send_data.Targets);
                    }
                }
            }
            else  // 각 타겟별 이펙트
            {
                for (int i = 0; i < target_cnt; i++)
                {
                    var target = skill.GetFindTargets()[i];

                    BATTLE_SEND_DATA send_data = new BATTLE_SEND_DATA();
                    send_data.Caster = this;
                    send_data.AddTarget(target);
                    send_data.Skill_Group = group;
                    send_data.Skill = skill;
                    send_data.Effect_Weight_Index = effect_weight_index;
                    //  트리거 이펙트가 있으면, 본 이펙트를 출현함으로써 일회성/지속성 스킬의 트리거로 사용할 수 있다.
                    SpawnOnetimeEffect(skill_trigger_effect_prefab, send_data, target, skill.GetFindTargets());
                    
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
                var track = all_tracks[i];
                if (track == null)
                {
                    continue;
                }
                track.TimeScale = 0f;
            }
        }
        //Skeleton.timeScale = 0f;
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
        //Skeleton.timeScale = Battle_Speed_Multiple;
    }
    /// <summary>
    /// 지속성 스킬 효과 데이터를 모두 제거<br/>
    /// 데이터를 제거하면 자동으로 이펙트도 사라지도록 구현되어 있음
    /// </summary>
    public void ClearDurationSkillDataList()
    {
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

        if (Damage_Text_Position_List.Count == 0)
        {
            //  중앙을 포함해서 8방향. 9개만 생성해주자.
            Damage_Text_Position_List.Add(new Vector3(0, 0, 0));        //  center
            
            Damage_Text_Position_List.Add(new Vector3(.3f, .3f, 0));       //  top right

            Damage_Text_Position_List.Add(new Vector3(-.3f, -.3f, 0));       //  left bottom 

            Damage_Text_Position_List.Add(new Vector3(.3f, 0, 0));       //  right

            Damage_Text_Position_List.Add(new Vector3(-.3f, 0, 0));       //  left

            Damage_Text_Position_List.Add(new Vector3(.3f, -.3f, 0));       //  right bottom

            Damage_Text_Position_List.Add(new Vector3(-.3f, .3f, 0));       //  left top 

            Damage_Text_Position_List.Add(new Vector3(0, -.3f, 0));       //  bottom

            Damage_Text_Position_List.Add(new Vector3(0, .3f, 0));       //  top

            
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
        ONETIME_EFFECT_TYPE etype = data.Onetime.GetOnetimeEffectType();

        //  물리 마법 결정 데미지
        double last_damage = etype == ONETIME_EFFECT_TYPE.MAGIC_DAMAGE ? data.Magic_Attack_Point : data.Physics_Attack_Point;

        //var target = data.Targets[0];
        //double vampire_hp = last_damage * Attack_Life_Recovery / (Attack_Life_Recovery + target.GetLevel() + 100);
        double vampire_hp = last_damage * (Attack_Life_Recovery / (500 + Attack_Life_Recovery));
        vampire_hp = Math.Truncate(vampire_hp);
        if (vampire_hp > 0)
        {
            AddLifeRecovery(vampire_hp);
        }
    }

    /// <summary>
    /// 피격자가 최종 합산 데미지를 계산 후 공격자에게 합산 데미지 정보를 전달해준다.<br/>
    /// 공격자는 필요에 따라 최종 합산 데미지를 이용하여 회복 등을 사용할 수 있다.<br/>
    /// 체력 회복량 = 준 데미지 * HP 흡수 포인트 / (HP 흡수 포인트 + 상대 레벨 + 100)
    /// </summary>
    /// <param name="total_data"></param>
    public void SendLastTotalDamage(Total_Damage_Data total_data)
    {
        if (total_data.Skill_ID == 0 || total_data.GetTotalDamage() == 0)
        {
            return;
        }
        double total_damage = total_data.GetTotalDamage();
        double vampire_hp = total_damage * (Attack_Life_Recovery / (500 + Attack_Life_Recovery));
        vampire_hp = Math.Truncate(vampire_hp);
        if (vampire_hp > 0)
        {
            AddLifeRecovery(vampire_hp);
        }
    }

    /// <summary>
    /// 데미지 합산을 계산하기 위한 함수<br/>
    /// Max_Effect_Weight_Count에 도달할 경우, 시전자에게 최종 합산 데미지 정보를 전달해주고<br/>
    /// 합산 데미지를 표시 요청해주는 역할을 해야한다<br/>
    /// 시전자에게 정보를 보내는것은 막타에서 항상 보내주고<br/>
    /// 합산 데미지는 2회 이상의 연타를 시전할 경우에만 보여준다.
    /// </summary>
    /// <param name="send_data"></param>
    protected void AddTotalDamageInfo(BATTLE_SEND_DATA send_data)
    {
        if (send_data.Skill == null)
        {
            return;
        }
        int skill_id = send_data.Skill.GetSkillID();
        int max_effect_count = send_data.Skill.GetMaxEffectWeightCount();
        Total_Damage_Data find = Total_Damage_Data.Find(x => x.Skill_ID == skill_id);
        if (find == null)
        {
            find = new Total_Damage_Data(skill_id);
            find.Max_Effect_Weight_Count = max_effect_count;
            find.Onetime_Effect_Type = send_data.Onetime.GetOnetimeEffectType();
            Total_Damage_Data.Add(find);
        }
        bool is_last_attack = false;
        if (find.Onetime_Effect_Type == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE)
        {
            is_last_attack = find.AddDamage(send_data.Effect_Weight_Index, send_data.Physics_Attack_Point);
        }
        else
        {
            is_last_attack = find.AddDamage(send_data.Effect_Weight_Index, send_data.Magic_Attack_Point);
        }
        //  막타라면 해야할일을 하자
        if (is_last_attack)
        {
            Total_Damage_Data.Remove(find);
            //  연타일 경우, 합산 정보 보여줌 (1타 공격인 경우 합산 정보 필요없음)
            if (max_effect_count > 1)
            {
                //  todo
                AddTotalDamageText(find);
            }

            //  시전자에게 최종 합산 데미지 정보 전달해줘야 함
            send_data.Caster.SendLastTotalDamage(find);
        }
    }

    /// <summary>
    /// 적에게서 데미지를 받는다.
    /// </summary>
    /// <param name="dmg"></param>
    public void AddDamage(BATTLE_SEND_DATA dmg)
    {
        if (dmg.Skill_Group != null)
        {
            if (dmg.Skill_Group.GetSkillType() != SKILL_TYPE.SPECIAL_SKILL)
            {
                if (!IsAlive())
                {
                    return;
                }
            }
        }
        else
        {
            if (!IsAlive())
            {
                return;
            }
        }
        

        ONETIME_EFFECT_TYPE etype = dmg.Onetime.GetOnetimeEffectType();
        bool is_physics_dmg = etype == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE;
        //  물리 마법 결정 데미지
        double damage = is_physics_dmg ? dmg.Physics_Attack_Point : dmg.Magic_Attack_Point;

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
        last_damage = GetCalcDamage(last_damage, is_physics_dmg);

        //  치명타 확률
        double cri_chance = dmg.Caster.GetCriticalChanceRate(dmg.Caster.GetLevel(), GetLevel(), is_physics_dmg);
        int r = UnityEngine.Random.Range(0, 100000);
        if (r < cri_chance)
        {
            if (is_physics_dmg)
            {
                dmg.Is_Physics_Critical = true;
                last_damage += last_damage * (1.5 + Physics_Critical_Power_Add);
            }
            else
            {
                dmg.Is_Magic_Critical = true;
                last_damage += last_damage * (1.5 + Magic_Critical_Power_Add);
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
            dmg.Is_Weak = true;
        }

        //  소수점 이하 버리기
        last_damage = Math.Truncate(last_damage);
        if (last_damage <= 0)
        {
            return;
        }
        if (is_physics_dmg)
        {
            dmg.Physics_Attack_Point = last_damage;
        }
        else
        {
            dmg.Magic_Attack_Point = last_damage;
        }


        //  색 반짝 / 넉백 
        AttackHittedKnockback();

        if (Game_Type != GAME_TYPE.EDITOR_SKILL_PREVIEW_MODE && Game_Type != GAME_TYPE.EDITOR_SKILL_EDIT_MODE)
        {
            Life -= last_damage;
        }

        
        if (Life <= 0)
        {
            Life = 0;

            AddDamageText(dmg);
            //  토탈 데미지 계산을 위해서
            if (dmg.Skill_Group != null && dmg.Skill_Group.GetSkillType() == SKILL_TYPE.SPECIAL_SKILL)
            {
                AddTotalDamageInfo(dmg);
            }

            UpdateLifeBar();
            //  궁극기 피격시 죽어도 당장 죽이지는 않는다.(궁극기 종료 후 사라지도록)
            if (GetCurrentState() != UNIT_STATES.ULTIMATE_PAUSE && GetCurrentState() != UNIT_STATES.SKILL_1)
            {
                ChangeState(UNIT_STATES.DEATH);
            }
            return;
        }

        AddDamageText(dmg);
        //  토탈 데미지 계산을 위해서
        if (dmg.Skill_Group != null && dmg.Skill_Group.GetSkillType() == SKILL_TYPE.SPECIAL_SKILL)
        {
            AddTotalDamageInfo(dmg);
        }

        UpdateLifeBar();

        SendSlotEvent(SKILL_SLOT_EVENT_TYPE.HITTED);
    }

    /// <summary>
    /// 합산 데미지 폰트 생성
    /// </summary>
    /// <param name="total"></param>
    void AddTotalDamageText(Total_Damage_Data total)
    {
        DAMAGE_TYPE dmg_type = DAMAGE_TYPE.TOTAL;
        Vector3 target_pos = GetReachPosTypeTransform(TARGET_REACH_POS_TYPE.BODY).position;
        bool is_physics = total.Onetime_Effect_Type == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE;
        string prefab_path = string.Empty;
        if (is_physics)
        {
            prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Physics_Total_Damage_Node";
        }
        else
        {
            prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Magic_Total_Damage_Node";
        }

        var d = new Effect_Queue_Data();
        d.Effect_path = prefab_path;
        d.Target_Position = target_pos;
        d.Data = total;
        d.Duration = 1f + ((Battle_Speed_Multiple - 1) * 1f);
        d.Parent_Transform = UI_Mng.GetDamageContainer();
        d.Damage_Type = dmg_type;
        SpawnQueueEffect(d);
    }


    /// <summary>
    /// 데미지 폰트 생성
    /// </summary>
    /// <param name="send_data"></param>
    void AddDamageText(BATTLE_SEND_DATA send_data)
    {
        DAMAGE_TYPE dmg_type = DAMAGE_TYPE.NORMAL;
        Vector3 dmg_text_position = Vector3.zero;
        var etype = send_data.Onetime.GetOnetimeEffectType();
        bool is_physics_dmg = etype == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE;
        bool is_multi_damage = send_data.Skill.GetMaxEffectWeightCount() > 1;
        string prefab_path = string.Empty;
        if (is_multi_damage)
        {
            dmg_text_position = GetDamageTextPositionByIndex(0);
            //dmg_text_position = GetDamageTextPositionOrder();
        }
        else
        {
            dmg_text_position = GetDamageTextPositionByIndex(0);
            //dmg_text_position = GetDamageTextPositionOrder();
        }

        if (is_physics_dmg)
        {
            //if (is_multi_damage)
            //{
            //    prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Physics_Multi_Damage_Node";
            //}
            //else
            //{
            //    prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Physics_Single_Damage_Node";
            //}
            prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Physics_Single_Damage_Node";
            if (send_data.Is_Weak)
            {
                if (send_data.Is_Physics_Critical)
                {
                    dmg_type = DAMAGE_TYPE.WEAK_CRITICAL;
                }
                else
                {
                    dmg_type = DAMAGE_TYPE.WEAK;
                }
            }
            else
            {
                if (send_data.Is_Physics_Critical)
                {
                    dmg_type = DAMAGE_TYPE.CRITICAL;
                }
            }
        }
        else
        {
            //if (is_multi_damage)
            //{
            //    prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Magic_Multi_Damage_Node";
            //}
            //else
            //{
            //    prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Magic_Single_Damage_Node";
            //}
            prefab_path = "Assets/AssetResources/Prefabs/Effects/Common/Magic_Single_Damage_Node";
            if (send_data.Is_Weak)
            {
                if (send_data.Is_Magic_Critical)
                {
                    dmg_type = DAMAGE_TYPE.WEAK_CRITICAL;
                }
                else
                {
                    dmg_type = DAMAGE_TYPE.WEAK;
                }
            }
            else
            {
                if (send_data.Is_Magic_Critical)
                {
                    dmg_type = DAMAGE_TYPE.CRITICAL;
                }
            }
        }

        var d = new Effect_Queue_Data();
        d.Effect_path = prefab_path;
        d.Target_Position = GetReachPosTypeTransform(TARGET_REACH_POS_TYPE.BODY).position + dmg_text_position;
        d.Data = send_data;
        d.Duration = 1f + ((Battle_Speed_Multiple - 1) * 1f);
        d.Parent_Transform = UI_Mng.GetDamageContainer();
        d.Damage_Type = dmg_type;
        //d.Need_Move = !is_multi_damage;
        d.Need_Move = true;
        d.Target_Team_Type = Team_Type;
        SpawnQueueEffect(d);

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

        AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/HealText_Effect", Life_Bar_Pos.position, recovery_hp, 1f, null);
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

    public void AddSpawnEffectText(string path, Vector3 target_pos, object data, float duration, Transform parent)
    {
        lock (Effect_Queue_Lock)
        {
            var d = new Effect_Queue_Data();
            d.Effect_path = path;
            d.Target_Position = target_pos;
            d.Data = data;
            d.Duration = duration + ((Battle_Speed_Multiple - 1) * 1f);
            d.Parent_Transform = parent;
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
        if (edata.Parent_Transform == null)
        {
            EffectBase effect = Battle_Mng.GetEffectFactory().CreateEffect(edata.Effect_path);
            effect.transform.position = edata.Target_Position;
            effect.SetData(edata.Data);
            effect.StartParticle(edata.Duration);
        }
        else
        {
            EffectBase effect = Battle_Mng.GetEffectFactory().CreateEffect(edata.Effect_path, edata.Parent_Transform);
            if (edata.Need_Move)
            {
                effect.SetData(edata.Data, edata.Damage_Type, edata.Target_Position, UIEaseBase.MOVE_TYPE.MOVE_OUT, edata.Target_Team_Type);
            }
            else
            {
                effect.SetData(edata.Data, edata.Damage_Type, edata.Target_Position, edata.Target_Team_Type);
            }
            
            effect.StartParticle(edata.Duration);
        }

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
        this.transform.localPosition = pos;
    }
    /// <summary>
    /// 궁극기 시전 요청
    /// </summary>
    public virtual void UltimateSkillExec()
    {
        var battle_state = Battle_Mng.GetCurrentState();
        if (battle_state != GAME_STATES.PLAYING)
        {
            return;
        }
        //  궁극기를 사용할 수 없는 상태
        var state = GetCurrentState();
        if (state == UNIT_STATES.SKILL_1 || state == UNIT_STATES.SKILL_READY_1 || state == UNIT_STATES.SKILL_END || state == UNIT_STATES.ULTIMATE_PAUSE)
        {
            return;
        }

        //  여러가지 상황상 궁극기를 사용할 수 없는 상황을 체크
        //  체크 완료 후 궁극기를 사용할 수 있는 경우에만 궁극기 사용
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        if (skill_grp == null)
        {
            return;
        }
        //  궁극기가 쿨타임이 남아있을 경우 안됨
        if (!skill_grp.IsPrepareCooltime())
        {
            return;
        }

        for (int i = 0; i < skill_grp.GetBattleSkillDataList().Count; i++)
        {
            var skill = skill_grp.GetBattleSkillDataList()[i];
            FindTargetsSkillAddTargets(skill);
        }

        var target_skill = skill_grp.GetSpecialSkillTargetSkill();
        
        //  타겟이 잡혀있지 않다면, 일단 스킬 사용 안되도록
        if (target_skill.IsEmptyFindTarget())
        {
            return;
        }
        //  모든 체력 게이지 숨기기
        Battle_Mng.HideAllUnitLifeBar();
        //  주인공 이외의 모든 캐릭 pause
        Battle_Mng.AllPauseUnitWithoutHero(this);

        //  진행중이던 이펙트 모두 숨기기
        Battle_Mng.GetEffectFactory().OnPauseAndHide();

        //  주인공 및 타겟을 제외한 다른 유닛은 모두 숨기기
        List<HeroBase_V2> targets = new List<HeroBase_V2>();
        targets.Add(this);
        targets.AddRange(target_skill.GetFindTargets().FindAll(x => !object.ReferenceEquals(x, this)));
        Battle_Mng.HideAllUnitWithoutTargets(targets);

        Skeleton.AnimationState.ClearTracks();
        if (state == UNIT_STATES.ATTACK_READY_1 || state == UNIT_STATES.ATTACK_1 || state == UNIT_STATES.ATTACK_END)
        {
            GetSkillManager().SetNextSkillPattern();
        }

        Battle_Mng.StartUltimateSkill();
        ChangeState(UNIT_STATES.SKILL_READY_1);
    }

    public void SetChangeColor(string color, bool is_rollback)
    {
        if (is_rollback)
        {
            Render_Texture.SetRollbackColor();
        }
        else
        {
            Render_Texture.SetChangeColor(color);
        }
        
    }

    /// <summary>
    /// 피격시 뒤로 살짝 밀렸다가 다시 돌아오도록<br/>
    /// 궁극기 피격시와 일반 스킬 피격시로 나눈다.<br/>
    /// 기본적으로 흰색 반짝이는 기능은 추가
    /// </summary>
    protected void AttackHittedKnockback()
    {
        //  피격 색 반짝 기능 기본
        Render_Texture.SetHitColorV2(Color.white, 0.1f);

        //  피격시 현재 상태에 따라 피격 동작 추가할 것인지 여부 판단
        var state = GetCurrentState();
        if (state == UNIT_STATES.ULTIMATE_PAUSE)
        {
            var prev_state = GetPreviousState();
            if (prev_state != UNIT_STATES.ATTACK_1 && prev_state != UNIT_STATES.SKILL_1)
            {
                //  아마 안움직여 질수도(timescale이 0인 상태라)
                PlayAnimation(HERO_PLAY_ANIMATION_TYPE.DAMAGE_01);
            }
            //  궁극기 피격 동작이기 때문에 제일 크게 밀림
            StartHittedAnim(.35f);
        }
        else if (state == UNIT_STATES.ATTACK_1 || state == UNIT_STATES.SKILL_1)
        {
            //  피격 동작이 없기 때문에 조금 더 크게 밀림
            StartHittedAnim(.25f);
        }
        else if (state == UNIT_STATES.MOVE || state == UNIT_STATES.MOVE_IN)
        {
            //  nothing (이동시에는 피격에 의한 넉백은 없도록 하자)
        }
        else
        {
            //  피격 동작이 있기 때문에 작게 움직여도 괜찮아 보임.
            PlayAnimation(HERO_PLAY_ANIMATION_TYPE.DAMAGE_01);
            StartHittedAnim(.2f);
        }
        
    }
    /// <summary>
    /// 히트 동작 요청
    /// </summary>
    protected void StartHittedAnim(float knockback_dist)
    {
        if (Hitted_Corouine != null)
        {
            StopCoroutine(Hitted_Corouine);
            this.transform.position = Before_Hitted_Position;
        }
        Hitted_Corouine = StartCoroutine(StartHittedKnockback(knockback_dist));
    }
    
    /// <summary>
    /// 뒤로 밀리는 동작 
    /// </summary>
    /// <returns></returns>
    IEnumerator StartHittedKnockback(float knockback_dist)
    {
        Before_Hitted_Position = this.transform.position;
        float duration = 0.15f / Battle_Speed_Multiple;
        float delta = 0f;

        var pos = this.transform.position;
        if (Team_Type == TEAM_TYPE.LEFT)
        {
            pos.x -= knockback_dist;
        }
        else
        {
            pos.x += knockback_dist;
        }

        while (delta < duration)
        {
            delta += Time.deltaTime;

            this.transform.position = Vector3.Lerp(this.transform.position, pos, delta / duration);

            yield return null;
        }

        Hitted_Corouine = null;
        this.transform.position = Before_Hitted_Position;
    }

    /// <summary>
    /// 이벤트 사용 필요.<br/>
    /// 궁극기의 효과를 풍부하게 만들어 줄 수 있음<br/><br/>
    /// <b>Trigger_ID</b> = <i>char_hide</i><br/>
    /// <b>IntValue</b><br/>
    /// 0 = 시전자와 타겟을 제외한 모든 캐릭터 숨기기<br/>
    /// 1 = 시전자를 제외한 모든 캐릭터 숨기기<br/>
    /// 2 = 타겟을 제외한 모든 캐릭터 숨기기<br/>
    /// 3 = 모든 캐릭터 숨기기<br/>
    /// <b>FloatValue</b>
    /// 알파값이 적용되는 시간(초 단위로 그래프는 리니어)<br/>
    /// <b>StrValue</b>
    /// 이벤트 이름이 필요한 경우<br/><br/>
    /// <b>Trigger_ID</b> = <i>chr_show</i><br/>
    /// <b>IntValue</b><br/>
    /// 0 = 시전자와 타겟만 보이기<br/>
    /// 1 = 시전자만 보이기<br/>
    /// 2 = 타겟만 보이기<br/>
    /// 3 = 모든 캐릭터 보이기<br/>
    /// <b>FloatValue</b>
    /// 알파값이 적용되는 시간(초 단위로 그래프는 리니어)<br/>
    /// <b>StrValue</b>
    /// 이벤트 이름이 필요한 경우
    /// </summary>
    /// <param name="trigger_id">chr_hide, chr_show, cast</param>
    /// <param name="evt_val"></param>
    public virtual void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    {
        Debug.Log($"{trigger_id} => {evt_val.ToString()}");
        
    }

  
    /// <summary>
    /// 궁극기 시전 중 캐릭터 숨기기 이벤트 적용
    /// </summary>
    /// <param name="evt_val"></param>
    protected void HideCharacters(EventTriggerValue evt_val)
    {
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        List<BattleSkillData> skill_list = new List<BattleSkillData>();
        if (string.IsNullOrEmpty(evt_val.StrValue))
        {
            var target_skill = skill_grp.GetSpecialSkillTargetSkill();
            if (target_skill != null)
            {
                skill_list.Add(target_skill);
            }
        }
        else
        {
            skill_list.AddRange(skill_grp.GetExecutableSkillDatas(evt_val.StrValue));
        }
        if (skill_list.Count == 0)
        {
            return;
        }

        //  제외 타겟 리스트
        List<HeroBase_V2> exclude_targets = new List<HeroBase_V2>();
        switch (evt_val.IntValue)
        {
            case 0:     //  시전자와 타겟을 제외한 모든 캐릭터 숨기기
                exclude_targets.Add(this);
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!exclude_targets.Contains(target))
                        {
                            exclude_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.HideAllUnitWithoutTargets(exclude_targets);
                break;
            case 1:     //  시전자를 제외한 모든 캐릭터 숨기기
                exclude_targets.Add(this);
                Battle_Mng.HideAllUnitWithoutTargets(exclude_targets);
                break;
            case 2:     //  타겟을 제외한 모든 캐릭터 숨기기
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!exclude_targets.Contains(target))
                        {
                            exclude_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.HideAllUnitWithoutTargets(exclude_targets);
                break;
            case 3:     //  모든 캐릭터 숨기기
                Battle_Mng.HideAllUnits();
                break;
        }
    }

    protected void RollbackColorCharacter(EventTriggerValue evt_val)
    {
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        List<BattleSkillData> skill_list = new List<BattleSkillData>();

        if (string.IsNullOrEmpty(evt_val.StrValue.Trim()))
        {
            var target_skill = skill_grp.GetSpecialSkillTargetSkill();
            if (target_skill != null)
            {
                skill_list.Add(target_skill);
            }
        }
        else
        {
            skill_list.AddRange(skill_grp.GetExecutableSkillDatas(evt_val.StrValue.Trim()));
        }
        if (skill_list.Count == 0)
        {
            return;
        }
        //  포함 타겟 리스트
        List<HeroBase_V2> include_targets = new List<HeroBase_V2>();
        for (int i = 0; i < skill_list.Count; i++)
        {
            var skill = skill_list[i];
            for (int t = 0; t < skill.GetFindTargets().Count; t++)
            {
                var target = skill.GetFindTargets()[t];
                if (!include_targets.Contains(target))
                {
                    include_targets.Add(target);
                }
            }
        }
        Battle_Mng.ChangeColorTargets(include_targets, string.Empty, true);
    }
    
    protected void ChangeColorCharacter(EventTriggerValue evt_val)
    {
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        List<BattleSkillData> skill_list = new List<BattleSkillData>();
        var evt_list = evt_val.StrValue.Split(",");
        if (evt_list.Length != 2)
        {
            return;
        }
        //  0 : 스킬 이벤트
        //  1 : 컬러값
        //  float : duration
        string event_key = evt_list[0];
        string color = evt_list[1];

        if (string.IsNullOrEmpty(event_key))
        {
            var target_skill = skill_grp.GetSpecialSkillTargetSkill();
            if (target_skill != null)
            {
                skill_list.Add(target_skill);
            }
        }
        else
        {
            skill_list.AddRange(skill_grp.GetExecutableSkillDatas(event_key));
        }
        if (skill_list.Count == 0)
        {
            return;
        }
        //  포함 타겟 리스트
        List<HeroBase_V2> include_targets = new List<HeroBase_V2>();
        for (int i = 0; i < skill_list.Count; i++)
        {
            var skill = skill_list[i];
            for (int t = 0; t < skill.GetFindTargets().Count; t++)
            {
                var target = skill.GetFindTargets()[t];
                if (!include_targets.Contains(target))
                {
                    include_targets.Add(target);
                }
            }
        }

        Battle_Mng.ChangeColorTargets(include_targets, color, false);

    }

    /// <summary>
    /// 궁극기 시전 중 캐릭터 보이기 이벤트 적용
    /// </summary>
    /// <param name="evt_val"></param>
    protected void ShowCharacters(EventTriggerValue evt_val)
    {
        var skill_grp = GetSkillManager().GetSpecialSkillGroup();
        List<BattleSkillData> skill_list = new List<BattleSkillData>();
        if (string.IsNullOrEmpty(evt_val.StrValue))
        {
            var target_skill = skill_grp.GetSpecialSkillTargetSkill();
            if (target_skill != null)
            {
                skill_list.Add(target_skill);
            }
        }
        else
        {
            skill_list.AddRange(skill_grp.GetExecutableSkillDatas(evt_val.StrValue));
        }
        if (skill_list.Count == 0)
        {
            return;
        }
        //  포함 타겟 리스트
        List<HeroBase_V2> include_targets = new List<HeroBase_V2>();
        switch (evt_val.IntValue)
        {
            case 0:     //  시전자와 타겟만 보이기
                include_targets.Add(this);
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!include_targets.Contains(target))
                        {
                            include_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.ShowUnitTargets(include_targets);
                break;
            case 1:     //  시전자만 보이기
                include_targets.Add(this);
                Battle_Mng.ShowUnitTargets(include_targets);
                break;
            case 2:     //  타겟만 보이기
                for (int i = 0; i < skill_list.Count; i++)
                {
                    var skill = skill_list[i];
                    for (int t = 0; t < skill.GetFindTargets().Count; t++)
                    {
                        var target = skill.GetFindTargets()[t];
                        if (!include_targets.Contains(target))
                        {
                            include_targets.Add(target);
                        }
                    }
                }
                Battle_Mng.ShowUnitTargets(include_targets);
                break;
            case 3:     //  모든 캐릭터 보이기
                Battle_Mng.ShowAllUnits();
                break;
        }
    }

    #region ToString
    public override string ToString()
    {
        var state = GetCurrentState();
        var sb = ZString.CreateStringBuilder();
        
        sb.AppendLine($"{nameof(Team_Type)} : <color=#ffffff>[{Team_Type}]</color>");
        sb.AppendLine($"position => [{this.transform.position.x} , {this.transform.position.z}]");
        sb.AppendLine($"{nameof(state)} => {state}");
        sb.AppendLine($"{nameof(Life)} => {Life}");
        sb.AppendLine($"{nameof(Max_Life)} => {Max_Life}");

        var history = GetStatesHistory();
        if (history != null && history.Count > 0)
        {
            sb.AppendLine("States History");
            for (int i = 0; i < history.Count; i++)
            {
                sb.AppendLine($"{history[i]}");
            }
        }

        return sb.ToString();
    }

    [ContextMenu("ToStringHeroBase")]
    public void ToStringHeroBase()
    {
        Debug.Log(ToString());

    }
    #endregion

}
