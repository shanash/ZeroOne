
using FluffyDuck.Util;
using System;
using UnityEngine;

public class BattleSkillDataFactory
{
    /// <summary>
    /// 플레이어 캐릭터의 일회성 효과 스킬 데이터를 각 타입에 맞는 클래스로 생성, 반환한다
    /// </summary>
    /// <param name="pc_onetime_skill_id"></param>
    /// <returns></returns>
    public static BattleOnetimeSkillData CreatePcBattleOnetimeSkillData(int pc_onetime_skill_id)
    {
        var data = MasterDataManager.Instance.Get_PlayerCharacterSkillOnetimeData(pc_onetime_skill_id);
        return data != null ? CreateBattleOnetimeSkillData(data) : null;
    }

    /// <summary>
    /// NPC(몬스터)의 일회성 효과 스킬 데이터 생성 반환
    /// </summary>
    /// <param name="npc_onetime_skill_id"></param>
    /// <returns></returns>
    public static BattleOnetimeSkillData CreateNpcBattleOnetimeSkillData(int npc_onetime_skill_id)
    {
        var data = MasterDataManager.Instance.Get_NpcSkillOnetimeData(npc_onetime_skill_id);
        return data != null ? CreateBattleOnetimeSkillData(data) : null;
    }

    /// <summary>
    /// 플레이어 캐릭터의 지속성 효과 스킬 데이터 생성 반환
    /// </summary>
    /// <param name="pc_duration_skill_id"></param>
    /// <returns></returns>
    public static BattleDurationSkillData CreatePcBattleDurationSkillData(int pc_duration_skill_id)
    {
        var data = MasterDataManager.Instance.Get_PlayerCharacterSkillDurationData(pc_duration_skill_id);
        return data != null ? CreateBattleDurationSkillData(data) : null;
    }

    /// <summary>
    /// NPC (몬스터)의 지속성 효과 스킬 데이터 생성 반환
    /// </summary>
    /// <param name="npc_duration_skill_id"></param>
    /// <returns></returns>
    public static BattleDurationSkillData CreateNpcBattleDurationSkillData(int npc_duration_skill_id)
    {
        var data = MasterDataManager.Instance.Get_NpcSkillDurationData(npc_duration_skill_id);
        return data != null ? CreateBattleDurationSkillData(data) : null;
    }

    /// <summary>
    /// 일회성 효과 스킬데이터 생성
    /// </summary>
    /// <param name="data">Json에서 파싱된 PC or NPC 스킬데이터</param>
    /// <returns>가공한 스킬데이터</returns>
    static BattleOnetimeSkillData CreateBattleOnetimeSkillData(object data)
    {
        Factory.AddTypeMapping<BattleOnetimeSkillData>(objs =>
        {
            if (objs[0] is Player_Character_Skill_Onetime_Data pc_skill)
            {
                switch (pc_skill.onetime_effect_type)
                {
                    case ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE:
                        return typeof(BattlePcOnetimeSkillData_PhysicsDamage);
                    case ONETIME_EFFECT_TYPE.MAGIC_DAMAGE:
                        return typeof(BattlePcOnetimeSkillData_MagicDamage);
                    case ONETIME_EFFECT_TYPE.LIFE_RECOVERY:
                        return typeof(BattlePcOnetimeSkillData_LifeRecovery);
                    case ONETIME_EFFECT_TYPE.NONE_EFFECT:
                        return typeof(BattlePcOnetimeSkillData);
                    default:
                        Debug.Assert(false, "PC 일회성 스킬데이터가 정상적이지 않습니다!!!");
                        return typeof(BattlePcOnetimeSkillData);
                }
            }
            else if (objs[0] is Npc_Skill_Onetime_Data npc_skill)
            {
                switch (npc_skill.onetime_effect_type)
                {
                    case ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE:
                        return typeof(BattleNpcOnetimeSkillData_PhysicsDamage);
                    case ONETIME_EFFECT_TYPE.MAGIC_DAMAGE:
                        return typeof(BattleNpcOnetimeSkillData_MagicDamage);
                    case ONETIME_EFFECT_TYPE.LIFE_RECOVERY:
                        return typeof(BattleNpcOnetimeSkillData_LifeRecovery);
                    case ONETIME_EFFECT_TYPE.NONE_EFFECT:
                        return typeof(BattleNpcOnetimeSkillData);
                    default:
                        Debug.Assert(false, "NPC 일회성 스킬데이터가 정상적이지 않습니다!!!");
                        return typeof(BattleNpcOnetimeSkillData);
                }
            }

            Debug.Assert(false, "Json 파싱된 일회성 스킬데이터가 정상적으로 입력되지 않았습니다!!!");
            return default(Type);
        });

        return Factory.Create<BattleOnetimeSkillData>(data);
    }

