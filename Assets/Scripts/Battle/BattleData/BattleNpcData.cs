

public class BattleNpcData : BattleUnitData
{
    protected override int Level { get; set; }

    Npc_Data Data;

    Npc_Battle_Data Battle_Data;

    Npc_Level_Stat_Data Stat_Data;

    public BattleNpcData() : base(CHARACTER_TYPE.NPC) { }


    public override BattleUnitData SetUnitID(params int[] unit_ids)
    {
        if (unit_ids.Length != 1)
        {
            return null;
        }
        int npc_id = unit_ids[0];
        var m = MasterDataManager.Instance;
        Data = m.Get_NpcData(npc_id);

        Battle_Data = m.Get_NpcBattleData(Data.npc_battle_id);

        return this;
    }

    public override object GetUnitData()
    {
        return Data;
    }

    public override object GetBattleData()
    {
        return Battle_Data;
    }

    public override int GetUnitID()
    {
        if (Data != null)
        {
            return Data.npc_data_id;
        }
        return 0;
    }

    public override void SetLevel(int lv)
    {
        Level = lv;
    }
    public override int GetLevel()
    {
        return Level;
    }
    public override int GetStarGrade()
    {
        return 1;
    }
    public override void SetStatDataID(int stat_id)
    {
        Stat_Data = MasterDataManager.Instance.Get_NpcLevelStatData(stat_id);
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
    double GetPhysicsAttackPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_attack;
        }
        return 0;
    }

    public override double GetMagicAttackPoint() => GetMagicAttackPoint(Battle_Data);
    double GetMagicAttackPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_attack;
        }
        return 0;
    }

    public override double GetPhysicsDefensePoint() => GetPhysicsDefensePoint(Battle_Data);
    double GetPhysicsDefensePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_defend;
        }
        return 0;
    }

    public override double GetMagicDefensePoint() => GetMagicDefensePoint(Battle_Data);
    double GetMagicDefensePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_defend;
        }
        return 0;
    }
    
    public override double GetLifePoint() => GetLifePoint(Battle_Data);
    double GetLifePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.life;
        }
        return 0;
    }

    public override double GetAttackRecovery() => GetAttackRecovery(Battle_Data);
    double GetAttackRecovery(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.attack_life_recovery;
        }
        return 0;
    }


    public override double GetAccuracyPoint() => GetAccuracyPoint(Battle_Data);
    double GetAccuracyPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.accuracy;
        }
        return 0;
    }

    public override double GetEvasionPoint() => GetEvasionPoint(Battle_Data);
    double GetEvasionPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.evasion;
        }
        return 0;
    }

    public override double GetVampirePoint() => GetVampirePoint(Battle_Data);
    double GetVampirePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.attack_life_recovery;
        }
        return 0;
    }
    public override double GetPhysicsCriticalChance() => GetPhysicsCriticalChance(Battle_Data);
    double GetPhysicsCriticalChance(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_chance;
        }
        return 0;
    }
    public override double GetPhysicsCriticalPower() => GetPhysicsCriticalPower(Battle_Data);
    double GetPhysicsCriticalPower(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_power_add;
        }
        return 0;
    }
    public override double GetMagicCriticalChance() => GetMagicCriticalChance(Battle_Data);
    double GetMagicCriticalChance(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_chance;
        }
        return 0;
    }
    public override double GetMagicCriticalPower() => GetMagicCriticalPower(Battle_Data);
    double GetMagicCriticalPower(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_power_add;
        }
        return 0;
    }
    public override double GetResistPoint() => GetResistPoint(Battle_Data);
    double GetResistPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.resist;
        }
        return 0;
    }
    public override double GetLifeRecoveryInc() => GetLifeRecoveryInc(Battle_Data);
    double GetLifeRecoveryInc(Npc_Battle_Data battle_data) 
    { 
        if (battle_data != null)
        {
            return battle_data.heal;
        }
        return 0; 
    }
    public override double GetWeight() => GetWeight(Battle_Data);
    double GetWeight(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
            return battle_data.weight;
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
            return Battle_Data.position_type;
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
        if (Data != null)
            return Data.npc_type;
        return NPC_TYPE.NONE;
    }

}
