using System;
using System.Diagnostics;

public class BattlePcData : BattleUnitData
{
    public UserHeroData User_Data { get; private set; } = null;
    public Player_Character_Data Data { get { return User_Data.Hero_Data; } }
    public Player_Character_Battle_Data Battle_Data { get { return User_Data.Battle_Data; } }

    public Player_Character_Level_Stat_Data Stat_Data { get; protected set; }

    protected override int Level { get => User_Data.GetLevel(); set { } }

    protected int _Essence_Founded_Percent = 0;
    protected int Essence_Founded_Percent
    {
        get
        {
            Update_Essence_Added_Stat();
            return _Essence_Founded_Percent;
        }
    }

    protected int Essence_Add_PhysicsAttackPoint { get; set; } = 0;
    protected int Essence_Add_MagicAttackPoint { get; set; } = 0;
    protected int Essence_Add_PhysicsDefensePoint { get; set; } = 0;
    protected int Essence_Add_MagicDefensePoint { get; set; } = 0;
    protected int Essence_Add_LifePoint { get; set; } = 0;


    public BattlePcData() : base(CHARACTER_TYPE.PC) { }

    public override BattleUnitData SetUnitID(params int[] unit_ids)
    {
        if (unit_ids.Length < 2)
        {
            return null;
        }
        int pc_id = unit_ids[0];
        int pc_num = unit_ids[1];

        User_Data = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(pc_id, pc_num);
        CheckStatData();
        CreateSkillManager();

        Init_Essence_Added_Stat();
        Update_Essence_Added_Stat();

        return this;
    }

    void CreateSkillManager()
    {
        if (Skill_Mng == null)
        {
            Skill_Mng = new BattleSkillManager();
            Skill_Mng.SetPlayerCharacterSkillGroups(User_Data, GetSkillPattern());
            Skill_Mng.SetPlayerCharacterSpecialSkillGroup(User_Data, GetSpecialSkillID());
            if (Hero != null)
            {
                Skill_Mng.SetHeroBase(Hero);
            }
        }
    }

    void Init_Essence_Added_Stat()
    {
        _Essence_Founded_Percent = 0;
        Essence_Add_PhysicsAttackPoint = 0;
        Essence_Add_MagicAttackPoint = 0;
        Essence_Add_PhysicsDefensePoint = 0;
        Essence_Add_MagicDefensePoint = 0;
        Essence_Add_LifePoint = 0;
    }

    void Update_Essence_Added_Stat()
    {
        if (_Essence_Founded_Percent > User_Data.Essence_Founded_Percent)
        {
            Init_Essence_Added_Stat();
        }

        if (!User_Data.Essence_Founded_Percent.Equals(_Essence_Founded_Percent))
        {
            var list = MasterDataManager.Instance.Get_EssenceStatusDataRange(_Essence_Founded_Percent + 1, User_Data.Essence_Founded_Percent);
            MasterDataManager.Instance.Sum_EssenceStatusList(list, out int add_phy_atk, out int add_mag_atk, out int add_phy_def, out int add_mag_def, out int add_hp);
            Essence_Add_PhysicsAttackPoint += add_phy_atk;
            Essence_Add_MagicAttackPoint += add_mag_atk;
            Essence_Add_PhysicsDefensePoint += add_phy_def;
            Essence_Add_MagicDefensePoint += add_mag_def;
            Essence_Add_LifePoint += add_hp;

            _Essence_Founded_Percent = User_Data.Essence_Founded_Percent;
        }
    }

    void CheckStatData()
    {
        if (Stat_Data == null)
        {
            Stat_Data = MasterDataManager.Instance.Get_PlayerCharacterLevelStatData(User_Data.GetPlayerCharacterID(), User_Data.GetStarGrade());
        }
    }

    public override int GetUnitID()
    {
        if (Data != null)
        {
            return Data.player_character_id;
        }
        return 0;
    }

    public override int GetUnitNum()
    {
        if (User_Data != null)
            return User_Data.Player_Character_Num;
        return 0;
    }

    public override object GetUnitData()
    {
        return Data;
    }

    public override float GetUnitScale()
    {
        return (float)Data.scale;
    }

    public override string GetIconPath()
    {
        return Data.icon_path;
    }

    public override object GetBattleData()
    {
        return Battle_Data;
    }
    public override ATTRIBUTE_TYPE GetAttributeType()
    {
        return Data.attribute_type;
    }
    public override void SetLevel(int lv)
    {
        //  nothing
    }

    public override int GetLevel()
    {
        return Level;
    }

    public override int GetStarGrade()
    {
        return User_Data.GetStarGrade();
    }

    public void AdvanceStarGrade(Action<RESPONSE_TYPE> callback)
    {
        RESPONSE_TYPE result = User_Data.AdvanceStarGrade();
        callback(result);
    }

