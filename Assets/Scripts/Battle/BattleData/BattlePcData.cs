public class BattlePcData : BattleUnitData
{
    Player_Character_Data Data;
    Player_Character_Battle_Data Battle_Data;

    UserHeroData User_Data;
    public BattlePcData() : base(CHARACTER_TYPE.PC) { }

    public override BattleUnitData SetUnitID(params int[] unit_ids)
    {
        if (unit_ids.Length < 2)
        {
            return null;
        }
        int pc_id = unit_ids[0];
        int pc_num = unit_ids[1];

        var m = MasterDataManager.Instance;
        Data = m.Get_PlayerCharacterData(pc_id);
        Battle_Data = m.Get_PlayerCharacterBattleData(Data.battle_info_id);

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

    public override int GetLevel()
    {
        return User_Data.GetLevel();
    }

    public override object GetUserUnitData()
    {
        return User_Data;
    }

    public override float GetMoveSpeed()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.move_speed;
        }
        return 0;
    }

    public override double GetAttackPoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.attack;
        }
        return 0;
    }
    public override double GetDefensePoint()
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

    public override double GetEvationPoint()
    {
        if (Battle_Data != null)
        {
            return Battle_Data.evasion;
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
            return Battle_Data.attack_recovery;
        }
        return 0;
    }

    public override double GetCriticalChance()
    {
        return 0;
    }

    public override double GetCriticalPower()
    {
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
