using System;
using System.Diagnostics;

public class BattlePcData : BattleUnitData
{
    public UserHeroData User_Data { get; private set; } = null;
    public Player_Character_Data Data { get { return User_Data.Hero_Data; } }
    public Player_Character_Battle_Data Battle_Data { get { return User_Data.Battle_Data; } }

    public Player_Character_Level_Stat_Data Stat_Data { get; protected set; }

    protected override int Level { get => User_Data.GetLevel(); set { } }

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
        return this;
    }

    void CreateSkillManager()
    {
        if (Skill_Mng == null)
        {
            Skill_Mng = new BattleSkillManager();
            Skill_Mng.SetPlayerCharacterSkillGroups(GetSkillPattern());
            Skill_Mng.SetPlayerCharacterSpecialSkillGroup(GetSpecialSkillID());
            if (Hero != null)
            {
                Skill_Mng.SetHeroBase(Hero);
            }
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

    public override object GetBattleData()
    {
        return Battle_Data;
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

    public void AdvanceStarGrade(Action<ERROR_CODE> callback)
    {
        ERROR_CODE result = User_Data.AdvanceStarGrade();
        callback(result);
    }

    public ERROR_CODE CheckAdvanceStarGrade()
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

    public override double GetPhysicsAttackPoint() => GetPhysicsAttackPoint(Battle_Data);
    double GetPhysicsAttackPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_attack + (GetLevel() - 1) * Stat_Data.physics_attack;
        }
        return 0;
    }

    public override double GetMagicAttackPoint() => GetMagicAttackPoint(Battle_Data);
    double GetMagicAttackPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            return battle_data.magic_attack + (GetLevel() - 1) * Stat_Data.magic_attack;
        }
        return 0;
    }

    public override double GetPhysicsDefensePoint() => GetPhysicsDefensePoint(Battle_Data);
    double GetPhysicsDefensePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            return battle_data.physics_defend + (GetLevel() - 1) * Stat_Data.physics_defend;
        }
        return 0;
    }

    public override double GetMagicDefensePoint() => GetMagicDefensePoint(Battle_Data);
    double GetMagicDefensePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            return battle_data.magic_defend + (GetLevel() - 1) * Stat_Data.magic_defend;
        }
        return 0;
    }

    public override double GetMaxLifePoint() => GetMaxLifePoint(Battle_Data);
    double GetMaxLifePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            return battle_data.life + (GetLevel() - 1) * Stat_Data.life;
        }
        return 0;
    }

    public override double GetAttackLifeRecovery() => GetAttackLifeRecovery(Battle_Data);
    double GetAttackLifeRecovery(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.attack_life_recovery * (GetLevel() - 1) * Stat_Data.attack_life_recovery;
        }
        return 0;
    }

    public override double GetAccuracyPoint() => GetAccuracyPoint(Battle_Data);
    double GetAccuracyPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.accuracy + (GetLevel() - 1) * Stat_Data.accuracy;
        }
        return 0;
    }

    public override double GetEvasionPoint() => GetEvasionPoint(Battle_Data);
    double GetEvasionPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.evasion + (GetLevel() - 1) * Stat_Data.evasion;
        }
        return 0;
    }
    public override double GetAutoRecoveryLife() => GetAutoRecoveryLife(Battle_Data);
    double GetAutoRecoveryLife(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.auto_recovery + (GetLevel() - 1) * Stat_Data.auto_recovery;
        }
        return 0;
    }


    public override double GetPhysicsCriticalChance() => GetPhysicsCriticalChance(Battle_Data);
    double GetPhysicsCriticalChance(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_chance + (GetLevel() - 1) * Stat_Data.physics_critical_chance;
        }
        return 0;
    }

    public override double GetPhysicsCriticalPowerAdd() => GetPhysicsCriticalPower(Battle_Data);
    double GetPhysicsCriticalPower(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_power_add + (GetLevel() - 1) * Stat_Data.physics_critical_power_add;
        }
        return 0;
    }

    public override double GetMagicCriticalChance() => GetMagicCriticalChance(Battle_Data);
    double GetMagicCriticalChance(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_chance + (GetLevel() - 1) * Stat_Data.magic_critical_chance;
        }
        return 0;
    }

    public override double GetMagicCriticalPowerAdd() => GetMagicCriticalPower(Battle_Data);
    double GetMagicCriticalPower(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_power_add + (GetLevel() - 1) * Stat_Data.magic_critical_power_add;
        }
        return 0;
    }

    public override double GetResistPoint() => GetResistPoint(Battle_Data);
    double GetResistPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.resist + (GetLevel() - 1) * Stat_Data.resist;
        }
        return 0;
    }

    public override double GetLifeRecoveryInc() => GetLifeRecoveryInc(Battle_Data);
    double GetLifeRecoveryInc(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.heal + (GetLevel() - 1) * Stat_Data.heal;
        }
        return 0;
    }

    public override double GetWeight() => GetWeight(Battle_Data);
    double GetWeight(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.weight + (GetLevel() - 1) * Stat_Data.weight;
        }
        return 0;
    }

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
            return Data.name_kr;
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
        if (result != ERROR_CODE.SUCCESS)
        {
            Debug.Assert(false, $"성급 진화에 실패했습니다 : {result}");
            return null;
        }
        return clone;
    }

    public override object Clone()
    {
        BattlePcData clone = (BattlePcData)this.MemberwiseClone();
        clone.User_Data = (UserHeroData)User_Data.Clone();
        return clone;
    }
}
