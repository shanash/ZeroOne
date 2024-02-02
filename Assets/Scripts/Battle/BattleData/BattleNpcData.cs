

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


    public override float GetMoveSpeed()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.move_speed;
        }
        return 0;
    }
    public override double GetAttackDamagePoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.attack;
        }
        return 0;
    }

    public override double GetAttackDefensePoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.defend;
        }
        return 0;
    }
    public override double GetLifePoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.hp;
        }
        return 0;
    }

    public override double GetAccuracyPoint()
    {
        if (Battle_Data != null)
            return Battle_Data.accuracy;
        return 0;
    }

    public override double GetEvasionPoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.evasion;
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
