

using FluffyDuck.Util;

public class BattleNpcData : BattleUnitData
{
    protected override int Level { get; set; }

    Npc_Data Data;

    Npc_Battle_Data Battle_Data;

    Npc_Level_Stat_Data Stat_Data;

    public BattleNpcData() : base(CHARACTER_TYPE.NPC) { }

    /// <summary>
    /// npc id, npc lv, stat id, skill lv, ultimate lv
    /// </summary>
    /// <param name="unit_ids"></param>
    /// <returns></returns>
    public override BattleUnitData SetUnitID(params int[] unit_ids)
    {
        if (unit_ids.Length != 5)
        {
            return null;
        }
        int npc_id = unit_ids[0];
        int lv = unit_ids[1];
        int stat_id = unit_ids[2];
        int skill_lv = unit_ids[3];
        int ultimate_skill_lv = unit_ids[4];

        var m = MasterDataManager.Instance;
        Data = m.Get_NpcData(npc_id);

        Battle_Data = m.Get_NpcBattleData(Data.npc_battle_id);
        SetLevel(lv);
        SetStatDataID(stat_id);

        CreateSkillManager(skill_lv, ultimate_skill_lv);

        return this;
    }

    void CreateSkillManager(int skill_lv, int ultimate_lv)
    {
        if (Skill_Mng == null)
        {
            Skill_Mng = new BattleSkillManager();
            Skill_Mng.SetNpcSkillGroups(GetSkillPattern(), skill_lv);
            Skill_Mng.SetNpcSpecialSkillGroup(GetSpecialSkillID(), ultimate_lv);
            if (Hero != null)
            {
                Skill_Mng.SetHeroBase(Hero);
            }
        }
    }

    public override object GetUnitData()
    {
        return Data;
    }
    public override float GetUnitScale()
    {
        return (float)Data.scale;
    }

    public override object GetBattleData()
    {
        return Battle_Data;
    }

    public override string GetIconPath()
    {
        return Data.icon_path;
    }

    public override int GetUnitID()
    {
        if (Data != null)
        {
            return Data.npc_data_id;
        }
        return 0;
    }
    public override ATTRIBUTE_TYPE GetAttributeType()
    {
        return Data.attribute_type;
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
            return battle_data.physics_attack + (GetLevel() - 1) * Stat_Data.physics_attack;
        }
        return 0;
    }

    public override double GetMagicAttackPoint() => GetMagicAttackPoint(Battle_Data);
    double GetMagicAttackPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_attack + (GetLevel() - 1) * Stat_Data.magic_attack;
        }
        return 0;
    }

    public override double GetPhysicsDefensePoint() => GetPhysicsDefensePoint(Battle_Data);
    double GetPhysicsDefensePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.physics_defend + (GetLevel() - 1) * Stat_Data.physics_defend;

            //  치트(적 방어력 증가)
            double cheat_multiple = BlackBoard.Instance.GetBlackBoardData<int>(BLACK_BOARD_KEY.ENEMY_DEFENSE_INC_MULTIPLE, 1);
            if (cheat_multiple > 1)
            {
                point *= cheat_multiple;
            }
            return point;
        }
        return 0;
    }

    public override double GetMagicDefensePoint() => GetMagicDefensePoint(Battle_Data);
    double GetMagicDefensePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            double point = battle_data.magic_defend + (GetLevel() - 1) * Stat_Data.magic_defend;
            //  치트(적 방어력 증가)
            double cheat_multiple = BlackBoard.Instance.GetBlackBoardData<int>(BLACK_BOARD_KEY.ENEMY_DEFENSE_INC_MULTIPLE, 1);
            if (cheat_multiple > 1)
            {
                point *= cheat_multiple;
            }

            return point;
        }
        return 0;
    }
    
    public override double GetMaxLifePoint() => GetLifePoint(Battle_Data);
    double GetLifePoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.life + (GetLevel() - 1) * Stat_Data.life;
        }
        return 0;
    }

    public override double GetAttackLifeRecovery() => GetAttackLifeRecovery(Battle_Data);
    double GetAttackLifeRecovery(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.attack_life_recovery + (GetLevel() - 1) * Stat_Data.attack_life_recovery;
        }
        return 0;
    }


    public override double GetAccuracyPoint() => GetAccuracyPoint(Battle_Data);
    double GetAccuracyPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.accuracy + (GetLevel() - 1) * Stat_Data.accuracy;
        }
        return 0;
    }

    public override double GetEvasionPoint() => GetEvasionPoint(Battle_Data);
    double GetEvasionPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.evasion + (GetLevel() - 1) * Stat_Data.evasion;
        }
        return 0;
    }

    public override double GetPhysicsCriticalChance() => GetPhysicsCriticalChance(Battle_Data);
    double GetPhysicsCriticalChance(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_chance + (GetLevel() - 1) * Stat_Data.physics_critical_chance;
        }
        return 0;
    }
    public override double GetPhysicsCriticalPowerAdd() => GetPhysicsCriticalPower(Battle_Data);
    double GetPhysicsCriticalPower(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.physics_critical_power_add + (GetLevel() - 1) * Stat_Data.physics_critical_power_add;
        }
        return 0;
    }
    public override double GetMagicCriticalChance() => GetMagicCriticalChance(Battle_Data);
    double GetMagicCriticalChance(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_chance + (GetLevel() - 1) * Stat_Data.magic_critical_chance;
        }
        return 0;
    }
    public override double GetMagicCriticalPowerAdd() => GetMagicCriticalPower(Battle_Data);
    double GetMagicCriticalPower(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.magic_critical_power_add + (GetLevel() - 1) * Stat_Data.magic_critical_power_add;
        }
        return 0;
    }
    public override double GetResistPoint() => GetResistPoint(Battle_Data);
    double GetResistPoint(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
        {
            return battle_data.resist + (GetLevel() - 1) * Stat_Data.resist;
        }
        return 0;
    }
    public override double GetLifeRecoveryInc() => GetLifeRecoveryInc(Battle_Data);
    double GetLifeRecoveryInc(Npc_Battle_Data battle_data) 
    { 
        if (battle_data != null)
        {
            return battle_data.heal + (GetLevel() - 1) * Stat_Data.heal;
        }
        return 0; 
    }
    public override double GetWeight() => GetWeight(Battle_Data);
    double GetWeight(Npc_Battle_Data battle_data)
    {
        if (battle_data != null)
            return battle_data.weight + (GetLevel() - 1) * Stat_Data.weight;
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
