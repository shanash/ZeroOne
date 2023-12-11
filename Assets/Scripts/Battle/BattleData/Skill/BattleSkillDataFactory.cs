
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
        if (data != null)
        {
            BattleOnetimeSkillData onetime_skill = null;
            switch (data.onetime_effect_type)
            {
                case ONETIME_EFFECT_TYPE.DAMAGE:
                    onetime_skill = new BattlePcOnetimeSkillData_Damage();
                    break;
                case ONETIME_EFFECT_TYPE.LIFE_RECOVERY:
                    onetime_skill = new BattlePcOnetimeSkillData_LifeRecovery();
                    break;
                default:
                    UnityEngine.Debug.Assert(false);
                    onetime_skill = new BattlePcOnetimeSkillData();
                    break;
            }
            onetime_skill?.SetOnetimeSkillDataID(pc_onetime_skill_id);
            return onetime_skill;
        }
        return null;
    }

    /// <summary>
    /// NPC(몬스터)의 일회성 효과 스킬 데이터 생성 반환
    /// </summary>
    /// <param name="npc_onetime_skill_id"></param>
    /// <returns></returns>
    public static BattleOnetimeSkillData CreateNpcBattleOnetimeSkillData(int npc_onetime_skill_id)
    {
        var data = MasterDataManager.Instance.Get_NpcSkillOnetimeData(npc_onetime_skill_id);
        if (data != null)
        {
            BattleOnetimeSkillData onetime_skill = null;
            switch (data.onetime_effect_type)
            {
                case ONETIME_EFFECT_TYPE.DAMAGE:
                    onetime_skill = new BattleNpcOnetimeSkillData_Damage();
                    break;
                case ONETIME_EFFECT_TYPE.LIFE_RECOVERY:
                    onetime_skill = new BattleNpcOnetimeSkillData_LifeRecovery();
                    break;
                    
                default:
                    onetime_skill = new BattleNpcOnetimeSkillData();
                    UnityEngine.Debug.Assert(false);
                    break;
            }
            onetime_skill?.SetOnetimeSkillDataID(npc_onetime_skill_id);
            return onetime_skill;
        }
        return null;
    }

    /// <summary>
    /// 플레이어 캐릭터의 지속성 효과 스킬 데이터 생성 반환
    /// </summary>
    /// <param name="pc_duration_skill_id"></param>
    /// <returns></returns>
    public static BattleDurationSkillData CreatePcBattleDurationSkillData(int pc_duration_skill_id)
    {
        var data = MasterDataManager.Instance.Get_PlayerCharacterSkillDurationData(pc_duration_skill_id);
        if (data != null)
        {
            BattleDurationSkillData duration_skill = null;
            switch (data.duration_effect_type)
            {
                case DURATION_EFFECT_TYPE.DAMAGE_REDUCE:
                    duration_skill = new BattlePcDurationSkillData_DamageReduce();
                    break;
                case DURATION_EFFECT_TYPE.POISON:
                    duration_skill = new BattlePcDurationSkillData_Poison();
                    break;
                case DURATION_EFFECT_TYPE.STUN:
                    duration_skill = new BattlePcDurationSkillData_Stun();
                    break;
                case DURATION_EFFECT_TYPE.SILENCE:
                    duration_skill = new BattlePcDurationSkillData_Silence();
                    break;
                case DURATION_EFFECT_TYPE.BIND:
                    duration_skill = new BattlePcDurationSkillData_Bind();
                    break;
                case DURATION_EFFECT_TYPE.FREEZE:
                    duration_skill = new BattlePcDurationSkillData_Freeze();
                    break;
                case DURATION_EFFECT_TYPE.ATK_UP:
                    duration_skill = new BattlePcDurationSkillData_AttackUp();
                    break;
                case DURATION_EFFECT_TYPE.DEF_UP:
                    duration_skill = new BattlePcDurationSkillData_DefenseUp();
                    break;
                case DURATION_EFFECT_TYPE.ATK_DOWN:
                    duration_skill = new BattlePcDurationSkillData_AttackDown();
                    break;
                case DURATION_EFFECT_TYPE.DEF_DOWN:
                    duration_skill = new BattlePcDurationSkillData_DefenseDown();
                    break;
                default:
                    UnityEngine.Debug.Assert(false);
                    duration_skill = new BattlePcDurationSkillData();
                    break;
            }
            duration_skill?.SetDurationSkillDataID(pc_duration_skill_id);
            return duration_skill;
        }
        return null;
    }

    /// <summary>
    /// NPC (몬스터)의 지속성 효과 스킬 데이터 생성 반환
    /// </summary>
    /// <param name="npc_duration_skill_id"></param>
    /// <returns></returns>
    public static BattleDurationSkillData CreateNpcBattleDurationSkillData(int npc_duration_skill_id)
    {
        var data = MasterDataManager.Instance.Get_NpcSkillDurationData(npc_duration_skill_id);
        if (data != null)
        {
            BattleDurationSkillData duration_skill = null;
            switch (data.duration_effect_type)
            {
                case DURATION_EFFECT_TYPE.DAMAGE_REDUCE:
                    duration_skill = new BattleNpcDurationSkillData_DamageReduce();
                    break;
                case DURATION_EFFECT_TYPE.POISON:
                    duration_skill = new BattleNpcDurationSkillData_Poison();
                    break;
                case DURATION_EFFECT_TYPE.STUN:
                    duration_skill = new BattleNpcDurationSkillData_Stun();
                    break;
                case DURATION_EFFECT_TYPE.SILENCE:
                    duration_skill = new BattleNpcDurationSkillData_Silence();
                    break;
                case DURATION_EFFECT_TYPE.BIND:
                    duration_skill = new BattleNpcDurationSkillData_Bind();
                    break;
                case DURATION_EFFECT_TYPE.FREEZE:
                    duration_skill = new BattleNpcDurationSkillData_Freeze();
                    break;
                case DURATION_EFFECT_TYPE.ATK_UP:
                    duration_skill = new BattleNpcDurationSkillData_AttackUp();
                    break;
                case DURATION_EFFECT_TYPE.ATK_DOWN:
                    duration_skill = new BattleNpcDurationSkillData_AttackDown();
                    break;
                case DURATION_EFFECT_TYPE.DEF_UP:
                    duration_skill = new BattleNpcDurationSkillData_DefenseUp();
                    break;
                case DURATION_EFFECT_TYPE.DEF_DOWN:
                    duration_skill = new BattleNpcDurationSkillData_DefenseDown();
                    break;
                default:
                    UnityEngine.Debug.Assert(false);
                    duration_skill = new BattleNpcDurationSkillData();
                    break;
            }
            duration_skill?.SetDurationSkillDataID(npc_duration_skill_id);
            return duration_skill;
        }
        return null;
    }
}
