

public class BattlePcData : BattleUnitData
{
    Player_Character_Data Data;
    Player_Character_Battle_Data Battle_Data;

    UserHeroData User_Data;

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

    public override float GetDistance()
    {
        if (Battle_Data != null)
        {
            return (float)Battle_Data.distance;
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
}