    /// <summary>
    /// 버프 스킬데이터 생성
    /// </summary>
    /// <param name="data">Json에서 파싱된 PC or NPC 버프 스킬 데이터</param>
    /// <returns>가공된 버프 스킬 데이터</returns>
    static BattleDurationSkillData CreateBattleDurationSkillData(object data)
    {
        Factory.AddTypeMapping<BattleDurationSkillData>(objs =>
        {
            if (objs[0] is Player_Character_Skill_Duration_Data pc_skill)
            {
                switch (pc_skill.duration_effect_type)
                {
                    case DURATION_EFFECT_TYPE.DAMAGE_REDUCE:
                        return typeof(BattlePcDurationSkillData_DamageReduce);
                    case DURATION_EFFECT_TYPE.POISON:
                        return typeof(BattlePcDurationSkillData_Poison);
                    case DURATION_EFFECT_TYPE.STUN:
                        return typeof(BattlePcDurationSkillData_Stun);
                    case DURATION_EFFECT_TYPE.SILENCE:
                        return typeof(BattlePcDurationSkillData_Silence);
                    case DURATION_EFFECT_TYPE.BIND:
                        return typeof(BattlePcDurationSkillData_Bind);
                    case DURATION_EFFECT_TYPE.FREEZE:
                        return typeof(BattlePcDurationSkillData_Freeze);
                    case DURATION_EFFECT_TYPE.PHYSICS_ATTACK_UP:
                        return typeof(BattlePcDurationSkillData_AttackUp);
                    case DURATION_EFFECT_TYPE.PHYSICS_DEFEND_UP:
                        return typeof(BattlePcDurationSkillData_DefenseUp);
                    case DURATION_EFFECT_TYPE.PHYSICS_ATTACK_DOWN:
                        return typeof(BattlePcDurationSkillData_AttackDown);
                    case DURATION_EFFECT_TYPE.PHYSICS_DEFEND_DOWN:
                        return typeof(BattlePcDurationSkillData_DefenseDown);
                    default:
                        Debug.Assert(false, "PC 버프 스킬데이터가 정상적이지 않습니다!!!");
                        return typeof(BattlePcDurationSkillData);
                }
            }
            else if (objs[0] is Npc_Skill_Duration_Data npc_skill)
            {
                switch (npc_skill.duration_effect_type)
                {
                    case DURATION_EFFECT_TYPE.DAMAGE_REDUCE:
                        return typeof(BattleNpcDurationSkillData_DamageReduce);
                    case DURATION_EFFECT_TYPE.POISON:
                        return typeof(BattleNpcDurationSkillData_Poison);
                    case DURATION_EFFECT_TYPE.STUN:
                        return typeof(BattleNpcDurationSkillData_Stun);
                    case DURATION_EFFECT_TYPE.SILENCE:
                        return typeof(BattleNpcDurationSkillData_Silence);
                    case DURATION_EFFECT_TYPE.BIND:
                        return typeof(BattleNpcDurationSkillData_Bind);
                    case DURATION_EFFECT_TYPE.FREEZE:
                        return typeof(BattleNpcDurationSkillData_Freeze);
                    case DURATION_EFFECT_TYPE.PHYSICS_ATTACK_UP:
                        return typeof(BattleNpcDurationSkillData_AttackUp);
                    case DURATION_EFFECT_TYPE.PHYSICS_DEFEND_UP:
                        return typeof(BattleNpcDurationSkillData_DefenseUp);
                    case DURATION_EFFECT_TYPE.PHYSICS_ATTACK_DOWN:
                        return typeof(BattleNpcDurationSkillData_AttackDown);
                    case DURATION_EFFECT_TYPE.PHYSICS_DEFEND_DOWN:
                        return typeof(BattleNpcDurationSkillData_DefenseDown);
                    default:
                        Debug.Assert(false, "NPC 버프 스킬데이터가 정상적이지 않습니다!!!");
                        return typeof(BattleNpcDurationSkillData);
                }
            }

            Debug.Assert(false, "Json 파싱된 버프 스킬데이터가 정상적으로 입력되지 않았습니다!!!");
            return default(Type);
        });

        return Factory.Create<BattleDurationSkillData>(data);
    }
}
