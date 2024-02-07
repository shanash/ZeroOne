public class BattlePcData : BattleUnitData
{
    public UserHeroData User_Data { get; private set; } = null;
    public Player_Character_Data Data { get { return User_Data.Hero_Data; } }
    public Player_Character_Battle_Data Battle_Data { get { return User_Data.Battle_Data; } }
    protected override int Level { get => User_Data.GetLevel(); set { } }

    public BattlePcData() : base(CHARACTER_TYPE.PC) { }

    public BattlePcData(BattlePcData battle_pc_data) : base(battle_pc_data.Character_Type)
    {
        User_Data = new UserHeroData(battle_pc_data.User_Data);
    }

    public override BattleUnitData SetUnitID(params int[] unit_ids)
    {
        if (unit_ids.Length < 2)
        {
            return null;
        }
        int pc_id = unit_ids[0];
        int pc_num = unit_ids[1];

        User_Data = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(pc_id, pc_num);

        return this;
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
        
    }

    public override int GetLevel()
    {
        return Level;
    }

    public override int GetStarGrade()
    {
        return User_Data.GetStarGrade();
    }

    public ERROR_CODE AdvanceStarGrade()
    {
        return User_Data.AdvanceStarGrade();
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
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.physics_attack + (GetLevel() - 1) * up_value;
        }
        return 0;
    }

    public override double GetMagicAttackPoint() => GetMagicAttackPoint(Battle_Data);
    double GetMagicAttackPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.magic_attack + (GetLevel() - 1) * up_value;
        }
        return 0;
    }

    public override double GetPhysicsDefensePoint() => GetPhysicsDefensePoint(Battle_Data);
    double GetPhysicsDefensePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.physics_defend + (GetLevel() - 1) * up_value;
        }
        return 0;
    }

    public override double GetMagicDefensePoint() => GetMagicDefensePoint(Battle_Data);
    double GetMagicDefensePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.magic_defend + (GetLevel() - 1) * up_value;
        }
        return 0;
    }

    public override double GetMaxLifePoint() => GetMaxLifePoint(Battle_Data);
    double GetMaxLifePoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.life + (GetLevel() - 1) * up_value;
        }
        return 0;
    }

    public override double GetAttackRecovery()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.attack_life_recovery;
        }
        return 0;
    }

    public override double GetAccuracyPoint() => GetAccuracyPoint(Battle_Data);
    double GetAccuracyPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.accuracy + (GetLevel() - 1) * up_value;
        }
        return 0;
    }

    public override double GetEvasionPoint() => GetEvasionPoint(Battle_Data);
    double GetEvasionPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            //TODO: 더미값을 임시로 곱해놓습니다.
            float up_value = 1f;
            return battle_data.evasion + (GetLevel() - 1) * up_value;
        }
        return 0;
    }
    public override double GetAutoRecoveryLife()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.auto_recovery;
        }
        return 0;
    }

    public override double GetVampirePoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.attack_life_recovery;
        }
        return 0;
    }

    public override double GetPhysicsCriticalChance() => GetPhysicsCriticalChance(Battle_Data);
    double GetPhysicsCriticalChance(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_chance;
        }
        return 0;
    }

    public override double GetPhysicsCriticalPowerAdd() => GetPhysicsCriticalPower(Battle_Data);
    double GetPhysicsCriticalPower(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_power_add;
        }
        return 0;
    }

    public override double GetMagicCriticalChance() => GetMagicCriticalChance(Battle_Data);
    double GetMagicCriticalChance(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_chance;
        }
        return 0;
    }

    public override double GetMagicCriticalPowerAdd() => GetMagicCriticalPower(Battle_Data);
    double GetMagicCriticalPower(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_power_add;
        }
        return 0;
    }

    public override double GetResistPoint() => GetResistPoint(Battle_Data);
    double GetResistPoint(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.resist;
        }
        return 0;
    }

    public override double GetLifeRecoveryInc() => GetLifeRecoveryInc(Battle_Data);
    double GetLifeRecoveryInc(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.heal;
        }
        return 0;
    }

    public override double GetWeight() => GetWeight(Battle_Data);
    double GetWeight(Player_Character_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.weight;
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
}
