using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleSkillManager : BattleDataBase
{
    /// <summary>
    /// 플레이 캐릭터
    /// </summary>
    HeroBase_V2 Hero;

    /// <summary>
    /// 스킬 사용 순서 인덱스
    /// </summary>
    int Skill_Pattern_Order;
    List<BattleSkillGroup> _Skill_Groups;

    /// <summary>
    /// 지속성 효과 데이터 관리
    /// </summary>
    List<BattleDurationSkillData> Used_Battle_Duration_Data_List = new List<BattleDurationSkillData>();
    /// <summary>
    /// 삭제 예약을 위한 지속성 데이터 리스트
    /// </summary>
    List<BattleDurationSkillData> Remove_Reserved_Battle_Duration_Data_List = new List<BattleDurationSkillData>();

    object Duration_Lock = new object();

    public IReadOnlyList<BattleSkillGroup> Skill_Groups => _Skill_Groups.AsReadOnly();

    protected override void Reset()
    {
        base.Reset();
        Skill_Pattern_Order = 0;
        _Skill_Groups = new List<BattleSkillGroup>();
    }

    protected override void Destroy()
    {
        Hero = null;
        ClearDurationSkillDataList();
    }

    public void SetHeroBase(HeroBase_V2 h)
    {
        Hero = h;
    }

    /// <summary>
    /// 플레이어 캐릭터의 스킬 패턴에 따른 스킬 그룹 추가
    /// </summary>
    /// <param name="skill_patterns"></param>
    public void SetPlayerCharacterSkillGroups(UserHeroData hero, int[] skill_patterns)
    {
        int len = skill_patterns.Length;
        for (int i = 0; i < len; i++)
        {
            int skill_group_id = skill_patterns[i];
            if (skill_group_id == 0)
                continue;
            AddPlayerCharacterBattleSkillGroup(hero.GetPlayerCharacterID(), hero.Player_Character_Num, skill_group_id, i);
        }
    }

    public void SetPlayerCharacterSpecialSkillGroup(UserHeroData hero, int special_skill_id)
    {
        if (special_skill_id == 0)
        {
            return;
        }
        AddPlayerCharacterBattleSkillGroup(hero.GetPlayerCharacterID(), hero.Player_Character_Num, special_skill_id, 10);
    }

    void AddPlayerCharacterBattleSkillGroup(int pc_id, int pc_num, int skill_group_id, int order)
    {
        var user_skill = GameData.Instance.GetUserHeroSkillDataManager().FindUserHeroSkillData(pc_id, pc_num, skill_group_id);
        var grp = new BattlePcSkillGroup(user_skill);
        grp.SetSkillOrder(order);
        grp.SetSkillGroupID(skill_group_id);
        _Skill_Groups.Add(grp);
    }

    /// <summary>
    /// npc 의 스킬 패턴에 따른 스킬 그룹 추가
    /// </summary>
    /// <param name="skill_patterns"></param>
    public void SetNpcSkillGroups(int[] skill_patterns)
    {
        int len = skill_patterns.Length;
        for (int i = 0; i < len; i++)
        {
            int skill_group_id = skill_patterns[i];
            if (skill_group_id == 0)
            {
                continue;
            }
            AddNpcBattleSkillGroup(skill_group_id, i);
        }
    }
    public void SetNpcSpecialSkillGroup(int special_skill_id)
    {
        if (special_skill_id == 0)
        {
            return;
        }
        AddNpcBattleSkillGroup(special_skill_id, 10);
    }
    void AddNpcBattleSkillGroup(int skill_group_id, int order)
    {
        var grp = new BattleNpcSkillGroup();
        grp.SetSkillOrder(order);
        grp.SetSkillGroupID(skill_group_id) ;
        _Skill_Groups.Add(grp);
    }

    /// <summary>
    /// 현재 진행중인 스킬 그룹 반환<br/>
    /// 해당 스킬 사용후에는 다음 스킬로 넘어가야 함
    /// </summary>
    /// <returns></returns>
    public BattleSkillGroup GetCurrentSkillGroup()
    {
        var skill_list = _Skill_Groups.FindAll(x => x.GetSkillType() != SKILL_TYPE.SPECIAL_SKILL);
        if (skill_list.Count > 0)
        {
            return skill_list.Find(x => x.Skill_Order == Skill_Pattern_Order);
        }
        return null;
    }
    /// <summary>
    /// 궁극기 스킬 데이터 반환
    /// </summary>
    /// <returns></returns>
    public BattleSkillGroup GetSpecialSkillGroup()
    {
        return FindSkillType(SKILL_TYPE.SPECIAL_SKILL);
    }

    /// <summary>
    /// 다음 스킬 패턴으로 이동
    /// </summary>
    public void SetNextSkillPattern()
    {
        //  현재 스킬 공격 대기시간 초기화
        var cur_skill = GetCurrentSkillGroup();
        cur_skill?.ResetSkill();

        //  다음 스킬로 이동
        Skill_Pattern_Order += 1;
        var skill_list = _Skill_Groups.FindAll(x => x.GetSkillType() != SKILL_TYPE.SPECIAL_SKILL);
        int max_order = skill_list.Max(x => x.Skill_Order);
        if (Skill_Pattern_Order > max_order)
        {
            Skill_Pattern_Order = 0;
        }
    }

    /// <summary>
    /// 현재 사용가능한 스킬의 선 쿨타임 진행
    /// </summary>
    /// <param name="delta_time"></param>
    /// <returns></returns>
    public bool CalcSkillUseDelay(float delta_time)
    {
        return GetCurrentSkillGroup().CalcDelayTime(delta_time);
    }

    public bool CalcSpecialSkillCooltime(float delta_time)
    {
        var special_skill = GetSpecialSkillGroup();
        if (special_skill != null)
        {
            return special_skill.CalcDelayTime(delta_time);
        }
        return false;
    }

    /// <summary>
    /// 스킬 타입의 스킬 그룹 데이터 반환
    /// </summary>
    /// <param name="stype"></param>
    /// <returns></returns>
    public BattleSkillGroup FindSkillType(SKILL_TYPE stype)
    {
        return _Skill_Groups.Find(x => x.GetSkillType() == stype);
    }

    /// <summary>
    /// 지속성 효과 추가(확률에 따라 적용됨)
    /// </summary>
    /// <param name="duration_skill"></param>
    public void AddDurationSkillEffect(BattleDurationSkillData duration_skill)
    {
        //var caster = duration_skill.GetBattleSendData().Caster;
        int rate = UnityEngine.Random.Range(0, 10000);
        if (rate < duration_skill.GetRate())
        {
            lock (Duration_Lock)
            {
                DURATION_EFFECT_TYPE d_type = duration_skill.GetDurationEffectType();
                if (duration_skill.IsOverlapable())
                {
                    Used_Battle_Duration_Data_List.Add(duration_skill);

                    //  add Spawn effect text
                    Hero?.AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/TransText_Effect", Hero.GetTargetReachPostionByTargetReachPosType(TARGET_REACH_POS_TYPE.BODY), d_type, 1f);
                }
                else
                {
                    var found = Used_Battle_Duration_Data_List.Find(x => x.GetDurationEffectType() == d_type);
                    if (found != null)
                    {
                        Used_Battle_Duration_Data_List.Remove(found);
                        found.Dispose();
                        found = null;
                    }

                    if (d_type == DURATION_EFFECT_TYPE.FREEZE)
                    {
                        Hero?.ChangeState(UNIT_STATES.FREEZE);
                    }
                    else if (d_type == DURATION_EFFECT_TYPE.STUN)
                    {
                        Hero?.ChangeState(UNIT_STATES.STUN);
                    }
                    else if (d_type == DURATION_EFFECT_TYPE.BIND)
                    {
                        Hero?.ChangeState(UNIT_STATES.BIND);
                    }

                    Used_Battle_Duration_Data_List.Add(duration_skill);

                    //  add spawn effect text
                    Hero?.AddSpawnEffectText("Assets/AssetResources/Prefabs/Effects/Common/TransText_Effect", Hero.GetTargetReachPostionByTargetReachPosType(TARGET_REACH_POS_TYPE.BODY), d_type, 1f);

                    Hero?.SendSlotEvent(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
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
    /// 지속성 스킬 효과 데이터를 모두 제거<br/>
    /// 데이터를 제거하면 자동으로 이펙트도 사라지도록 구현되어 있음
    /// </summary>
    public void ClearDurationSkillDataList()
    {
        lock (Duration_Lock)
        {
            int cnt = Used_Battle_Duration_Data_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                var dur = Used_Battle_Duration_Data_List[i];
                dur.Dispose();
            }
            Used_Battle_Duration_Data_List.Clear();
        }
    }
    /// <summary>
    /// 지속성 효과의 시간을 계산 해주기.<br/>
    /// 반복/종료 등 지속성 효과의 유지 및 효과도 적용 계산
    /// </summary>
    public void CalcDurationSkillTime(float dt)
    {
        lock(Duration_Lock)
        {
            if (Used_Battle_Duration_Data_List.Count == 0)
            {
                return;
            }

            //  상태 변경용 내부 함수
            System.Action<UNIT_STATES, BattleDurationSkillData> trans_change_state = (state, dur) =>
            {
                DURATION_EFFECT_TYPE dtype = dur.GetDurationEffectType();

                if (dtype == DURATION_EFFECT_TYPE.FREEZE)
                {
                    if (state == UNIT_STATES.FREEZE)
                    {
                        Hero?.ChangeState(UNIT_STATES.ATTACK_READY_1);
                    }
                }
                else if (dtype == DURATION_EFFECT_TYPE.STUN)
                {
                    if (state == UNIT_STATES.STUN)
                    {
                        Hero?.ChangeState(UNIT_STATES.ATTACK_READY_1);
                    }
                }
                else if (dtype == DURATION_EFFECT_TYPE.BIND)
                {
                    if (state == UNIT_STATES.BIND)
                    {
                        Hero?.ChangeState(UNIT_STATES.ATTACK_READY_1);
                    }
                }
            };

            UNIT_STATES state = Hero.GetCurrentState();
            Remove_Reserved_Battle_Duration_Data_List.Clear();
            int cnt = Used_Battle_Duration_Data_List.Count;
            for (int i = 0; i < cnt; i++)
            {
                BattleDurationSkillData duration = Used_Battle_Duration_Data_List[i];
                DURATION_CALC_RESULT_TYPE result = duration.CalcDuration_V2(dt);
                if (result != DURATION_CALC_RESULT_TYPE.NONE)
                {
                    switch (result)
                    {
                        case DURATION_CALC_RESULT_TYPE.REPEAT_INTERVAL:
                            {
                                //  반복 효과 적용
                                if (duration.IsUseRepeatInterval())
                                {
                                    var repeat_onetime_list = duration.GetRepeatOnetimeSkillDataList();
                                    int repeat_onetime_cnt = repeat_onetime_list.Count;
                                    for (int o = 0; o < cnt; o++)
                                    {
                                        BATTLE_SEND_DATA send_data = duration.GetBattleSendData().Clone();
                                        //  exec skill
                                        BattleOnetimeSkillData onetime = repeat_onetime_list[i];
                                        send_data.Onetime = onetime;
                                        //  일회성 효과 이펙트
                                        Hero.SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }
                            }
                            break;
                        case DURATION_CALC_RESULT_TYPE.FINISH:
                            {
                                //   종료 효과 적용
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
                                        Hero.SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }
                                Remove_Reserved_Battle_Duration_Data_List.Add(duration);
                                //  상태 변경
                                trans_change_state(state, duration);
                            }
                            break;
                        case DURATION_CALC_RESULT_TYPE.REPEAT_AND_FINISH:
                            {
                                //  반복 효과 적용
                                if (duration.IsUseRepeatInterval())
                                {
                                    var repeat_onetime_list = duration.GetRepeatOnetimeSkillDataList();
                                    int repeat_onetime_cnt = repeat_onetime_list.Count;
                                    for (int o = 0; o < cnt; o++)
                                    {
                                        BATTLE_SEND_DATA send_data = duration.GetBattleSendData().Clone();
                                        //  exec skill
                                        BattleOnetimeSkillData onetime = repeat_onetime_list[i];
                                        send_data.Onetime = onetime;
                                        //  일회성 효과 이펙트
                                        Hero.SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
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
                                        Hero.SpawnOnetimeEffectFromDurationSkill(onetime, send_data);
                                    }
                                }
                                Remove_Reserved_Battle_Duration_Data_List.Add(duration);
                                //  상태 변경
                                trans_change_state(state, duration);
                            }
                            break;
                    }
                }
            }

            //  종료된 스킬 제거
            if (Remove_Reserved_Battle_Duration_Data_List.Count > 0)
            {
                int reserved_cnt = Remove_Reserved_Battle_Duration_Data_List.Count;
                for (int i = 0; i < reserved_cnt; i++)
                {
                    var duration = Remove_Reserved_Battle_Duration_Data_List[i];
                    Used_Battle_Duration_Data_List.Remove(duration);
                    duration.Dispose();
                    duration = null;
                }
                //  슬롯 지속성 효과 아이콘 업데이트 요청
                Hero.SendSlotEvent(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
            }
        }
    }

    /// <summary>
    /// 지속성 효과의 지속성 방식 타입 갱신<br/>
    /// 시간 지속성 방식은 <see cref="CalcDurationSkillTime"/>을 사용한다<br/>
    /// 그 외에 피격 횟수 제한, 공격 횟수 제한의 경우 본 함수 사용
    /// </summary>
    /// <param name="ptype"></param>
    public void CalcDurationCountUse(PERSISTENCE_TYPE ptype)
    {
        if (ptype == PERSISTENCE_TYPE.NONE || ptype == PERSISTENCE_TYPE.TIME)
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
            Remove_Reserved_Battle_Duration_Data_List.Clear();
            for (int i = 0; i < cnt; i++)
            {
                BattleDurationSkillData duration = Used_Battle_Duration_Data_List[i];
                if (duration.CalcEtcPersistenceCount(ptype))
                {
                    //  지속 횟수 종료
                    Remove_Reserved_Battle_Duration_Data_List.Add(duration);
                }
            }
            //  종료된 지속성 효과 제거
            if (Remove_Reserved_Battle_Duration_Data_List.Count > 0)
            {
                int reserved_cnt = Remove_Reserved_Battle_Duration_Data_List.Count;
                for (int i = 0; i < reserved_cnt; i++)
                {
                    BattleDurationSkillData duration = Remove_Reserved_Battle_Duration_Data_List[i];
                    Used_Battle_Duration_Data_List.Remove(duration);
                    duration.Dispose();
                    duration = null;
                }
                Hero.SendSlotEvent(SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE);
            }
        }
    }

    /// <summary>
    /// 지속성 효과 타입의 멀티플 총합 반환
    /// </summary>
    /// <param name="dtype"></param>
    /// <returns></returns>
    public double GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE dtype)
    {
        double sum = 0;
        int cnt = Used_Battle_Duration_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var dur = Used_Battle_Duration_Data_List[i];
            if (dur.GetDurationEffectType() == dtype)
            {
                sum += dur.GetMultipleByMultipleType();
            }
        }
        return sum;
    }
    /// <summary>
    /// 지속성 효과 중 지정 타입의 절대 값 총합 반환
    /// </summary>
    /// <param name="dtype"></param>
    /// <returns></returns>
    public double GetDurationValuesByDurEffectType(DURATION_EFFECT_TYPE dtype)
    {
        double sum = 0;
        int cnt = Used_Battle_Duration_Data_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var dur = Used_Battle_Duration_Data_List[i];
            if (dur.GetDurationEffectType() == dtype)
            {
                sum += dur.GetValuesByMultipleType();
            }
        }
        return sum;
    }

    /// <summary>
    /// 보유중인 스킬 레벨의 총 합
    /// </summary>
    /// <returns></returns>
    public int GetSkillLevelSum()
    {
        int sum = Skill_Groups.Sum(x => x.GetSkillLevel());
        return sum;
    }

}