    public RESPONSE_TYPE CheckAdvanceStarGrade()
    {
        return User_Data.CheckAdvanceStarGrade();
    }

    public override void SetStatDataID(int stat_id)
    {

    }

    public override object GetUserUnitData()
    {
        return User_Data;
    }

    #region Stats
    public override float GetMoveSpeed()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.move_speed;
        }
        return 0;
    }

    public override double GetTotalAttackPoint()
    {
        return GetPhysicsAttackPoint() + GetMagicAttackPoint();
    }
    public override double GetTotalDefensePoint()
    {
        return GetPhysicsDefensePoint() + GetMagicDefensePoint();
    }

    public override double GetPhysicsAttackPoint() => GetPhysicsAttackPoint(Battle_Data);
    double GetPhysicsAttackPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.physics_attack + (GetLevel() - 1) * Stat_Data.physics_attack;
            //  team synergy
            double team_synergy_inc = GetTeamAttributeAddPoint(STAT_MULTIPLE_TYPE.ATTACK_RATE);
            if (team_synergy_inc > 0)
            {
                point += point * team_synergy_inc;
            }

            //  지속성 효과 (공격력 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_ATTACK_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_ATTACK_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            // 근원전달 포인트 추가
            point += Essence_Add_PhysicsAttackPoint;

            return point;
        }
        return 0;
    }

    public override double GetMagicAttackPoint() => GetMagicAttackPoint(Battle_Data);
    double GetMagicAttackPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.magic_attack + (GetLevel() - 1) * Stat_Data.magic_attack;
            //  team synergy
            double team_synergy_inc = GetTeamAttributeAddPoint(STAT_MULTIPLE_TYPE.ATTACK_RATE);
            if (team_synergy_inc > 0)
            {
                point += point * team_synergy_inc;
            }
            //  지속성 효과 (공격력 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_ATTACK_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_ATTACK_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            // 근원전달 포인트 추가
            point += Essence_Add_MagicAttackPoint;

            return point;
        }
        return 0;
    }

    public override double GetPhysicsDefensePoint() => GetPhysicsDefensePoint(Battle_Data);
    double GetPhysicsDefensePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.physics_defend + (GetLevel() - 1) * Stat_Data.physics_defend;
            //  지속성 효과 (물리 방어력 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_DEFEND_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_DEFEND_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            // 근원전달 포인트 추가
            point += Essence_Add_PhysicsDefensePoint;

            return point;
        }
        return 0;
    }

    public override double GetMagicDefensePoint() => GetMagicDefensePoint(Battle_Data);
    double GetMagicDefensePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.magic_defend + (GetLevel() - 1) * Stat_Data.magic_defend;
            //  지속성 효과 (마법 방어력 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_DEFEND_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_DEFEND_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            // 근원전달 포인트 추가
            point += Essence_Add_MagicDefensePoint;

            return point;
        }
        return 0;
    }

    public override double GetMaxLifePoint() => GetMaxLifePoint(Battle_Data);
    double GetMaxLifePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.life + (GetLevel() - 1) * Stat_Data.life;
            return point;
        }
        return 0;
    }

    public override double GetAttackLifeRecovery() => GetAttackLifeRecovery(Battle_Data);
    double GetAttackLifeRecovery(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.attack_life_recovery * (GetLevel() - 1) * Stat_Data.attack_life_recovery;
            //  지속성 효과 (공격후 흡혈 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.ATTACK_LIFE_RECOVERY_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.ATTACK_LIFE_RECOVERY_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }
            return point;
        }
        return 0;
    }

    public override double GetAccuracyPoint() => GetAccuracyPoint(Battle_Data);
    double GetAccuracyPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.accuracy + (GetLevel() - 1) * Stat_Data.accuracy;
            //  지속성 효과 (명중 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.ACCURACY_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.ACCURACY_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            return point;
        }
        return 0;
    }

    public override double GetEvasionPoint() => GetEvasionPoint(Battle_Data);
    double GetEvasionPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.evasion + (GetLevel() - 1) * Stat_Data.evasion;
            //  지속성 효과 (회피 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.EVASION_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.EVASION_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }
            return point;
        }
        return 0;
    }
    public override double GetAutoRecoveryLife() => GetAutoRecoveryLife(Battle_Data);
    double GetAutoRecoveryLife(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.auto_recovery + (GetLevel() - 1) * Stat_Data.auto_recovery;
            return point;
        }
        return 0;
    }


    public override double GetPhysicsCriticalChance() => GetPhysicsCriticalChance(Battle_Data);
    double GetPhysicsCriticalChance(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.physics_critical_chance + (GetLevel() - 1) * Stat_Data.physics_critical_chance;
            //  지속성 효과 (물리 치명타 확률 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_CRITICAL_CHANCE_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_CRITICAL_CHANCE_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }
            return point;
        }
        return 0;
    }

    public override double GetPhysicsCriticalPowerAdd() => GetPhysicsCriticalPower(Battle_Data);
    double GetPhysicsCriticalPower(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.physics_critical_power_add + (GetLevel() - 1) * Stat_Data.physics_critical_power_add;
            //  지속성 효과 (물리 치명타 데미지 추가 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_CRITICAL_POWER_ADD_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.PHYSICS_CRITICAL_POWER_ADD_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }
            return point;
        }
        return 0;
    }

    public override double GetMagicCriticalChance() => GetMagicCriticalChance(Battle_Data);
    double GetMagicCriticalChance(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.magic_critical_chance + (GetLevel() - 1) * Stat_Data.magic_critical_chance;
            //  지속성 효과 (마법 치명타 확률 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_CRITICAL_CHANCE_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_CRITICAL_CHANCE_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            return point;
        }
        return 0;
    }

    public override double GetMagicCriticalPowerAdd() => GetMagicCriticalPower(Battle_Data);
    double GetMagicCriticalPower(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.magic_critical_power_add + (GetLevel() - 1) * Stat_Data.magic_critical_power_add;
            //  지속성 효과 (마법 치명타 확률 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_CRITICAL_POWER_ADD_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.MAGIC_CRITICAL_POWER_ADD_UP);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            return point;
        }
        return 0;
    }

    public override double GetResistPoint() => GetResistPoint(Battle_Data);
    double GetResistPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.resist + (GetLevel() - 1) * Stat_Data.resist;
            //  지속성 효과에 강인함 증가/감소 효과가 없음 [todo] 
            return point;
        }
        return 0;
    }

    public override double GetLifeRecoveryInc() => GetLifeRecoveryInc(Battle_Data);
    double GetLifeRecoveryInc(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.heal + (GetLevel() - 1) * Stat_Data.heal;
            //  지속성 효과 (힐량 증가/감소)
            double dur_inc = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.HEAL_UP);
            double dur_dec = Skill_Mng.GetDurationMultiplesByDurEffectType(DURATION_EFFECT_TYPE.HEAL_DOWN);
            if (dur_inc - dur_dec != 0)
            {
                point += point * (dur_inc - dur_dec);
            }

            return point;
        }
        return 0;
    }

    public override double GetWeight() => GetWeight(Battle_Data);
    double GetWeight(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.weight + (GetLevel() - 1) * Stat_Data.weight;
            return point;
        }
        return 0;
    }

    public override int GetSumSkillsLevel() => Skill_Mng.GetSkillLevelSum();

    public override float GetApproachDistance()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.approach;
        }
        return 0;
    }
    #endregion



    public override int[] GetSkillPattern()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.skill_pattern;
        }
        return null;
    }

    public override int GetSpecialSkillID()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.special_skill_group_id;
        }
        return 0;
    }

    public override POSITION_TYPE GetPositionType()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.position_type;
        }
        return POSITION_TYPE.NONE;
    }

    public override string GetPrefabPath()
    {
        if (Data != null)
        {
            return Data.prefab_path;
        }
        return null;
    }

    public override string GetThumbnailPath()
    {
        if (Data != null)
        {
            return Data.icon_path;
        }
        return null;
    }

    public override string GetUnitName()
    {
        if (Data != null)
            return GameDefine.GetLocalizeString(Data.name_id);
        return null;
    }
    public override TRIBE_TYPE GetTribeType()
    {
        if (Data != null)
        {
            return Data.tribe_type;
        }
        return TRIBE_TYPE.NONE;
    }
    public override NPC_TYPE GetNpctype()
    {
        return NPC_TYPE.NONE;
    }

    public BattlePcData GetNextStarGradeData()
    {
        var clone = (BattlePcData)this.Clone();
        var result = clone.User_Data.TryUpNextStarGrade();
        if (result != RESPONSE_TYPE.SUCCESS)
        {
            Debug.Assert(false, $"성급 진화에 실패했습니다 : {result}");
            return null;
        }
        return clone;
    }

    /// <summary>
    /// 객체를 복사한 후, 해당 객체의 레벨을 지정.<br/>
    /// 변경된 스탯 값을 가져올 수 있다.
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    public BattlePcData GetSimulateLevelUpData(int lv)
    {
        var clone = (BattlePcData)this.Clone();
        clone.User_Data.SetLevel(lv);
        return clone;
    }

    public override object Clone()
    {
        BattlePcData clone = (BattlePcData)this.MemberwiseClone();
        clone.User_Data = (UserHeroData)User_Data.Clone();
        return clone;
    }
}
